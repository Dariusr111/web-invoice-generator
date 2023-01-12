using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

           



            clientInfo.type = ""; clientInfo.name = ""; clientInfo.address = ""; clientInfo.code = ""; clientInfo.vat_code = "";
            successMessage = "Klientas pridëtas sëkmingai";
        }
    }
}
