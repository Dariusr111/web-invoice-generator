using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Invoices
{
    public class EditModel : PageModel
    {

        public InvoiceInfo invoiceInfo = new InvoiceInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

			// reading id
			String id = Request.Query["id"];

			// connecting to db geting info from db and filling object invoiceInfo
			try
			{
				// connectionString
				String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
				// creating SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// open the connection
					connection.Open();
					// selecting service with corresponding id (@id recieved from request)
					String sql = "SELECT * FROM invoices WHERE id=@id"; 
					// creating command which let us execute sql query
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// replacing @id with id recieved from request
						command.Parameters.AddWithValue("@id", id);
						// creating SqlDataReader
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								// and filling invoiceInfo with data from db
								invoiceInfo.id = "" + reader.GetInt32(0); //"" + int = converting int to string
								invoiceInfo.serial = reader.GetString(1);
								invoiceInfo.number = "" + reader.GetInt32(2);
								invoiceInfo.client_id = "" + reader.GetInt32(3);
								invoiceInfo.provider_id = "" + reader.GetInt32(4);
								invoiceInfo.total_price = "" + reader.GetInt32(5);
							}
						}

					}

				}

			}

			catch (Exception ex)
			{
				errorMessage = ex.Message;
			}
		}

		public void OnPost()
		{
			// filling invoiceInfo data with data from Form
			invoiceInfo.id = Request.Form["id"];
			invoiceInfo.serial = Request.Form["serial"];
			invoiceInfo.number = Request.Form["number"];
			invoiceInfo.client_id = Request.Form["client_id"];
			invoiceInfo.provider_id = Request.Form["provider_id"];
			invoiceInfo.total_price = Request.Form["total_price"];

			// if form empty, displaying error message

			if (invoiceInfo.id.Length == 0 || invoiceInfo.serial.Length == 0 || invoiceInfo.number.Length == 0 || invoiceInfo.client_id.Length == 0 || invoiceInfo.provider_id.Length == 0 || invoiceInfo.total_price.Length == 0)
			{
				errorMessage = "Visi laukai turi bûti uþpildyti";
				return;
			}


            // in the try conecting to db
			try
			{
				String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE invoices " +
						"SET serial=@serial, number=@number, client_id=@client_id, provider_id=@provider_id, total_price=@total_price " + "WHERE id=@id";
					// replacing @serial .. with parameters from Form 
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@serial", invoiceInfo.serial);
						command.Parameters.AddWithValue("@number", invoiceInfo.number);
						command.Parameters.AddWithValue("@client_id", invoiceInfo.client_id);
						command.Parameters.AddWithValue("@provider_id", invoiceInfo.provider_id);
						command.Parameters.AddWithValue("@total_price", invoiceInfo.total_price);		
						command.Parameters.AddWithValue("@id", invoiceInfo.id);

						command.ExecuteNonQuery();
					}

				}

			}

			catch(Exception ex)
			{
				// errorMessage when we have to exit method
				errorMessage = ex.Message;
				return;
			}
			// if update Services ok, redirect to index
			Response.Redirect("/Invoices/Index");
		}
    }
}
