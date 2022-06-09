using Microsoft.AspNetCore.Components;
using YammerReader.Client.Library;
using YammerReader.Shared;

namespace YammerReader.Client.Shared
{
    public partial class ThreadDisplay : CommonBlazorBase
    {
        [Parameter] public List<YammerMessage>? ListData { get; set; }

        private async Task RetrieveReplies(YammerMessage item)
        {
            YammerFilter query = new YammerFilter()
            {
                thread_id = item.thread_id
            };
            item.Replies = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetThreadReplies", query);
        }

        private string SetDisabled(YammerMessage item)
        {
            return (item.Replies != null ? "disabled" : "");
        }
    }
}
