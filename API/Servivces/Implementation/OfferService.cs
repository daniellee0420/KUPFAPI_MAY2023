using API.DTOs;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.Common;
using API.Helpers;

namespace API.Servivces.Implementation
{
    public class OfferService : IOfferService
    {
        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;
        public OfferService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddOffer(OffersDto offersDto)
        {
            int result = 0;

            if (_context != null)
            {
                // To get the max Id.
                var maxIdServiceId = (from d in _context.ServiceSetups
                                      where d.TenentId == offersDto.TenentId
                                      select new
                                      {
                                          ServiceId = d.ServiceId + 1
                                      })
                         .Distinct()
                         .OrderBy(x => 1).Max(c => c.ServiceId);

                var newService = new ServiceSetup()
                {
                    TenentId = offersDto.TenentId,
                    Userid = offersDto.Userid,
                    OfferType = offersDto.OfferType,
                    Offer = offersDto.OfferTypeName,
                    OfferStartDate = offersDto.OfferStartDate,
                    OfferEndDate = offersDto.OfferEndDate,
                    OfferAmount = offersDto.OfferAmount,
                    MasterServiceId = "0",
                    ElectronicForm1URL = offersDto.ElectronicForm1URL,
                    ElectronicForm2URL = offersDto.ElectronicForm2URL,
                    EnglishHTML = offersDto.EnglishHTML,
                    ArabicHTML = offersDto.ArabicHTML,
                    EnglishWebPageName = offersDto.EnglishWebPageName,
                    ArabicWebPageName = offersDto.ArabicWebPageName,
                    OfferTypeName = offersDto.OfferTypeName,
                    WebEnglish = offersDto.WebEnglish,
                    WebArabic = offersDto.WebArabic,
                    Active = "1",
                    IsElectronicForm = offersDto.IsElectronicForm,
                    OfferName = offersDto.OfferName
                };
                newService.ServiceId = maxIdServiceId;
                
                var path = @"C:\HostingSpace\kupf1\kuweb.erp53.com\wwwroot\Offers";                
                //var path = @"E:\\";
                if (offersDto.File1 != null && offersDto.File1.Length != 0)
                {   
                    var fileExtenstion = Path.GetExtension(offersDto.File1.FileName);
                    var fileName = Guid.NewGuid() + fileExtenstion;
                    var filePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        offersDto.File1.CopyTo(stream);
                    }
                    newService.OfferImage = "/Offers/"+fileName;

                }
                if (offersDto.ElectronicForm1Attachment != null && offersDto.ElectronicForm1Attachment.Length != 0)
                {
                    var fileExtenstion = Path.GetExtension(offersDto.ElectronicForm1Attachment.FileName);
                    var fileName = Guid.NewGuid() + fileExtenstion;
                    var filePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        offersDto.ElectronicForm1Attachment.CopyTo(stream);
                    }
                    newService.ElectronicForm1 = "/Offers/" + fileName;
                }
                if (offersDto.ElectronicForm2Attachment != null && offersDto.ElectronicForm2Attachment.Length != 0)
                {
                    var fileExtenstion = Path.GetExtension(offersDto.ElectronicForm2Attachment.FileName);
                    var fileName = Guid.NewGuid() + fileExtenstion;
                    var filePath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        offersDto.ElectronicForm2Attachment.CopyTo(stream);
                    }
                    newService.ElectronicForm2 = "/Offers/" + fileName;
                }
                await _context.ServiceSetups.AddAsync(newService);
                result = await _context.SaveChangesAsync();
                return result;
            }

            return result;
        }

        public async Task<int> DeleteOffer(int id)
        {
            int result = 0;

            if (_context != null)
            {
                var serviceSetup = await _context.ServiceSetups.FirstOrDefaultAsync(x => x.ServiceId == id);

                if (serviceSetup != null)
                {
                    _context.ServiceSetups.Remove(serviceSetup);

                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }

        public async Task<int> EditOffer(OffersDto offersDto)
        {
            int result = 0;
            if (_context != null)
            {
                if (offersDto != null)
                {
                    var existingService = _context.ServiceSetups.Where(c => c.ServiceId == offersDto.ServiceId).FirstOrDefault();
                                       
                    existingService.TenentId = offersDto.TenentId;
                    existingService.Userid = offersDto.Userid;
                    existingService.OfferType = offersDto.OfferType;
                    existingService.Offer = offersDto.OfferTypeName;
                    existingService.OfferStartDate = offersDto.OfferStartDate;
                    existingService.OfferEndDate = offersDto.OfferEndDate;
                    existingService.OfferAmount = offersDto.OfferAmount;
                    existingService.ElectronicForm1URL = offersDto.ElectronicForm1URL;
                    existingService.ElectronicForm2URL = offersDto.ElectronicForm2URL;
                    existingService.EnglishHTML = offersDto.EnglishHTML;
                    existingService.ArabicHTML = offersDto.ArabicHTML;
                    existingService.EnglishWebPageName = offersDto.EnglishWebPageName;
                    existingService.ArabicWebPageName = offersDto.ArabicWebPageName;
                    existingService.OfferTypeName = offersDto.OfferTypeName;
                    existingService.WebEnglish = offersDto.WebEnglish;
                    existingService.WebArabic = offersDto.WebArabic;
                    existingService.OfferName = offersDto.OfferName;

                    var path = @"/HostingSpaces/kupf1/kuweb.erp53.com/wwwroot/Offers/";
                    
                    if (offersDto.File1 != null && offersDto.File1.Length != 0)
                    {
                        var fileExtenstion = Path.GetExtension(offersDto.File1.FileName);
                        var fileName = Guid.NewGuid() + fileExtenstion;
                        var filePath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            offersDto.File1.CopyTo(stream);
                        }
                        existingService.OfferImage = "/Offers/" + fileName;
                    }
                    if (offersDto.ElectronicForm1Attachment != null && offersDto.ElectronicForm1Attachment.Length != 0)
                    {
                        var fileExtenstion = Path.GetExtension(offersDto.ElectronicForm1Attachment.FileName);
                        var fileName = Guid.NewGuid() + fileExtenstion;
                        var filePath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            offersDto.ElectronicForm1Attachment.CopyTo(stream);
                        }
                        existingService.ElectronicForm1 = "/Offers/" + fileName;
                    }
                    if (offersDto.ElectronicForm2Attachment != null && offersDto.ElectronicForm2Attachment.Length != 0)
                    {
                        var fileExtenstion = Path.GetExtension(offersDto.ElectronicForm2Attachment.FileName);
                        var fileName = Guid.NewGuid() + fileExtenstion;
                        var filePath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            offersDto.ElectronicForm2Attachment.CopyTo(stream);
                        }
                        existingService.ElectronicForm2 = "/Offers/" + fileName;
                    }
                    // 
                    _context.ServiceSetups.Update(existingService);
                    result = await _context.SaveChangesAsync();
                    return result;
                }

            };
            return result;
        }

        public async Task<ServiceSetupDto> GetOfferById(int id)
        {
            //var path = @"/HostingSpaces/kupf1/kuweb.erp53.com/wwwroot"; 
            //var path = @"E:\Offers\";
            //var path = @"/kupf1/kuweb.erp53.com/wwwroot/Offers/"; C:\HostingSpace\kupf1\kuweb.erp53.com\wwwroot\Offers
            //var path = @"C:\HostingSpace\kupf1\kuweb.erp53.com\wwwroot";
            var result = await _context.ServiceSetups.Where(c => c.ServiceId == id && c.OfferType == "1").FirstOrDefaultAsync();
            
            var data = _mapper.Map<ServiceSetupDto>(result);
            
            //if(result.OfferImage != null) 
            //    data.OfferImageFile = CommonMethods.GetFileFromFolder(path + result.OfferImage);

            //if(result.ElectronicForm1 != null)
            //    data.ElectronicForm1File = CommonMethods.GetFileFromFolder(path + result.ElectronicForm1);

            //if (result.ElectronicForm2 != null)
            //    data.ElectronicForm2File = CommonMethods.GetFileFromFolder(path + result.ElectronicForm2);

            return data;
        }

        public async Task<ServiceSetupDtoObj> GetOffers(PaginationParams paginationParams)
        {
            var result = _context.ServiceSetups.Where(c=>c.OfferType == "1").OrderByDescending(x => x.ServiceId).ToList();
            var data = _mapper.Map<IEnumerable<ServiceSetupDto>>(result);
            var serviceSetupDtoObj = new ServiceSetupDtoObj();
            if (!string.IsNullOrEmpty(paginationParams.Query))
            {
                data = data.Where(u => u.ServiceName1.ToLower().Contains(paginationParams.Query.ToLower())
                || u.ServiceName2.ToLower().Contains(paginationParams.Query.ToLower()));  
            }
            serviceSetupDtoObj.TotalRecords = data.Count();
            serviceSetupDtoObj.serviceSetupDto = data.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize).Take(paginationParams.PageSize).ToList();
            return serviceSetupDtoObj;
        }
        
        //public static byte[] GetFileFromFolder(string filePath)
        //{
        //    byte[] result = null;
        //    if (filePath != null)
        //    {
        //        var file=System.IO.File.ReadAllBytes(filePath);
        //        result = file;
        //    }
        //    return result;
        //}
    }
}
