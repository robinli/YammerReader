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
        [Route("GetGroupThreads")]
        public async Task<List<YammerMessage>> GetGroupThreads(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            List<YammerMessage> result = await dal.GetGroupThreads(filter);
            return result;
        }

        [HttpPost]
        [Route("GetThreadReplies")]
        public async Task<List<YammerMessage>> GetThreadReplies(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            return await dal.GetThreadReplies(filter!.thread_id!);
        }

        [HttpPost]
        [Route("SingleThread")]
        public async Task<YammerMessage> SingleThread(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            YammerMessage result = await dal.SingleThread(filter!.thread_id!);
            return result;
        }
    }
}
