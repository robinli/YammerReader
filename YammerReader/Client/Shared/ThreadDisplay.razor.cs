using Microsoft.AspNetCore.Components;
using System.Text;
using YammerReader.Client.Library;
using YammerReader.Shared;

namespace YammerReader.Client.Shared
{
    public partial class ThreadDisplay : CommonBlazorBase
    {
        [Inject] NavigationManager? NavigationManager { get; set; }

        [Parameter] public List<YammerMessage>? ListData { get; set; }

        private async Task RetrieveReplies(YammerMessage item)
        {
            YammerFilter query = new YammerFilter()
            {
                thread_id = item.thread_id,
                offect_rows = (item.Replies==null ? 0 : item.Replies.Count)
            };
            List<YammerMessage>? replyMessages = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetThreadReplies", query);
            if(replyMessages == null || replyMessages.Any() == false)
            {
                return;
            }
            foreach(YammerMessage reply in replyMessages)
            {
                if (item.Replies == null)
                {
                    item.Replies = new List<YammerMessage>();
                }
                //比對不重複才加入
                if (item.Replies.Any(r => r.id.Equals(reply.id))==false) 
                { 
                    item.Replies.Add(reply); 
                }
            }
        }

        private string SetDisabled(YammerMessage item)
        {
            return (item.CountPrevoiusReplies() == 0 ? "disabled" : "");
        }

        private async Task ThreadLinkClick(YammerMessage item)
        {
            await Task.Delay(0);
            NavigationManager!.NavigateTo($"/Threads/{item.thread_id}", forceLoad: false, replace: false);
        }

        private bool CheckIsPicture(string file_type)
        {
            string picture_file_types = "gif|jpeg|jpg|png";
            return picture_file_types.Contains(file_type);
        }

        private string FormatContext(string contextBody)
        {
            // 分行 \n
            StringBuilder htmlText = new StringBuilder();

            foreach(string line in contextBody.Split("\n"))
            {
                htmlText.AppendLine($"<p>{line}</p>");
            }

            return htmlText.ToString();
        }
    }
}
