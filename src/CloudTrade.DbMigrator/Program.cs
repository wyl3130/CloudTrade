using CloudTrade.Domain.CommodityCategorys;
using CloudTrade.Domain.CommodityCompanys;
using CloudTrade.Domain.Commoditys;
using CloudTrade.Domain.CommodityStocks;
using CloudTrade.Domain.CompanyInfos;
using CloudTrade.Domain.CustomerCategorys;
using CloudTrade.Domain.CustomerCompanys;
using CloudTrade.Domain.Departments;
using CloudTrade.Domain.DepositWithdrawals;
using CloudTrade.Domain.Employees;
using CloudTrade.Domain.ExWareHouseReceiveItems;
using CloudTrade.Domain.ExWareHouseReceives;
using CloudTrade.Domain.FinanceCategorys;
using CloudTrade.Domain.LoginRecords;
using CloudTrade.Domain.MenuPermissions;
using CloudTrade.Domain.Menus;
using CloudTrade.Domain.ModeInfos;
using CloudTrade.Domain.OtherExWareHouseItems;
using CloudTrade.Domain.OtherExWareHouses;
using CloudTrade.Domain.OtherFinances;
using CloudTrade.Domain.OtherWareHouseItems;
using CloudTrade.Domain.OtherWareHouses;
using CloudTrade.Domain.PaymentAccounts;
using CloudTrade.Domain.PaymentCategorys;
using CloudTrade.Domain.Payments;
using CloudTrade.Domain.Permissions;
using CloudTrade.Domain.Positions;
using CloudTrade.Domain.PurchaseOrderItems;
using CloudTrade.Domain.PurchaseOrders;
using CloudTrade.Domain.PurchaseRefundItems;
using CloudTrade.Domain.PurchaseRefunds;
using CloudTrade.Domain.PurchaseWareHouse;
using CloudTrade.Domain.PurchaseWareHouseItems;
using CloudTrade.Domain.Receipts;
using CloudTrade.Domain.RolePermissions;
using CloudTrade.Domain.Roles;
using CloudTrade.Domain.SalesExWareHouseItem;
using CloudTrade.Domain.SalesExWareHouses;
using CloudTrade.Domain.SalesModes;
using CloudTrade.Domain.SalesOrderItems;
using CloudTrade.Domain.SalesOrders;
using CloudTrade.Domain.SalesRefundItems;
using CloudTrade.Domain.SalesRefunds;
using CloudTrade.Domain.SystemInfos;
using CloudTrade.Domain.UserRoles;
using CloudTrade.Domain.Users;
using CloudTrade.Domain.WareHouseAllocationItems;
using CloudTrade.Domain.WareHouseAllocations;
using CloudTrade.Domain.WareHouseReceiveItems;
using CloudTrade.Domain.WareHouseReceives;
using CloudTrade.Domain.WareHouses;
using CloudTrade.Domain.WareHouseStocktakingItems;
using CloudTrade.Domain.WareHouseStocktakings;
using SqlSugar;

