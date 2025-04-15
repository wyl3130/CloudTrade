


namespace CloudTrade.Application.Users
{
    public class UserService: BaseService, IUserService
    {
        private readonly ISqlSugarClient db;
        public UserService(ISqlSugarClient db):base(db)
        {
            this.db = db;
        }

        public async Task<bool> LoginAsync(string UserName,string PassWord)
        {
            try
            {
                var list = await db.Queryable<User>().ToListAsync();
                var result = list.FirstOrDefault(x => x.UserName.Equals(UserName) && x.PassWord.Equals(PassWord));
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw new Exception("模型是空的");
            }


        }
    }
}
