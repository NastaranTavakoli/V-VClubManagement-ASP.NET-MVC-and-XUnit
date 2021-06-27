using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VandVCLubManagementSystem.Models.ViewModels.Member
{
    public class Create
    {
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.Date), Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
    }
}
