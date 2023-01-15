using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Orders
{
    public class CreateModel : PageModel
    {

		// global variables
		public OrderInfo orderInfo = new OrderInfo();
        // if any field empty we get error message 
        public String errorMessage = "";
        // if data saved success message
        public String successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read date of the Form ant put to orderInfo object
		public void OnPost()
        {
			orderInfo.services_id = Request.Form["services_id"];
			orderInfo.hours = Request.Form["hours"];
           
            // validating if any field empty
            if (orderInfo.services_id.Length == 0 || orderInfo.hours.Length == 0)
            {
                errorMessage = "Visi laukai turi bûti uþpildyti";
                return;
            }

            // saving the new order info into the database
           try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO orders " +
                        "(services_id, hours) VALUES " +
						"(@services_id, @hours);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@services_id", orderInfo.services_id);
                        command.Parameters.AddWithValue("@hours", orderInfo.hours);

                        command.ExecuteNonQuery();
                    }

                }

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


			orderInfo.services_id = ""; orderInfo.hours = "";
            // if ok:
            successMessage = "Sàskaita faktûra pridëta sëkmingai";
            // if not, returning to the list of the clients:
            Response.Redirect("/Orders/Index");
        }
    }
}
