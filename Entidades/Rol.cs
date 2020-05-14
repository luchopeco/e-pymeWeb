using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Descripcion { get; set; }

        List<Funcionalidad> listFuncionalidades = new List<Funcionalidad>();
        public List<Funcionalidad> ListFuncionalidades
        {
            get { return listFuncionalidades; }
            set { listFuncionalidades = value; }
        }
        List<Pagina> listPaginas = new List<Pagina>();

        public List<Pagina> ListPaginas
        {
            get { return listPaginas; }
            set { listPaginas = value; }
        }
    }
}
