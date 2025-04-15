using CloudTrade.Application.Contracts.Payments;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.Payments
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly ISqlSugarClient db;
        public PaymentService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<(IEnumerable<PaymentDto>, int)> PaymentQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string CustomerCompanyName = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<Payment, CustomerCompany, PaymentCategory, PaymentAccount>((p, cc, pc, pa) => new JoinQueryInfos(
    JoinType.Inner,p.CustomerCompanyId.Equals(cc.Id),
    JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
    JoinType.Inner, p.PaymentAccountId.Equals(pa.Id)
    ))
    //.Where((p, cc, pc, pa) =>p.CodeNo)
    .Select((p, cc, pc, pa) => new PaymentDto()
    {
        Id=p.Id,
        CreateTime = p.CreateTime,
        LastUpdateTime = p.LastUpdateTime,
        CustomerCompanyId = p.CustomerCompanyId,
        CustomerCompanyName = cc.CustomerCompanyName,
        PaymentCategoryId = pc.Id,
        PaymentCategoryName=pc.PaymentCategoryName,
        PaymentAccountId = pa.Id,
        PaymentAccountName = pa.PaymentAccountName,
        RealName = p.RealName,
        Preparer = p.Preparer,
        CurrentAmount = p.CurrentAmount,
        Remark = p.Remark,
        PaymentDate = p.PaymentDate,
        PaymentTypes = p.PaymentTypes,
        BillNo = p.BillNo,
        IsAdvance = p.IsAdvance,
        CodeNo = p.CodeNo,
        PurchaseWarehouseId = p.PurchaseWarehouseId,
        SalesRefundId = p.SalesRefundId,

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
