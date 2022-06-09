using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace YammerReader.Client.Library
{
    public class CommonBlazorBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        #region 停駐輸入欄位
        [Inject] public IJSRuntime js { get; set; }

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

        #region 緩存變數
        [Inject] private Blazored.LocalStorage.ILocalStorageService localStorage { get; set; }

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
        [Inject] IHttpClientFactory HttpClientFactory { get; set; }

        protected async Task<T?> PostAsJsonAsync<T>(string url, object postValue)
        {
            var Http = HttpClientFactory.CreateClient("YammerReader.PublicServerAPI");
            var response = await Http.PostAsJsonAsync(url, postValue);
            if (response.IsSuccessStatusCode)
            {
                //ReadAsStringAsync 可以先讀出Json 再轉，比較好debug
                //var result = await response.Content.ReadFromJsonAsync<T>();
                string json = await response.Content.ReadAsStringAsync();
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
