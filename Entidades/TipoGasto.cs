using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    [ABMClass("Negocio", "ControladorGastos", "BuscarListTipoGasto", "ModificarTipoGasto", "EliminarTipoGasto", "AgregarTipoGasto", "BuscarTipoGasto", "Tipo Gasto")]
    public class TipoGasto
    {
        [ABMProperty(false, true, false)]
        public int IdTipoGasto { get; set; }
        public string Descripcion { get; set; }
    }
}
