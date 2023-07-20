namespace misa.hust._21h.api.Entities
{
    public class Position
    {
        /// <summary>
        /// ID vị trí 
        /// </summary>
        public Guid PositionID { get; set; }
        public string PositionName { get; set; }
        public string PositionCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

    }
}
