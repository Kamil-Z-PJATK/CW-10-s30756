namespace APBD_CW_5.DTOs;

public class ClientCreateDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Pesel { get; set; }
    public int TripId { get; set; }
    public string TripName { get; set; }
    public int? PaymentDate {get; set;} //w mojej bazie danych to jest int zgodnie z obrazkiem diagramu (tak miałem teżw poprzednim zadaniu z użyciem tego diagramu)
}