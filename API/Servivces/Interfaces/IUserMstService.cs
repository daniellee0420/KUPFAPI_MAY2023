using API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Servivces.Interfaces
{
    public interface IUserMstService
    {
        Task<int> AddUserMstAsync(UserMstDto userMstDto);
        Task<int> UpdatUserMstAsync(UserMstDto userMstDto);
        Task<int> DeleteUserMstAsync(int id);
        Task<UserMstDto> GetUserMstByIdAsync(int userId);        
        Task<IEnumerable<UserMstDto>> GetUserMstAsync();

        Task<int> UpatePasswordAsync(UpdatePasswordDto userMstDto);
    }
}
