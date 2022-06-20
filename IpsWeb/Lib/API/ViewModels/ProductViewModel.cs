﻿using System.ComponentModel.DataAnnotations;
using Warehouse.Core.Domain.Entities;

namespace IpsWeb.Lib.API.ViewModels
{
    public class ProductViewModel
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? MacAddress { get; set; }
        public ProductMetadata? Metadata { get; set; }
    }
}
