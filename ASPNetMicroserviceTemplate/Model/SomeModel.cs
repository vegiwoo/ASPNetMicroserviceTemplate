using System.ComponentModel.DataAnnotations;

namespace ASPNetMicroserviceTemplate.Model 
{
    /// <summary>
    /// The model is given for presentation purposes.
    /// Should be removed from the project!
    /// </summary>
    public class SomeModel 
    {
        /// <summary>
        /// Unique key for storing entities in database
        /// </summary>
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string? SomeStringName {get; set;}
        [Required]
        public DateTime UpdateAt { get; set; }
    }
}