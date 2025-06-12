using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.PurchaseWareHouses;
using CloudTrade.Application.PurchaseWarehouses;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PurchaseRefunds
{
    public class PurchaseRefundService : PurchaseWareHouseService, IPurchaseRefundService
    {
        private readonly ISqlSugarClient db;
        public PurchaseRefundService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }

        public async Task<bool> PurchaseRefundInsertAsync(PurchaseRefund entity, IEnumerable<PurchaseRefundItem> iList)
        {
            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<PurchaseRefund>(entity).ExecuteCommandAsync();
                    await db.Insertable<PurchaseRefundItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Insertable<PurchaseRefund>(entity).ExecuteCommandAsync();

                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<PurchaseRefundItem>(iList.ToList()).ExecuteCommandAsync();

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


        public async Task<bool> PurchaseRefundDeleteAsync(Guid Id)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<PurchaseRefund>(Id).ExecuteCommandHasChangeAsync();
                if (result)
                {
                    await db.Deleteable<PurchaseRefundItem>(x => x.PurchaseRefundId.Equals(Id)).ExecuteCommandAsync();
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
        public async Task<bool> PurchaseRefundUpdateAsync(PurchaseRefund entity, IEnumerable<PurchaseRefundItem> iList)
        {
            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Updateable<PurchaseRefund>(entity).ExecuteCommandAsync();
                    await db.Deleteable<PurchaseRefundItem>().Where(x => x.PurchaseRefundId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<PurchaseRefundItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Updateable<PurchaseRefund>(entity).ExecuteCommandAsync();
                    await db.Deleteable<PurchaseRefundItem>().Where(x => x.PurchaseRefundId.Equals(entity.Id)).ExecuteCommandAsync();
                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<PurchaseRefundItem>(iList.ToList()).ExecuteCommandAsync();

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
        public async Task<(IEnumerable<PurchaseRefundDto>, int)> PurchaseRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;
                if (StartTime != "" && EndTime != "")
                {
                    var queryable = db.Queryable<PurchaseRefund, PurchaseWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((p, pw, c, pc, pa, w) => new JoinQueryInfos(
                        JoinType.Left, p.PurchaseWareHouseId.Equals(pw.Id),
                        JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
                        JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
                        JoinType.Inner, p.PaymentAccountId.Equals(pa.Id),
                        JoinType.Inner, p.WareHouseId.Equals(w.Id)
  ))
     .Where((p, pw, c, pc, pa, w) => p.CodeNo.Contains(query) &&
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Convert.ToDateTime(StartTime).ToString("yyyy/MM/dd")) &&
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Convert.ToDateTime(EndTime).ToString("yyyy/MM/dd")))
  .Select((p, pw, c, pc, pa, w) => new PurchaseRefundDto()
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
      ExWareHouseDate = p.ExWareHouseDate,
      RealName = p.RealName,
      Remark = p.Remark,
      PurchaseWareHouseId = pw.Id,

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
                    var queryable = db.Queryable<PurchaseRefund, PurchaseWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((p, pw, c, pc, pa, w) => new JoinQueryInfos(
                        JoinType.Left, p.PurchaseWareHouseId.Equals(pw.Id),
                        JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
                        JoinType.Inner, p.PaymentCategoryId.Equals(pc.Id),
                        JoinType.Inner, p.PaymentAccountId.Equals(pa.Id),
                        JoinType.Inner, p.WareHouseId.Equals(w.Id)
                    ))
                    .Where((p, pw, c, pc, pa, w) => p.CodeNo.Contains(query))
                    .Select((p, pw, c, pc, pa, w) => new PurchaseRefundDto()
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
                        ExWareHouseDate = p.ExWareHouseDate,
                        RealName = p.RealName,
                        Remark = p.Remark,
                        PurchaseWareHouseId = pw.Id,
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
