using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApiV1.Helpers;
using webApiV1.Models.Requests;
using webApiV1.Models.Responses;
using webApiV1.Services;

namespace webApiV1.Controllers.Product
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectsController : ControllerBase
    {

        private readonly ProductService _productServices;
        public ProjectsController(MongoDbSettings settings)
        {
            _productServices = new ProductService(settings);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> index()
        {
            var products = await _productServices.GetAll();
            return Ok(products);
        }
    }


}
