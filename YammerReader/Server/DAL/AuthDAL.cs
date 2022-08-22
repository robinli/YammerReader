using Dapper;
using System.Data;
using System.Data.Common;
using YammerReader.Shared;

namespace YammerReader.Server.DAL
{
    public class AuthDAL : BaseDAL
    {

        public async Task<AppUserDto?> ValidateLogin(LoginDto login)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select * from dbo.APPUSER where LOGIN_ID=@LOGIN_ID and USER_PASSWORD=@USER_PASSWORD";
            var parms = new DynamicParameters();
            parms.Add("LOGIN_ID", login.LOGIN_ID);
            parms.Add("USER_PASSWORD", login.USER_PASSWORD);
            AppUserDto? data = (await connection.QueryAsync<AppUserDto>(tsql, parms))?.FirstOrDefault();
            if (data != null)
                data.Roles = await GetUserRoles(data.ID);
            return data;
        }

        private async Task<List<string>> GetUserRoles(int userId)
        {
            DbConnection connection = GetSqlConnection();
            string tsql = $@"select ROLE_NAME from dbo.APPUSER_ROLE UR,dbo.APPROLE R where UR.APPROLE_ID=r.ID  and UR.APPUSER_ID=@APPUSER_ID";
            var parms = new DynamicParameters();
            parms.Add("APPUSER_ID", userId);
            var data = (await connection.QueryAsync<string>(tsql, parms));
            return data.ToList();
        }


        //TOD: GetUserRoles

    }
}
