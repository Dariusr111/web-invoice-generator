using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace web_invoice_generator.Pages.Invoices
{
    public class IndexModel : PageModel
    {
        // to store all invoices creating listInvoices (public variable)
        public List<InvoiceInfo> listInvoices = new List<InvoiceInfo>();

        // filling this list with OnGet method
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=localhost\\sqlexpress;Initial Catalog=invoice-gen;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM invoices";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                InvoiceInfo invoiceInfo = new InvoiceInfo();
								invoiceInfo.id = "" + reader.GetInt32(0);
								invoiceInfo.serial = reader.GetString(1);
								invoiceInfo.number = "" + reader.GetInt32(2);
								invoiceInfo.client_id = "" + reader.GetInt32(3);
								invoiceInfo.provider_id = "" + reader.GetInt32(4);
								invoiceInfo.created_at = reader.GetDateTime(5).ToString();
                                
                                // adding object clientInfo to our list
                                listInvoices.Add(invoiceInfo);
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

    // class InvoiceInfo allow to store one client data
    public class InvoiceInfo
    {
        public String id;
        public String serial;
        public String number;
        public String client_id;
        public String provider_id;
        public String total_price;
        public String created_at;
    }
}
