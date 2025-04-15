using CloudTrade.Application.Contracts.CustomerCompanys;
using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.Domain.CustomerCompanys;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.CustomerCompanys
{
    public class CustomerCompanyService : BaseService, ICustomerCompanyService
    {
        private readonly ISqlSugarClient db;
        public CustomerCompanyService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<(IEnumerable<CustomerCompanyDto>, int)> CustomerCompanyQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<CustomerCompany, CustomerCategory>((cc, ca) => new JoinQueryInfos(
    JoinType.Inner, cc.CustomerCategoryId.Equals(ca.Id)
    ))
    .Where((cc, ca) => cc.CustomerCompanyName.Contains(query))
    .Select((cc, ca) => new CustomerCompanyDto()
    {
        Id = cc.Id,
        CustomerCategoryId = ca.Id,
        CustomerCategoryName = ca.CustomerCategoryName,
        CustomerCompanyName = cc.CustomerCompanyName,
        Address = cc.Address,
        BankCardNo = cc.BankCardNo,
        BankName = cc.BankName,
        ContactName = cc.ContactName,
        CreateTime = cc.CreateTime,
        LastUpdateTime = cc.LastUpdateTime,
        Email = cc.Email,
        Fax = cc.Fax,
        Phone = cc.Phone,
        LegalRepresentative = cc.LegalRepresentative,
        //MobilePhone = cc.MobilePhone,
        PostalCode = cc.PostalCode,
        //Remark = cc.Remark,
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
