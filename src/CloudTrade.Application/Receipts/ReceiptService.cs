using CloudTrade.Application.Contracts.Receipts;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.Receipts;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Receipts
{
    public class ReceiptService : BaseService, IReceiptService
    {
        private readonly ISqlSugarClient db;
        public ReceiptService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<(IEnumerable<ReceiptDto>, int)> ReceiptQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CustomerCompanyName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Receipt, CustomerCompany, PaymentCategory, PaymentAccount>((r, cc, pc, pa) => new JoinQueryInfos(
                        JoinType.Inner, r.CustomerCompanyId.Equals(cc.Id),
                        JoinType.Inner, r.PaymentCategoryId.Equals(pc.Id),
                        JoinType.Inner, r.PaymentAccountId.Equals(pa.Id)
                        ))
                    //.Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
                    .Select((r, cc, pc, pa) => new ReceiptDto()
                    {
                        Id = r.Id,
                        CreateTime = r.CreateTime,
                        LastUpdateTime = r.LastUpdateTime,
                        CustomerCompanyId = r.CustomerCompanyId,
                        CustomerCompanyName = cc.CustomerCompanyName,
                        PaymentCategoryId = pc.Id,
                        PaymentCategoryName = pc.PaymentCategoryName,
                        PaymentAccountId = pa.Id,
                        PaymentAccountName = pa.PaymentAccountName,
                        CurrentAmount = r.CurrentAmount,
                        IsAdvance = r.IsAdvance,
                        BillNo = r.BillNo,
                        CodeNo = r.CodeNo,
                        Preparer = r.Preparer,
                        PurchaseRefundId = r.PurchaseRefundId,
                        RealName = r.RealName,
                        ReceiptDate = r.ReceiptDate,
                        ReceiptTypes= r.ReceiptTypes,
                        Remark= r.Remark,
                        SalesExWarehouseId=r.SalesExWarehouseId

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
