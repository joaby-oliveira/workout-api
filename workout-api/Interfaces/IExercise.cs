﻿using System;
namespace workout_api.Interfaces
{
	public interface IExercise
	{
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Muscle { get; set; }
        public string? Equipment { get; set; }
        public string? Difficulty { get; set; }
        public string? Instructions { get; set; }
    }
}

