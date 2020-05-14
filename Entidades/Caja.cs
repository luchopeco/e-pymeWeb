using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;

namespace Entidades
{
    [Serializable]
    public class Caja
    {
        public Caja() { }
        /// <summary>
        /// Busca una caja para el usuario y la fecha indicada abierta
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="fecha"></param>
        public Caja(DateTime fecha, int idUsuario, bool conExcepcion)
        {
            string sql = "SELECT  * FROM caja c WHERE c.idusuario = :p1 AND DATE(c.fecha)=:p2 AND fecha_cierre is null";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, idUsuario, fecha);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                mapearCaja(row);
            }
            else
            {
                if (conExcepcion)
                {
                    throw new Exception("No existe una caja abierta");

                }
            }

        }
        public Caja(int idCaja)
        {
            string sql = "SELECT * FROM caja c WHERE c.idcaja=:p1";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, idCaja);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                mapearCaja(row);
            }
        }


        private static Caja mapearCajaNueva(DataRow row)
        {
            Caja c = new Caja();
            c.Idcaja = Convert.ToInt32(row["idcaja"]);
            c.Descripcion = row["descripcion"].ToString();
            c.Fecha = Convert.ToDateTime(row["fecha"]);
            c.FechaCierre = row["fecha_cierre"] as DateTime?;
            c.Idusuario = Convert.ToInt32(row["idusuario"]);
            c.FondoInicial = Convert.ToDecimal(row["fondo_inicial"]);
            c.FondoFinal = Convert.ToInt32(row["fondo_final"]);
            c.Usuario = new Usuario();
            c.Usuario.Idusuario = c.Idusuario;
            c.Usuario.NombreUsuario = row["nombre_usuario"].ToString();
            return c;
        }

        private void mapearCaja(DataRow row)
        {
            Idcaja = Convert.ToInt32(row["idcaja"]);
            this.Descripcion = row["descripcion"].ToString();
            this.Fecha = Convert.ToDateTime(row["fecha"]);
            this.FechaCierre = row["fecha_cierre"] as DateTime?;
            this.Idusuario = Convert.ToInt32(row["idusuario"]);
            this.FondoInicial = Convert.ToDecimal(row["fondo_inicial"]);
            this.FondoFinal = Convert.ToInt32(row["fondo_final"]);
        }

        // Referencia: caja.idcaja
        public System.Int32 Idcaja { get; set; }
        // Referencia: caja.idusuario
        public System.Int32 Idusuario { get; set; }

        /// <summary>
        /// El Usuario no se setea por defecto
        /// </summary>
        public Usuario Usuario { get; set; }
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public string DescUsuario
        {
            get
            {
                if (Usuario != null)
                {
                    return Usuario.NombreUsuario;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        // Referencia: caja.fecha
        public System.DateTime Fecha { get; set; }
        public DateTime? FechaCierre { get; set; }
        // Referencia: caja.string
        public System.String Descripcion { get; set; }
        public decimal FondoInicial { get; set; }
        public decimal FondoFinal { get; set; }
        /// <summary>
        /// Es la resta entre los movimientos q representan gancnacia menos los q representan perdidadas
        /// </summary>
        public decimal TotalMovimientos
        {
            get
            {
                if (ListMovimientos == null)
                {
                    return 0;
                }
                else
                {
                    decimal totalSuma = ListMovimientos.Where(m => m.TipoMovimiento.EsSuma == true).Sum(m => m.Monto);
                    decimal totalResta = ListMovimientos.Where(m => m.TipoMovimiento.EsSuma == false).Sum(m => m.Monto);
                    return totalResta + FondoFinal - FondoInicial - totalSuma;
                }
            }
        }
        private List<MovimientoCaja> listMovimientos = null;
        /// <summary>
        /// Movimientos en efectivo q contabiizan la caja
        /// </summary>
        public List<MovimientoCaja> ListMovimientos
        {
            get
            {
                if (Idcaja != 0 && listMovimientos == null)
                {
                    buscarlistMovimientosEfectivo();
                }
                return listMovimientos;
            }
        }

        /// <summary>
        /// Movimientos con otras formas de pago
        /// </summary>
        private List<MovimientoCaja> listMovimientosNoEfectivo = null;
        /// <summary>
        /// Movimientos con otras formas de pago
        /// </summary>
        public List<MovimientoCaja> ListMovimientosNoEfectivo
        {
            get
            {
                if (Idcaja != 0 && listMovimientosNoEfectivo == null)
                {
                    buscarlistMovimientosNOEfectivo();
                }
                return listMovimientosNoEfectivo;
            }
        }



        public int Abrir()
        {
            validarCajasCerradas();
            string sql = @"INSERT INTO caja
                    (
	                  
	                    idusuario,
	                    fecha,
	                    descripcion,
	                    fondo_inicial	                    
                    )
                    VALUES
                    (
	                    :idusuario ,
	                    :fecha ,
	                    :descripcion ,
	                    :fondo	                    
                    )";
            using (Connection conn = new Connection())
            {
                conn.Open();
                conn.Execute(sql, Idusuario, Fecha, Descripcion, FondoInicial);
                Idcaja = conn.LastInsertedId("caja_idcaja_seq");
            }
            return Idcaja;
        }
        public void Cerrar()
        {
            Caja ca = new Caja(Idcaja);
            if (ca.FechaCierre != null)
            {
                throw new Exception("La caja ya se encuentra cerrada");
            }
            string sql = @"UPDATE caja
                            SET	
	                            descripcion = :p1,
	                            fondo_inicial = :p2,
	                            fecha_cierre = :p3,
	                            fondo_final = :p4
                            WHERE idcaja = :p5";
            using (Connection conn = new Connection())
            {
                conn.Open();
                conn.Execute(sql, Descripcion, FondoInicial, DateTime.Now, FondoFinal, Idcaja);
            }
        }

        public static void Reabrir(int idCaja)
        {
            Caja ca = new Caja(idCaja);
            if (ca.FechaCierre == null)
            {
                throw new Exception("La caja ya se encuentra abierta");
            }
            string sql = @"UPDATE caja
                            SET	
	                            fecha_cierre = null
                            WHERE idcaja = :p5";
            using (Connection conn = new Connection())
            {
                conn.Open();
                conn.Execute(sql, idCaja);
            }
        }

        private void buscarlistMovimientosEfectivo()
        {
            buscarMovimientoCompraEfectivo();
            buscarMovimientoGastosEfectivo();
            buscarMovimientoVentasEfectivo();
            buscarMovimientoCajaEfectivo();

        }
        private void buscarlistMovimientosNOEfectivo()
        {
            buscarMovimientoCompraNoEfectivo();
            buscarMovimientoGastosNOEfectivo();
            buscarMovimientoVentasNOEfectivo();


        }
        /// <summary>
        /// Completa las lista de mov efectivo correspondientes a la compra
        /// </summary>
        private void buscarMovimientoCompraEfectivo()
        {
            string sql = @"SELECT SUM (fpc.monto) total, fpc.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo  FROM compra c 
                            INNER JOIN forma_pago_compra fpc ON fpc.idcompra = c.idcompra
                            INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpc.idtipo_forma_pago  
                            WHERE c.idcaja=:p1 and tfp.es_efectivo =true
                            GROUP BY fpc.idtipo_forma_pago,tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = false;
                    tm.Nombre = row["descripcion"].ToString();


                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Compra";
                    m.Fecha = Fecha;


                    if (listMovimientos == null)
                    {
                        listMovimientos = new List<MovimientoCaja>();
                    }
                    listMovimientos.Add(m);

                }
            }
        }

        /// <summary>
        /// Completa las lista de mov NO efectivo correspondientes a la compra
        /// </summary>
        private void buscarMovimientoCompraNoEfectivo()
        {
            string sql = @"SELECT SUM (fpc.monto) total, fpc.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo  FROM compra c 
                            INNER JOIN forma_pago_compra fpc ON fpc.idcompra = c.idcompra
                            INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpc.idtipo_forma_pago  
                            WHERE c.idcaja=:p1 and tfp.es_efectivo =false
                            GROUP BY fpc.idtipo_forma_pago,tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = false;
                    tm.Nombre = row["descripcion"].ToString();


                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Compra";
                    m.Fecha = Fecha;

                    if (listMovimientosNoEfectivo == null)
                    {
                        listMovimientosNoEfectivo = new List<MovimientoCaja>();
                    }
                    listMovimientosNoEfectivo.Add(m);

                }
            }
        }

        /// <summary>
        /// Completa las lista de mov efectivo correspondientes a los gastos
        /// </summary>
        private void buscarMovimientoGastosEfectivo()
        {
            string sql = @"SELECT SUM(fpg.monto) total, fpg.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo  FROM gasto g 
                            INNER JOIN forma_pago_gasto fpg ON fpg.idgasto = g.idgasto
                            INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpg.idtipo_forma_pago
                            WHERE g.idcaja = :p1 AND g.fecha_anulado IS NULL AND tfp.es_efectivo=true
                            GROUP BY fpg.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = false;
                    tm.Nombre = row["descripcion"].ToString();

                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Gasto";
                    m.Fecha = Fecha;


                    if (listMovimientos == null)
                    {
                        listMovimientos = new List<MovimientoCaja>();
                    }
                    listMovimientos.Add(m);

                }
            }
        }

        /// <summary>
        /// Completa las lista de mov NO efectivo correspondientes a los gastos
        /// </summary>
        private void buscarMovimientoGastosNOEfectivo()
        {
            string sql = @"SELECT SUM(fpg.monto) total, fpg.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo  FROM gasto g 
                            INNER JOIN forma_pago_gasto fpg ON fpg.idgasto = g.idgasto
                            INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpg.idtipo_forma_pago
                            WHERE g.idcaja = :p1 AND g.fecha_anulado IS NULL and tfp.es_efectivo = false
                            GROUP BY fpg.idtipo_forma_pago, tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = false;
                    tm.Nombre = row["descripcion"].ToString();

                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Gasto";
                    m.Fecha = Fecha;


                    if (listMovimientosNoEfectivo == null)
                    {
                        listMovimientosNoEfectivo = new List<MovimientoCaja>();
                    }
                    listMovimientosNoEfectivo.Add(m);

                }
            }
        }

        /// <summary>
        /// Completa las lista de mov  efectivo correspondientes a las ventas
        /// </summary>
        private void buscarMovimientoVentasEfectivo()
        {
            string sql = @"SELECT sum(fpv.monto) total, fpv.idforma_pago,tfp.descripcion,tfp.es_efectivo FROM venta v
                        INNER JOIN forma_pago_venta fpv ON fpv.idventa = v.idventa
                        INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpv.idforma_pago
                         WHERE v.idcaja = :p1 AND v.fecha_baja IS NULL AND tfp.es_efectivo = true
                        GROUP BY fpv.idforma_pago,tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = true;
                    tm.Nombre = row["descripcion"].ToString();

                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Ventas";
                    m.Fecha = Fecha;


                    if (listMovimientos == null)
                    {
                        listMovimientos = new List<MovimientoCaja>();
                    }
                    listMovimientos.Add(m);

                }
            }
        }

        /// <summary>
        /// Completa las lista de mov  no efectivo correspondientes a las ventas
        /// </summary>
        private void buscarMovimientoVentasNOEfectivo()
        {
            string sql = @"SELECT sum(fpv.monto) total, fpv.idforma_pago,tfp.descripcion,tfp.es_efectivo FROM venta v
                        INNER JOIN forma_pago_venta fpv ON fpv.idventa = v.idventa
                        INNER JOIN tipo_forma_pago tfp ON tfp.idtipo_forma_pago = fpv.idforma_pago
                         WHERE v.idcaja = :p1 AND v.fecha_baja IS NULL AND tfp.es_efectivo=false
                        GROUP BY fpv.idforma_pago,tfp.descripcion,tfp.es_efectivo";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    bool esEfectivo = Convert.ToBoolean(row["es_efectivo"]);
                    TipoMovimientoCaja tm = new TipoMovimientoCaja();
                    tm.EsSuma = true;
                    tm.Nombre = row["descripcion"].ToString();

                    MovimientoCaja m = new MovimientoCaja(tm);
                    m.Monto = Convert.ToDecimal(row["total"]);
                    m.Descripcion = "Ventas";
                    m.Fecha = Fecha;

                    if (listMovimientosNoEfectivo == null)
                    {
                        listMovimientosNoEfectivo = new List<MovimientoCaja>();
                    }
                    listMovimientosNoEfectivo.Add(m);

                }
            }
        }

        private void buscarMovimientoCajaEfectivo()
        {
            string sql = "SELECT * FROM caja_movimiento cm WHERE cm.idcaja = :p1 ORDER BY cm.fecha";
            DataTable dt;
            using (Connection con = new Connection())
            {
                con.Open();
                dt = con.GetDT(sql, Idcaja);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MovimientoCaja m = new MovimientoCaja();
                    m.Descripcion = row["descripcion"].ToString();
                    m.Fecha = Convert.ToDateTime(row["fecha"]);
                    m.Idcaja = Idcaja;
                    m.IdtipoMovimiento = Convert.ToInt32(row["idtipo_movimiento"]);
                    m.Monto = Convert.ToDecimal(row["monto"]);
                    m.NumeroMovimiento = Convert.ToInt32(row["numero_movimiento"]);
                    if (listMovimientos == null)
                    {
                        listMovimientos = new List<MovimientoCaja>();
                    }
                    listMovimientos.Add(m);
                }
            }
        }
        /// <summary>
        /// Valida que no existan cajas abiertas por el usuario
        /// </summary>
        private void validarCajasCerradas()
        {
            string sql = @"SELECT 1 FROM caja c WHERE c.idusuario  = :p1 AND c.fecha_cierre IS NULL";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                conn.Open();
                dt = conn.GetDT(sql, Idusuario);
            }
            if (dt.Rows.Count > 0)
            {
                throw new ExcepcionPropia("Existe una caja abierta sin cerrar por el usuario activo");
            }
        }

        public static List<Caja> BuscarListCajas(DateTime fechaDesde, DateTime fechaHasta, int idUsuario)
        {
            string sql = "SELECT c.*, u.nombre_usuario FROM caja c INNER JOIN usuario u ON u.idusuario = c.idusuario WHERE c.fecha BETWEEN :p1 AND :p2 ORDER BY c.fecha";
            DataTable dt;
            using (Connection conn = new Connection())
            {
                dt = conn.GetDT(sql, fechaDesde, fechaHasta);
            }
            List<Caja> listC = null;
            if (dt.Rows.Count > 0)
            {
                listC = new List<Caja>();
                foreach (DataRow row in dt.Rows)
                {
                    Caja c = mapearCajaNueva(row);
                    listC.Add(c);
                }
            }
            return listC;
        }
    }
}
