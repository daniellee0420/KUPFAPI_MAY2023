using API.DTOs;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly KUPFDbContext _context;
        private readonly ICommonService _commonServiceService;
        public IMapper _mapper;

        public WebsiteController(KUPFDbContext context, ICommonService commonServiceService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _commonServiceService = commonServiceService;
        }
        /// <summary>
        /// This api will get all active services from service setup for web menu...
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetServicesForWebMenu")]
        public async Task<IEnumerable<ServiceSetupServicesDto>> GetServicesForWebMenu()
        {
            var result = await _commonServiceService.GetServicesForWebMenu();
            return result;
        }
    }
}
