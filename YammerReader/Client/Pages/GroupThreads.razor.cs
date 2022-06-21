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

        private bool IsLoadingData { get; set; } = false;

        private Pager? pagerLink { get; set; }

        private async Task GetData()
        {
            await RetrieveData(1);
        }

        protected override async Task OnParametersSetAsync()
        {
            var dotnethelper = DotNetObjectReference.Create(this);
            await js.InvokeVoidAsync("RegisterPage", dotnethelper);

            await ResetUI();
            await RetrieveData(1);
        }

        private async Task OnPageClicked(int pageIndex) 
        {
            Model.ListData = null;
            await RetrieveData(pageIndex);
        }

        private async Task ResetUI()
        {
            await Task.Delay(0);
            Model.Group.group_name = "";
            Model.ListData = null;
            Model.Pager.AllCount = 0;
        }

        private async Task RetrieveData(int pageIndex)
        {
            //TODO 分頁元件 與 捲軸自動載入下一頁 操作上有衝突 ??

            YammerFilter query = new YammerFilter()
            {
                group_id = group_id,
                PageIndex = pageIndex,
                PageSize = Model.Pager.PageSize
            };

            Model.Group = await base.PostAsJsonAsync<YammerGroup>("Yammer/GetGroup", query);

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
        }

        [JSInvokable]
        public async Task OnPageScrollEnd()
        {
            //await Task.Delay(0);
            int pageIndex = Model.Pager.PageIndex + 1;
            if (pageIndex > Model.Pager.TotalPage)
            {
                return;
            }
            IsLoadingData = true;
            StateHasChanged();
            await RetrieveData(pageIndex);
            IsLoadingData = false;
            StateHasChanged();
        }

    }
}
