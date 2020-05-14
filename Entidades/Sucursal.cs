using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}
