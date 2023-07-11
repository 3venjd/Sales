using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sales.Shared.Entities
{
    public class State
    {
        public int Id { get; set; }

        [Display(Name = "State/department")]
        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The field {0} can't have more than {1} characters")]
        public string Name { get; set; } = null!;

        public int CountryId { get; set; }

        public Country? Country { get; set; }

        public ICollection<City>? Cities { get; set; }

        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
