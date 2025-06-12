using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseRefunds;
using CloudTrade.Application.Contracts.SalesRefunds;
using CloudTrade.Application.SalesExWarehouses;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesRefundItems;
using CloudTrade.Domain.SalesRefunds;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.SalesRefunds
{
    public class SalesRefundService : SalesExWareHouseService, ISalesRefundService
    {
        private readonly ISqlSugarClient db;
        public SalesRefundService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }



        public async Task<bool> SalesRefundInsertAsync(SalesRefund entity, IEnumerable<SalesRefundItem> iList)
        {
            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Insertable<SalesRefund>(entity).ExecuteCommandAsync();
                    await db.Insertable<SalesRefundItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Insertable<SalesRefund>(entity).ExecuteCommandAsync();
                    await db.Insertable<SalesRefundItem>(iList.ToList()).ExecuteCommandAsync();
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
            
                            item.StockCount += pitem.Count;
                             item.SalesCount -= pitem.Count;

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

        public async Task<bool> SalesRefundDeleteAsync(Guid Id)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<SalesRefund>(Id).ExecuteCommandHasChangeAsync();
                if (result)
                {
                     await db.Deleteable<SalesRefundItem>(x => x.SalesRefundId.Equals(Id)).ExecuteCommandHasChangeAsync();
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
        public async Task<bool> SalesRefundUpdateAsync(SalesRefund entity, IEnumerable<SalesRefundItem> iList)
        {

            if (entity.OrderState == 0)
            {
                try
                {
                    await db.Ado.BeginTranAsync();
                    await db.Updateable<SalesRefund>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesRefundItem>().Where(x => x.SalesRefundId.Equals(entity.Id)).ExecuteCommandAsync();
                    await db.Insertable<SalesRefundItem>(iList.ToList()).ExecuteCommandAsync();
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
                    await db.Updateable<SalesRefund>(entity).ExecuteCommandAsync();
                    await db.Deleteable<SalesRefundItem>().Where(x => x.SalesRefundId.Equals(entity.Id)).ExecuteCommandAsync();
                    // 插入 PurchaseWareHouseItem 列表
                    await db.Insertable<SalesRefundItem>(iList.ToList()).ExecuteCommandAsync();

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
                            item.StockCount += pitem.Count;
                            item.SalesCount -= pitem.Count;

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

        public async Task<(IEnumerable<SalesRefundDto>, int )> SalesRefundQueryAsync(int PageIndex = 1, int PageSize = 10, string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize < 1) PageSize = 10;

                if (StartTime != "" && EndTime != "")
                {
                    var queryable = db.Queryable<SalesRefund, SalesExWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((s, se,c, pc, pa, w) => new JoinQueryInfos(
                             JoinType.Left, s.SalesExWareHouseId.Equals(se.Id),
JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
JoinType.Inner, s.PaymentCategoryId.Equals(pc.Id),
JoinType.Inner, s.PaymentAccountId.Equals(pa.Id),
JoinType.Inner, s.WareHouseId.Equals(w.Id)
))
.Where((s, se,c, pc, pa, w) => s.CodeNo.Contains(query) &&
Convert.ToDateTime(Convert.ToDateTime(s.CreateTime).ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Convert.ToDateTime(StartTime).ToString("yyyy/MM/dd")) &&
Convert.ToDateTime(Convert.ToDateTime(s.CreateTime).ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Convert.ToDateTime(EndTime).ToString("yyyy/MM/dd")))
.Select((s,se, c, pc, pa, w) => new SalesRefundDto()
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
    WareHouseDate = s.WareHouseDate,
    OrderState = s.OrderState,
    PaymentCategoryName = pc.PaymentCategoryName,
    Preparer = s.Preparer,
    RealName = s.RealName,
    Remark = s.Remark,
    CurrentAmount=s.CurrentAmount,
    SalesExWareHouseId=se.Id,
    WareHouseId = w.Id,

    WareHouseName = w.WareHouseName,
});
                    var totalRecords = await queryable.CountAsync();
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);
                    var data = await queryable.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToListAsync();
                    return (data, pageCount);
                }
                else
                {
                    var queryable = db.Queryable<SalesRefund,SalesExWareHouse, CustomerCompany, PaymentCategory, PaymentAccount, WareHouse>((s, se,c, pc, pa, w) => new JoinQueryInfos(
                        JoinType.Left , s.SalesExWareHouseId.Equals(se.Id),
JoinType.Inner, s.CustomerCompanyId.Equals(c.Id),
JoinType.Inner, s.PaymentCategoryId.Equals(pc.Id),
JoinType.Inner, s.PaymentAccountId.Equals(pa.Id),
JoinType.Inner, s.WareHouseId.Equals(w.Id)
))
.Where((s, se,c, pc, pa, w) =>s.CodeNo.Contains(query))
.Select((s,se, c, pc, pa, w) => new SalesRefundDto()
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
    WareHouseDate = s.WareHouseDate,
    OrderState = s.OrderState,
    PaymentCategoryName = pc.PaymentCategoryName,
    Preparer = s.Preparer,
    RealName = s.RealName,
    Remark = s.Remark,
    CurrentAmount = s.CurrentAmount,
    SalesExWareHouseId = se.Id,
    WareHouseId = w.Id,
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
