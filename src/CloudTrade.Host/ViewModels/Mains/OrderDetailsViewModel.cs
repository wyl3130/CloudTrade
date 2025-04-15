using CloudTrade.Application.Contracts.PurchaseOrders;
using CloudTrade.Domain.PurchaseOrders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudTrade.Host.ViewModels.Mains
{
    public class OrderDetailsViewModel:BindableBase
    {
        public OrderDetailsViewModel()
        {

        }

        public string Title { get; set; } = "订单详情";
        
        public PurchaseOrderDto PurchaseOrder { get; set; }
   
    }
}

public interface iorder
{
    
}
public class Order1 : iorder
{

}
public class Order2 : iorder
{

}