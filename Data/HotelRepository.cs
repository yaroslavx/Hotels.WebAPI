namespace Hotels.WebAPI.Data;

public class HotelRepository : IHotelRepository
{
    private HotelDb _context;
    
    public HotelRepository(HotelDb context)
    {
        _context = context;
    }
    
    public async Task<List<Hotel>> GetHotelsAsync()
    {
        return await _context.Hotels.ToListAsync();
    }

    public async Task<List<Hotel>> GetHotelsAsync(string keywords)
    {
        return await _context.Hotels.Where(h => h.Name.Contains(keywords)).ToListAsync();

    }

    public async Task<List<Hotel>> GetHotelsAsync(Coordinates coordinates)
    {
        return await _context.Hotels.Where(hotel =>
            hotel.Latitude > coordinates.Latitude - 1 &&
            hotel.Latitude < coordinates.Latitude + 1 &&
            hotel.Longitude > coordinates.Longitude - 1 &&
            hotel.Longitude < coordinates.Longitude + 1).ToListAsync();
    }

    public async Task<Hotel> GetHotelAsync(int hotelId)
    {
        return await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
    }

    public async Task CreateHotelAsync(Hotel hotel)
    {
        await _context.Hotels.AddAsync(hotel);
    }

    public async Task UpdateHotelAsync(Hotel hotel)
    {
        var hotelFromDb = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotel.Id);

        if (hotelFromDb == null)
        {
            return;
        }
        
        hotelFromDb.Name = hotel.Name;
        hotelFromDb.Latitude = hotel.Latitude;
        hotelFromDb.Longitude = hotel.Longitude;
    }

    public async Task DeleteHotelAsync(int hotelId)
    {
        var hotelFromDb = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == hotelId);
        
        if (hotelFromDb == null)
        {
            return;
        }
        
        _context.Hotels.Remove(hotelFromDb);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}