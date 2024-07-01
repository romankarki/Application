using Microsoft.AspNetCore.Http;

namespace Contracts.Models.Request
{
    public class BulkRegisterModel
    {
        public IFormFile file { get; set; }
    }
}
