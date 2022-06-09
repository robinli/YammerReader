using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Shared;
namespace YammerReader.Client.Pages;

public partial class Index : CommonBlazorBase
{
    private List<YammerGroup> ListData { get; set; } = new List<YammerGroup>();

    protected override async Task OnInitializedAsync()
    {
        await Task.Delay(0);

        await RetrieveData();
    }


    private async Task RetrieveData()
    {
        string url = "Yammer/GetAllGroups";
        List<YammerGroup> result = await base.PostAsJsonAsync<List<YammerGroup>>(url, null);

        ListData = result;
    }

}

