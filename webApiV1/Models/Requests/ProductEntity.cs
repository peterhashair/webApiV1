
using System;
using System.ComponentModel.DataAnnotations;

namespace MyAwesomeWebApi.Models.Requests
{
    public class ProductEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}