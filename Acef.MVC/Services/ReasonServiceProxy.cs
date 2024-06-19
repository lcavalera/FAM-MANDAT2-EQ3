using Acef.MVC.Interfaces;
using Acef.MVC.Models.DTO;

namespace Acef.MVC.Services
{
    public class ReasonServiceProxy : IReasonService
    {
        private readonly HttpClient _httpClient;
        private readonly string _reasonApiUrl = "api/reasons/";

        public ReasonServiceProxy (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Add(ReasonDTO reason)
        {
            var response = await _httpClient.PostAsJsonAsync(_reasonApiUrl, reason);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when adding a consultation reason");
            }
        }

        public async Task Edit(ReasonDTO reason)
        {
            var response = await _httpClient.PutAsJsonAsync(_reasonApiUrl + reason.ID, reason);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when modifying a consultation reason");
            }
        }

        public async Task<ReasonDTO> GetById(int id)
        {
            var response = await _httpClient.GetAsync(_reasonApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error retrieving consultation reason");
            }

            return await response.Content.ReadFromJsonAsync<ReasonDTO>();
        }

        public async Task<IEnumerable<ReasonDTO>> Get()
        {
            var response = await _httpClient.GetAsync(_reasonApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when retrieving consultation reasons");
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<ReasonDTO>>();
        }

        public async Task Delete(ReasonDTO reason)
        {
            var response = await _httpClient.DeleteAsync(_reasonApiUrl + reason.ID);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when deleting consultation reason");
            }
        }
    }
}
