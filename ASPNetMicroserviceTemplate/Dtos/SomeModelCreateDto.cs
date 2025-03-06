using System.ComponentModel.DataAnnotations;

namespace ASPNetMicroserviceTemplate.Dtos 
{
      /// Should be removed from the real project!
    public class SomeModelCreateDto
    {
        [Required]
        public string? SomeStringName {get; set;}
    }
} 