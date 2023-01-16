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
                    // String sql = "SELECT * FROM invoices";
                    String sql = "SELECT i.id AS i_id, serial, number, p.name AS p_name, c.name AS c_name, s.name AS ser_name, s.hour_price*o.hours AS total_price, i.created_at AS i_created_at FROM orders o LEFT JOIN invoices i ON i.id = o.invoice_id LEFT JOIN services s ON o.services_id = s.id LEFT JOIN providers p ON i.provider_id = p.id LEFT JOIN clients c ON i.client_id = c.id";

					using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            { 
                                InvoiceInfo invoiceInfo = new InvoiceInfo();
								invoiceInfo.i_id = "" + reader.GetInt32(0);
								invoiceInfo.serial = reader.GetString(1);
								invoiceInfo.number = reader.GetString(2);
								invoiceInfo.p_name = reader.GetString(3);
								invoiceInfo.c_name = reader.GetString(4);
								invoiceInfo.ser_name = reader.GetString(5);
                                invoiceInfo.total_price = "" + reader.GetInt32(6);
								invoiceInfo.i_created_at = reader.GetDateTime(7).ToString();
                                
                                // adding object invoiceInfo to our list
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
        public String i_id;
        public String serial;
        public String number;
        public String p_name;
        public String c_name;
        public String ser_name;
        public String total_price;
        public String i_created_at;
    }
}
