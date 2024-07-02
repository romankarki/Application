using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;
using Contracts.Models.Request;
using Contracts.Models.Response;
using CsvHelper;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Application.Implementation
{
    public class FacilityService : IFacilityService
    {
        private readonly IRepository<Facility> _repo;
        private readonly IFacilityRepository _facilityRepository;
        public FacilityService(IFacilityRepository facilityRepository, IRepository<Facility> repo)
        {
            _repo = repo;
            _facilityRepository = facilityRepository;
        }

        public async Task<BulkFacilityRegistrationResponseModel> BulkUploadFacilityAsync(IFormFile file, int officerId)
        {
            var result = new BulkFacilityRegistrationResponseModel();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<RegisterFacilityModel>();
                var recordsAdded = new List<Facility>();
                var recordsDeleted = new List<Facility>();
                var recordsUpdated  = new List<Facility>();
                var recordsRejectd = new List<RejectedFacilityRecords>();
                
                foreach(var record in records)
                {
                    var exists = await _repo.FindFirstAsync(x => x.FacilityCode == record.Code);
                    if(exists != null)
                    {
                        var facility = ToFacility(record, officerId);
                        facility.Id = exists.Id;
                        if(record.Delete.ToLower() == "yes")
                        {
                            await _repo.DeleteAsync(facility.Id);
                            recordsDeleted.Add(facility);
                            continue;
                        }
                        var res = await _repo.UpdateAsync(facility);
                        if(res == null )
                        {
                            var reject = new RejectedFacilityRecords { Address = record.Address, FacilityCode = record.Code, ErrorMessage = "Failed to update the given data" };
                            recordsRejectd.Add(reject);
                            continue;
                        }
                        recordsUpdated.Add(facility);
                        continue;
                    }
                    var facilityToAdd = ToFacility(record, officerId);
                    var resAdd = await _repo.AddAsync(facilityToAdd);
                    if(resAdd == null)
                    {
                        var reject = new RejectedFacilityRecords { Address = record.Address, FacilityCode = record.Code, ErrorMessage = "Failed to Add a new record for this value" };
                        recordsRejectd.Add(reject);
                        continue;
                    }
                    recordsAdded.Add(facilityToAdd);
                }
                result.RejectedRecords = recordsRejectd;
                result.TotalInserted = recordsAdded.Count;
                result.TotalUpdated = recordsUpdated.Count;
                result.TotalDeleted = recordsDeleted.Count;
            }

            return result;
        }

        public async Task<IEnumerable<FacilitiesModel>> GetALlFacilitiesAsync(int pageNumber)
        {
            var result = await _facilityRepository.GetFacilityByPageNumber(pageNumber);
            return result.Select(x => ToFacilityModel(x));
        }
        public async Task<int> GetALlFacilitiesCountAsync()
        {
            var result = await _facilityRepository.GetFacilityByPageNumber(0);
            return result.Count();
        }


        private FacilitiesModel ToFacilityModel(Facility entity)
        {
            return new FacilitiesModel
            {
                Id = entity.Id,
                FacilityName = entity.FacilityName,
                ContactEmail = entity.FacilityContactEmail,
                ContactPerson = entity.FacilityContactPerson,
                ContactNumber = entity.FacilityContactNumber,
                Code = entity.FacilityCode,
                Address = entity.Address
            };
        }

        private Facility ToFacility(RegisterFacilityModel model, int officerId)
        {
            return new Facility 
            {
                FacilityCode = model.Code,
                FacilityContactEmail = model.Email,
                FacilityContactNumber = model.Number,
                FacilityContactPerson = model.PersonName,
                FacilityName = model.Name,
                Address = model.Address,
                UpdatedBy = officerId,
                UpdatedDate = DateTime.UtcNow,
                CreatedBy = officerId,
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}
