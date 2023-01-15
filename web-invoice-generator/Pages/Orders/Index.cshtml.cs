using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using web_invoice_generator.Pages.Invoices;


namespace web_invoice_generator.Pages.Orders
{
    public class IndexModel : PageModel
    {
        // to store all invoices creating listOrders (public variable)
        public List<OrderInfo> listOrders = new List<OrderInfo>();

        // filling this list with OnGet method
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM orders";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                OrderInfo orderInfo = new OrderInfo();
								orderInfo.id = "" + reader.GetInt32(0);
								orderInfo.invoice_id = "" + reader.GetInt32(1);
								orderInfo.services_id = "" + reader.GetInt32(2);
                                orderInfo.hours = "" + reader.GetInt32(3);
								orderInfo.created_at = reader.GetDateTime(4).ToString();

								// adding object clientInfo to our list
								listOrders.Add(orderInfo);
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

    // class OrderInfo allow to store one client data
    public class OrderInfo
    {
        public String id;
        public String invoice_id;
        public String services_id;
        public String hours;
        public String created_at;
    }
}
