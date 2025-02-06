using SQLite;

namespace SQLite_ORM_Test
{
    internal class EmployeeService
    {
        private SQLiteAsyncConnection db;

        public EmployeeService(SQLiteAsyncConnection db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retrieves the employee table from the database.
        /// </summary>
        /// <returns>A list of Employee objects.</returns>
        internal async Task<List<Employee>> GetEmployeeTableAsync()
        {
            try
            {
                // Retrieve the employee table
                return await db.Table<Employee>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Employee>();
            }
        }

        /// <summary>
        /// Inserts a new employee into the database.
        /// </summary>
        /// <param name="inputs">A list containing the name, NT ID, and password of the employee.</param>
        internal async Task InsertEmployeeAsync(List<string> inputs)
        {
            if (inputs.Count == 3)
            {
                await db.InsertAsync(new Employee()
                {
                    Name = inputs[0],
                    NT_ID = inputs[1],
                    Password = inputs[2],
                });
            }
            else
            {
                Console.WriteLine("Invalid input data.");
            }
        }

        /// <summary>
        /// Updates an existing employee in the database using NT_ID.
        /// </summary>
        /// <param name="inputs">A list containing the name, NT ID, and password of the employee.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task UpdateEmployeeAsync(List<string> inputs)
        {
            var nt_id = inputs[1];
            if (inputs.Count == 3)
            {
                try
                {
                    // var employee = await  db.Table<Employee>().FirstOrDefaultAsync(x => x.NT_ID == nt_id);
                    var employee = await  db.Table<Employee>().FirstOrDefaultAsync(x => x.NT_ID == nt_id);
                    // var employee = employeeQuery.FirstOrDefault();
                    if (employee != null)
                    {
                        // Update the employee details
                        employee.Name = inputs[0];
                        employee.Password = inputs[2];

                        // Save the updated employee
                        await db.UpdateAsync(employee);
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }
                }
                catch (SQLiteException ex)
                {
                    // Log or handle SQLite-specific exceptions
                    Console.WriteLine($"SQLiteException: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid input data.");
            }
        }


        /// <summary>
        /// Deletes an employee from the database using NT_ID.
        /// </summary>
        /// <param name="nt_id">The NT ID of the employee to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        internal async Task<int> DeleteEmployeeByNT_IDAsync(string? nt_id)
        {
            try
            {
                var employeeToDelete = await db.Table<Employee>().FirstOrDefaultAsync(x => x.NT_ID == nt_id);
                if (employeeToDelete != null)
                {
                    return await db.DeleteAsync(employeeToDelete);
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                    return 0; // Indicates no rows were deleted
                }
            }
            catch (SQLiteException ex)
            {
                // Log or handle SQLite-specific exceptions
                Console.WriteLine($"SQLiteException: {ex.Message}");
                return -1; // Indicates an error occurred
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return -1; // Indicates an error occurred
            }
        }
    }
}
