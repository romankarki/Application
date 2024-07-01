using Contracts.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BaseController : Controller
    {
        private int _officerID;
        public int OfficerID
        {
            get
            {
                if (_officerID > 0)
                {
                    return _officerID;
                }

                var user = (OfficerModel)HttpContext.Items["Officer"];
                if (user == null)
                {
                    _officerID = 0;
                }

                _officerID = user.OfficerId;
                return _officerID;
            }
        }
    }
}
