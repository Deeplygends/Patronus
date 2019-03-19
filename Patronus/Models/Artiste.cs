//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Patronus.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Artiste
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Artiste()
        {
            this.Notes = new HashSet<NoteArtiste>();
            this.Favoris = new HashSet<Oeuvre>();
            this.Participations = new HashSet<Oeuvre>();
            this.Realisations = new HashSet<Oeuvre>();
        }
    
        public string IdArtiste { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public Nullable<System.DateTime> DateNaissance { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NoteArtiste> Notes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oeuvre> Favoris { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oeuvre> Participations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oeuvre> Realisations { get; set; }
    }
}
