using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace web_invoice_generator.Pages.Services
{
    public class IndexModel : PageModel
    {
        // to store all services creating listServices (public variable)
        public List<ServiceInfo> listServices = new List<ServiceInfo>();

        // filling this list with OnGet method
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM services";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                ServiceInfo serviceInfo = new ServiceInfo();
								serviceInfo.id = "" + reader.GetInt32(0);
								serviceInfo.name = reader.GetString(1);
								serviceInfo.hour_price = "" + reader.GetInt32(2);
								serviceInfo.created_at = reader.GetDateTime(3).ToString();
                                
                                // adding object clientInfo to our list
                                listServices.Add(serviceInfo);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)

            {
                // adding statement to show error incase exeption
                Console.WriteLine("Exeption: " + ex.ToString());
            }
        }
    }

    // class ServiceInfo allow to store one client data
    public class ServiceInfo
    {
        public String id;
        public String name;
        public String hour_price;
        public String created_at;
    }
}
