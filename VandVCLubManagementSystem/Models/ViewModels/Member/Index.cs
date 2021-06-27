using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace VandVCLubManagementSystem.Models.ViewModels.Member
{
    public class Index
    {
        [Display(Name = "Search for member")]
        public string SearchString { get; set; }
        public int? PageNumber { get; set; }

        public int TotalPages { get; set; }
        public int TotalResults { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public SelectList OrderOptions { get; set; }
        [Display(Name = "Order by")]
        public int? OrderOptionId { get; set; }
        public ICollection<Person> People { get; set; }

    }
}
