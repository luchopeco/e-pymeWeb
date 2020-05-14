using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
   public class Menuu
    {
        public int IdMenu{ get; set; }
        public string Nombre { get; set; }
        public List<ItemMenu> ListItems { get; set; }
    }
}