try
{
    Console.WriteLine("正在初始化数据库");
    SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.SqlServer,
        ConnectionString = "Server=154.201.64.195;Database=CloudTrade;uid=sa;pwd=wuyule123.;TrustServerCertificate=True;Encrypt=True;TrustServerCertificate=True;",
        IsAutoCloseConnection = true,
        InitKeyType = InitKeyType.Attribute
    })
    {

    };

    var result = db.DbMaintenance.CreateDatabase();
    if (result)
        Console.WriteLine("数据库已存在");
    else
        Console.WriteLine("数据库不存在，创建数据库");

    db.CodeFirst.InitTables(typeof(User));
    db.CodeFirst.InitTables(typeof(Menu));
    db.CodeFirst.InitTables(typeof(Role));
    db.CodeFirst.InitTables(typeof(Permission));
    db.CodeFirst.InitTables(typeof(UserRole));
    db.CodeFirst.InitTables(typeof(RolePermission));
    db.CodeFirst.InitTables(typeof(MenuPermission));


    db.CodeFirst.InitTables<CommodityCategory>();
    db.CodeFirst.InitTables<CommodityCompany>();
    db.CodeFirst.InitTables<Commodity>();
    db.CodeFirst.InitTables<CommodityStock>();
    db.CodeFirst.InitTables<CompanyInfo>();
    db.CodeFirst.InitTables<CustomerCategory>();
    db.CodeFirst.InitTables<CustomerCompany>();
    db.CodeFirst.InitTables<Department>();
    db.CodeFirst.InitTables<DepositWithdrawal>();
    db.CodeFirst.InitTables<Employe>();

    db.CodeFirst.InitTables<ExWareHouseReceive>();
    db.CodeFirst.InitTables<ExWareHouseReceiveItem>();
    db.CodeFirst.InitTables<FinanceCategory>();
    db.CodeFirst.InitTables<LoginRecord>();
    db.CodeFirst.InitTables<MenuPermission>();
    db.CodeFirst.InitTables<Menu>();
    db.CodeFirst.InitTables<ModeInfo>();
    db.CodeFirst.InitTables<OtherExWareHouse>();
    db.CodeFirst.InitTables<OtherExWareHouseItem>();
    db.CodeFirst.InitTables<OtherFinance>();

    db.CodeFirst.InitTables<OtherWareHouseItem>();
    db.CodeFirst.InitTables<OtherWareHouse>();
    db.CodeFirst.InitTables<PaymentAccount>();
    db.CodeFirst.InitTables<PaymentCategory>();
    db.CodeFirst.InitTables<Payment>();
    db.CodeFirst.InitTables<Permission>();
    db.CodeFirst.InitTables<Position>();
    db.CodeFirst.InitTables<PurchaseOrder>();
    db.CodeFirst.InitTables<PurchaseOrderItem>();
    db.CodeFirst.InitTables<PurchaseRefund>();

    db.CodeFirst.InitTables<PurchaseRefundItem>();
    db.CodeFirst.InitTables<PurchaseWareHouse>();
    db.CodeFirst.InitTables<PurchaseWareHouseItem>();
    db.CodeFirst.InitTables<Receipt>();
    db.CodeFirst.InitTables<RolePermission>();
    db.CodeFirst.InitTables<Role>();
    db.CodeFirst.InitTables<SalesExWareHouse>();
    db.CodeFirst.InitTables<SalesExWareHouseItem>();
    db.CodeFirst.InitTables<SalesMode>();
    db.CodeFirst.InitTables<SalesOrder>();

    db.CodeFirst.InitTables<SalesOrderItem>();
    db.CodeFirst.InitTables<SalesRefund>();
    db.CodeFirst.InitTables<SalesRefundItem>(); ;
    db.CodeFirst.InitTables<SystemInfo>();
    db.CodeFirst.InitTables<UserRole>();
    db.CodeFirst.InitTables<User>();
    db.CodeFirst.InitTables<WareHouseAllocation>();
    db.CodeFirst.InitTables<WareHouseAllocationItem>();
    db.CodeFirst.InitTables<WareHouseReceive>();
    db.CodeFirst.InitTables<WareHouseReceiveItem>();

    db.CodeFirst.InitTables<WareHouse>();
    db.CodeFirst.InitTables<WareHouseStocktaking>();
    db.CodeFirst.InitTables<WareHouseStocktakingItem>();


    //Console.WriteLine("创建用户表");
    //var count = db.Queryable<User>().Count();
    //if (count == 0)
    //{
    //    var entity = new User()
    //    {
    //        Id = Guid.NewGuid(),
    //        UserName = "admin",
    //        PassWord = "admin",
    //        State = 0,
    //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    //        LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
    //    };
    //    await db.Insertable<User>(entity).ExecuteCommandAsync();
    //    Console.WriteLine("初始化管理员账号，插入一条数据，用户名：admin 密码：admin");
    //}

    //Console.WriteLine("创建菜单表");
    //if (db.Queryable<Menu>().Count() == 0)
    //{
    //    //db.MappingColumns("MenuIcon", v => string.IsNullOrEmpty(v) ? null : ((int)v.ToCharArray()[0]).ToString("x"),
    //    //  v => v == null ? "" : ((char)int.Parse(v, NumberStyles.HexNumber)).ToString());
    //    Console.WriteLine("初始化菜单数据");
    //    //主菜单
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "首页",MenuName="Home", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 0, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e2"), DisplayName = "销售管理", MenuName = "SalesManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 1, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e3"), DisplayName = "采购管理", MenuName = "ProcurementManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 2, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e4"), DisplayName = "人事管理", MenuName = "PersonnelManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 3, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e5"), DisplayName = "财务管理", MenuName = "FinancialManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 4, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), DisplayName = "综合管理", MenuName = "IntegratedManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 5, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), DisplayName = "库存管理", MenuName = "StockManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 6, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e8"), DisplayName = "货品管理", MenuName = "CommodityManagement", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 7, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), DisplayName = "数据查询", MenuName = "DataQuery", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 8, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c910"), DisplayName = "统计报表", MenuName = "Reports", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 9, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c911"), DisplayName = "系统信息", MenuName = "SystemInfo", ParentId = new Guid("00000000-0000-0000-0000-000000000000"), Icon = "f015", Sort = 10, TargetView = "", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    //子菜单
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("1b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "仪表盘", MenuName = "Dashboard", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), Icon = "", Sort = 0, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("2b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "销售订单", MenuName = "SalesOrder", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e2"), Icon = "", Sort = 0, TargetView = "SalesOrderView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("3b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "销售出库", MenuName = "SalesExWarehouse", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e2"), Icon = "", Sort = 1, TargetView = "SalesExWarehouseView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("4b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "销售退货", MenuName = "SalesRefund", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e2"), Icon = "", Sort = 2, TargetView = "SalesRefundView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("5b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "采购订单", MenuName = "PurchaseOrder", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e3"), Icon = "", Sort = 0, TargetView = "PurchaseOrderView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("6b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "采购入库", MenuName = "PurchaseRefund", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e3"), Icon = "", Sort = 1, TargetView = "PurchaseRefundView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("7b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "采购退货", MenuName = "PurchaseWarehouse", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e3"), Icon = "", Sort = 2, TargetView = "PurchaseWarehouseView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("8b5d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "部门管理", MenuName = "Department", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e4"), Icon = "", Sort = 0, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("915d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "职位管理", MenuName = "Position", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e4"), Icon = "", Sort = 1, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("105d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "职工管理", MenuName = "Employees", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e4"), Icon = "", Sort = 2, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("115d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收款单", MenuName = "Receipt", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e5"), Icon = "", Sort = 0, TargetView = "ReceiptView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("125d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "付款单", MenuName = "Payments", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e5"), Icon = "", Sort = 1, TargetView = "PaymentsView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("135d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "其他收入", MenuName = "OtherFinance", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e5"), Icon = "", Sort = 2, TargetView = "OtherFinanceView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("145d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "其他支出", MenuName = "OtherFinance", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e5"), Icon = "", Sort = 3, TargetView = "OtherFinanceView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("155d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付方式", MenuName = "PaymentCategory", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 0, TargetView = "PaymentCategoryView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("165d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付账号", MenuName = "PaymentAccount", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 1, TargetView = "PaymentAccountView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("175d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付项目", MenuName = "OtherFinance", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 2, TargetView = "OtherFinanceView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    //await db.Insertable<Menu>(new Menu() { Id = new Guid("175d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付项目类别", MenuName = "FinanceCategory", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 2, TargetView = "FinanceCategoryView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("185d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "单位类别", MenuName = "CustomerCategory", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 4, TargetView = "CustomerCategoryView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("195d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "往来单位", MenuName = "CustomerCompany", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 5, TargetView = "CustomerCompanyView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("205d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "货品销售方式", MenuName = "ModeInfo", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort =7, TargetView = "ModeInfoView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("215d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "销售方式", MenuName = "SalesMode", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e6"), Icon = "", Sort = 6, TargetView = "SalesModeView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("225d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "仓库信息", MenuName = "WareHouse", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 0, TargetView = "WareHouseView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("235d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "领用仓库", MenuName = "ExWareHouseReceive", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 1, TargetView = "ExWareHouseReceiveView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("245d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "领用退库", MenuName = "WareHouseReceive", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 2, TargetView = "WareHouseReceiveView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("255d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "其他入库", MenuName = "OtherWareHouse", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 4, TargetView = "OtherWareHouseView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("265d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "其他出库", MenuName = "OtherExWareHouse", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 5, TargetView = "OtherExWareHouseView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("275d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "仓库调拨", MenuName = "WareHouseAllocation", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 6, TargetView = "WareHouseAllocationView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("285d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "仓库盘点", MenuName = "WareHouseStocktaking", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e7"), Icon = "", Sort = 7, TargetView = "WareHouseStocktakingView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("295d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "货品类别", MenuName = "CommodityCategory", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e8"), Icon = "", Sort = 0, TargetView = "CommodityCategoryView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("305d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "计量单位", MenuName = "CommodityCompany", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e8"), Icon = "", Sort = 1, TargetView = "CommodityCompanyView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("315d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "货品资料", MenuName = "Commodity", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e8"), Icon = "", Sort = 2, TargetView = "CommodityView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("325d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "库存查询", MenuName = "CommodityStock", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 0, TargetView = "CommodityStockView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("335d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "库存警报", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 1, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("345d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "单据查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 2, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("355d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "单据明细查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 4, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("365d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "账号余额查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 5, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("375d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "账号余额明细查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 6, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("385d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付款查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 7, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("395d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "收付款明细查询", MenuName = "Home", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c9e9"), Icon = "", Sort = 7, TargetView = "DashboardView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("405d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "系统信息", MenuName = "SystemInfo", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c911"), Icon = "", Sort = 1, TargetView = "SystemInfoView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();
    //    await db.Insertable<Menu>(new Menu() { Id = new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c9e1"), DisplayName = "公司相关", MenuName = "CompanyInfo", ParentId = new Guid("9b5d3e77-3b3c-4d56-9a5d-31f04be3c911"), Icon = "", Sort = 0, TargetView = "CompanyInfoView", CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }).ExecuteCommandAsync();

    //    Console.WriteLine("初始化菜单数据完成");


    //}




    //db.Insertable<CommodityCategory>(new CommodityCategory() {Id=new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c911"), CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ,CommodityCategoryName="类别1",Remark="没有备注" }).ExecuteCommand();
    //db.Insertable<CommodityCompany>(new CommodityCompany() { Id = new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c921"), CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommodityCompanyName = "单位1", Remark = "没有备注" }).ExecuteCommand();
    //db.Insertable<Commodity>(new Commodity() { Id = Guid.NewGuid(), CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommodityName = "AK47", CommodityCategoryId=new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c911"),CommodityCompanyId=new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c921"),Sort=0,BarCode="xxx",CodeNo="xxx",MaxStockCount=100,MinStockCount=10,Ppecification="7.62MM",PurchasePrice=10,Price=100, Remark = "卡拉什尼科夫" }).ExecuteCommand();
    //var productNames = new[] { "AK47", "M4A1", "MP5", "伍兹冲锋枪", "M1911", "步枪", "沙漠之鹰", "光明枪", "雷明顿", "斯特林", "Glock 17", "狙击步枪", "狙击枪", "火箭炮", "加特林机枪", "卡宾枪", "G36", "P90", "Uzi", "巴雷特", "M249", "RPG", "FN SCAR", "M870", "AK74", "M110", "VSS", "SVD", "MP7", "TAR-21", "VZ58", "PP-19" };

    //for (int i = 0; i < 30; i++)
    //{
    //    var commodity = new Commodity()
    //    {
    //        Id = Guid.NewGuid(),
    //        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    //        LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
    //        CommodityName = productNames[i],  // 商品名称从数组中动态获取
    //        CommodityCategoryId = new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c911"),  // 示例类别 ID
    //        CommodityCompanyId = new Guid("415d3e77-3b3c-4d56-9a5d-31f04be3c921"),  // 示例公司 ID
    //        Sort = i,  // 根据索引设置排序
    //        BarCode = $"barcode_{i + 1}",  // 条形码动态生成
    //        CodeNo = $"code_{i + 1}",  // 编号动态生成
    //        MaxStockCount = 100,  // 最大库存
    //        MinStockCount = 10,  // 最小库存
    //        Ppecification = "7.62MM",  // 规格
    //        PurchasePrice = 10 + i,  // 采购价格动态生成
    //        Price = 100 + i,  // 销售价格动态生成
    //        Remark = $"{productNames[i]}"  // 商品备注动态生成
    //    };

    //    // 执行插入操作
    //    db.Insertable(commodity).ExecuteCommand();


    //}
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}