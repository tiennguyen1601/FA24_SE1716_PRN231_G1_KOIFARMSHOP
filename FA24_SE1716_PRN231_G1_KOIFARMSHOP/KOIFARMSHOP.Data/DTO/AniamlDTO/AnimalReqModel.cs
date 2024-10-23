using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KOIFARMSHOP.Data.DTO.AniamlDTO
{
    public class AnimalReqModel
    {
        public int? animalId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        [StringLength(100, ErrorMessage = "Origin cannot be longer than 100 characters.")]
        public string? Origin { get; set; }

        [StringLength(100, ErrorMessage = "Species cannot be longer than 100 characters.")]
        public string? Species { get; set; }

        [StringLength(50, ErrorMessage = "Type cannot be longer than 50 characters.")]
        public string? Type { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters.")]
        public string? Gender { get; set; }

        [StringLength(10, ErrorMessage = "Size cannot be longer than 10 characters.")]
        public string? Size { get; set; }

        public string? Certificate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maintenance cost must be a positive value.")]
        public decimal? MaintenanceCost { get; set; }

        public string? Color { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount of feed must be a positive value.")]
        public decimal? AmountFeed { get; set; }

        public string? HealthStatus { get; set; }

        public string? FarmOrigin { get; set; }

        public int? BirthYear { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        public List<string>? Images { get; set; } = new List<string>();
    }
}