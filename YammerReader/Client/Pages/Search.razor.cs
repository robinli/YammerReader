using Microsoft.AspNetCore.Components;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Pages;

public partial class Search : CommonBlazorBase
{
    [Parameter] public string? key_word { get; set; }
    private int TotalRows { get; set; }

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
            search_keyword = key_word,
            PageIndex = Pager.PageIndex,
            PageSize = Pager.PageSize
        };

        List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/Search", query);
        if (result != null && result!.Any())
        {
            Model.ListData = result;
            TotalRows = result[0].ttlrows;
            Pager.AllCount = TotalRows;
        }

    }
}

