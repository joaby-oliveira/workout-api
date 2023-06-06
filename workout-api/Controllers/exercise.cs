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
        public ExerciseController()
        {
            FetchDataFromApi().Wait();
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
                string apiUrl = "https://api.api-ninjas.com/v1/exercises";
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                httpClient.DefaultRequestHeaders.Add("X-Api-Key", "CNL3wiZgGs32CrdSe8A1iw==re8QY91HStDBXONB");

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                List<Exercise> fetchedData = JsonConvert.DeserializeObject<List<Exercise>>(responseBody);
                exercises = fetchedData;
                Console.WriteLine(exercises);
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

            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteExercise(string name)
        {
            var Exercise = exercises.Find(exercise => exercise.Name == name);
            if (Exercise == null)
            {
                return NotFound();
            }

            exercises.Remove(Exercise);
            return NoContent();
        }
    }
}
