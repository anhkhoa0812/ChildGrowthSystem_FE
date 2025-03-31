using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChildGrowth.WebPage.Pages
{
    public class ConsultationModel : PageModel
    {
        public List<ConsultationAppointment> Appointments { get; set; }

        public void OnGet()
        {
            Appointments = new List<ConsultationAppointment>
            {
                new ConsultationAppointment
                {
                    Id = 1,
                    AppointmentDateTime = DateTime.ParseExact("2025-04-15 10:00 AM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture),
                    Doctor = "Dr. Smith"
                },
                new ConsultationAppointment
                {
                    Id = 2,
                    AppointmentDateTime = DateTime.ParseExact("2025-04-20 02:00 PM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture),
                    Doctor = "Dr. Johnson"
                }
            };
        }
    }

    public class ConsultationAppointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Doctor { get; set; }
    }
}