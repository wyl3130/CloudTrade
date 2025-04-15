using CloudTrade.Domain.ModeInfos;


namespace CloudTrade.Application.Contracts.ModeInfos
{
    public interface IModeInfoService:IBaseService
    {
        Task<(IEnumerable<ModeInfo>, int)> ModeInfoQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "");
    }
}
