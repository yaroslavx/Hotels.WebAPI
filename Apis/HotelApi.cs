using Hotels.WebAPI.Auth;
using Hotels.WebAPI.Data;

namespace Hotels.WebAPI.Apis;

public class HotelApi : IApi
{
    public void Register(WebApplication app)
    {
        app.MapGet("/hotels", Get)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .WithName("GetHotels")
            .WithTags("Getters");

        app.MapGet("/hotels/{id}", GetById)
            .Produces<Hotel>(StatusCodes.Status200OK)
            .WithName("GetHotel")
            .WithTags("Getters");

        app.MapPost("/hotels", Post)
            .Accepts<Hotel>("application/json")
            .Produces<Hotel>(StatusCodes.Status201Created)
            .WithName("CreateHotel")
            .WithTags("Creators");

        app.MapPut("/hotels/{id}", Update)
            .Accepts<Hotel>("application/json")
            .WithName("UpdateHotel")
            .WithTags("Updaters");

        app.MapDelete("/hotels/{id}", Delete)
            .WithName("DeleteHotel")
            .WithTags("Deleters");

        app.MapGet("/hotels/search/name/{query}", GetByQuery)
            .Produces<List<Hotel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("SearchHotels")
            .WithTags("Getters")
            .ExcludeFromDescription();

        app.MapGet("/hotels/search/location/{coordinates}", GetByCoordinates)
            .ExcludeFromDescription();
    }
    
    [Authorize]
    private async Task<IResult> Get(IHotelRepository repository) =>
        Results.Extensions.Xml(await repository.GetHotelsAsync());

    [Authorize]
    private async Task<IResult> GetById(int id, IHotelRepository repository) =>
        await repository.GetHotelAsync(id) is Hotel hotel
            ? Results.Ok(hotel)
            : Results.NotFound();
    
    [Authorize] 
    private async Task<IResult> Post([FromBody] Hotel hotel, IHotelRepository repository)
    {
        await repository.CreateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.Created($"/hotels/{hotel.Id}", hotel);
    }
    
    [Authorize] 
    private async Task<IResult> Update(int id, [FromBody] Hotel hotel, IHotelRepository repository)  
    {
        await repository.UpdateHotelAsync(hotel);
        await repository.SaveAsync();
        return Results.NoContent();
    }
    
    [Authorize] 
    private async Task<IResult> Delete(int id, IHotelRepository repository)
    {
        await repository.DeleteHotelAsync(id);
        await repository.SaveAsync();
        return Results.NoContent();
    }

    [Authorize]
    private async Task<IResult> GetByQuery(string query, IHotelRepository repository) =>
        await repository.GetHotelsAsync(query) is IEnumerable<Hotel> hotels
            ? Results.Ok((object?)hotels)
            : Results.NotFound(Array.Empty<Hotel>());
    
    [Authorize]
    private async Task<IResult> GetByCoordinates(Coordinates coordinates, IHotelRepository repository) 
    {
        return await repository.GetHotelsAsync(coordinates) is IEnumerable<Hotel> hotels
            ? Results.Ok(hotels)
            : Results.NotFound(Array.Empty<Hotel>());
    }
}
