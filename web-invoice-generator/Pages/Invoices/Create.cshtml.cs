using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace web_invoice_generator.Pages.Invoices
{
    public class CreateModel : PageModel
    {

		// global variables
		public InvoiceInfo invoiceInfo = new InvoiceInfo();
        // if any field empty we get error message 
        public string errorMessage = "";
        // if data saved success message
        public string successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read date of the Form ant put to invoiceInfo object
		public void OnPost()
        {
			invoiceInfo.serial = Request.Form["serial"];
			invoiceInfo.number = Request.Form["number"];
 
           
            // validating if any field empty
            if (invoiceInfo.serial.Length == 0 || invoiceInfo.number.Length == 0)
            {
                errorMessage = "Visi laukai turi bûti uþpildyti";
                return;
            }

            // saving the new order info into the database
           try
            {
				string connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
					string sql = "INSERT INTO invoices " +
                        "(serial, number) VALUES " +
                    "(@serial, @number);";

					// BEGIN TRANSACTION
                    //   DECLARE @DataID int;
					//   INSERT INTO DataTable(Column1...) VALUES(....);
					//   SELECT @DataID = scope_identity();
					//   INSERT INTO LinkTable VALUES(@ObjectID, @DataID);
					// COMMIT





					using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@serial", invoiceInfo.serial);
                        command.Parameters.AddWithValue("@number", invoiceInfo.number);

                        command.ExecuteNonQuery();
                    }

                }


			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


			invoiceInfo.serial = ""; invoiceInfo.number = "";
            // if ok:
            successMessage = "Sàskaita faktûra pridëta sëkmingai";
            // if not, returning to the list of the clients:
            Response.Redirect("/Invoices/Index");
        }
    }
}


