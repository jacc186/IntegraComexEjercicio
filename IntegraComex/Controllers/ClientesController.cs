using IntegraComex.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace IntegraComex.Controllers
{
    public class ClientesController : Controller
    {
        public readonly clientesContext context;

        public ClientesController(clientesContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ClienteVM clientes = new ClienteVM();
            clientes.ListaClientes = context.Clientes.ToList();
            return View(clientes);
        }
        [HttpPost]
        public IActionResult Index(string cuit, int telefono, string direccion)
        {
            if(cuit != null && telefono != 0 && direccion != null)
            {
                var url = "http://sistemaintegracomex.com.ar:581/cuit?=";
                url += cuit;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse(); 

            }
            ClienteVM clientes = new ClienteVM();
            clientes.ListaClientes = context.Clientes.ToList();
            return View(clientes);
        }
        public IActionResult Modificar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Modificar(ClienteVM cliente)
        {
            return View();
        }
        public IActionResult Borrar()
        {
            return View();
        }
    }
}
