using API.Common;
using API.DTOs;
using API.Models;
using API.Servivces.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Implementation
{
    public class UserMstService : IUserMstService
    {
        private readonly KUPFDbContext _context;
        private readonly IMapper _mapper;

        public UserMstService(KUPFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddUserMstAsync(UserMstDto userMstDto)
        {
            int result = 0;
            if (_context != null)
            {
                var newUserMst = _mapper.Map<UserMst>(userMstDto);
                newUserMst.UserId = CommonMethods.CreateUserId();
                newUserMst.ActiveFlag = "Y";
                newUserMst.AccLock = "N";
                newUserMst.FirstTime="N";
                newUserMst.LocationId = 1;
                newUserMst.TenentId = 21;
                await _context.UserMsts.AddAsync(newUserMst);
                result = await _context.SaveChangesAsync();
                return result;
            }
            return result;
        }
        public async Task<int> UpdatUserMstAsync(UserMstDto userMstDto)
        {
            int result = 0;
            if (_context != null)
            {
                var existingUserMst = _mapper.Map<Models.UserMst>(userMstDto);
                existingUserMst.ActiveFlag = "Y";
                existingUserMst.AccLock = "N";
                existingUserMst.FirstTime="N";
                existingUserMst.LocationId = 1;
                existingUserMst.TenentId = 21;
                _context.UserMsts.Update(existingUserMst);

                result = await _context.SaveChangesAsync();
                return result;
            };
            return result;
        }
        public async Task<int> DeleteUserMstAsync(int id)
        {
            int result = 0;

            if (_context != null)
            {
                var userMst = await _context.UserMsts.FirstOrDefaultAsync(x => x.UserId == id);

                if (userMst != null)
                {
                    _context.UserMsts.Remove(userMst);

                    result = await _context.SaveChangesAsync();
                }
                return result;
            }
            return result;
        }
        public async Task<UserMstDto> GetUserMstByIdAsync(int userId)
        {
            var result = await _context.UserMsts.Where(c => c.UserId == userId).FirstOrDefaultAsync();
            var data = _mapper.Map<UserMstDto>(result);
            return data;
        }
        public async Task<IEnumerable<UserMstDto>> GetUserMstAsync()
        {
            var result = await _context.UserMsts.ToListAsync();
            var data = _mapper.Map<IEnumerable<UserMstDto>>(result);
            return data;
        }

        public async Task<int> UpatePasswordAsync(UpdatePasswordDto userMstDto)
        {
            int result = 0;
            if (_context != null)
            {
                string decodedPass = CommonMethods.EncodePass(userMstDto.oldPassword);
                var user = _context.UserMsts.Where(c => c.UserId == userMstDto.UserId && c.Password == decodedPass).FirstOrDefault();
                if (user != null)
                {
                    string encodedPass = CommonMethods.EncodePass(userMstDto.newPassword);
                    user.Password = encodedPass;
                    _context.UserMsts.Update(user);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
