using Microsoft.AspNetCore.Components;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Pages;
public partial class Threads : CommonBlazorBase
{
    [Parameter] public string? thread_id { get; set; }

    public string ThreadName { get; set; } = "Thread name";

    private ListThreadsModel Model { get; set; } = new ListThreadsModel();

    private ThreadDisplay? threadDisplay { get; set; }

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
        YammerFilter query = new YammerFilter()
        {
            thread_id = thread_id
        };

        YammerMessage? result = await base.PostAsJsonAsync<YammerMessage>("Yammer/SingleThread", query);

        if(result == null)
        {
            return;
        }
        ThreadName = result.group_name;
        Model.ListData = new List<YammerMessage>();
        Model.ListData.Add(result);
    }
}
