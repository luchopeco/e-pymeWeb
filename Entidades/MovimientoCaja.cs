using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;

namespace Entidades
{
    [Serializable]
    public class MovimientoCaja
    {
        public MovimientoCaja()
        {

        }
        public MovimientoCaja(TipoMovimientoCaja tm)
        {
            tipoMovimiento = tm;
        }
        // Referencia: caja_movimiento.idcaja
        public System.Int32 Idcaja { get; set; }
        // Referencia: caja_movimiento.numero_movimiento
        public System.Int32 NumeroMovimiento { get; set; }
        // Referencia: caja_movimiento.monto
        public System.Decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        // Referencia: caja_movimiento.idtipo_movimiento
        public System.Int32 IdtipoMovimiento { get; set; }
        private TipoMovimientoCaja tipoMovimiento;
        /// <summary>
        /// Solo Lectura
        /// </summary>
        public TipoMovimientoCaja TipoMovimiento
        {
            get
            {
                if (IdtipoMovimiento != 0 && tipoMovimiento == null)
                {
                    tipoMovimiento = new TipoMovimientoCaja(IdtipoMovimiento);
                }
                return tipoMovimiento;
            }

        }
        public string DescTipoMovimiento
        {
            get
            {
                if (TipoMovimiento != null)
                {
                    return tipoMovimiento.Nombre;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        // Referencia: caja_movimiento.descripcion
        public System.String Descripcion { get; set; }

        public void Agregar()
        {
            string sql = @"INSERT INTO caja_movimiento
                            (
	                            idcaja,	                         
	                            monto,
	                            idtipo_movimiento,
	                            descripcion,
	                            fecha
                            )
                            VALUES
                            (
	                            :idcaja,
	                            :monto,
	                            :idtipo_movimiento,
	                            :descripcion ,
	                            :fecha 
                            )";
            using (Connection conn = new Connection())
            {
                conn.Open();
                conn.Execute(sql, Idcaja, Monto, IdtipoMovimiento, Descripcion, Fecha);
            }
        }
    }
}
