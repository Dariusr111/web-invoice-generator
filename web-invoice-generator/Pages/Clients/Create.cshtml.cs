using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace web_invoice_generator.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        // if any field empty we get error message (global var)
        public String errorMessage = "";
        // if data saved success message (global var)
        public String successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read date of the Form ant put to clientInfo object
		public void OnPost()
        {
            clientInfo.type = Request.Form["type"];
            clientInfo.name = Request.Form["name"];
            clientInfo.address = Request.Form["address"];
            clientInfo.code = Request.Form["code"];
            clientInfo.vat_code = Request.Form["vat_code"];

            // validating if any field empty
            if (clientInfo.type.Length == 0 || clientInfo.name.Length == 0 || clientInfo.address.Length == 0 || clientInfo.code.Length == 0 || clientInfo.vat_code.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            // save the new client info into the database

           try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " +
                        "(type, name, address, code, vat_code) VALUES " +
                        "(@type, @name, @address, @code, @vat_code);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@type", clientInfo.type);
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@code", clientInfo.code);
                        command.Parameters.AddWithValue("@vat_code", clientInfo.vat_code);

                        command.ExecuteNonQuery();
                    }

                }

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


            clientInfo.type = ""; clientInfo.name = ""; clientInfo.address = ""; clientInfo.code = ""; clientInfo.vat_code = "";
            // if ok:
            successMessage = "Klientas pridëtas sëkmingai";
            // if not, returning to the list of the clients:
            Response.Redirect("/Clients/Index");
        }
    }
}
