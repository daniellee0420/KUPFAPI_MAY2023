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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("GetVoucher")]
        public async Task<IEnumerable<VoucherDto>> GetVoucher()
        {
            var result =  _accountService.GetVoucher();
            return result;
        }
        [HttpGet]
        [Route("GetVoucherDetails")]
        public async Task<IEnumerable<VoucherDetailsDto>> GetVoucherDetails(int voucherId)
        {
            var result = _accountService.GetVoucherDetails(voucherId);
            return result;
        }
    }
}
