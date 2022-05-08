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
        public IActionResult Editar(int id)
        {
            var model = context.Clientes.Find(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            cliente.Activo = true;
            context.Attach(cliente);
            context.Entry(cliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Borrar(int id)
        {
            if(id != 0)
            {
                var model = new Cliente();
                model = context.Clientes.Find(id);
                context.Clientes.Remove(model);
                context.SaveChanges();
            }
            return RedirectToAction("Index","Clientes");
        }
    }
}
