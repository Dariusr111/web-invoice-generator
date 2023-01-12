using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace web_invoice_generator.Pages.Clients
{
    public class IndexModel : PageModel
    {
        // to store all clients creating listClients (public variable)
        public List<ClientInfo> listClients = new List<ClientInfo>();

        // filling this list with OnGet method
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.type = reader.GetString(1);
                                clientInfo.name = reader.GetString(2);
                                clientInfo.address = reader.GetString(3);  
                                clientInfo.code = reader.GetString(4);
                                clientInfo.vat_code = reader.GetString(5);
                                clientInfo.created_at = reader.GetDateTime(6).ToString();
                                
                                // adding object clientInfo to our list
                                listClients.Add(clientInfo);


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
    public class ClientInfo
    {
        public String id;
        public String type;
        public String name;
        public String address;
        public String code;
        public String vat_code;
        public String created_at;
    }
}
