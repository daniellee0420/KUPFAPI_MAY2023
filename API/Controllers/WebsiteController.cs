using API.DTOs;
using API.Helpers;
using API.Models;
using API.Servivces.Implementation;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// it is using to add new subscription from website
        /// </summary>
        /// <param name="detailedEmployeeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddNewSubscription")]
        public async Task<ActionResult<int>> AddNewSubscription(NewSubscriptionModel newSubscriptionModel)
        {
            var result = await _commonServiceService.AddNewSubscription(newSubscriptionModel);
            await _context.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// This api will get all active services from service setup for web menu...
        /// </summary>
        /// <returns></returns>
         [Authorize]
        [HttpGet]
        [Route("GetNewSubscription")]
        public async Task<NewSubscriberDto> GetNewSubscription([FromQuery] PaginationParams paginationParams, int tenentId, int locationId)
        {
            var result = await _commonServiceService.GetNewSubscription(paginationParams,tenentId,locationId);
            return result;
        }
    }
}
