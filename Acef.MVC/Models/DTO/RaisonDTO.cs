﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Acef.MVC.Models.DTO
{
    public class ReasonDTO
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [Required(ErrorMessage = "The Name field is mandatory")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
