using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Clients
{
    public class EditModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

			// reading id
			String id = Request.Query["id"];

			// connecting to db geting info from db and filling object clientInfo
			try
			{
				// connectionString
				String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
				// creating SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// open the connection
					connection.Open();
					// selecting client with corresponding id (@id recieved from request)
					String sql = "SELECT * FROM clients WHERE id=@id"; 
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
								clientInfo.id = "" + reader.GetInt32(0); //"" + int = converting int to string
								clientInfo.type = reader.GetString(1);
								clientInfo.name = reader.GetString(2);
								clientInfo.address = reader.GetString(3);
								clientInfo.code = reader.GetString(4);
								clientInfo.vat_code = reader.GetString(5);
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
			clientInfo.id = Request.Form["id"];
			clientInfo.type = Request.Form["type"];
			clientInfo.name = Request.Form["name"];
			clientInfo.address = Request.Form["address"];
			clientInfo.code = Request.Form["code"];
			clientInfo.vat_code = Request.Form["vat_code"];

			// if form empty, displaying error message

			if (clientInfo.id.Length == 0 || clientInfo.type.Length == 0 || clientInfo.name.Length == 0 || clientInfo.address.Length == 0 || clientInfo.code.Length == 0 || clientInfo.vat_code.Length == 0)
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
					String sql = "UPDATE clients " +
						"SET type=@type, name=@name, address=@address, code=@code, vat_code=@vat_code " + "WHERE id=@id";
					// replacing @type .. with parameters from form 
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@type", clientInfo.type);
						command.Parameters.AddWithValue("@name", clientInfo.name);
						command.Parameters.AddWithValue("@address", clientInfo.address);
						command.Parameters.AddWithValue("@code", clientInfo.code);
						command.Parameters.AddWithValue("@vat_code", clientInfo.vat_code);
						command.Parameters.AddWithValue("@id", clientInfo.id);

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
			// if update clients ok, redirect to index
			Response.Redirect("/Clients/Index");
		}




    }
}
