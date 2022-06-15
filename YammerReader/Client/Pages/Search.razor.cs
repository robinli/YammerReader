﻿using Microsoft.AspNetCore.Components;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Pages;

public partial class Search : CommonBlazorBase
{
    [Parameter] public string? key_word { get; set; }
    private string? saved_key_word { get; set; }

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
        if(key_word != saved_key_word)
        {
            await RetrieveData(1);
        }
    }

    private async Task RetrieveData(int pageIndex)
    {
        if (string.IsNullOrEmpty(key_word))
        {
            return;
        }
        
        Model.Pager.PageIndex = pageIndex;

        YammerFilter query = new YammerFilter()
        {
            search_keyword = key_word,
            PageIndex = Model.Pager.PageIndex,
            PageSize = Model.Pager.PageSize
        };

        List<YammerMessage>? result = await base.PostAsJsonAsync<List<YammerMessage>>("Yammer/Search", query);
        
        Model.ListData = result;
        YammerMessage? firstMessage = result?.FirstOrDefault();
        Model.Pager.AllCount = (firstMessage != null ? firstMessage.ttlrows : 0);
        saved_key_word = key_word;
        StateHasChanged();

    }
}

