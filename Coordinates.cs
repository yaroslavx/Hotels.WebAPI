namespace Hotels.WebAPI;

public record Coordinates(double Latitude, double Longitude)
{
    public static bool TryParse(string input, out Coordinates? coordinates)
    {
        coordinates = default;
        var splitArray = input.Split(',', 2);
        if (splitArray.Length != 2)
        {
            return false;
        }
        if (!double.TryParse(splitArray[0], out double latitude))
        {
            return false;
        } 
        if (!double.TryParse(splitArray[1], out double longitude))
        {
            return false;
        }
        coordinates = new Coordinates(latitude, longitude);
        return true;
    }

    public static async ValueTask<Coordinates?> BindAsync(HttpContext httpContext, ParameterInfo parameter)
    {
        var input = httpContext.GetRouteValue(parameter.Name!) as string ?? string.Empty;
        TryParse(input, out Coordinates coordinates);
        return coordinates;
    }
}