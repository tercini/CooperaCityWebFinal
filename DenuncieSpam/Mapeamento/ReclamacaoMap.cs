using DenuncieSpam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DenuncieSpam.Mapeamento
{
    public class ReclamacaoMap : IEntityTypeConfiguration<Reclamacao>
    {
        public void Configure(EntityTypeBuilder<Reclamacao> builder)
        {            
            builder.HasKey(r => r.IdReclamacao);
            builder.Property(r => r.Data).IsRequired();            
            builder.Property(r => r.Telefone);
            builder.Property(r => r.Email);
            builder.Property(r => r.Descricao).IsRequired();
            builder.Property(r => r.Imagem);
            builder.Property(r => r.Latitude);
            builder.Property(r => r.Longitude);
            builder.Property(r => r.Endereco);

            builder.HasOne(r => r.Usuario).WithMany(r => r.Reclamacoes).HasForeignKey(r => r.IdUsuario);

            builder.ToTable("reclamacoes");
        }
    }
}

