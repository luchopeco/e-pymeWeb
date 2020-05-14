using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class TipoMovimientoArticulo
    {
        public int IdTipoMovimiento { get; set; }
        public string Descripcion { get; set; }
        public bool EsSuma { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}
