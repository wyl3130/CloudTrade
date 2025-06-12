using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesModes;
using CloudTrade.Domain.SalesOrderItems;
using CloudTrade.Domain.SalesOrders;
using System;
using System.Collections;
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

        public async Task<bool> SalesOrderInsertAsync(SalesOrder entity, IEnumerable<SalesOrderItem> iList)
        {
            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<SalesOrder>(entity).ExecuteCommandAsync();
                    await db.Insertable<SalesOrderItem>(iList.ToList()).ExecuteCommandAsync();
                    await db.Ado.CommitTranAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await db.Ado.RollbackTranAsync();
                    throw ex;
                }
            }
            else
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<SalesOrder>(entity).ExecuteCommandAsync();
                    await db.Insertable<SalesOrderItem>(iList.ToList()).ExecuteCommandAsync();
                    await db.Ado.CommitTranAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await db.Ado.RollbackTranAsync();
                    throw ex;
                }


            }
        }

        public async Task<bool> SalesOrderUpdateAsync(SalesOrder entity, IEnumerable<SalesOrderItem> iList)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                if (entity.OrderState == 0)
                {
                    await db.Updateable<SalesOrder>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesOrderItem>().Where(x => x.SalesOrderId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<SalesOrderItem>(iList.ToList()).ExecuteCommandAsync();
                    return true;
                }
                else
                {
                    await db.Updateable<SalesOrder>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesOrderItem>().Where(x => x.SalesOrderId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<SalesOrderItem>(iList.ToList()).ExecuteCommandAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                await db.Ado.RollbackTranAsync();
                throw ex;
            }
            finally
            {
                // 所有操作成功，提交事务
                await db.Ado.CommitTranAsync();
            }
        }

        public async Task<bool> SalesOrderDeleteAsync(Guid Id)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesOrder>(Id).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    await db.Deleteable<SalesOrderItem>(x => x.SalesOrderId.Equals(Id)).ExecuteCommandHasChangeAsync();
                    await db.Ado.CommitTranAsync();
                    return true;


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

        public async Task<(IEnumerable<SalesOrderDto>, int)> SalesOrderQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                var queryable = db.Queryable<SalesOrder, WareHouse, CustomerCompany, SalesMode>((s, w, c, sm) => new JoinQueryInfos(
    JoinType.Inner, s.WareHouseId.Equals(w.Id),
    JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
    JoinType.Left, s.SalesModeId.Equals(sm.Id)
    ))
    //.Where((c, ca, co) => c.CommodityName.Contains(query) && ca.CommodityCategoryName.Contains(CommodityCategoryName) && co.CommodityCompanyName.Contains(CommodityCompanyName))
    .Select((s, w, c, sm) => new SalesOrderDto()
    {
        Id = s.Id,
        CodeNo = s.CodeNo,
        CreateTime = s.CreateTime,
        CustomerCompanyId = c.Id,
        CustomerCompanyName = c.CustomerCompanyName,
        ExWareHouseDate = s.ExWareHouseDate,
        LastUpdateTime = s.LastUpdateTime,
        OrderState = s.OrderState,
        Preparer = s.Preparer,
        RealName = s.RealName,
        Remark = s.Remark,
        SalesModeId = s.SalesModeId,
        WareHouseId = s.WareHouseId,
        WareHouseName = w.WareHouseName,
        SalesModeName = sm.ModeName

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


                //var queryable = await db.Queryable<CommodityStock, Commodity, WareHouse, SalesOrderItem, ModeInfo>((cs, c, w, s, m) => new JoinQueryInfos(
                //    JoinType.Inner, cs.CommodityId.Equals(c.Id),
                //    JoinType.Inner, w.Id.Equals(cs.WareHouseId),
                //    JoinType.Inner, s.CommodityId.Equals(c.Id),
                //    JoinType.Left, s.ModeInfoId.Equals(m.Id)
                //    ))
                //    .Where((cs, c, w, s, m) => s.SalesOrderId.Equals(Id))
                //    .Select((cs, c, w, s, m) => new SalesOrderItemDto()
                //    {
                //        Id = s.Id,
                //        Amount = s.Amount,
                //        TaxAmount = s.TaxAmount,
                //        CommodityName=c.CommodityName,
                //        CommodityId = c.Id,
                //        Count = s.Count,
                //        CreateTime=s.CreateTime,
                //        LastUpdateTime=s.LastUpdateTime,
                //        ModeInfoId= s.ModeInfoId,
                //        ModeInfoName = m.ModeInfoName,
                //        Price = s.Price,
                //        SalesOrderId = Id,
                //        Rebate = s.Rebate,
                //        Remark = s.Remark,
                //        StockCount = cs.StockCount,
                //        Tax=s.Tax,
                //        TaxPrice=s.TaxPrice


                //    }).ToListAsync();
                //return queryable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
