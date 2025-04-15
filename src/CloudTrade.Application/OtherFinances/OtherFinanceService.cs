using CloudTrade.Application.Contracts.OtherFinances;
using CloudTrade.Application.Contracts.Payments;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.FinanceCategorys;
using CloudTrade.Domain.OtherFinances;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.OtherFinances
{
    public class OtherFinanceService : BaseService, IOtherFinanceService
    {
        private readonly ISqlSugarClient db;
        public OtherFinanceService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<(IEnumerable<OtherFinanceDto>, int)> OtherFinanceQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<OtherFinance, PaymentAccount, PaymentCategory, FinanceCategory>((o, pa, pc, f) => new JoinQueryInfos(
    JoinType.Inner,o.DepositAccountId.Equals(pa.Id),
    JoinType.Inner,o.PaymentCategoryId.Equals(pc.Id),
    JoinType.Inner, o.FinanceCategoryId.Equals(f.Id)
    ))
    // .Where((p, cc, pc, pa) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((o, pa, pc, f) => new OtherFinanceDto()
    {
        Id = o.Id
    });
                var totalRecords = await queryable.CountAsync();
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                return (data, pageCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
