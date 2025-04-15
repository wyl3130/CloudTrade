using CloudTrade.Application.Commoditys;
using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Application.PurchaseOrders
{
    public class PurchaseOrderService : CommodityService, IPurchaseOrderService
    {
        private readonly ISqlSugarClient db;
        public PurchaseOrderService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        /// <summary>
        /// 购买订单查询
        /// </summary>
        /// <param name="query">查询关键字</param>
        /// <param name="PageIndex">页数</param>
        /// <param name="count">每页显示条数</param>
        /// <returns></returns>
        public async Task<(IEnumerable<PurchaseOrderDto>, int PageCount)> PurchaseOrderQueryAsync(int PageIndex=1,int PageSize=10,string query = "", string StartTime = "", string EndTime = "")
        {
            try
            {
                // 检查分页参数有效性
                if (PageIndex < 1) PageIndex = 1;
                if (PageSize <= 0) PageSize = 10;

                if (StartTime != "" && EndTime != "")
                {
                    var queryable = db.Queryable<PurchaseOrder, CustomerCompany, WareHouse>((p, c, w) => new JoinQueryInfos(
    JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
    JoinType.Inner, p.WareHouseId.Equals(w.Id)
))
.Where((p, c, w) => p.CodeNo.Contains(query) && 
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) >= Convert.ToDateTime(Convert.ToDateTime(StartTime).ToString("yyyy/MM/dd")) && 
Convert.ToDateTime(Convert.ToDateTime(p.CreateTime).ToString("yyyy/MM/dd")) <= Convert.ToDateTime(Convert.ToDateTime(EndTime).ToString("yyyy/MM/dd")))
 .Select((p, c, w) => new PurchaseOrderDto()
 {
     Id = p.Id,
     CodeNo = p.CodeNo,
     CustomerCompanyName = c.CustomerCompanyName,
     RealName = p.RealName,
     OrderState = p.OrderState,
     CreateTime = p.CreateTime,
     CustomerCompanyId = p.CustomerCompanyId,
     LastUpdateTime = p.LastUpdateTime,
     Preparer = p.Preparer,
     Remark = p.Remark,
     WareHouseDate = p.WareHouseDate,
     WareHouseId = p.WareHouseId,
     WareHouseName = w.WareHouseName
 });







                    // 获取总记录数
                    var totalRecords = await queryable.CountAsync();

                    // 计算页数
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);

                    // 获取当前页数据
                    var data = await queryable.Skip((PageIndex - 1) * PageSize)
                                               .Take(PageSize)
                                               .ToListAsync();

                    return (data, pageCount);
                }
                else
                {
                    var queryable = db.Queryable<PurchaseOrder, CustomerCompany, WareHouse>((p, c, w) => new JoinQueryInfos(
JoinType.Inner, p.CustomerCompanyId.Equals(c.Id),
JoinType.Inner, p.WareHouseId.Equals(w.Id)
))
.Where((p, c, w) => p.CodeNo.Contains(query))
.Select((p, c, w) => new PurchaseOrderDto()
{
Id = p.Id,
CodeNo = p.CodeNo,
CustomerCompanyName = c.CustomerCompanyName,
RealName = p.RealName,
OrderState = p.OrderState,
CreateTime = p.CreateTime,
CustomerCompanyId = p.CustomerCompanyId,
LastUpdateTime = p.LastUpdateTime,
Preparer = p.Preparer,
Remark = p.Remark,
WareHouseDate = p.WareHouseDate,
WareHouseId = p.WareHouseId,
WareHouseName = w.WareHouseName
});







                    // 获取总记录数
                    var totalRecords = await queryable.CountAsync();

                    // 计算页数
                    var pageCount = (int)Math.Ceiling(totalRecords / (double)PageSize);

                    // 获取当前页数据
                    var data = await queryable.Skip((PageIndex - 1) * PageSize)
                                               .Take(PageSize)
                                               .ToListAsync();

                    return (data, pageCount);
                }
            }
            catch (Exception ex)
            {
                // 这里可以记录日志，并抛出更具体的异常
                // 例如：Log.Error(ex, "购买订单查询异常");
                throw new ApplicationException("购买订单查询失败", ex);
            }
        }

        public async Task<IEnumerable<PurchaseOrderItemDto>> PurchaseOrderViewAsync(Guid Id)
        {
            if(Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(Id));
            }
            try
            {
                var list = await db.Queryable<PurchaseOrderItem, Commodity, ModeInfo>((p, c, m) => new JoinQueryInfos(
                    JoinType.Inner, p.CommodityId.Equals(c.Id),
                    JoinType.Left, p.ModeInfoId.Equals(m.Id)
                    )).Where((p, c, m) => p.PurchaseOrderId.Equals(Id))
                    .Select((p, c, m) => new PurchaseOrderItemDto()
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
                        PurchaseOrderId = p.PurchaseOrderId,
                        Tax=p.Tax,
                        TaxPrice = p.TaxPrice

                    }).ToListAsync();
 
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool>PurchaseOrderDeleteAsync(PurchaseOrder entity)
        {
            try
            {
                await db.Ado.BeginTranAsync();
                var result = await db.Deleteable<PurchaseOrder>(entity).ExecuteCommandHasChangeAsync();
                if(result)
                {
                    var resulti= await db.Deleteable<PurchaseOrderItem>(x=>x.PurchaseOrderId.Equals(entity.Id)).ExecuteCommandHasChangeAsync();
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
    }
}
