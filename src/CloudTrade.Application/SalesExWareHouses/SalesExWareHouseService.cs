using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesExWarehouses
{
    public class SalesExWareHouseService : CommodityService, ISalesExWareHouseService
    {
        private readonly ISqlSugarClient db;
        public SalesExWareHouseService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> SalesExWareHouseDeleteAsync(SalesExWareHouse entity)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesExWareHouse>(entity).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    var resulti = await db.Deleteable<SalesExWareHouseItem>(x => x.SalesExWareHouseId.Equals(entity.Id)).ExecuteCommandHasChangeAsync();
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

        public async Task<(IEnumerable<SalesExWareHouseDto>, int)> SalesExWarehouseQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<SalesExWareHouse,CustomerCompany,PaymentCategory,PaymentAccount,WareHouse>((s,c,pc,pa,w) => new JoinQueryInfos(
    JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
    JoinType.Inner,s.PaymentCategoryId.Equals(pc.Id),
    JoinType.Inner,s.PaymentAccountId.Equals(pa.Id),
    JoinType.Inner,s.WareHouseId.Equals(w.Id)
    ))
  //  .Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((s, c, pc, pa,w) => new SalesExWareHouseDto()
    {
        Id = s.Id,

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

        public async Task<IEnumerable<SalesExWareHouseItemDto>> SalesExWareHouseViewAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<SalesExWareHouseItem, Commodity, ModeInfo>((p, c, m) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id),
                    JoinType.Left, p.ModeInfoId.Equals(m.Id)
                    )).Where((p, c, m) => p.SalesExWareHouseId.Equals(Id))
                    .Select((p, c, m) => new SalesExWareHouseItemDto()
                    {
                        Id = p.Id,
                        CommodityId = c.Id,
                        ModeInfoId = m.Id,
                        Rebate = p.Rebate,
                        Remark = p.Remark,
                        Amount = p.Amount,
                    
                        CommodityName = c.CommodityName,
                        Count = p.Count,
                        CreateTime = p.CreateTime,
                        LastUpdateTime = p.LastUpdateTime,
                        ModeInfoName = m.ModeInfoName,
                        Price = p.Price,
      SalesExWareHouseId = p.SalesExWareHouseId,

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
