using CloudTrade.Application.Contracts.PaymentCategorys;
using CloudTrade.Domain.PaymentCategorys;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PaymentCategorys
{
    public class PaymentCategoryService: BaseService, IPaymentCategoryService
    {
        private readonly ISqlSugarClient db;
        public PaymentCategoryService(ISqlSugarClient db):base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<PaymentCategory>, int)> PaymentCategoryQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<PaymentCategory>()
.Where(x => x.PaymentCategoryName.Contains(query));
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

        //    async Task<(IEnumerable<PaymentCategory>, int PageCount)> IPaymentCategoryService.PaymentCategoryQueryAsync(string query, int PageIndex)
        //    {
        //        var queryable = db.Queryable<PaymentCategory>()
        //.Where(x => x.PaymentCategoryName.Contains(query));
        //        var totalRecords = await queryable.CountAsync();
        //        var pageCount = (int)Math.Ceiling(totalRecords / 10.0);
        //        var data = await queryable.Skip((PageIndex - 1) * 10).Take(10).ToListAsync();
        //        return (data, pageCount);
        //    }
    }
}
