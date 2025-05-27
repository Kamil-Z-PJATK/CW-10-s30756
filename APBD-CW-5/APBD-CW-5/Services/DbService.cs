using APBD_CW_5.Data;
using APBD_CW_5.DTOs;
using APBD_CW_5.Exceptions;
using APBD_CW_5.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace APBD_CW_5.Services;

public class DbService(S30756Context data): IDbService
{
    public async Task<PagedTripsGetDTO> GetTripsAsync(int page, int pageSize)
    {
        var trips = new List<TripGetDTO>();

        
        var res = await data.Trips.ToListAsync();
        
        
        foreach (Trip tr in res)
        {
            
            var countrys = new List<CountryGetDTO>();
            var result = await data.Trips.Where(t=>t.IdTrip==tr.IdTrip).SelectMany(t=>t.CountryIdCountries).Select(c=>c.Name).ToListAsync();
            foreach (var country in result)
            {
                countrys.Add(new CountryGetDTO
                {
                    Name = country
                });
            }
            
            var cl= data.ClientTrips.Where(ct=>ct.TripIdTrip == tr.IdTrip).Select(ct=>ct.ClientIdClientNavigation).Distinct().ToList();
            
            var clienst= new List<ClientGetDTO>();
            foreach (var client in cl)
            {
                clienst.Add(new ClientGetDTO
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                });
            }
            trips.Add(new TripGetDTO
            {
                Name = tr.Name,
                Description = tr.Description,
                DateFrom = tr.DateFrom,
                DateTo = tr.DateTo,
                MaxPeople = tr.MaxPeople,
                Countrys = countrys,
                Clients= clienst
            });
        }
        
        trips = trips.OrderByDescending(t => t.DateFrom).ToList();
        
        var pages =  (int)Math.Ceiling(trips.Count/(double)pageSize) ;
        
        var paged = new PagedTripsGetDTO
        {
            PageNumber = page,
            PageSize = pageSize,
            AllPages = pages,
            trips = trips.Skip((page - 1) * pageSize).Take(pageSize).ToList()
        };
        return paged;
    }
    
    public async Task DeleteClientAsync(int id)
    {
        var transaction= await data.Database.BeginTransactionAsync();
        try
        {
            
            var client = await data.Clients.Where(c => c.IdClient == id).FirstOrDefaultAsync();
            if (client == null)
            {
                throw new ClientNotFoundException("Client " + id+" not found");
            }
            
            var trips=await data.ClientTrips.Where(c => c.ClientIdClient == id).FirstOrDefaultAsync();
            if (trips !=null)
            {
                throw new ClientHasAssighnedTripsExceptions("Client " + id+" has assighted its trips");
            }
            data.Clients.Remove(client);
            await data.SaveChangesAsync();
            await transaction.CommitAsync();

        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw ex;
        }
    }

    public async Task<ClientCreateDTO> CreateClientAsync(int id,ClientCreateDTO client)
    {
        var transaction = await data.Database.BeginTransactionAsync();

        try
        {
            var rep = await data.Clients.Where(c => c.Pesel==client.Pesel).FirstOrDefaultAsync();
            
            if (rep != null)
            {
                throw new ClientAlreadyExistsException("Client with pesel "+client.Pesel +" already exists");
            }
            
            var newClient = new Client
            {
                IdClient = data.Clients.Count()+1,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Pesel = client.Pesel,
                Telephone = client.Phone,
                Email = client.Email,
                
            };
            
            await data.Clients.AddAsync(newClient);
            
            data.SaveChanges();
            
            var repct= await data.ClientTrips.Where(ct=>ct.ClientIdClient ==newClient.IdClient && ct.TripIdTrip==id).FirstOrDefaultAsync();
            
            if (repct != null)
            {
                throw new ClientAlreadyExistsException("Client with pesel "+client.Pesel +" is already assigned to this trip");
            }
            
            var trdate= await data.ClientTrips.Join(data.Trips, ct=>ct.TripIdTrip, t=>t.IdTrip,(ct,t) => new { ct,t})
                            .Where(ct=>ct.t.DateFrom>DateTime.Now && ct.t.DateTo>DateTime.Now && ct.t.IdTrip==id).FirstOrDefaultAsync();
            if (trdate==null)
            {
                throw new WrongDateException("Trip already begun");
            }
            
           await data.ClientTrips.AddAsync(new ClientTrip
            {
                ClientIdClient = newClient.IdClient,
                TripIdTrip = id,
                PaymentDate = client.PaymentDate,
                RegisteredAt = 19 //w mojej bazie danych to pole to int zgodnie ze zdjęciem diagramu
            });
            data.SaveChanges();
            await transaction.CommitAsync();
          
            return client;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw e;
        }
        
        
        
        
    }
}