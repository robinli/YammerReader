using Microsoft.AspNetCore.Components;
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
                thread_id = item.thread_id
            };
            item.Replies = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetThreadReplies", query);
        }

        private string SetDisabled(YammerMessage item)
        {
            return (item.Replies != null ? "disabled" : "");
        }

        private async Task ThreadLinkClick(YammerMessage item)
        {
            await Task.Delay(0);
            NavigationManager!.NavigateTo($"/Threads/{item.id}", forceLoad: false, replace: false);
        }

        private bool CheckIsPicture(string file_type)
        {
            string picture_file_types = "gif|jpeg|jpg|png";
            return picture_file_types.Contains(file_type);
        }
    }
}
