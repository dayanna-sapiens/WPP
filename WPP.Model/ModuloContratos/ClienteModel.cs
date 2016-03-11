using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPP.Entities.Objects.Generales;
using WPP.Entities.Objects.ModuloContratos;
using WPP.Model.General;

namespace WPP.Model.ModuloContratos
{
    public class ClienteModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre")]
        public  String Nombre { get; set; }

        [Required(ErrorMessage = "Por favor introduzca la cédula")]
        [RegularExpression("^([a-zA-Z0-9]+)$", ErrorMessage = "No se permiten caracteres especiales -/.*!?")]
        public  String Cedula { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el grupo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el grupo")]
        public  long Grupo { get; set; }

        public String GrupoNombre { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el tipo")]
        [Range(1, long.MaxValue, ErrorMessage = "Por favor seleccione el tipo")]
        public  long Tipo { get; set; }

        public String TipoNombre { get; set; }

        public virtual String NombreComercial { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el nombre corto de la compañía")]
        public  String NombreCorto { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el correo")]
        [EmailAddress(ErrorMessage = "Por favor introduzca un correo válido")]
        [StringLength(80, ErrorMessage = "El correo no puede ser mayor a 80 caracteres")]
        public  String Email { get; set; }

        public  String Fax { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el número de teléfono")]
        public  String Telefono1 { get; set; }

        public  String Telefono2 { get; set; }

        public  DateTime? FechaDesactivacion { get; set; }

        [Required(ErrorMessage = "Por favor introduzca el representante legal del cliente")]
        public  String RepresentanteLegal { get; set; }

        public  String Direccion { get; set; }

        public  long? Provincia { get; set; }

        public String ProvinciaNombre { get; set; }

        public  long? Canton { get; set; }

        public String CantonNombre { get; set; }

        public  long? Distrito { get; set; }

        public String DistritoNombre { get; set; }

        public  long? Numero { get; set; }

        public  IList<ContactoCliente> Contactos { get; set; }

        public ContactoModel Contacto { get; set; }

        public IList<UbicacionCliente> Ubicaciones{ get; set; }

        public UbicacionClienteModel Ubicacion { get; set; }

        public IList<Contrato> Contratos { get; set; }
    }
}
