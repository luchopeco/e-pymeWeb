using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
   public class Funcionalidad
    {
        public int IdFuncionalidad { get; set; }
        public string Descripcion { get; set; }
        public string NombreClase { get; set; }
        public Boolean SoloLectura { get; set; }
        public Boolean ReingresaClave { get; set; }
        public Boolean PideAutorizacion { get; set; }
        public Boolean Restringido { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Funcionalidad f = obj as Funcionalidad;
            if ((System.Object)f == null)
            {
                return false;
            }

            return (IdFuncionalidad == f.IdFuncionalidad) && (SoloLectura == f.SoloLectura) && (ReingresaClave == f.ReingresaClave) && (PideAutorizacion == f.PideAutorizacion) && (Restringido == f.Restringido);
        }
        public bool Equals(Funcionalidad f)
        {
            if ((object)f == null)
            {
                return false;
            }

            return (IdFuncionalidad == f.IdFuncionalidad) && (SoloLectura == f.SoloLectura) && (ReingresaClave == f.ReingresaClave) && (PideAutorizacion == f.PideAutorizacion) && (Restringido == f.Restringido);
        }
    }
}
