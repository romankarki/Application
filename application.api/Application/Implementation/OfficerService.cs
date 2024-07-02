using Application.Interfaces.Infrastructure;
using Application.Interfaces.Services;
using Contracts.Models.Request;
using Contracts.Models.Response;
using CsvHelper;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;

namespace Application.Implementation
{
    public class OfficerService : IOfficerService
    {
        private readonly IOfficerRepository _officeRepo;
        private readonly IRepository<Officer> _repo;
        private String KEY = "This is secret key for encryption of apis and authentication purposes";

        public OfficerService(IOfficerRepository officeRepo, IRepository<Officer> repo) 
        {
            _officeRepo = officeRepo;
            _repo = repo;
        }

        public async Task<OfficerModel> AuthenticateOfficerAsync(string identificationNumbber, string password)
        {
            var officer = await _repo.FindFirstAsync(x=> x.IdentificationNumber == identificationNumbber);
            if (officer == null) throw new UnauthorizedAccessException("User Does not exist with this identification number");
            if (!BCrypt.Net.BCrypt.Verify(password, officer.Password))
            {
                throw new UnauthorizedAccessException("Invalid Credential Provided");
            }
            return OfficerModel(officer);
        }

        public async Task<BulkRegisterOfficerModel> BulkRegistrationAsync(IFormFile file, int officerId)
        {
            var result = new BulkRegisterOfficerModel();
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<RegisterOfficerModel>();
                var recordsAdded = new List<Officer>();
                var recordsDeleted = new List<Officer>();
                var recordsUpdated = new List<Officer>();
                var recordsRejected = new List<RejectedRecords>();
                foreach(var record in records)
                {
                    var exists = await _officeRepo.FindOfficerByIdentificationNumberAsync(record.IdentificationNumber);
                    if(exists != null)
                    {
                        var officer = ToOfficer(record, officerId);
                        officer.Id = exists.Id;
                        if(record.Delete.ToLower() == "yes")
                        {
                            await _repo.DeleteAsync(officer.Id);
                            recordsDeleted.Add(officer);
                            continue;
                        }
                        var res = await _repo.UpdateAsync(officer);
                        if (res == null)
                        {
                            var reject = new RejectedRecords { IdentificationNumber = officer.IdentificationNumber, ErrorMessage = "failed to update with this duplicate identification number, please recheck values" };
                            recordsRejected.Add(reject);
                            continue;
                        }
                        recordsUpdated.Add(officer);
                        continue;
                    }
                    var officerToAdd = ToOfficer(record, officerId);
                    var resAdd = await _repo.AddAsync(officerToAdd);
                    if(resAdd == null) 
                    {
                        var reject = new RejectedRecords { IdentificationNumber = officerToAdd.IdentificationNumber, ErrorMessage = "failed to create a new record, please recheck values" };
                        recordsRejected.Add(reject);
                        continue;
                    }
                    recordsAdded.Add(officerToAdd);
                }
                result.RejectedRecords = recordsRejected;
                result.TotalUpdated = recordsUpdated.Count;
                result.TotalDeleted = recordsDeleted.Count;
                result.TotalInserted = recordsAdded.Count;
            }
            return result;
        }

        public async Task<OfficerModel> GetOfficerByIdAsync(int id)
        {
            var result = await _repo.GetAsync(id);
            return OfficerModel(result);
        }

        public async Task<OfficerModel> SignUpAsync(RegisterOfficerModel model, int officerId = 0)
        {
            var existingOfficer = await _repo.FindFirstAsync(x=> x.IdentificationNumber == model.IdentificationNumber);
            if (existingOfficer != null)
            {
                throw new InvalidDataException("Officer already exists with this identificaiton number");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var officerData = new Officer
            {
                IdentificationNumber = model.IdentificationNumber,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = officerId,
                UpdatedBy = officerId,
                UpdatedDate = DateTime.UtcNow,
                ContactNumber = model.PhoneNumber,
                ContactEmail = model.Email,
                Name = model.Name,
                Password = hashedPassword
            };
            var result = await _repo.AddAsync(officerData);
            if(result == null) { }
            return OfficerModel(result);
        }

        private string GetToken(int officerId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(KEY);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", officerId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private OfficerModel OfficerModel(Officer entity)
        {
            if (entity == null) return null;
            return new OfficerModel
            {
                Name = entity.Name,
                Email = entity.ContactEmail,
                OfficerId = entity.Id,
                IdentificationNumber = entity.IdentificationNumber,
                Token = GetToken(entity.Id) 
            };
        }

        private Officer ToOfficer(RegisterOfficerModel model, int OfficerId)
        {
            return new Officer
            {
                Name = model.Name,
                ContactEmail = model.Email,
                ContactNumber = model.PhoneNumber,
                IdentificationNumber = model.IdentificationNumber,
                Password =  BCrypt.Net.BCrypt.HashPassword(model.Password),
                CreatedBy = OfficerId,
                UpdatedBy = OfficerId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
            };
        }
    }
}
