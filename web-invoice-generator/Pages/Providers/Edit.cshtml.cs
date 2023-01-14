using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Providers
{
    public class EditModel : PageModel
    {

        public ProviderInfo providerInfo = new ProviderInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {

			// reading id
			String id = Request.Query["id"];

			// connecting to db geting info from db and filling object providerInfo
			try
			{
				// connectionString
				String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
				// creating SqlConnection
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					// open the connection
					connection.Open();
					// selecting provider with corresponding id (@id recieved from request)
					String sql = "SELECT * FROM providers WHERE id=@id"; 
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
								// and filling providerInfo with data from db
								providerInfo.id = "" + reader.GetInt32(0); //"" + int = converting int to string
								providerInfo.type = reader.GetString(1);
								providerInfo.name = reader.GetString(2);
								providerInfo.address = reader.GetString(3);
								providerInfo.code = reader.GetString(4);
								providerInfo.vat_code = reader.GetString(5);
								providerInfo.vat_payer = reader.GetString(6);
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
			// filling providerInfo data with data from Form
			providerInfo.id = Request.Form["id"];
			providerInfo.type = Request.Form["type"];
			providerInfo.name = Request.Form["name"];
			providerInfo.address = Request.Form["address"];
			providerInfo.code = Request.Form["code"];
			providerInfo.vat_code = Request.Form["vat_code"];
			providerInfo.vat_payer = Request.Form["vat_payer"];

			// if form empty, displaying error message

			if (providerInfo.id.Length == 0 || providerInfo.type.Length == 0 || providerInfo.name.Length == 0 || providerInfo.address.Length == 0 || providerInfo.code.Length == 0 || providerInfo.vat_code.Length == 0)
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
					String sql = "UPDATE providers " +
						"SET type=@type, name=@name, address=@address, code=@code, vat_code=@vat_code, vat_payer=@vat_payer  " + "WHERE id=@id";
					// replacing @type .. with parameters from form 
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@type", providerInfo.type);
						command.Parameters.AddWithValue("@name", providerInfo.name);
						command.Parameters.AddWithValue("@address", providerInfo.address);
						command.Parameters.AddWithValue("@code", providerInfo.code);
						command.Parameters.AddWithValue("@vat_code", providerInfo.vat_code);
						command.Parameters.AddWithValue("@vat_payer", providerInfo.vat_payer);
						command.Parameters.AddWithValue("@id", providerInfo.id);

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
			// if update providers ok, redirect to index
			Response.Redirect("/Providers/Index");
		}




    }
}
