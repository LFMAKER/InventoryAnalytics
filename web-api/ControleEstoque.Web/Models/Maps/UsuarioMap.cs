﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ControleEstoque.Web.Models
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            //Definindo nome da tabela
            ToTable("usuario");
            //Definindo PK
            HasKey(x => x.Id);
            //Definindo nome da coluna e auto increment
            Property(x => x.Id).HasColumnName("id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //Definindo nome da coluna, tamanho e obrigatório
            Property(x => x.Login).HasColumnName("login").HasMaxLength(50).IsRequired();
            //Definindo nome da coluna, tamanho e obrigatório
            Property(x => x.Senha).HasColumnName("senha").HasMaxLength(50).IsRequired();
            //Definindo nome da coluna, tamanho e obrigatório
            Property(x => x.Nome).HasColumnName("nome").HasMaxLength(200).IsRequired();
            //Definindo o nome da coluna, tamanho e obrigatório
            Property(x => x.Email).HasColumnName("email").HasMaxLength(200).IsRequired();
            //Defininando nome da coluna e obrigatório
            Property(x => x.IdPerfil).HasColumnName("id_perfil").IsRequired();
            //Definindo a FK
            HasRequired(x => x.Perfil).WithMany().HasForeignKey(x => x.IdPerfil).WillCascadeOnDelete(false);




        }
    }
}