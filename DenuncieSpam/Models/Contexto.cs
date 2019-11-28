using System;
using System.Collections.Generic;
using System.Text;
using DenuncieSpam.Mapeamento;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DenuncieSpam.Models
{
    public class Contexto : IdentityDbContext<Usuario, NiveisAcesso, string>
    {

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Reclamacao> Reclamacoes { get; set; }

        public DbSet<NiveisAcesso> NiveisAcessos { get; set; }

        


        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ReclamacaoMap());

            builder.ApplyConfiguration(new UsuarioMap());

            builder.ApplyConfiguration(new NivelAcessoMap());

            

        }

        

    }
}
