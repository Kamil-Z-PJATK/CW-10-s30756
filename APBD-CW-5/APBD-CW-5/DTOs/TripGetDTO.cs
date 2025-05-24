using APBD_CW_5.Models;

namespace APBD_CW_5.DTOs;

public class TripGetDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    public List<CountryGetDTO> Countrys { get; set; }
    public ICollection<ClientGetDTO> Clients { get; set; }
}