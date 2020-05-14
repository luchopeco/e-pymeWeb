using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Entidades;

namespace Negocio
{
    public class ControladorSucursal : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorSucursal()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorSucursal(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorSucursal(Connection c)
            : base(c)
        {

        }

        #endregion

        /// <summary>
        /// Trae las sucursales habilitadas si no encuenra devuelve null
        /// </summary>
        public List<Sucursal> BuscarListSucursales()
        {
            try
            {
                string sql = "SELECT * FROM sucursal s WHERE s.fecha_baja IS NULL";
                DataTable dt = conn.GetDT(sql);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return null;
                }
                else
                {
                    List<Sucursal> listS = new List<Sucursal>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Sucursal s = mapearSucursal(row);
                        listS.Add(s);
                    }
                    return listS;
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }

        private static Sucursal mapearSucursal(DataRow row)
        {
            Sucursal s = new Sucursal();
            s.Descripcion = row["descripcion"].ToString();
            s.IdSucursal = Convert.ToInt32(row["idsucursal"]);
            s.FechaBaja = row["fecha_baja"] as DateTime?;
            return s;
        }

        /// <summary>
        /// devuelve una sucursal si no la encuentra devevle excepcion proia
        /// </summary>
        /// <param name="idsucursal"></param>
        /// <returns></returns>
        public Sucursal BuscarSucursal(int idsucursal)
        {
            try
            {
                string sql = "SELECT * FROM sucursal s WHERE s.idsucursal=:p1";
                DataTable dt = conn.GetDT(sql,idsucursal);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado la sucrusal");
                }
                else
                {
                    DataRow row = dt.Rows[0];
                    return (mapearSucursal(row));
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
        }
    }
}
