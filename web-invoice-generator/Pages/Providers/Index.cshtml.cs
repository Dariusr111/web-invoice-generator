using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace web_invoice_generator.Pages.Providers
{
    public class IndexModel : PageModel
    {
        // to store all providers creating listProviders (public variable)
        public List<ProviderInfo> listProviders = new List<ProviderInfo>();

        // filling this list with OnGet method
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM providers";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                ProviderInfo providerInfo = new ProviderInfo();
                                providerInfo.id = "" + reader.GetInt32(0);
                                providerInfo.type = reader.GetString(1);
                                providerInfo.name = reader.GetString(2);
                                providerInfo.address = reader.GetString(3);  
                                providerInfo.code = reader.GetString(4);
                                providerInfo.vat_code = reader.GetString(5);
                                providerInfo.vat_payer = reader.GetString(5);
                                providerInfo.created_at = reader.GetDateTime(6).ToString();
                                
                                // adding object clientInfo to our list
                                listProviders.Add(providerInfo);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)

            {
                // adding statement to show error incase exeption
                Console.WriteLine("Exeption: " + ex.ToString());
            }
        }
    }

    // class ClientInfo allow to store one client data
    public class ProviderInfo
    {
        public String id;
        public String type;
        public String name;
        public String address;
        public String code;
        public String vat_code;
        public String vat_payer;
        public String created_at;
    }
}
