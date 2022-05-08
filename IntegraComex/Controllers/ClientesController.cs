using IntegraComex.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Net;

namespace IntegraComex.Controllers
{
    public class ClientesController : Controller
    {
        public readonly ClientesContext context;

        public ClientesController(ClientesContext context)
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
        public IActionResult Index(string cuit, int telPais, int telPersonal, string direccion)
        {
            if (cuit != null && telPais != 0 && telPersonal != 0 && direccion != null)
            {
                string telCompleto = telPais.ToString() + telPersonal.ToString();
                long telefonoCompleto = long.Parse(telCompleto);
                var url = "http://sistemaintegracomex.com.ar:581/?cuit=";
                url += cuit;
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "application/json";
                request.ContentType = "application/json; charset=utf-8";
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusDescription == "OK")
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string json = reader.ReadToEnd(); 
                    var jObject = JObject.Parse(json);
                    var jToken = jObject.GetValue("data");
                    Data value = jToken.ToObject<Data>();
                    var cliente = new Cliente
                    {
                        Activo = true,
                        Direccion = direccion,
                        Telefono = telefonoCompleto,
                        NroCuit = cuit,
                        RazonSocial = value.Nombre
                    };
                    context.Clientes.Add(cliente);
                    context.SaveChanges();
                }
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
