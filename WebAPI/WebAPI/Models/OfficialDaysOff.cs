﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class OfficialDaysOff
    {
        [Key]
        public DateOnly Date { get; set; }

        public string Name { get; set; }
    }
}
