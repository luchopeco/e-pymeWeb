using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Text.RegularExpressions;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorExcepcion
    {
        public static void tiraExcepcion(NpgsqlException myE)
        {
            //ver las posibles excepciones de sql acá!!

            if (myE.Code == "23514")//viola la restricción check
            {
                //este mensaje hay q mejorarlo es solo para articulos
                string ex = myE.Message;
                if (ex.Contains("stock_existencias_chk"))
                {
                    throw new ExcepcionPropia("Algun articulo NO posee stock necesario");
                }
                else
                {
                    throw new ExcepcionPropia(myE.Message);
                }

            }
            else
            {
                throw new ExcepcionPropia(myE.Message);
            }
            ////EXCEPCIONES QUE NO SE PUEDEN ARREGLAR           
            //if (myE.Code == 1042) //1042: Sin servicio MYSQL o sin conexión a la BD
            //{
            //    //Mando excepción propia a la capa de Presentación con el mensaje que quiero                    
            //    throw new ExcepcionCritica("Código de Error: 1042 - Ocurrió un problema en la conexión a la Base de Datos. Comuníquese con el Soporte Técnico.");

            //}
            //else if (myE.Number == 1044)//1044: Está mal la cadena de conexión
            //{
            //    //Mando excepción propia a la capa de Presentación con el mensaje que quiero
            //    throw new ExcepcionCritica("Código de Error: 1044 - Ocurrió un problema en la conexión a la Base de Datos. Comuníquese con el Soporte Técnico.");
            //}
            //else if (myE.Number == 0) //Falta el stored Procedure 
            //{
            //    //Mando excepción propia a la capa de Presentación con el mensaje que quiero
            //    throw new ExcepcionCritica("Código de Error: 0 - Ocurrió un problema en la conexión a la Base de Datos. Comuníquese con el Soporte Técnico.");
            //}
            //else
            //{
            //    //Mando excepción propia a la capa de Presentación con el mensaje que quiero
            //    throw new ExcepcionPropia(myE.Message);
            //}
      
        }

        public static void tiraExcepcion(string mensaje)
        {

            throw new ExcepcionPropia(mensaje);
      
        }


    }
}
