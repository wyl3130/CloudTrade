namespace CloudTrade.Application.Menus
{
    public class MenuService : BaseService, IMenuService
    {
        private readonly ISqlSugarClient db;
        public MenuService(ISqlSugarClient db) : base(db)
        {
            this.db = db;
        }
        public IEnumerable<Menu> SelectMenuList()
        {
            try
            {
                var list = db.Queryable<Menu>().ToList();
                foreach (var item in list)
                {
                    if (!string.IsNullOrEmpty(item.Icon))
                    {
                        item.Icon = "u" + item.Icon;
                        item.Icon = "\\" + item.Icon;
                    }
                }

                // user  用户表
                // role  角色表
                // menu  菜单表
                // permission 权限表

                //user role 用户角色表
                //role menu 角色菜单表
                //menu permission   菜单权限表

                //role menu permission 角色菜单权限表

                // user < - > user role < - > role < - > role menu permission < - > role menu < - > menu < - > menu permission < - > permission

                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
