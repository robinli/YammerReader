using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace YammerReader.Client.Library
{
    public class CommonBlazorBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        [Inject] protected IJSRuntime js { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] private Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

        #region JavaScript 停駐輸入欄位
        protected async Task SetFocusAsync(string Id, string stringValue)
        {
            try
            {
                await js.InvokeVoidAsync("SetFocus", Id, stringValue);
            }
            catch (Exception e)
            {
                var error = e;
                return;
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            NavigationManager.LocationChanged += HandleLocationChanged;
        }
        private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            if(e.Location.Contains("GroupThreads"))
            {
                return;
            }

            if (js != null)
            {
                _ = js.InvokeVoidAsync("UnRegisterPage");
            }
        }

        #region 緩存變數
        protected async Task SaveCache(string cacheKey, object? value)
        {
            await localStorage.SetItemAsync(cacheKey, value);
        }

        protected async ValueTask<T> GetCache<T>(string cacheKey)
        {
            try
            {
                return await localStorage.GetItemAsync<T>(cacheKey);
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #region 呼叫API
        //[Inject] HttpClient Http { get; set; }
        protected async Task<T?> PostAsJsonAsync<T>(string url, object postValue)
        {
            var Http = HttpClientFactory.CreateClient("YammerReader.PublicServerAPI");
            var response = await Http.PostAsJsonAsync(url, postValue);
            if (response.IsSuccessStatusCode)
            {
                //ReadAsStringAsync 可以先讀出Json 再轉，比較好debug
                //var result = await response.Content.ReadFromJsonAsync<T>();
                string json = await response.Content.ReadAsStringAsync();
                if (json =="")
                {
                    return default(T);
                }

                var result = JsonSerializer.Deserialize<T>(json);

                if (result != null)
                {
                    return result;
                }
            }
            return default(T);
        }
        #endregion
    }
}
