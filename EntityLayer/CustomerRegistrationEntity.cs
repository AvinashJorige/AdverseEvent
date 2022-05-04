using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class CustomerRegistrationEntity
    {
        public Guid? Id { get; set; }

        public int SeqId { get; set; }

        public string CustomerCode { get; set; }

        public string CountryCode { get; set; }

        public Guid? Language { get; set; }

        public string Name { get; set; }

        public Guid? GenderCode { get; set; }

        public Guid? Occupation { get; set; }

        public DateTime? DOB { get; set; }

        public int? Age { get; set; }

        public decimal? Phone { get; set; }

        public string Email { get; set; }

        public string StreetAddress { get; set; }

        public string DoctorRegNo { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsActive { get; set; }
    }

    public class CustomerInfoEntity
    {
        public string country { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string occupation { get; set; }
        public string dob { get; set; }
        public string age { get; set; }
        public string Phone { get; set; }
        public string email { get; set; }
        public string stAddress { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string docRegNo { get; set; }
        public string language { get; set; }
        public string CustomerCode { get; set; }
    }
}
