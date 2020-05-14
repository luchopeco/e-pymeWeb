using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Datos;

namespace Entidades
{
    [Serializable]
    public class Cliente
    {
        public Cliente()
        {
        }

        public Cliente(int idCliente)
        {
            const string sql = "SELECT * FROM cliente c WHERE c.idcliente=:p1";
            DataTable dt = null;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, idCliente);

            }
            if (dt.Rows.Count > 0)
            {
                mapearClienteNuevo(dt.Rows[0]);
            }
        }

        // Referencia: cliente.idcliente
        public System.Int32 Idcliente { get; set; }
        // Referencia: cliente.nombre_apellido
        public System.String Nombre { get; set; }
        public System.String Apellido { get; set; }
        // Referencia: cliente.tipo_documento        
        // Referencia: cliente.numero_documento
        public System.String NumeroDocumento { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string Observaciones { get; set; }

        public string NombreApellido
        {
            get { return Apellido + ", " + Nombre; }
        }


        public void Agregar()
        {
            string sql = @"INSERT cINTO cliente
                            (
	                            -- idcliente -- this column value is auto-generated
	                            nombre_apellido,
	                            numero_documento,
	                            fecha_nacimiento,
	                            direccion,
	                            telefono,
	                            mail,
	                            observaciones,
                                apellido
                            )
                            VALUES
                            (
	                            :nombre_apellido,
	                            :numero_documento,
	                            :fecha_nacimiento,
	                            :direccion,
	                            :telefono,
	                            :mail,
	                            :observaciones,
                                :p1
                            )";

            using (Connection conn = new Connection())
            {
                conn.Open();
                conn.Execute(sql, Nombre, NumeroDocumento, FechaNacimiento, Direccion, Telefono, Mail, Observaciones, Apellido);
            }


        }

        public static List<Cliente> BuscarLikeNombreApellido(string nombreApellido)
        {
            string sql = "SELECT * FROM cliente c WHERE UPPER(c.nombre) LIKE UPPER('%'||:p1||'%') OR UPPER(c.apellido) LIKE UPPER('%'||:p3||'%')";
            DataTable dt = null;
            List<Cliente> listC = null;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, nombreApellido, nombreApellido);
            }
            if (dt.Rows.Count > 0)
            {
                listC = (from DataRow row in dt.Rows select mapearCliente(row)).ToList();
            }
            return listC;
        }

        private static Cliente mapearCliente(DataRow row)
        {
            Cliente c = new Cliente();
            c.Idcliente = Convert.ToInt32(row["idcliente"]);
            c.Nombre = row["nombre"].ToString();
            c.Apellido = row["apellido"].ToString();
            c.Direccion = row["direccion"].ToString();
            c.FechaNacimiento = row["fecha_nacimiento"] as DateTime?;
            c.Mail = row["mail"].ToString();
            c.NumeroDocumento = row["numero_documento"].ToString();
            c.Observaciones = row["observaciones"].ToString();
            c.Telefono = row["telefono"].ToString();
            return c;

        }
        private void mapearClienteNuevo(DataRow row)
        {

            this.Idcliente = Convert.ToInt32(row["idcliente"]);
            this.Nombre = row["nombre"].ToString();
            this.Apellido = row["apellido"].ToString();
            this.Direccion = row["direccion"].ToString();
            this.FechaNacimiento = row["fecha_nacimiento"] as DateTime?;
            this.Mail = row["mail"].ToString();
            this.NumeroDocumento = row["numero_documento"].ToString();
            this.Observaciones = row["observaciones"].ToString();
            this.Telefono = row["telefono"].ToString();


        }

    }
}
