using SQLite;

namespace SQLite_ORM_Test
{
    class Program
    {
        /// <summary>
        /// Get values from Terminal
        /// </summary>
        /// <returns></returns>
        public static List<string> GetInputs()
        {
            Console.Write("Enter Values:\nName : ");
            var name = Console.ReadLine()!;
            Console.Write("NT ID : ");
            var nt_id = Console.ReadLine()!;
            Console.Write("Password : ");
            var password = Console.ReadLine()!;
            return new List<string> { name, nt_id, password };
        }

        public static async Task DisplayEmployeeTableAsync(EmployeeService employeeService)
        {
            var table = await employeeService.GetEmployeeTableAsync();
            System.Console.WriteLine("---Table---\nName\tNT_ID\t");
            foreach (var employee in table)
            {
                Console.Write($"{employee.Name}\t{employee.NT_ID}\n");
            }
            System.Console.WriteLine("---Table---");
        }

        public static async Task MenuDriver(EmployeeService employeeService)
        {
            await DisplayEmployeeTableAsync(employeeService);

            string Menu = @"
---------- Menu ----------
1. Display Employee Table
2. Insert a Value
3. Update a Value
4. Delete a Value
5. Exit
--------------------------
>>> ";
            string? input;
            do
            {
                Console.Write(Menu);
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        await DisplayEmployeeTableAsync(employeeService);
                        break;
                    case "2":
                        await employeeService.InsertEmployeeAsync(GetInputs());
                        break;
                    case "3":
                        await employeeService.UpdateEmployeeAsync(GetInputs());
                        break;
                    case "4":
                        System.Console.WriteLine("Enter NT_ID : ");
                        await employeeService.DeleteEmployeeByNT_IDAsync(Console.ReadLine());
                        break;
                    case "5":
                        break;
                    default:
                        break;
                }
            } while (input != "5");
        }

        public static async Task StartAndRunConnectionAsync()
        {
            var directoryPath = "Databases";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var dbPath = Path.Combine(directoryPath, "employee.db");

            try
            {
                var db = new SQLiteAsyncConnection(dbPath);

                var employeeService = new EmployeeService(db);

                var tableInfo = await db.GetTableInfoAsync("Employee");
                if (tableInfo.Count == 0)
                {
                    await db.CreateTableAsync<Employee>();
                    /// inserting dummy data
                    await employeeService.InsertEmployeeAsync(new List<string> {
                    "Admin",
                    "0001",
                    "987654321",
                });
                    for (int i = 1; i < 10; i++)
                    {
                        await employeeService.InsertEmployeeAsync(
                            new List<string>{
                            $"DummyName {i}",
                            $"{i}",
                            $"DummyPassword_{i}",
                            }
                        );
                    }
                    Console.WriteLine("Table created!");
                }
                else
                {
                    Console.WriteLine("Table already exists.");
                }

                if (File.Exists(dbPath))
                {
                    Console.WriteLine($"Database file created at: {dbPath}");
                }
                else
                {
                    Console.WriteLine("Database file not found.");
                }

                await MenuDriver(employeeService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public static void Main(string[] args)
        {
            StartAndRunConnectionAsync().Wait();
        }
    }
}
