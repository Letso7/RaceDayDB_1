namespace RaceDayDB1.Models
{
    public class Enrolment
    {
        public int EnrolmentId { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int CategoryId { get; set; }
        public DateTime EnrolmentDate { get; set; } = DateTime.Now;
    }
}