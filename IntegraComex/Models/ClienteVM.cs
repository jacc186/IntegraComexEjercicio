using System.Collections.Generic;

namespace IntegraComex.Models
{
    public class ClienteVM
    {
        public int Id { get; set; }
        public string NroCuit { get; set; }
        public string RazonSocial { get; set; }
        public long Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }

        public List<Cliente> ListaClientes { get; set; }
    }
}
