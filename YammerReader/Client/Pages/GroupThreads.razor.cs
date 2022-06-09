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
        
        private ListThreadsModel Model { get; set; } = new ListThreadsModel();

        private ThreadDisplay? threadDisplay { get; set; }

        private PagerDto Pager = new PagerDto { PageIndex = 1, PageSize = 10 };

        private async Task GetData()
        {
            await RetrieveData(1);
        }

        protected override async Task OnInitializedAsync()
        {
            await RetrieveData(1);
        }

        protected override async Task OnParametersSetAsync()
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

            List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetGroupThreads", query);
            
            Model.ListData = result;
            Pager.AllCount = result[0].ttlrows;
        }
    }
}
