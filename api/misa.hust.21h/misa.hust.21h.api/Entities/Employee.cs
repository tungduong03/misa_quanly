namespace misa.hust._21h.api.Entities
{
    public class Employee
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set;}
        public string IdentityNumber { get; set;}
        public string IdentityIssuedPlace { get; set; }
        public DateTime IdentityIssuedDate { get; set; }
        public string Email { get; set;}
        public string PhoneNumber { get; set;}
        public Guid PositionID { get; set;}
        public string PositionName { get; set;}
        public Guid DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int HomeTelephone { get; set; }
        public int MobilePhone { get; set;}
        public string AccountNumber { get; set;}
        public string Bank { get; set;}
        public string BankBranch { get; set;}
        public int Functions { get; set;}
        public string Address { get; set; }
        public DateTime CreatedDate { get; set;}
        public string CreatedBy { get; set;}
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }



    }
}
