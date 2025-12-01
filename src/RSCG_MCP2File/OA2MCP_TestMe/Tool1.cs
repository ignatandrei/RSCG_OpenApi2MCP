namespace OA2MCP_TestMe;

using ModelContextProtocol.Server;
using System.ComponentModel;
[McpServerToolType]
[MCP2File.AddMCPExportToFile]
public partial class WeatherTool
{
    [McpServerTool, Description("Returns the current weather for a specific city")]
    public string MyGetWeatherForCity(string cityName)
    { 
        Console.WriteLine("==========================");
        Console.WriteLine($"Function Start WeatherTool: GetWeatherForCity called with cityName: {cityName}");

        // generate a random weather report and return the result
        var random = new Random();
        var temperature = random.Next(-20, 40);
        var condition = random.Next(0, 2) == 0 ? "Sunny" : "Rainy";
        var humidity = random.Next(0, 100);
        var windSpeed = random.Next(0, 20);
        var report = $"Weather in {cityName}: {temperature}°C, {condition}, Humidity: {humidity}%, Wind Speed: {windSpeed} km/h";

        Console.WriteLine("Function report: " + report);
        Console.WriteLine($"Function End WeatherTool: GetWeatherForCity called with cityName: {cityName}");
        Console.WriteLine("==========================");

        return report;
    }
    //[ModelContextProtocol.Server.McpServerTool, System.ComponentModel.Description("TODO: desc from comments: ")]
    //public static string MCP_api_usefull_date_start_Get()
    //{
    //    HttpClient httpClient = new HttpClient();
    //    var url = "https://localhost/usefull_date/start";
    //    var response = httpClient.GetAsync(url).Result;
    //    if (response.IsSuccessStatusCode)
    //    {
    //        var result = response.Content.ReadAsStringAsync().Result;
    //        return result;
    //    }
    //    else
    //    {
    //        throw new Exception($"Error: {response.StatusCode}");
    //    }
    //}

}