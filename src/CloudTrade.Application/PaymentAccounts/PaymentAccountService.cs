﻿using CloudTrade.Application.Contracts.PaymentAccounts;
using CloudTrade.Domain.PaymentAccounts;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PaymentAccounts
{
    public class PaymentAccountService: BaseService, IPaymentAccountService
    {
        private readonly ISqlSugarClient db;
        public PaymentAccountService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<PaymentAccount>, int)> PaymentAccountQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<PaymentAccount>()
.Where(x => x.PaymentAccountName.Contains(query));
                var totalRecords = await queryable.CountAsync();
                var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                return (data, pageCount);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
