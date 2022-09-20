namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }

        public ReportingStructure ()
        {

        }

        public ReportingStructure (Employee inEmployee, int inNumberOfReports)
        {
            Employee = inEmployee;
            NumberOfReports = inNumberOfReports;
        }
    }
}
