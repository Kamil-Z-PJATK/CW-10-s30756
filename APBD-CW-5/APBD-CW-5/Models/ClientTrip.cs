using System;
using System.Collections.Generic;

namespace APBD_CW_5.Models;

public partial class ClientTrip
{
    public int ClientIdClient { get; set; }

    public int TripIdTrip { get; set; }

    public int RegisteredAt { get; set; }

    public int? PaymentDate { get; set; }

    public virtual Client ClientIdClientNavigation { get; set; } = null!;

    public virtual Trip TripIdTripNavigation { get; set; } = null!;
}
