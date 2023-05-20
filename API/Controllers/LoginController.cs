using API.Common;
using API.DTOs;
using API.DTOs.EmployeeDto;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly KUPFDbContext _context;
        private readonly IFunctionUserService _functionUserService;
        private readonly IMapper _mapper;
        //private readonly ITokenService _tokenService;
        public LoginController(KUPFDbContext context, IFunctionUserService functionUserService, IMapper mapper)
        {
            _context = context;
            _functionUserService = functionUserService;
            _mapper = mapper;
            //_tokenService = tokenService;
        }
        /// <summary>
        /// Api to Employee Login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EmployeeLogin")]
        public async Task<ActionResult<IEnumerable<LoginDto>>> EmployeeLogin(LoginDto loginDto)
        {
            string decodedPass = CommonMethods.EncodePass(loginDto.password);
            var user = await _context.UserMsts.
                Where(c => c.LoginId == loginDto.username && c.Password == decodedPass)
                .ToListAsync();

            // Get period code.
            var periodCode = _context.Tblperiods.Where(c => c.PrdStartDate <= DateTime.Now && c.PrdEndDate >= DateTime.Now).FirstOrDefault().PeriodCode;
            
            // Get the previous code...
            var now = DateTime.Now;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddDays(-30);
            var lastDayLastMonth = firstDayCurrentMonth.AddDays(-1);
            var firstDayNextMonth = firstDayCurrentMonth.AddMonths(1);
            var lastDayNextMonth = firstDayNextMonth.AddDays(30);

            var prePeriodCode = _context.Tblperiods.Where(c => c.PrdStartDate <= lastDayLastMonth && c.PrdEndDate >= firstDayLastMonth).FirstOrDefault().PeriodCode;
            //
            var nextPeriodCode = _context.Tblperiods.Where(c => c.PrdStartDate <= lastDayNextMonth && c.PrdEndDate >= firstDayNextMonth).FirstOrDefault().PeriodCode;
            
            List<LoginDto> userList = new List<LoginDto>();
            if (user.Count() >= 1)
            {
                for (int i = 0; i < user.Count(); i++)
                {
                    var dto = new LoginDto
                    {
                        username = user[i].LoginId,
                        LocationId = user[i].LocationId,
                        TenantId = user[i].TenentId,
                        UserId = user[i].UserId,
                        RoleId = user[i].ROLEID,
                        PeriodCode = Convert.ToString(periodCode),
                        PrevPeriodCode = Convert.ToString(prePeriodCode),
                        NextPeriodCode = Convert.ToString(nextPeriodCode)
                        //Token = _tokenService.CreateToken(user[i].LoginId)                        
                    };
                    userList.Add(dto);
                }
                return userList;
            }

            return userList;
        }
        /// <summary>
        /// Api to Get User Functions By User Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserFunctionsByUserId")]
        public async Task<ActionResult<IEnumerable<MenuHeadingDto>>> GetUserFunctionsByUserId(int id)
        {
            List<MenuHeadingDto> menuHeader = new List<MenuHeadingDto>();
            // Get menu data by UserId...
            var result = await _functionUserService.GetFunctionUserByUserIdAsync(id);

            if (result.Count() > 0)
            {
                // If has dashboard access...
                var dasboard = result.FirstOrDefault(o => o.MENU_ID == 1);
                if (dasboard != null)
                {
                    menuHeader.Add(new MenuHeadingDto
                    {
                        HeadingNameEnglish = dasboard.MENU_NAMEEnglish,
                        HeadingNameArabic = dasboard.MENU_NAMEArabic,
                        HeadingIconPath = dasboard.ICONPATH,
                        HeadingSmallText = dasboard.SMALLTEXT,
                        HeadingFullName = dasboard.FULL_NAME,
                        HeadingLink = dasboard.LINK,
                        HeadingURLOption = dasboard.Urloption,
                        HeadingURLRewrite = dasboard.URLREWRITE,
                        HeadingMenuLocation = dasboard.MENU_LOCATION,
                        HeadingMenuOrder = dasboard.MENU_ORDER,
                        HeadingDocParent = dasboard.DOC_PARENT,
                        HeadingAddFlage = dasboard.ADDFLAGE,
                        HeadingEditFlage = dasboard.EDITFLAGE,
                        HeadingDelFlage = dasboard.DELFLAGE,
                        HeadingPrintFlage = dasboard.PRINTFLAGE,
                        HeadingAmIGlobale = dasboard.AMIGLOBALE,
                        HeadingMyPersonal = dasboard.MYPERSONAL,
                        HeadingSp1 = dasboard.SP1,
                        HeadingSp2 = dasboard.SP2,
                        HeadingSp3 = dasboard.SP3,
                        HeadingSp4 = dasboard.SP4,
                        HeadingSp5 = dasboard.SP5,
                        HeadingSpName1 = dasboard.SP1Name,
                        HeadingSpName2 = dasboard.SP2Name,
                        HeadingSpName3 = dasboard.SP3Name,
                        HeadingSpName4 = dasboard.SP4Name,
                        HeadingSpName5 = dasboard.SP5Name

                    });
                }
              // Get menu items
                var menuItems = result.Where(c => c.MENU_TYPE == "1").ToArray();
                if (menuItems.Length > 0)
                {
                    for (int x = 0; x <= menuItems.Count() - 1; x++)
                    {
                        menuHeader.Add(new MenuHeadingDto
                        {
                            HeadingNameEnglish = menuItems[x].MENU_NAMEEnglish,
                            HeadingNameArabic = menuItems[x].MENU_NAMEArabic,
                            HeadingMenuId = menuItems[x].MENU_ID,
                            HeadingIconPath = menuItems[x].ICONPATH,
                            HeadingSmallText = menuItems[x].SMALLTEXT,
                            HeadingFullName = menuItems[x].FULL_NAME,
                            HeadingLink = menuItems[x].LINK,
                            HeadingURLOption = menuItems[x].Urloption,
                            HeadingURLRewrite = menuItems[x].URLREWRITE,
                            HeadingMenuLocation = menuItems[x].MENU_LOCATION,
                            HeadingMenuOrder = menuItems[x].MENU_ORDER,
                            HeadingDocParent = menuItems[x].DOC_PARENT,
                            HeadingAddFlage = menuItems[x].ADDFLAGE,
                            HeadingEditFlage = menuItems[x].EDITFLAGE,
                            HeadingDelFlage = menuItems[x].DELFLAGE,
                            HeadingPrintFlage = menuItems[x].PRINTFLAGE,
                            HeadingAmIGlobale = menuItems[x].AMIGLOBALE,
                            HeadingMyPersonal = menuItems[x].MYPERSONAL,
                            HeadingSp1 = menuItems[x].SP1,
                            HeadingSp2 = menuItems[x].SP2,
                            HeadingSp3 = menuItems[x].SP3,
                            HeadingSp4 = menuItems[x].SP4,
                            HeadingSp5 = menuItems[x].SP5,
                            HeadingSpName1 = menuItems[x].SP1Name,
                            HeadingSpName2 = menuItems[x].SP2Name,
                            HeadingSpName3 = menuItems[x].SP3Name,
                            HeadingSpName4 = menuItems[x].SP4Name,
                            HeadingSpName5 = menuItems[x].SP5Name

                        });
                        
                        // To Get Childres of Menu Items...
                        var menuItemsChildrens = result.Where(c =>c.MODULE_ID == menuItems[x].MODULE_ID && c.MASTER_ID == menuItems[x].MASTER_ID && c.MENU_NAMEEnglish != menuItems[x].MENU_NAMEEnglish).ToArray();

                        for (int i = 0; i <= menuItemsChildrens.Count() - 1; i++)
                        {
                            menuHeader[x].MenuItems.Add(new MenuItemsDto()
                            {
                                MenuItemNameEnglish = menuItemsChildrens[i].MENU_NAMEEnglish,
                                MenuItemNameArabic = menuItemsChildrens[i].MENU_NAMEArabic,
                                MenuItemIconPath = menuItemsChildrens[i].ICONPATH,
                                MenuItemSmallText = menuItemsChildrens[i].SMALLTEXT, 
                                MenuItemFullName = menuItemsChildrens[i].FULL_NAME,
                                MenuItemLink = menuItemsChildrens[i].LINK,
                                MenuItemURLOption = menuItemsChildrens[i].Urloption,
                                MenuItemURLRewrite = menuItemsChildrens[i].URLREWRITE,
                                MenuItemMenuLocation = menuItemsChildrens[i].MENU_LOCATION,
                                MenuItemMenuOrder = menuItemsChildrens[i].MENU_ORDER,
                                MenuItemDocParent = menuItemsChildrens[i].DOC_PARENT,
                                MenuItemAddFlage = menuItemsChildrens[i].ADDFLAGE,
                                MenuItemEditFlage = menuItemsChildrens[i].EDITFLAGE,
                                MenuItemDelFlage = menuItemsChildrens[i].DELFLAGE,
                                MenuItemPrintFlage = menuItemsChildrens[i].PRINTFLAGE,
                                MenuItemAmIGlobale = menuItemsChildrens[i].AMIGLOBALE,
                                MenuItemMyPersonal = menuItemsChildrens[i].MYPERSONAL,
                                MenuItemSp1 = menuItemsChildrens[i].SP1,
                                MenuItemSp2 = menuItemsChildrens[i].SP2,
                                MenuItemSp3 = menuItemsChildrens[i].SP3,
                                MenuItemSp4 = menuItemsChildrens[i].SP4,
                                MenuItemSp5 = menuItemsChildrens[i].SP5,
                                MenuItemSpName1 = menuItemsChildrens[i].SP1Name,
                                MenuItemSpName2 = menuItemsChildrens[i].SP2Name,
                                MenuItemSpName3 = menuItemsChildrens[i].SP3Name,
                                MenuItemSpName4 = menuItemsChildrens[i].SP4Name,
                                MenuItemSpName5 = menuItemsChildrens[i].SP5Name
                            });

                           // var menuItemsGrandChildrens = result.Where(c => c.MODULE_ID == menuItemsChildrens[i].MODULE_ID && c.MENU_TYPE == "3" ).ToArray();

                            if (menuItemsChildrens[i].MENU_TYPE == "3")
                            {
                                var menuItemsGrandChildrens = result.Where(c => c.MODULE_ID == menuItemsChildrens[i].MODULE_ID && c.MASTER_ID == menuItemsChildrens[i].MENU_ID).ToArray();
                                for (int z = 0; z < menuItemsGrandChildrens.Count(); z++)
                                {
                                    menuHeader[x].MenuItems[i].MenuItems.Add(new MenuItemsDto()
                                    {
                                        MenuItemNameEnglish = menuItemsGrandChildrens[z].MENU_NAMEEnglish,
                                        MenuItemNameArabic = menuItemsGrandChildrens[z].MENU_NAMEArabic,
                                        MenuItemIconPath = menuItemsGrandChildrens[z].ICONPATH,
                                        MenuItemSmallText = menuItemsGrandChildrens[z].SMALLTEXT,
                                        MenuItemFullName = menuItemsGrandChildrens[z].FULL_NAME,
                                        MenuItemLink = menuItemsGrandChildrens[z].LINK,
                                        MenuItemURLOption = menuItemsGrandChildrens[z].Urloption,
                                        MenuItemURLRewrite = menuItemsGrandChildrens[z].URLREWRITE,
                                        MenuItemMenuLocation = menuItemsGrandChildrens[z].MENU_LOCATION,
                                        MenuItemMenuOrder = menuItemsGrandChildrens[z].MENU_ORDER,
                                        MenuItemDocParent = menuItemsGrandChildrens[z].DOC_PARENT,
                                        MenuItemAddFlage = menuItemsGrandChildrens[z].ADDFLAGE,
                                        MenuItemEditFlage = menuItemsGrandChildrens[z].EDITFLAGE,
                                        MenuItemDelFlage = menuItemsGrandChildrens[z].DELFLAGE,
                                        MenuItemPrintFlage = menuItemsGrandChildrens[z].PRINTFLAGE,
                                        MenuItemAmIGlobale = menuItemsGrandChildrens[z].AMIGLOBALE,
                                        MenuItemMyPersonal = menuItemsGrandChildrens[z].MYPERSONAL,
                                        MenuItemSp1 = menuItemsGrandChildrens[z].SP1,
                                        MenuItemSp2 = menuItemsGrandChildrens[z].SP2,
                                        MenuItemSp3 = menuItemsGrandChildrens[z].SP3,
                                        MenuItemSp4 = menuItemsGrandChildrens[z].SP4,
                                        MenuItemSp5 = menuItemsGrandChildrens[z].SP5,
                                        MenuItemSpName1 = menuItemsGrandChildrens[z].SP1Name,
                                        MenuItemSpName2 = menuItemsGrandChildrens[z].SP2Name,
                                        MenuItemSpName3 = menuItemsGrandChildrens[z].SP3Name,
                                        MenuItemSpName4 = menuItemsGrandChildrens[z].SP4Name,
                                        MenuItemSpName5 = menuItemsGrandChildrens[z].SP5Name
                                    });
                                }
                            }
                        }
                    }
                }

               
            }
            return Ok(menuHeader);

        }

        /// <summary>
        /// Api to Employee Login
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("MobileLogin")]
        public async Task<ActionResult<DetailedEmployeeDto>> MobileLogin(MobileLoginDto mobileLoginDto)
        {
            var employee = await _context.DetailedEmployees.
                Where(c => c.EmployeeLoginId == mobileLoginDto.username && c.EmployeePassword == mobileLoginDto.password)
                .FirstOrDefaultAsync();

           var user = _mapper.Map<DetailedEmployeeDto>(employee);

            return user;
        }
    }
}

