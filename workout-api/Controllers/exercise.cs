using Microsoft.AspNetCore.Mvc;
using workout_api.Interfaces;
using Newtonsoft.Json;

namespace workout_api.Controllers;

public class ExerciseDTO
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Muscle { get; set; }
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? Instructions { get; set; }
}



[ApiController]
[Route("api/[controller]")]
public class ExerciseController : ControllerBase
{
    private HttpClient httpClient = new HttpClient();
    private List<ExerciseDTO>? exercises;

    [HttpGet]
    public IActionResult Get()
    {
        if(this.exercises?.Count == 0)
        {
            return NotFound("Dados não encontrados");
        }
        return Ok(this.exercises);
    }

    [HttpGet]
    [Route("/update-data")]
    public async Task<IActionResult> UpdateData()
    {
        string url = "https://api.api-ninjas.com/v1/exercises";

        httpClient.DefaultRequestHeaders.Add("X-Api-Key", "CNL3wiZgGs32CrdSe8A1iw==re8QY91HStDBXONB");

        HttpResponseMessage response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        this.exercises = JsonConvert.DeserializeObject<List<ExerciseDTO>>(responseBody);

        return Ok(this.exercises);
    }

}

