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
    public class InmateService : IInmateService
    {
        private readonly IRepository<InmateData> _repo;
        private readonly IInmateRepository _inmateRepository;
        private readonly IRepository<Transfer> _transferRepo;
        public InmateService(IInmateRepository inmateRepository, IRepository<InmateData> repo, IRepository<Transfer> transferRepo)
        {
            _repo = repo;
            _transferRepo = transferRepo;
            _inmateRepository = inmateRepository;
        }

        public async Task<BulkInmateRegistrationResponseModel> BulkUploadInmatesAsync(IFormFile file, int officerId)
        {
            var result = new BulkInmateRegistrationResponseModel();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<RegisterInmateModel>();
                var recordsAdded = new List<InmateData>();
                var recordsDeleted = new List<InmateData>();
                var recordsUpdated = new List<InmateData>();
                var recordsRejectd = new List<RejectedInmateRecords>();

                foreach (var record in records)
                {
                    var exists = await _repo.FindFirstAsync(x => x.IdentificationNumber == record.IdentificationNumber);
                    if (exists != null)
                    {
                        var inmate = ToInmateData(record, officerId);
                        inmate.Id = exists.Id;
                        if (record.Delete.ToLower() == "yes")
                        {
                            await _repo.DeleteAsync(inmate.Id);
                            recordsDeleted.Add(inmate);
                            continue;
                        }
                        var res = await _repo.UpdateAsync(inmate);
                        if (res == null)
                        {
                            var reject = new RejectedInmateRecords() { IdentificationNumber = inmate.IdentificationNumber, ErrorMessage = "Unable to update the give inmate data" };
                            recordsRejectd.Add(reject);
                            continue;
                        }
                        recordsUpdated.Add(inmate);
                        continue;
                    }
                    var inmateToAdd = ToInmateData(record, officerId);
                    var resAdd = await _repo.AddAsync(inmateToAdd);
                    if (resAdd == null)
                    {
                        var reject = new RejectedInmateRecords { IdentificationNumber = inmateToAdd.IdentificationNumber, ErrorMessage = "Error while adding the given inmate" };
                        recordsRejectd.Add(reject);
                        continue;
                    }
                    recordsAdded.Add(inmateToAdd);
                }
                result.RejectedRecords = recordsRejectd;
                result.TotalDeleted = recordsDeleted.Count;
                result.TotalUpdated = recordsUpdated.Count;
                result.TotalInserted = recordsAdded.Count;

            }
            return result;
        }

        public async Task<IEnumerable<InmateModel>> GetInmatesAsync(int pageNumber, string search, string filter)
        {
            var result = await _inmateRepository.GetInmatesAsync(pageNumber, search, filter);
            return result;
        }

        public async Task<int> GetInmatesCountAsync( string search, string filter)
        {
            var result = await _inmateRepository.GetInmatesAsync(0, search, filter);
            return result.Count();
        }


        public async Task<bool> TransferInmatesAsync(RequestTransferModel model, int officerId)
        {
            foreach (var idNumber in model.IdentificationNumbers)
            {
                var inmate = await _repo.FindFirstAsync(x => x.IdentificationNumber == idNumber);
                var transferData = new Transfer
                {
                    SourceFacilityId = inmate.CurrentFacility,
                    DestinationFacilityId = model.DestinationFacilityId,
                    ArrivalTime = model.ArrivalTime,
                    DepartureTime = model.DepartureTime,
                    InmateId = inmate.Id,
                    CreatedBy = officerId,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedBy = officerId,
                    UpdatedDate = DateTime.UtcNow,
                };
                await _transferRepo.AddAsync(transferData);
                inmate.CurrentFacility = model.DestinationFacilityId;
                await _repo.UpdateAsync(inmate);
            }
            return true;
        }

        private InmateData ToInmateData(RegisterInmateModel model, int officerId)
        {
            return new InmateData
            {
                Name = model.Name,
                CurrentFacility = model.FacilityId,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                IdentificationNumber = model.IdentificationNumber,
                UpdatedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = officerId,
                UpdatedBy = officerId
            };
        }
    }
}
