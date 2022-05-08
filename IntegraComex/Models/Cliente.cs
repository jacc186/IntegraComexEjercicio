using System;
using System.Collections.Generic;

#nullable disable

namespace IntegraComex.Models
{
    public partial class Cliente
    {
        public int Id { get; set; }
        public string NroCuit { get; set; }
        public string RazonSocial { get; set; }
        public long Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Activo { get; set; }
    }
}
