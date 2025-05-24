namespace APBD_CW_5.DTOs;

public class PagedTripsGetDTO
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int AllPages { get; set; }
    public ICollection<TripGetDTO> trips { get; set; }
}