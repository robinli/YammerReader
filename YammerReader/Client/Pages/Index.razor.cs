using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Shared;
namespace YammerReader.Client.Pages;

public partial class Index : CommonBlazorBase
{
    private List<YammerGroup> ListData { get; set; } = new List<YammerGroup>();

    protected override async Task OnInitializedAsync()
    {
        await RetrieveData();
    }

    private async Task RetrieveData()
    {
        string url = "Yammer/GetAllGroups";
        List<YammerGroup>? result = await GetCache<List<YammerGroup>>(url);
        if(result == null)
        {
            result = await PostAsJsonAsync<List<YammerGroup>>(url, null);
            await SaveCache(url, result);
        }
        if(result != null)
        {
            ListData = result;
        }
    }

}

