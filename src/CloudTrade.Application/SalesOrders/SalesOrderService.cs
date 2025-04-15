using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.SalesOrderItems;
using CloudTrade.Domain.SalesOrders;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesOrders
{
    public class SalesOrderService : CommodityService, ISalesOrderService
    {
        private readonly ISqlSugarClient db;
        public SalesOrderService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> SalesOrderDeleteAsync(SalesOrder entity)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesOrder>(entity).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    var resulti = await db.Deleteable<SalesOrderItem>(x => x.SalesOrderId.Equals(entity.Id)).ExecuteCommandHasChangeAsync();
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

        public async Task<(IEnumerable<SalesOrderDto>, int)> SalesOrderQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<SalesOrder, WareHouse,CustomerCompany>((s,w,c) => new JoinQueryInfos(
    JoinType.Inner, s.WareHouseId.Equals(w.Id),
    JoinType.Inner,s.CustomerCompanyId.Equals(c.Id)
    ))
    //.Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((s, w, c) => new SalesOrderDto()
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

        public Task<(IEnumerable<SalesOrderDto>, int)> SalesOrderQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SalesOrderItemDto>> SalesOrderViewAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<SalesOrderItem, Commodity, ModeInfo>((p, c, m) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id),
                    JoinType.Left, p.ModeInfoId.Equals(m.Id)
                    )).Where((p, c, m) => p.SalesOrderId.Equals(Id))
                    .Select((p, c, m) => new SalesOrderItemDto()
                    {
                        Id = p.Id,
                        CommodityId = c.Id,
                        ModeInfoId = m.Id,
                        Rebate = p.Rebate,
                        Remark = p.Remark,
                        Amount = p.Amount,
                        TaxAmount = p.TaxAmount,
                        CommodityName = c.CommodityName,
                        Count = p.Count,
                        CreateTime = p.CreateTime,
                        LastUpdateTime = p.LastUpdateTime,
                        ModeInfoName = m.ModeInfoName,
                        Price = p.Price,
                        SalesOrderId = p.SalesOrderId,
                        Tax = p.Tax,
                        TaxPrice = p.TaxPrice

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
