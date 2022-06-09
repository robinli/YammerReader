using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YammerReader.Server.DAL;
using YammerReader.Shared;

namespace YammerReader.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class YammerController : ControllerBase
    {
        [HttpPost]
        [Route("GetGroup")]
        public async Task<YammerGroup?> GetGroup(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            return await dal.GetGroup(filter.group_id!);
        }

        [HttpPost]
        [Route("GetAllGroups")]
        public async Task<List<YammerGroup>> GetAllGroups()
        {
            YammerDAL dal = new YammerDAL();
            return await dal.GetAllGroups();
        }


        [HttpPost]
        [Route("QueryRootMessage")]
        public async Task<List<YammerMessage>> QueryRootMessage(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            List<YammerMessage> result = await dal.QueryRootMessage(filter);
            return result;
        }

        [HttpPost]
        [Route("QueryThreadMessage")]
        public async Task<List<YammerMessage>> QueryThreadMessage(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            return await dal.QueryThreadMessage(filter?.thread_id!);
        }

    }
}
