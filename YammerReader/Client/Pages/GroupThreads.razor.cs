using Microsoft.AspNetCore.Components;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Shared;

namespace YammerReader.Client.Pages
{
    public partial class GroupThreads : CommonBlazorBase
    {
        [Parameter] public string? group_id { get; set; }

        private ListThreadsModel Model { get; set; } = new ListThreadsModel();

        private PagerDto Pager = new PagerDto { PageIndex = 1, PageSize = 10 };

        private async Task GetData()
        {
            await RetrieveData(1);
        }

        protected override async Task OnInitializedAsync()
        {
            await RetrieveData(1);
        }

        private async Task RetrieveData(int pageIndex)
        {
            Pager.PageIndex = pageIndex;

            YammerFilter query = new YammerFilter()
            {
                group_id = group_id,
                PageIndex = Pager.PageIndex,
                PageSize = Pager.PageSize
            };

            Model.Group = await base.PostAsJsonAsync<YammerGroup>("Yammer/GetGroup", query);

            List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/QueryRootMessage", query);
            
            Model.ListData = result;
            Pager.AllCount = result[0].ttlrows;
        }

        private async Task RetrieveReplies(YammerMessage item)
        {
            YammerFilter query = new YammerFilter()
            {
                thread_id = item.thread_id
            };
            item.Replies = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/QueryThreadMessage", query);
        }

        private string SetDisabled(YammerMessage item)
        {
            return (item.Replies != null ? "disabled" : "");
        }
    }
}
