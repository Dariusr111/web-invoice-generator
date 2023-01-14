using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Services
{
    public class CreateModel : PageModel
    {

		// global variables
		public ServiceInfo serviceInfo = new ServiceInfo();
        // if any field empty - error message 
        public String errorMessage = "";
        // if data saved - success message
        public String successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read data of the Form ant put to serviceInfo object
		public void OnPost()
        {
            serviceInfo.name = Request.Form["name"];
			serviceInfo.hour_price = Request.Form["hour_price"];

            // validating if any field empty
            if (serviceInfo.name.Length == 0 || serviceInfo.hour_price.Length == 0)
            {
                errorMessage = "Visi laukai turi bûti uþpildyti";
                return;
            }

            // save the new service info into the database

           try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO services " +
                        "(name, hour_price) VALUES " +
                        "(@name, @hour_price);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", serviceInfo.name);
                        command.Parameters.AddWithValue("@hour_price", serviceInfo.hour_price);

                        command.ExecuteNonQuery();
                    }

                }

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


            serviceInfo.name = ""; serviceInfo.hour_price = "";
            // if ok:
            successMessage = "Paslauga pridëta sëkmingai";
            // if not, returning to the list of the clients:
            Response.Redirect("/Services/Index");
        }
    }
}
