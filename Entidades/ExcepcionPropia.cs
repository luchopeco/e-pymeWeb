using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class ExcepcionPropia : Exception
    {
        #region CONSTRUCTORES
        public ExcepcionPropia()
        {
            // = "Error...";
        }
        public ExcepcionPropia(string message): base(message)
        {
            End = false;
        }
        public ExcepcionPropia(string message, Exception inner): base(message, inner)
        {
            End = false;
        }
        public ExcepcionPropia(string message, bool termina): base(message)           
        {
            end = termina;
        }
        #endregion CONSTRUCTORES

        #region PROPIEDADES
        private bool end;
        public bool End
        {
            get { return end; }
            set { end = value; }
        }
        #endregion PROPIEDADES
    }
}
