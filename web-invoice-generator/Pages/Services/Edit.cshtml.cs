using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Services
{
    public class EditModel : PageModel
    {

        public ServiceInfo serviceInfo = new ServiceInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

			// reading id
			String id = Request.Query["id"];

			// connecting to db geting info from db and filling object serviceInfo
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
					String sql = "SELECT * FROM services WHERE id=@id"; 
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
								// and filling clientInfo with data from db
								serviceInfo.id = "" + reader.GetInt32(0); //"" + int = converting int to string
								serviceInfo.name = reader.GetString(1);
								serviceInfo.hour_price = "" + reader.GetInt32(2);
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
			// filling clientInfo data with data from Form
			serviceInfo.id = Request.Form["id"];
			serviceInfo.name = Request.Form["name"];
			serviceInfo.hour_price = Request.Form["hour_price"];

			// if form empty, displaying error message

			if (serviceInfo.id.Length == 0 || serviceInfo.name.Length == 0 || serviceInfo.hour_price.Length == 0)
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
					String sql = "UPDATE services " +
						"SET name=@name, hour_price=@hour_price " + "WHERE id=@id";
					// replacing @name .. with parameters from Form 
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", serviceInfo.name);
						command.Parameters.AddWithValue("@hour_price", serviceInfo.hour_price);
						command.Parameters.AddWithValue("@id", serviceInfo.id);

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
			Response.Redirect("/Services/Index");
		}




    }
}
