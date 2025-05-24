using APBD_CW_5.DTOs;
using APBD_CW_5.Models;

namespace APBD_CW_5.Services;

public interface IDbService
{
    //public Task<IEnumerable<TripGetDTO>> GetTripsAsync();
    public Task<PagedTripsGetDTO> GetTripsAsync(int page, int pageSize);
    public Task DeleteClientAsync(int id);
    public Task<ClientCreateDTO> CreateClientAsync(int id,ClientCreateDTO client);
}