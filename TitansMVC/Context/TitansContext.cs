using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using TitansMVC.EntityConfiguration;
using TitansMVC.Models;
using TitansMVC.Models.Relatorios;

namespace TitansMVC.Context
{
    public class TitansContext : DbContext
    {
        private string schemaName = "controlepi_hard";

        public TitansContext()
            : base("TitansContext")
        {
        }

        public DbSet<CentroCustoModel> CentrosCusto { get; set; }
        public DbSet<ColaboradorModel> Colaboradores { get; set; }
        public DbSet<ColaboradorImportacaoDuplicadoModel> ColaboradoresImportados { get; set; }
        public DbSet<ConfiguracaoModel> Configuracoes { get; set; }
        public DbSet<EmailConfirmacaoRegistroModel> EmailsConfirmacao { get; set; }
        public DbSet<EmailEmpresaModel> EmailsEmpresas { get; set; }
        public DbSet<EmpresaModel> Empresas { get; set; }
        public DbSet<EpiColaboradorModel> EpisColaboradores { get; set; }
        public DbSet<EpiModel> Epis { get; set; }
        public DbSet<EpiSetorModel> EpisSetores { get; set; }
        public DbSet<EstoqueEpi> EstoquesEpi { get; set; }
        public DbSet<FamiliaEpiModel> FamiliasEpi { get; set; }
        public DbSet<FichaEpiModel> FichasEpi { get; set; } 
        public DbSet<LbcModel> Lbcs { get; set; } 
        public DbSet<MunicipioModel> Municipios { get; set; }
        public DbSet<PermissaoUsuarioModel> PermissoesUsuarios { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<SetorModel> Setores { get; set; } 
        public DbSet<TelefoneEmpresaModel> TelefonesEmpresas { get; set; }
        public DbSet<TipoEpiModel> TiposEpis { get; set; } 
        public DbSet<UfModel> Ufs { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<PlanoModel> Plano { get; set; }
        public DbSet<FuncaoModel> Funcoes { get; set; }
        public DbSet<TipoUniformeModel> TiposUniformes { get; set; }
        public DbSet<UniformeModel> Uniformes { get; set; }
        public DbSet<UniformeColaboradorModel> UniformesColaboradores { get; set; }
        public DbSet<UniformeSetorModel> UniformesSetores { get; set; }
        public DbSet<EstoqueUniforme> EstoquesUniformes { get; set; }
        public DbSet<FichaUniformeModel> FichasUniformes { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema(schemaName);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(200));
            modelBuilder.Properties<decimal>().Configure(p => p.HasPrecision(18, 2));
            modelBuilder.Properties<Int64>().Configure(p => p.HasColumnType("bigint"));
            modelBuilder.Configurations.Add(new CentroCustoConfiguration());
            modelBuilder.Configurations.Add(new ColaboradorConfiguration());
            modelBuilder.Configurations.Add(new ColaboradorImportacaoDuplicadoConfig());
            modelBuilder.Configurations.Add(new ConfiguracaoConfiguration());
            modelBuilder.Configurations.Add(new EmailConfirmacaoRegistroConfig());
            modelBuilder.Configurations.Add(new EmailEmpresaConfiguration());
            modelBuilder.Configurations.Add(new EmpresaConfiguration());
            modelBuilder.Configurations.Add(new EpiColaboradorConfiguration());
            modelBuilder.Configurations.Add(new EpiConfiguration());
            modelBuilder.Configurations.Add(new EpiSetorConfiguration());
            modelBuilder.Configurations.Add(new EstoqueEpiConfiguration());
            modelBuilder.Configurations.Add(new FamiliaEpiConfiguration());
            modelBuilder.Configurations.Add(new FichaEpiConfiguration());
            modelBuilder.Configurations.Add(new LbcConfiguration());
            modelBuilder.Configurations.Add(new MunicipioConfiguration());
            modelBuilder.Configurations.Add(new PermissaoUsuarioConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new SetorConfiguration());
            modelBuilder.Configurations.Add(new TelefoneEmpresaConfiguration());
            modelBuilder.Configurations.Add(new TipoEpiConfiguration());
            modelBuilder.Configurations.Add(new UfConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new PlanoConfiguration());
            modelBuilder.Configurations.Add(new FuncaoConfiguration());
            modelBuilder.Configurations.Add(new UniformeColaboradorConfiguration());
            modelBuilder.Configurations.Add(new UniformeConfiguration());
            modelBuilder.Configurations.Add(new UniformeSetorConfiguration());
            modelBuilder.Configurations.Add(new EstoqueUniformeConfiguration());
            modelBuilder.Configurations.Add(new FichaUniformeConfiguration());
            modelBuilder.Configurations.Add(new TipoUniformeConfiguration());

        }

        public override int SaveChanges()
        {
            EmailsEmpresas.Local.Where(e => e.Empresa == null).ToList().ForEach(e => EmailsEmpresas.Remove(e));

            EpisColaboradores.Local.Where(e => e.Colaborador == null).ToList().ForEach(e => EpisColaboradores.Remove(e));

            EpisSetores.Local.Where(e => e.Setor == null).ToList().ForEach(e => EpisSetores.Remove(e));

            EstoquesEpi.Local.Where(e => e.Epi == null).ToList().ForEach(e => EstoquesEpi.Remove(e));

            UniformesColaboradores.Local.Where(e => e.Colaborador == null).ToList().ForEach(e => UniformesColaboradores.Remove(e));

            UniformesSetores.Local.Where(e => e.Setor == null).ToList().ForEach(e => UniformesSetores.Remove(e));

            EstoquesUniformes.Local.Where(e => e.Uniforme == null).ToList().ForEach(e => EstoquesUniformes.Remove(e));

            PermissoesUsuarios.Local.Where(p => p.Usuario == null).ToList().ForEach(p => PermissoesUsuarios.Remove(p));

            TelefonesEmpresas.Local.Where(t => t.Empresa == null).ToList().ForEach(t => TelefonesEmpresas.Remove(t));

            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}