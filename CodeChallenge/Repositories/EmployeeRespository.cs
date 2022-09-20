using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public async Task<ReportingStructure> GetReportingStructureById(string id)
        {
            //var employee = GetById(id);

            //This is how I have tried this along with FirstOrDefaultAsync
            var employee = await _employeeContext.Employees.SingleOrDefaultAsync(x => x.EmployeeId == id);
            ReportingStructure structure = new();

            if(employee is null)
            {
                return structure;
            }

            structure.Employee = employee;
            var directReports = employee.DirectReports;
            int numOfReports = directReports.Count;
            foreach (var report in directReports)
            {
                //Attemting alternative Calls for same effect
                var reportingEmployee = await _employeeContext.Employees.FirstOrDefaultAsync(y => y.EmployeeId == report.EmployeeId);

                foreach(var x in reportingEmployee.DirectReports)
                {
                    numOfReports++;
                }
            }

            structure.NumberOfReports = numOfReports;
            return structure;

        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
