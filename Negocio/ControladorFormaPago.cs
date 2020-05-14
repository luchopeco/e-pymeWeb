using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using Entidades;


namespace Negocio
{
    public class ControladorFormaPago : ControladorGenerico
    {
        #region Constructores
        /// <summary>
        /// constructor por defecto de la clase base
        /// </summary>
        public ControladorFormaPago()
            : base()
        {

        }
        /// <summary>
        ///para Utilizar sin using. Libera todo el garbage Collector (Espero)
        /// No maneja la coneccion
        /// </summary>
        public ControladorFormaPago(bool sinConexion)
            : base(sinConexion)
        {

        }
        /// <summary>
        ///Constructor al cual le paso la coneccion para no utilizar la del Controlador actua si no otra
        /// </summary>
        public ControladorFormaPago(Connection c)
            : base(c)
        {

        }

        #endregion

        private int insertFormaPago(FormaPago fp)
        {
            string sql = @"INSERT INTO tipo_forma_pago
                        (	                     
	                        descripcion,
	                        habilitado_venta,
	                        habilitado_compra,
	                        habilitado_gasto,
                            es_nota_credito,
                            es_efectivo
                        )
                        VALUES
                        (
	                        :descripcion,
	                        :habilitado_venta,
	                        :habilitado_compra,
	                        :habilitado_gasto,
                            :p1, 
                            :p2
                        )";
            conn.Execute(sql, fp.Descripcion, fp.HabilitadoVenta, fp.HabilitadoCompra, fp.HabilitadoGasto, fp.AceptaNotaCredito,fp.EsEfectivo);
            return conn.LastInsertedId("tipo_forma_pago_idtipo_forma_pago_seq");
        }
        private void updateFormaPago(FormaPago fp)
        {
            string sql = @"UPDATE tipo_forma_pago
                        SET	
	                        descripcion = :p5,
	                        habilitado_venta = :p4,
	                        habilitado_compra = :p3,
	                        habilitado_gasto = :p2,
                            es_nota_credito=:p7,
                            es_efectivo =:p8
                        WHERE idtipo_forma_pago=:p1";
            conn.Execute(sql, fp.Descripcion, fp.HabilitadoVenta, fp.HabilitadoCompra, fp.HabilitadoGasto, fp.AceptaNotaCredito, fp.EsEfectivo, fp.IdtipoFormaPago);
        }
        private DataTable selectFormaPago(int idFormaPago)
        {
            string sql = "SELECT * FROM tipo_forma_pago tfp WHERE tfp.idtipo_forma_pago=:p1";
            return conn.GetDT(sql, idFormaPago);

        }
        private DataTable selectFormasPagos()
        {
            string sql = "SELECT * FROM tipo_forma_pago tfp";
            return conn.GetDT(sql);
        }

        public int AgregarFormaPago(string descripcion, bool habilitadoVenta, bool habilitadoCompra,
            bool habilitadoGasto, bool aceptaNotaCredito, bool esEfectivo)
        {
            try
            {
                FormaPago fp = new FormaPago();
                fp.Descripcion = descripcion;
                fp.HabilitadoCompra = habilitadoCompra;
                fp.HabilitadoVenta = habilitadoVenta;
                fp.HabilitadoGasto = habilitadoGasto;
                fp.AceptaNotaCredito = aceptaNotaCredito;
                fp.EsEfectivo = esEfectivo;
                int id = insertFormaPago(fp);
                return id;
            }
            catch (Exception ex)
            {

                ControladorExcepcion.tiraExcepcion(ex.Message);
                return 0;
            }

        }
        public void ModificarFormaPago(int idFormaPago, string descripcion, bool habilitadoVenta, bool habilitadoCompra,
            bool habilitadoGasto, bool aceptaNotaCredito, bool esEfectivo)
        {
            try
            {
                FormaPago fp = new FormaPago();
                fp.IdtipoFormaPago = idFormaPago;
                fp.Descripcion = descripcion;
                fp.HabilitadoCompra = habilitadoCompra;
                fp.HabilitadoVenta = habilitadoVenta;
                fp.HabilitadoGasto = habilitadoGasto;
                fp.AceptaNotaCredito = aceptaNotaCredito;
                fp.EsEfectivo = esEfectivo;
                updateFormaPago(fp);


            }
            catch (Exception ex)
            {
                ControladorExcepcion.tiraExcepcion(ex.Message);
            }
        }
        /// <summary>
        /// Obtiene uan forma pago sino la enceuntra excepcion propia
        /// </summary>
        /// <param name="idFormaPago"></param>
        /// <returns></returns>
        public FormaPago BuscarFormaPago(int idFormaPago)
        {
            try
            {
                DataTable dt = selectFormaPago(idFormaPago);
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado la forma de pago");
                }
                DataRow row = dt.Rows[0]; 
                FormaPago fp = mapearFormaPago(row);
                
                return fp;
            }
            catch (Exception ex)
            {                
                ControladorExcepcion.tiraExcepcion(ex.Message);
                return null;
            }            
        }

        /// <summary>
        /// Obtiene una lista de forma pago sino la enceuntra excepcion propia
        /// </summary>
        /// <param name="idFormaPago"></param>
        /// <returns></returns>
        public List<FormaPago> BuscarListFormaPago()
        {
            try
            {
                DataTable dt = selectFormasPagos();
                if (dt == null || dt.Rows.Count == 0)
                {
                    throw new ExcepcionPropia("No se ha encontrado formas de pago");
                }
                List<FormaPago> listFp = new List<FormaPago>();
                foreach (DataRow row in dt.Rows)
                {
                    FormaPago fp = mapearFormaPago(row);
                    listFp.Add(fp);
                }

                CommitTransaction();
                return listFp.OrderBy(fp => fp.Descripcion).ToList();
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
                return null;
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
                return null;
            }
        }

        public void EliminarFormaPago(int idFormaPago)
        {
            try
            {
                conn.Execute("DELETE FROM tipo_forma_pago WHERE idtipo_forma_pago=:p1", idFormaPago);
            }
            catch (Npgsql.NpgsqlException ex)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(ex);
            }
            catch (ExcepcionPropia myEx)
            {
                RollbackTransaction();
                ControladorExcepcion.tiraExcepcion(myEx.Message);
            }
        }

        private static FormaPago mapearFormaPago(DataRow row)
        {

            FormaPago fp = new FormaPago();
            fp.IdtipoFormaPago = Convert.ToInt32(row["idtipo_forma_pago"]);
            fp.Descripcion = row["descripcion"].ToString();
            fp.HabilitadoCompra = Convert.ToBoolean(row["habilitado_compra"]);
            fp.HabilitadoVenta = Convert.ToBoolean(row["habilitado_venta"]);
            fp.HabilitadoGasto = Convert.ToBoolean(row["habilitado_gasto"]);
            fp.AceptaNotaCredito = Convert.ToBoolean(row["es_nota_credito"]);
            fp.EsEfectivo = Convert.ToBoolean(row["es_efectivo"]);
            return fp;
        }
    }
}
