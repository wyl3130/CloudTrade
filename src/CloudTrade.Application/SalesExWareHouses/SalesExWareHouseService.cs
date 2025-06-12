using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.SalesOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesModes;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesExWarehouses
{
    public class SalesExWareHouseService : SalesOrderService, ISalesExWareHouseService
    {
        private readonly ISqlSugarClient db;
        public SalesExWareHouseService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }


        public async Task<bool> SalesExWareHouseInsertAsync(SalesExWareHouse entity, IEnumerable<SalesExWareHouseItem> iList)
        {
            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<SalesExWareHouse>(entity).ExecuteCommandAsync();
                    await db.Insertable<SalesExWareHouseItem>(iList.ToList()).ExecuteCommandAsync();
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
                    // 开始事务
                    await db.Ado.BeginTranAsync();

                    // 插入 PurchaseWareHouse 实体
                    await db.Insertable<SalesExWareHouse>(entity).ExecuteCommandAsync();

                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<SalesExWareHouseItem>(iList.ToList()).ExecuteCommandAsync();

                    // 获取所有相关的库存商品
                    var stock = await db.Queryable<CommodityStock>()
                                         .Where(x => x.WareHouseId.Equals(entity.WareHouseId))
                                         .ToListAsync();

                    // 使用字典加速查找，避免嵌套循环
                    var stockDictionary = stock.ToDictionary(x => x.CommodityId);

                    // 遍历商品列表，更新库存
                    foreach (var pitem in iList)
                    {
                        // 检查字典中是否存在该商品
                        if (stockDictionary.TryGetValue(pitem.CommodityId, out var item))
                        {
                            // 更新库存数量
                            item.StockCount -= pitem.Count;
                            // 可选择更新销售数量 (如需要的话)
                             item.SalesCount += pitem.Count;

                            var result = await db.Updateable<CommodityStock>(item).ExecuteCommandAsync();

                            // 如果更新失败，回滚事务并返回 false
                            if (result == 0)
                            {
                                await db.Ado.RollbackTranAsync();
                                return false;
                            }
                        }
                        else
                        {
                            // 如果找不到对应的商品，回滚事务并返回 false
                            await db.Ado.RollbackTranAsync();
                            return false;
                        }
                    }

                    // 所有操作成功，提交事务
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
        public async Task<bool> SalesExWareHouseDeleteAsync(Guid Id)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesExWareHouse>(Id).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    await db.Deleteable<SalesExWareHouseItem>(x => x.SalesExWareHouseId.Equals(Id)).ExecuteCommandHasChangeAsync();
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
        public async Task<bool> SalesExWareHouseUpdateAsync(SalesExWareHouse entity, IEnumerable<SalesExWareHouseItem> iList)
        {

            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Updateable<SalesExWareHouse>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesExWareHouseItem>().Where(x => x.SalesExWareHouseId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<SalesExWareHouseItem>(iList.ToList()).ExecuteCommandAsync();
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
                    // 开始事务
                    await db.Ado.BeginTranAsync();

                    // 插入 PurchaseWareHouse 实体
                    await db.Updateable<SalesExWareHouse>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesExWareHouseItem>().Where(x => x.SalesExWareHouseId.Equals(entity.Id)).ExecuteCommandAsync();
                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<SalesExWareHouseItem>(iList.ToList()).ExecuteCommandAsync();

                    // 获取所有相关的库存商品
                    var stock = await db.Queryable<CommodityStock>()
                                         .Where(x => x.WareHouseId.Equals(entity.WareHouseId))
                                         .ToListAsync();

                    // 使用字典加速查找，避免嵌套循环
                    var stockDictionary = stock.ToDictionary(x => x.CommodityId);

                    // 遍历商品列表，更新库存
                    foreach (var pitem in iList)
                    {
                        // 检查字典中是否存在该商品
                        if (stockDictionary.TryGetValue(pitem.CommodityId, out var item))
                        {
                            // 更新库存数量
                            item.StockCount -= pitem.Count;
                            // 可选择更新销售数量 (如需要的话)
                             item.SalesCount += pitem.Count;

                            var result = await db.Updateable<CommodityStock>(item).ExecuteCommandAsync();

                            // 如果更新失败，回滚事务并返回 false
                            if (result == 0)
                            {
                                await db.Ado.RollbackTranAsync();
                                return false;
                            }
                        }
                        else
                        {
                            // 如果找不到对应的商品，回滚事务并返回 false
                            await db.Ado.RollbackTranAsync();
                            return false;
                        }
                    }

                    // 所有操作成功，提交事务
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




        public async Task<(IEnumerable<SalesExWareHouseDto>, int)> SalesExWarehouseQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;


                if (StartTime != "" && EndTime != "")
                {
                    var queryable = db.Queryable<SalesExWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse,SalesMode>((s, c, pc, pa, w,sm) => new JoinQueryInfos(
 JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
 JoinType.Inner, s.PaymentCategoryId.Equals(pc.Id),
 JoinType.Inner, s.PaymentAccountId.Equals(pa.Id),
 JoinType.Inner, s.WareHouseId.Equals(w.Id),
 JoinType.Left, s.SalesOrderId.Equals(sm.Id)
 ))
.Where((s, c, pc, pa, w, sm) => s.CodeNo.Contains(query) &&
Convert.ToDateTime(Convert.ToDateTime(s.CreateTime).ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Convert.ToDateTime(StartTime).ToString("yyyy/MM/dd")) &&
Convert.ToDateTime(Convert.ToDateTime(s.CreateTime).ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Convert.ToDateTime(EndTime).ToString("yyyy/MM/dd")))
 .Select((s, c, pc, pa, w, sm) => new SalesExWareHouseDto()
 {
     Id = s.Id,
     PaymentAccountId = pa.Id,
     PaymentAccountName = pa.PaymentAccountName,
     CodeNo = s.CodeNo,
     CreateTime = s.CreateTime,
     LastUpdateTime = s.LastUpdateTime,
     CustomerCompanyId = c.Id,
     CustomerCompanyName = c.CustomerCompanyName,
     PaymentCategoryId = pc.Id,
     ExWareHouseDate = s.ExWareHouseDate,
     OrderState = s.OrderState,
     PaymentCategoryName = pc.PaymentCategoryName,
     Preparer = s.Preparer,
     RealName = s.RealName,
     Remark = s.Remark,
     SalesModeId = sm.Id,
     SalesModeName=sm.ModeName,
     SalesOrderId = s.SalesOrderId,
     WareHouseId = w.Id,
     WareHouseName = w.WareHouseName,
     CurrentAmount = s.CurrentAmount,
 });
                    var totalRecords = await queryable.CountAsync();
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                    var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                    return (data, pageCount);
                }
                else
                {
                    var queryable = db.Queryable<SalesExWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse, SalesMode>((s, c, pc, pa, w,sm) => new JoinQueryInfos(
 JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
 JoinType.Inner, s.PaymentCategoryId.Equals(pc.Id),
 JoinType.Inner, s.PaymentAccountId.Equals(pa.Id),
 JoinType.Inner, s.WareHouseId.Equals(w.Id),
 JoinType.Left, s.SalesOrderId.Equals(sm.Id)
 ))
.Where((s, c, pc, pa, w,sm) => s.CodeNo.Contains(query))
 .Select((s, c, pc, pa, w,sm) => new SalesExWareHouseDto()
 {
     Id = s.Id,
     PaymentAccountId = pa.Id,
     PaymentAccountName = pa.PaymentAccountName,
     CodeNo = s.CodeNo,
     CreateTime = s.CreateTime,
     LastUpdateTime = s.LastUpdateTime,
     CustomerCompanyId = c.Id,
     CustomerCompanyName = c.CustomerCompanyName,
     PaymentCategoryId = pc.Id,
     ExWareHouseDate = s.ExWareHouseDate,
     OrderState = s.OrderState,
     PaymentCategoryName = pc.PaymentCategoryName,
     Preparer = s.Preparer,
     RealName = s.RealName,
     Remark = s.Remark,
     SalesModeId = sm.Id,
     SalesModeName = sm.ModeName,
     SalesOrderId = s.SalesOrderId,
     WareHouseId = w.Id,
     WareHouseName = w.WareHouseName,
     CurrentAmount = s.CurrentAmount,

 });
                    var totalRecords = await queryable.CountAsync();
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                    var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                    return (data, pageCount);
                }

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
