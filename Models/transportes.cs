//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElGatoMensajero.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class transportes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public transportes()
        {
            this.conduce = new HashSet<conduce>();
        }

        [Required(ErrorMessage = "La matricula es obligatoria")]
        //[RegularExpression("(([A-Z]{1,2})(\\d{4})([A-Z]{0,2}))|((E)(\\d{4})([A-Z]{3})", ErrorMessage = "Debe ser una matricula válida")]
        public string matricula { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(32, ErrorMessage = "Debe tener entre 2 y 32 carácteres", MinimumLength = 2)]
        public string marca { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(64, ErrorMessage = "Debe tener entre 2 y 64 carácteres", MinimumLength = 2)]
        public string modelo { get; set; }

        [Required(ErrorMessage = "El consumo es obligatorio")]
        public double consumo { get; set; }

        [Required(ErrorMessage = "Los kilometros son obligatorios")]
        public double km { get; set; }

        [Required(ErrorMessage = "El fecha de matriculación es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fecha_matriculacion { get; set; }

        public Nullable<int> sede { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<conduce> conduce { get; set; }
        public virtual sedes sedes { get; set; }
    }
}
