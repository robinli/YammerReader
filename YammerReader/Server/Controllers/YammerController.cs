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

        [HttpPost]
        [Route("Search")]
        public async Task<List<YammerMessage>> Search(YammerFilter filter)
        {
            YammerDAL dal = new YammerDAL();
            List<YammerMessage> result = await dal.Search(filter);
            return result;
        }

        #region 下載圖片, 檔案
        [HttpGet]
        [Route("GetPicture/dir/{user_code}/id/{file_id}/files/{file_name}")]
        public IActionResult GetPicture(string user_code, string file_id, string file_name)
        {
            Byte[] b = System.IO.File.ReadAllBytes($@"E:\WEB_SITE\YammerReader.bud4.net_uploaded_files\{user_code}\{file_name}");   // You can use your own method over here.         
            if (CheckIsPicture(file_name))
            {
                return File(b, "image/jpeg");
            }
            
            string fileType = GetFileType(file_name);
            string office_file_types = "pdf";
            if(office_file_types.Contains(fileType))
            {
                return File(b, $"Application/{fileType}");
            }

            return File(b, "application/octet-stream");
        }

        private bool CheckIsPicture(string file_name)
        {
            string file_type = GetFileType(file_name);
            string picture_file_types = "gif|jpeg|jpg|png";
            return picture_file_types.Contains(file_type);
        }

        private string GetFileType(string file_name)
        {
            int idx = file_name.IndexOf('.');
            if(idx < 1)
            {
                return "";
            }
            return file_name.Substring(idx + 1, file_name.Length - idx - 1);
        }
        #endregion
    }
}
