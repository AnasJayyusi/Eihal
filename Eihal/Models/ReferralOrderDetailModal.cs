﻿using Eihal.Data.Entites;
using Eihal.Enums;
using static Eihal.Data.SharedEnum;

namespace Eihal.Models
{
    public class ReferralOrderDetailModal
    {
        public int OrderId { get; set; }
        public string ReferralRequestNumber { get; set; }
        public ReferralStatusEnum Status { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public string Date { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CountryTextAr { get; set; }
        public string CountryTextEn { get; set; }
        public string StateTextAr { get; set; }
        public string StateTextEn { get; set; }
        public string CityTextAr { get; set; }
        public string CityTextEn { get; set; }
        public Gender Gender { get; set; }
        public Age Age { get; set; }
        public string ChronicDisease { get; set; }
        //public List<OrderServiceDetail> ServicesApproved { get; set; }
        public List<Services> ServicesRequests { get; set; }

    }
}
