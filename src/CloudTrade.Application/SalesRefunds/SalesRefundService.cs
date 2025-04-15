using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesRefundItems;
using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesRefunds
{
    public class SalesRefundService : CommodityService, ISalesRefundService
    {
        private readonly ISqlSugarClient db;
        public SalesRefundService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> SalesRefundDeleteAsync(SalesRefund entity)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesRefund>(entity).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    var resulti = await db.Deleteable<SalesRefundItem>(x => x.SalesRefundId.Equals(entity.Id)).ExecuteCommandHasChangeAsync();
                    if (resulti)
                        return true;
                    else
                    {
                        await db.Ado.RollbackTranAsync();
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await db.Ado.RollbackTranAsync();
                throw ex;
            }
            finally
            {

                await db.Ado.CommitTranAsync();

            }
        }

        public async Task<(IEnumerable<SalesRefundDto>, int )> SalesRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<SalesRefund, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((s, c, pc, pa, w) => new JoinQueryInfos(
    JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
    JoinType.Inner, s.PaymentCategoryId.Equals(pc.Id),
    JoinType.Inner, s.PaymentAccountId.Equals(pa.Id),
    JoinType.Inner, s.WareHouseId.Equals(w.Id)
    ))
   // .Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((s, c, pc, pa, w) => new SalesRefundDto()
    {
        Id=s.Id
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

        public async Task<IEnumerable<SalesRefundItemDto>> SalesRefundViewAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<SalesRefundItem, Commodity>((p, c) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id)

                    )).Where((p, c) => p.SalesRefundId.Equals(Id))
                    .Select((p, c) => new SalesRefundItemDto()
                    {

                        Id = p.Id,
                        CommodityId = c.Id,

                        Remark = p.Remark,
                        Amount = p.Amount,

                        CommodityName = c.CommodityName,
                        Count = p.Count,
                        CreateTime = p.CreateTime,
                        LastUpdateTime = p.LastUpdateTime,

                        Price = p.Price,
                        SalesRefundId = p.SalesRefundId,
                        

                    }).ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
