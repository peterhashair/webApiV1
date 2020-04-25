using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiV1.Models;
using webApiV1.Services;

namespace webApiV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserServices _userServices;
        public UsersController(UserServices userServices)
        {
            _userServices = userServices;
        }


        [HttpGet]
        public ActionResult<List<User>> Get() => _userServices.Get();
     
        [HttpPost]
        public ActionResult<User> Create(User user)
        {
            _userServices.Create(user);

            return user;
        }

    }
}