using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Base;
using WPP.Entities.Objects.ModuloContratos;

namespace WPP.Entities.Objects.ModuloBascula
{
    public class Equipo : Entity
    {
        public virtual String Nombre { get; set; }
        public virtual String Placa { get; set; }
        public virtual String Marca { get; set; }
        public virtual double Peso { get; set; }
        public virtual double Eje1 { get; set; }
        public virtual double Eje2 { get; set; }
        public virtual double Eje3 { get; set; }
        public virtual String Tipo { get; set; }
        public virtual String TipoCombustible { get; set; }
        public virtual Compania Compania { get; set; }
    }
}
