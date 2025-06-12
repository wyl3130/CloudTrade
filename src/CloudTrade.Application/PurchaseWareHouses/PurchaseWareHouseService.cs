using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.Contracts.SalesExWareHouses;
using CloudTrade.Application.PurchaseOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PurchaseWarehouses
{
    public class PurchaseWareHouseService : PurchaseOrderService, IPurchaseWareHouseService
    {
        private readonly ISqlSugarClient db;
        public PurchaseWareHouseService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public async Task<bool> PurchaseWareHouseInsertAsync(PurchaseWareHouse entity, IEnumerable<PurchaseWareHouseItem> iList)
        {

            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<PurchaseWareHouse>(entity).ExecuteCommandAsync();
                    await db.Insertable<PurchaseWareHouseItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Insertable<PurchaseWareHouse>(entity).ExecuteCommandAsync();

                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<PurchaseWareHouseItem>(iList.ToList()).ExecuteCommandAsync();

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
                            item.StockCount += pitem.Count;
                            // 可选择更新销售数量 (如需要的话)
                            // item.SalesCount += pitem.Count;

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




        public async Task<bool> PurchaseWareHouseDeleteAsync(Guid Id)
        {
            try
            {
                await db.Ado.BeginTranAsync();

                // 删除 PurchaseWareHouse 记录
                var result = await db.Deleteable<PurchaseWareHouse>(Id).ExecuteCommandHasChangeAsync();
                if (result)
                {

                    await db.Deleteable<PurchaseWareHouseItem>().Where(x => x.PurchaseWareHouseId.Equals(Id)).ExecuteCommandAsync();
                    await db.Ado.CommitTranAsync();
                    return true;
                }
                else
                {
                    // 如果主订单删除失败，直接返回 false
                    await db.Ado.RollbackTranAsync();
                    return false;

                }
            }
            catch (Exception ex)
            {
                // 捕获异常并回滚事务
                await db.Ado.RollbackTranAsync();
                throw new Exception("An error occurred during the deletion process.", ex);
            }
        }
        public async Task<bool> PurchaseWareHouseUpdateAsync(PurchaseWareHouse entity, IEnumerable<PurchaseWareHouseItem> iList)
        {

            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Updateable<PurchaseWareHouse>(entity).ExecuteCommandAsync();
                    await db.Deleteable<PurchaseWareHouseItem>().Where(x => x.PurchaseWareHouseId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<PurchaseWareHouseItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Updateable<PurchaseWareHouse>(entity).ExecuteCommandAsync();
                    await db.Deleteable<PurchaseWareHouseItem>().Where(x => x.PurchaseWareHouseId.Equals(entity.Id)).ExecuteCommandAsync();
                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<PurchaseWareHouseItem>(iList.ToList()).ExecuteCommandAsync();

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
                            item.StockCount += pitem.Count;
                            // 可选择更新销售数量 (如需要的话)
                            // item.SalesCount += pitem.Count;

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

        public async Task<(IEnumerable<PurchaseWareHouseDto>, int)> PurchaseWareHouseQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                if (StartTime != "" && EndTime != "")
                {
                    var queryable = db.Queryable<PurchaseWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse, PurchaseOrder>((p, c, pc, pa, w, po) => new JoinQueryInfos(
  JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
  JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
  JoinType.Inner, p.PaymentAccountId.Equals(pa.Id),
  JoinType.Inner, p.WareHouseId.Equals(w.Id),
  JoinType.Left, p.PurchaseOrderId.Equals(po.Id)
  ))
   .Where((p, c, pc, pa, w, po) => p.CodeNo.Contains(query) &&
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Convert.ToDateTime(StartTime).ToString("yyyy/MM/dd")) &&
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Convert.ToDateTime(EndTime).ToString("yyyy/MM/dd")))
  .Select((p, c, pc, pa, w, po) => new PurchaseWareHouseDto()
  {
      Id = p.Id,
      CurrentAmount = p.CurrentAmount,
      PaymentAccountId = pa.Id,
      PaymentAccountName = pa.PaymentAccountName,
      CodeNo = p.CodeNo,
      CustomerCompanyId = c.Id,
      CreateTime = p.CreateTime,
      LastUpdateTime = p.LastUpdateTime,
      CustomerCompanyName = c.CustomerCompanyName,
      OrderState = p.OrderState,
      PaymentCategoryId = pc.Id,
      PaymentCategoryName = pc.PaymentCategoryName,
      Preparer = p.Preparer,
      PurchaseOrderId = po.Id,
      RealName = p.RealName,
      Remark = p.Remark,
      WareHouseDate = p.WareHouseDate,

      WareHouseId = p.WareHouseId,
      WareHouseName = w.WareHouseName,


  });
                    var totalRecords = await queryable.CountAsync();
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                    var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                    return (data, pageCount);
                }
                else
                {
                    var queryable = db.Queryable<PurchaseWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse, PurchaseOrder>((p, c, pc, pa, w, po) => new JoinQueryInfos(
 JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
 JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
 JoinType.Inner, p.PaymentAccountId.Equals(pa.Id),
 JoinType.Inner, p.WareHouseId.Equals(w.Id),
 JoinType.Left, p.PurchaseOrderId.Equals(po.Id)
 ))
 .Where((p,c,pc,pa,w,po)=>p.CodeNo.Contains(query))
 .Select((p, c, pc, pa, w, po) => new PurchaseWareHouseDto()
 {
     Id = p.Id,
     CurrentAmount = p.CurrentAmount,
     PaymentAccountId = pa.Id,
     PaymentAccountName = pa.PaymentAccountName,
     CodeNo = p.CodeNo,
     CustomerCompanyId = c.Id,
     CreateTime = p.CreateTime,
     LastUpdateTime = p.LastUpdateTime,
     CustomerCompanyName = c.CustomerCompanyName,
     OrderState = p.OrderState,
     PaymentCategoryId = pc.Id,
     PaymentCategoryName = pc.PaymentCategoryName,
     Preparer = p.Preparer,
     PurchaseOrderId = po.Id,
     RealName = p.RealName,
     Remark = p.Remark,
     WareHouseDate = p.WareHouseDate,

     WareHouseId = p.WareHouseId,
     WareHouseName = w.WareHouseName,


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

        public async Task<IEnumerable<PurchaseWareHouseItemDto>> PurchaseWareHouseViewAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<PurchaseWareHouseItem, Commodity, ModeInfo>((p, c, m) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id),
                    JoinType.Left, p.ModeInfoId.Equals(m.Id)
                    )).Where((p, c, m) => p.PurchaseWareHouseId.Equals(Id))
                    .Select((p, c, m) => new PurchaseWareHouseItemDto()
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
                        PurchaseWareHouseId = Id,

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
