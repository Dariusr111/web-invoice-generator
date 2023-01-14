using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using web_invoice_generator.Pages.Providers;

namespace web_invoice_generator.Pages.Provider
{
    public class CreateModel : PageModel
    {

		// global variables
		public ProviderInfo providerInfo = new ProviderInfo();
        // if any field empty we get error message 
        public String errorMessage = "";
        // if data saved success message
        public String successMessage = "";

        public void OnGet()
        {
        }

		// with method we can read date of the Form and put to providerInfo object
		public void OnPost()
        {
            providerInfo.type = Request.Form["type"];
            providerInfo.name = Request.Form["name"];
            providerInfo.address = Request.Form["address"];
            providerInfo.code = Request.Form["code"];
            providerInfo.vat_code = Request.Form["vat_code"];
            providerInfo.vat_payer = Request.Form["vat_payer"];

            // validating if any field empty
            if (providerInfo.type.Length == 0 || providerInfo.name.Length == 0 || providerInfo.address.Length == 0 || providerInfo.code.Length == 0 || providerInfo.vat_code.Length == 0)
            {
                errorMessage = "Visi laukai turi bûti uþpildyti";
                return;
            }

            // save the new client info into the database

           try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO providers " +
                        "(type, name, address, code, vat_code, vat_payer) VALUES " +
                        "(@type, @name, @address, @code, @vat_code, @vat_payer);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@type", providerInfo.type);
                        command.Parameters.AddWithValue("@name", providerInfo.name);
                        command.Parameters.AddWithValue("@address", providerInfo.address);
                        command.Parameters.AddWithValue("@code", providerInfo.code);
                        command.Parameters.AddWithValue("@vat_code", providerInfo.vat_code);
                        command.Parameters.AddWithValue("@vat_payer", providerInfo.vat_payer);

                        command.ExecuteNonQuery();
                    }

                }

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;

            }


            providerInfo.type = ""; providerInfo.name = ""; providerInfo.address = ""; providerInfo.code = ""; providerInfo.vat_code = "";
            // if ok:
            successMessage = "Tiekëjas pridëtas sëkmingai";
            // if not, returning to the list of the providers:
            Response.Redirect("/Providers/Index");
        }
    }
}
