using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace EnterpriseBudget.Model
{
    /// <summary>
    /// The different type of jobs that are available
    /// </summary>
    public enum JobTypes
    {
        /// <summary>
        /// No job specified
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Teacher (faculty)
        /// </summary>
        Faculty = 1,

        /// <summary>
        /// chair person
        /// </summary>
        Chair = 2,

        /// <summary>
        /// job with absolute power
        /// </summary>
        Admin = 3
    } 
    
    /// <summary>
    /// Employee class
    /// </summary>
    public class Employee
    {
        JobTypes _jobType;
        String _jobName;
        int _deptId;
        String _departmentName;
        String _userName;
        
        /// <summary>
        /// what job type does the employee have?
        /// </summary>
        public JobTypes jobType { get { return _jobType; } }

        /// <summary>
        /// what is the department name of the employee
        /// </summary>
        public String departmentName { get { return _departmentName; } }

        /// <summary>
        /// what is the name of the employee's job?
        /// </summary>
        public String jobName { get { return _jobName; } }

        /// <summary>
        /// employee name
        /// </summary>
        public String userName { get { return _userName; } }

        /// <summary>
        /// employee department id
        /// </summary>
        public int deptartmentID { get { return _deptId; } }

        /// <summary>
        /// Verify that there is an employee with username/password
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>an Employee object if such a user exists, null otherwise</returns>
        public static Employee validateUser(String username, String password)
        {
            Employee person = new Employee();
            SqlDataReader rdr = null;
            try
            {
                SqlCommand userInfo = Connection.cnn.CreateCommand();

                int departmentId =0;
                int jobId=0 ;
                // get user departmentId and jobId
                userInfo.CommandText = "SELECT departmentId, jobId FROM employees WHERE name = @name AND password = @password";

                userInfo.Parameters.AddWithValue("@name", username);
                userInfo.Parameters.AddWithValue("@password", password);

                rdr = userInfo.ExecuteReader();

                while (rdr.Read())
                {
                    departmentId= rdr.GetInt32(0);
                    jobId = rdr.GetInt32(1);
                }
                person._deptId = departmentId;
                person._jobType = (JobTypes)jobId;

                rdr.Close();

                // get the name of the department of user  
                
                userInfo.CommandText = "SELECT name FROM departments WHERE id = @departmentId";

                userInfo.Parameters.AddWithValue("@departmentId", departmentId );
                rdr = userInfo.ExecuteReader();

                while (rdr.Read())
                {
                    person._departmentName = rdr.GetString(0);
                }

                rdr.Close();

                // get the name of the job of user  
                userInfo.CommandText = "SELECT name FROM jobTitles WHERE id = @jobId";

                userInfo.Parameters.AddWithValue("@jobId", jobId);
                rdr = userInfo.ExecuteReader();

                while (rdr.Read())
                {
                    person._jobName = rdr.GetString(0);
                }

                rdr.Close();

            }
            catch (Exception e) {
                var txt = e.Message;
            }
            if (rdr != null)
            {
                try { rdr.Close(); } catch { }
            }
            return person;
        }

        private Employee()
        {
        }

    }
}
