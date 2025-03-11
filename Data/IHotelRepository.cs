namespace Hotels.WebAPI.Data;

public interface IHotelRepository : IDisposable
{
    Task<List<Hotel>> GetHotelsAsync();
    Task<List<Hotel>> GetHotelsAsync(string keywords);
    Task<List<Hotel>> GetHotelsAsync(Coordinates coordinates);

    Task<Hotel> GetHotelAsync(int hotelId);

    Task CreateHotelAsync(Hotel hotel);

    Task UpdateHotelAsync(Hotel hotel);

    Task DeleteHotelAsync(int id);
    
    Task SaveAsync();
}