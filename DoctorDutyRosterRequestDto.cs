 public class DoctorRosterRequestDetails
    {
        public List<DoctorRosterRequestHdr> DoctorRosterRequestHeader { get; set; }
        public List<DoctorRosterRequestDtl> DoctorRosterRequestDetail { get; set; }

    }

    public class DoctorRosterRequestHdr 
    {
        public decimal RequestNo { get; set; }
        public string DoctorCode { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Status { get; set; }
        public string ModifiedUser { get; set; }

    }
    public class DoctorRosterRequestDtl:CommonDto 
    {
        public decimal DetailNo { get; set; }
        public string ClinicCode { get; set; }
        public string SessionCode { get; set; }        
        public string TimeFrom { get; set; }
        public string TimeTo { get; set; }
        public string SlotPeriod { get; set; }
        public decimal? DayCode { get; set; }
        public string DayName { get; set; }
        public string DeletedFlag { get; set; }
        public string SessionName { get; set; }
        public string ClinicName { get; set; }        
    }
