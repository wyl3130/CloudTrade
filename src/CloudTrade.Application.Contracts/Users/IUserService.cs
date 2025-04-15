

using CloudTrade.Domain.Users;

namespace CloudTrade.Application.Contracts.Users
{
    public interface IUserService:IBaseService
    {
        Task<bool> LoginAsync(string UserName,string PassWord);
    }
}
