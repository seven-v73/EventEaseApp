using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Number of tickets is required.")]
        [Range(1, 5, ErrorMessage = "You can register between 1 and 5 tickets.")]
        public int TicketCount { get; set; } = 1;

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms to proceed.")]
        public bool AcceptTerms { get; set; }
    }
}