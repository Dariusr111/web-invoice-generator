using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using web_invoice_generator.Pages.Orders;

namespace web_invoice_generator.Pages.Orders
{
    public class EditModel : PageModel
    {

        public OrderInfo orderInfo = new OrderInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

			// reading id
			String id = Request.Query["id"];

			// connecting to db geting info from db and filling object orderInfo
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
					String sql = "SELECT * FROM orders WHERE id=@id"; 
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
								// and filling serviceInfo with data from db
								orderInfo.id = "" + reader.GetInt32(0); //"" + int = converting int to string
								orderInfo.invoice_id = "" + reader.GetInt32(1);
								orderInfo.services_id = "" + reader.GetInt32(2);
								orderInfo.hours = "" + reader.GetInt32(3);
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
			// filling orderInfo data with data from Form
			orderInfo.id = Request.Form["id"];
			orderInfo.invoice_id = Request.Form["invoice_id"];
			orderInfo.services_id = Request.Form["services_id"];
			orderInfo.hours = Request.Form["hours"];

			// if form empty, displaying error message

			if (orderInfo.id.Length == 0 || orderInfo.invoice_id.Length == 0 || orderInfo.services_id.Length == 0 || orderInfo.hours.Length == 0)
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
					String sql = "UPDATE orders " +
						"SET invoice_id=@invoice_id, services_id=@services_id, hours=@hours " + "WHERE id=@id";
					// replacing @invoice_id .. with parameters from Form 
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@invoice_id", orderInfo.invoice_id);
						command.Parameters.AddWithValue("@services_id", orderInfo.services_id);
						command.Parameters.AddWithValue("@hours", orderInfo.hours);
						command.Parameters.AddWithValue("@id", orderInfo.id);

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
			Response.Redirect("/Orders/Index");
		}




    }
}
