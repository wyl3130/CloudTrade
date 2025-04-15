using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseRefunds;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PurchaseRefunds
{
    public class PurchaseRefundService : CommodityService, IPurchaseRefundService
    {
        private readonly ISqlSugarClient db;
        public PurchaseRefundService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> PurchaseRefundDeleteAsync(PurchaseRefund entity)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<PurchaseRefund>(entity).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    var resulti = await db.Deleteable<PurchaseRefundItem>(x => x.PurchaseRefundId.Equals(entity.Id)).ExecuteCommandHasChangeAsync();
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

        public async Task<(IEnumerable<PurchaseRefundDto>, int)> PurchaseRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<PurchaseRefund, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((p, c, pc, pa, w) => new JoinQueryInfos(
    JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
    JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
    JoinType.Inner, p.PaymentAccountId.Equals(pa.Id),
    JoinType.Inner, p.WareHouseId.Equals(w.Id)
    ))
    // .Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((p, c, pc, pa, w) => new PurchaseRefundDto()
    {
        Id = p.Id,

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

        public async Task<IEnumerable<PurchaseRefundItemDto>> PurchaseRefundViewAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<PurchaseRefundItem, Commodity>((p, c) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id)

                    )).Where((p, c) => p.PurchaseRefundId.Equals(Id))
                    .Select((p, c) => new PurchaseRefundItemDto()
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
                        PurchaseRefundId = p.PurchaseRefundId,


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
