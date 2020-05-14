using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Pagina
    {
        public int Idpagina { get; set; }
        public string Descripcion { get; set; }
        public string NombrePagina { get; set; }
        public Boolean SoloLectura { get; set; }
        public Boolean ReingresaClave { get; set; }
        public Boolean PideAutorizacion { get; set; }
        public Boolean Restringido { get; set; }
        public string Codigo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Pagina p = obj as Pagina;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (Idpagina == p.Idpagina) && (SoloLectura == p.SoloLectura) && (ReingresaClave == p.ReingresaClave) && (PideAutorizacion == p.PideAutorizacion) && (Restringido == p.Restringido);
        }
        public bool Equals(Pagina p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return (Idpagina == p.Idpagina) && (SoloLectura == p.SoloLectura) && (ReingresaClave == p.ReingresaClave) && (PideAutorizacion == p.PideAutorizacion) && (Restringido == p.Restringido);
        }
    }
}
