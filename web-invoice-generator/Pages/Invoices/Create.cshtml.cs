using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Invoices
{
    public class CreateModel : PageModel
    {

		// global variables
		public InvoiceInfo invoiceInfo = new InvoiceInfo();
        // if any field empty we get error message 
        public String errorMessage = "";
        // if data saved success message
        public String successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read date of the Form ant put to clientInfo object
		public void OnPost()
        {
			invoiceInfo.serial = Request.Form["serial"];
			invoiceInfo.number = Request.Form["number"];
			invoiceInfo.client_id = Request.Form["client_id"];
			invoiceInfo.provider_id = Request.Form["provider_id"];
			invoiceInfo.total_price = Request.Form["total_price"];
           
            // validating if any field empty
            if (invoiceInfo.serial.Length == 0 || invoiceInfo.number.Length == 0 || invoiceInfo.client_id.Length == 0 || invoiceInfo.provider_id.Length == 0 || invoiceInfo.total_price.Length == 0)
            {
                errorMessage = "Visi laukai turi bûti uþpildyti";
                return;
            }

            // saving the new invoice info into the database
           try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO invoices " +
                        "(serial, number, client_id, provider_id, total_price) VALUES " +
                        "(@serial, @number, @client_id, @provider_id, @total_price);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@serial", invoiceInfo.serial);
                        command.Parameters.AddWithValue("@number", invoiceInfo.number);
                        command.Parameters.AddWithValue("@client_id", invoiceInfo.client_id);
                        command.Parameters.AddWithValue("@provider_id", invoiceInfo.provider_id);
                        command.Parameters.AddWithValue("@total_price", invoiceInfo.total_price);

                        command.ExecuteNonQuery();
                    }

                }

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


			invoiceInfo.serial = ""; invoiceInfo.number = ""; invoiceInfo.client_id = ""; invoiceInfo.provider_id = ""; invoiceInfo.total_price = "";
            // if ok:
            successMessage = "Sàskaita faktûra pridëta sëkmingai";
            // if not, returning to the list of the clients:
            Response.Redirect("/Invoices/Index");
        }
    }
}
