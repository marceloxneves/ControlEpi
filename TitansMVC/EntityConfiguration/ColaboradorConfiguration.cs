using System.Data.Entity.ModelConfiguration;
using TitansMVC.Models;

namespace TitansMVC.EntityConfiguration
{
    public class ColaboradorConfiguration: EntityTypeConfiguration<ColaboradorModel>
    {
        public ColaboradorConfiguration()
        {
            ToTable("colaborador");

            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("id");
            Property(c => c.IdEmpresa).HasColumnName("id_empresa");
            Property(c => c.Nome).HasColumnName("nome").IsRequired();
            Property(c => c.Cpf).HasColumnName("cpf").HasMaxLength(20).IsOptional();
            Property(c => c.SetorId).HasColumnName("id_setor").IsOptional();
            Property(c => c.NumRegEmpresa).HasColumnName("num_reg_empresa").IsOptional();
            Property(c => c.Genero).HasColumnName("genero").IsOptional();            
            Property(c => c.DataAdmissao).HasColumnName("data_admissao").IsOptional();
            Property(c => c.DataNascimento).HasColumnName("data_nascimento").IsOptional();
            Property(c => c.Ativo).HasColumnName("ativo").IsOptional();
            Property(c => c.RecebeuTreinamento).HasColumnName("recebeu_treinamento").IsOptional();
            Property(c => c.RecebeuAdvertencia).HasColumnName("recebeu_advertencia").IsOptional();
            Property(c => c.MotivoAdvertencia).HasColumnName("motivo_advertencia").IsOptional();
            Property(c => c.Obs).HasColumnName("obs").HasMaxLength(500).IsOptional();
            Property(c => c.Foto).HasColumnName("foto").IsOptional();
            Property(c => c.Assinatura).HasColumnName("assinatura").IsOptional();
            Property(c => c.Biometria).HasColumnName("registro_biometrico").HasMaxLength(8000).IsOptional();
            Property(c => c.Importado).HasColumnName("importado").IsOptional();
            Property(c => c.LbcId).HasColumnName("id_unidade_negocio");
            Property(c => c.FuncaoId).HasColumnName("id_funcao");

            //Ignore(c => c.DataCad);
            Property(c => c.DataCad).HasColumnName("data_cad").IsOptional();

            HasRequired(c => c.Empresa).WithMany().HasForeignKey(c => c.IdEmpresa);
            HasOptional(c => c.Setor).WithMany().HasForeignKey(c => c.SetorId);
            HasRequired(c => c.Lbc).WithMany().HasForeignKey(c => c.LbcId);
            //HasOptional(c => c.Funcao).WithMany().HasForeignKey(c => c.FuncaoId);

            //HasMany<FotoModel>(c => c.Fotos).WithRequired(c => c.Colaborador).HasForeignKey(f => f.ColaboradorId);
            HasMany<EpiColaboradorModel>(c => c.Epis).WithRequired(e => e.Colaborador).HasForeignKey(e => e.ColaboradorId).WillCascadeOnDelete(true);
            HasMany<UniformeColaboradorModel>(c => c.Uniformes).WithRequired(e => e.Colaborador).HasForeignKey(e => e.ColaboradorId).WillCascadeOnDelete(true);
        }
    }
}