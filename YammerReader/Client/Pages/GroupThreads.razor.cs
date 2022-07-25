using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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

        /// <summary>
        /// 正在載入下一頁的資料
        /// </summary>
        private bool IsLoadingNextPageData { get; set; } = false;

        private Pager? pagerLink { get; set; }

        private async Task GetData()
        {
            await RetrieveData(1);
        }

        protected override async Task OnParametersSetAsync()
        {
            await ResetUI();
            await GetGroupInfo();
            await RetrieveData(1);
        }

        private async Task ResetUI()
        {
            await Task.Delay(0);
            Model.Group.group_name = "";
            Model.ListData = null;
            Model.Pager.AllCount = 0;
        }

        private async Task GetGroupInfo()
        {
            YammerFilter query = new YammerFilter()
            {
                group_id = group_id
            };
            Model.Group = await base.PostAsJsonAsync<YammerGroup>("Yammer/GetGroup", query);
        }

        private bool IsRetrieveData = false;
        private async Task RetrieveData(int pageIndex)
        {
            if (IsRetrieveData) return;
            IsRetrieveData = true;

            YammerFilter query = new YammerFilter()
            {
                group_id = group_id,
                PageIndex = pageIndex,
                PageSize = Model.Pager.PageSize
            };
            List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/GetGroupThreads", query);
            if (result != null)
            {
                if (Model.ListData == null)
                {
                    Model.ListData = new List<YammerMessage>();
                }
                Model.ListData.AddRange(result);
            }
           
            YammerMessage? firstMessage = result?.FirstOrDefault();
            Model.Pager.PageIndex = pageIndex;
            Model.Pager.AllCount = (firstMessage != null ? firstMessage.ttlrows : 0);
            StateHasChanged();
            IsRetrieveData = false;
        }

        private async Task OnPageClicked(int pageIndex)
        {
            Model.ListData = null;
            await RetrieveData(pageIndex);
        }

        public async Task OnPageScrollEnd(int pageIndex)
        {
            if (IsLoadingNextPageData) return;
            IsLoadingNextPageData = true;
            StateHasChanged();

            await RetrieveData(pageIndex);
            
            IsLoadingNextPageData = false;
            StateHasChanged();
        }

    }
}
