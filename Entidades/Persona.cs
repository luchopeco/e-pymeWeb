using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable]
    public class Persona
    {
        // Referencia: persona.idpersona
        public System.Int32 Idpersona { get; set; }
        // Referencia: persona.nombre_persona
        public System.String NombrePersona { get; set; }
        // Referencia: persona.direccion_persona
        public System.String DireccionPersona { get; set; }
        // Referencia: persona.telefono_persona
        public System.String TelefonoPersona { get; set; }
        // Referencia: persona.celular_persona
        public System.String CelularPersona { get; set; }
        // Referencia: persona.email_persona
        public System.String EmailPersona { get; set; }
        // Referencia: persona.ciudad
        public System.String Ciudad { get; set; }
        // Referencia: persona.provincia
        public System.String Provincia { get; set; }
        // Referencia: persona.observaciones
        public System.String Observaciones { get; set; }
        // Referencia: persona.tipo_documento
        public System.String TipoDocumento { get; set; }
        // Referencia: persona.numero_documento
        public System.String NumeroDocumento { get; set; }
        public bool EsProveedor { get; set; }
    }
}
