using Acef.MVC.Interfaces;
using Acef.MVC.Models.DTO;

namespace Acef.MVC.Services
{
    public class RaisonServiceProxy : IRaisonService
    {
        private readonly HttpClient _httpClient;
        private readonly string _raisonApiUrl = "api/raison/";

        public RaisonServiceProxy (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Ajouter(RaisonDTO raison)
        {
            var response = await _httpClient.PostAsJsonAsync(_raisonApiUrl, raison);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when adding a consultation reason");
            }
        }

        public async Task Modifier(RaisonDTO raison)
        {
            var response = await _httpClient.PutAsJsonAsync(_raisonApiUrl + raison.ID, raison);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when modifying a consultation reason");
            }
        }

        public async Task<RaisonDTO> ObtenirSelonId(int id)
        {
            var response = await _httpClient.GetAsync(_raisonApiUrl + id);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error retrieving consultation reason");
            }

            return await response.Content.ReadFromJsonAsync<RaisonDTO>();
        }

        public async Task<IEnumerable<RaisonDTO>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_raisonApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when retrieving consultation reasons");
            }

            return await response.Content.ReadFromJsonAsync<IEnumerable<RaisonDTO>>();
        }

        public async Task Supprimer(RaisonDTO raison)
        {
            var response = await _httpClient.DeleteAsync(_raisonApiUrl + raison.ID);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error when deleting consultation reason");
            }
        }
    }
}
