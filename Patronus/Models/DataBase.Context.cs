﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PatronusDBEntities : DbContext
    {
        public PatronusDBEntities()
            : base("name=PatronusDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Artiste> Artistes { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Groupe> Groupes { get; set; }
        public virtual DbSet<NoteArtiste> NoteArtistes { get; set; }
        public virtual DbSet<NoteOeuvre> NoteOeuvres { get; set; }
        public virtual DbSet<Organisme> Organismes { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<TypeOeuvre> TypeOeuvres { get; set; }
        public virtual DbSet<Participe> Participes { get; set; }
        public virtual DbSet<Oeuvre> Oeuvres { get; set; }
    }
}
