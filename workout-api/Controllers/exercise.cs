using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using workout_api.Classes;
using workout_api.Dtos;
using Newtonsoft.Json;

namespace workout_api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ExerciseController : ControllerBase
  {

    private static bool alreadyRequested = false;
    public ExerciseController()
    {
      if (!alreadyRequested)
      {
        FetchDataFromApi().Wait();
      }
    }

    //public async Task<List<Exercise>> getData()
    //{
    //    HttpClient httpClient = new HttpClient();
    //    httpClient.DefaultRequestHeaders.Add("X-Token-Auth", "CNL3wiZgGs32CrdSe8A1iw==re8QY91HStDBXONB");
    //    string url = "https://api.api-ninjas.com/v1/exercises";

    //    HttpResponseMessage response = await httpClient.GetAsync(url);
    //    response.EnsureSuccessStatusCode();
    //    var exercisesSerialized = await response.Content.ReadAsStringAsync();
    //    return JsonConvert.DeserializeObject<List<Exercise>>(exercisesSerialized);
    //}


    private async Task FetchDataFromApi()
    {
      using (HttpClient httpClient = new HttpClient())
      {
        try
        {
          string apiUrl = "https://api.api-ninjas.com/v1/exercises";
          httpClient.DefaultRequestHeaders.Add("X-Api-Key", "CNL3wiZgGs32CrdSe8A1iw==re8QY91HStDBXONB");

          HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
          response.EnsureSuccessStatusCode();

          string responseBody = await response.Content.ReadAsStringAsync();
          List<Exercise> fetchedData = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);
          exercises = fetchedData;
          alreadyRequested = true;
        }
        catch (Exception error)
        {
          Console.WriteLine(error);
        }
      }
    }


    private static List<Exercise> exercises = new List<Exercise>();

    [HttpGet]
    public IActionResult GetAllExercises()
    {
      return Ok(exercises);
    }

    //[HttpGet]
    //[Route("update-data")]
    //public async Task<IActionResult> UpdateData ()
    //{
    //    exercises = await this.getData();

    //    return Ok(exercises);
    //}
    [HttpGet("{name}")]
    public IActionResult GetExerciseById(string name)
    {
      var Exercise = exercises.Find(c => c.Name == name);
      if (Exercise == null)
      {
        return NotFound();
      }
      return Ok(Exercise);
    }

    [HttpPut("{name}")]
    public IActionResult UpdateExercise(string name, ExerciseDTO updatedExercise)
    {
      var existingExercise = exercises.Find(exercise => exercise.Name == name);
      if (existingExercise == null)
      {
        return NotFound();
      }

      existingExercise.Name = updatedExercise.Name;
      existingExercise.Difficulty = updatedExercise.Difficulty;
      existingExercise.Instructions = updatedExercise.Instructions;
      existingExercise.Equipment = updatedExercise.Equipment;
      existingExercise.Type = updatedExercise.Type;
      existingExercise.Muscle = updatedExercise.Muscle;

      return Ok("Exercício editado com sucesso");
    }

    [HttpDelete("{name}")]
    public IActionResult DeleteExercise(string name)
    {
      Console.WriteLine(name);
      var Exercise = exercises.Find(exercise => exercise.Name == name);
      Console.WriteLine(Exercise.Instructions);
      if (Exercise == null)
      {
        return NotFound();
      }

      exercises.Remove(Exercise);

      foreach (var exerciseItem in exercises)
      {
        Console.WriteLine(exerciseItem.Name);
      }

      return Ok("Dados excluidos com sucesso");
    }
  }
}
