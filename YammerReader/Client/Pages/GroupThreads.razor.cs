using Microsoft.AspNetCore.Components;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Pages
{
    public partial class GroupThreads : CommonBlazorBase
    {
        [Parameter] public string? group_id { get; set; }
        public string? save_group_id { get; set; }

        private ListThreadsModel Model { get; set; } = new ListThreadsModel();

        private ThreadDisplay? threadDisplay { get; set; }
        private Pager? pagerLink { get; set; }

        private async Task GetData()
        {
            await RetrieveData(1);
        }

        protected override async Task OnInitializedAsync()
        {
            await RetrieveData(1);

            pagerLink!.PageTo = (async (int pageIndex) =>
            {
                await RetrieveData(pageIndex);
            });
        }

        protected override async Task OnParametersSetAsync()
        {
            if(group_id != save_group_id)
            {
                await RetrieveData(1);
            }
        }

        private async Task RetrieveData(int pageIndex)
        {
            Model.Pager.PageIndex = pageIndex;

            YammerFilter query = new YammerFilter()
            {
                group_id = group_id,
                PageIndex = Model.Pager.PageIndex,
                PageSize = Model.Pager.PageSize
            };

            Model.Group = await base.PostAsJsonAsync<YammerGroup>("Yammer/GetGroup", query);

            List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetGroupThreads", query);
            
            Model.ListData = result;
            YammerMessage? firstMessage = result?.FirstOrDefault();
            Model.Pager.AllCount = (firstMessage != null ? firstMessage.ttlrows : 0);
            save_group_id = group_id;
            StateHasChanged();
        }
    }
}
