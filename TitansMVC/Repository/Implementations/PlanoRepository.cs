using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Context;
using TitansMVC.Models;
using TitansMVC.Models.Enums;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Repository.Implementations
{
    public class PlanoRepository : RepositoryBase<PlanoModel>, IPlanoRepository
    {
        public override void Add(PlanoModel plano)
        {
            plano.CnpjCriptografado = Encryptor.Encrypt(plano.Cnpj);
            plano.ValidadeCriptografada = Encryptor.Encrypt(plano.Validade.ToString());
            plano.DataUltimaValidacao = Encryptor.Encrypt(DateTime.Now.ToString());
            this.CreatePlanoServidor(plano);
            Db.Plano.Add(plano);
            Db.SaveChanges();
            
        }

        public override void Update(PlanoModel plano)
        {
            plano.CnpjCriptografado = Encryptor.Encrypt(plano.Cnpj);
            plano.ValidadeCriptografada = Encryptor.Encrypt(plano.Validade.ToString());
            plano.DataUltimaValidacao = Encryptor.Encrypt(DateTime.Now.ToString());
            Db.Entry(plano).State = EntityState.Modified;
            Db.SaveChanges();

            Db.Database.ExecuteSqlCommand(string.Format(
                    "insert into [controlepi_hard].[logs] (entidade, operacao, id_reg, id_usuario, datahora, id_empresa) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                    "Plano", "update", plano.Id, HttpContext.Current.User.Identity.GetUserId(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Util.GetEmpresaId()));
        }

        public PlanoModel GetByCnpj(string cnpj)
        {
            var cnpjCriptografado = Encryptor.Encrypt(cnpj);
            return Db.Plano.AsNoTracking().FirstOrDefault(e => e.CnpjCriptografado == cnpjCriptografado);
        }
        

        public IEnumerable<PlanoModel> BuscarTodosPlanos()
        {
            return Db.Plano.AsNoTracking().OrderBy(p=>p.CnpjCriptografado);
        }

        public Boolean CreatePlanoServidor(PlanoModel plano)
        {
            const string myConn = @"Data Source=hardnet-aws.cm8pvtainpgb.sa-east-1.rds.amazonaws.com,1433;Initial Catalog=control_epiPlanos;User Id=HardNet; Password=Hnsi@CL113; MultipleActiveResultSets=True;";
            var str = "INSERT INTO [controlepi_planos].[schema_control] ([schema_reg] ,[schema_type] ,[schema_val] ,[schema_inf]) VALUES (@cnpj, @type, @validade, @lastupdated)";

            //var str = String.Format("UPDATE [controlepi_planos].[schema_control] SET schema_type = @type,schema_val = @validade ,schema_inf = @lastupdated WHERE schema_reg = '{0}'", Encryptor.Encrypt(plano.Cnpj));
            using (var con = new SqlConnection(myConn))
            {
                try
                {

                    con.Open();
                    try
                    {
                        using (var oCommand = new SqlCommand(str, con))
                        {
                            oCommand.CommandText = str;
                            oCommand.Parameters.AddWithValue("@cnpj", Encryptor.Encrypt(plano.Cnpj));
                            oCommand.Parameters.AddWithValue("@type", plano.NivelPlano);
                            oCommand.Parameters.AddWithValue("@validade", Encryptor.Encrypt(plano.Validade.ToString()));
                            oCommand.Parameters.AddWithValue("@lastupdated", Encryptor.Encrypt(DateTime.Now.ToString()));
                            oCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw new Exception(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();

                }

            }
            return true;

        }

       
        public PlanoModel CopiarPlanoServidorParaCliente(string cnpj)
        {
            //copiar plano
            PlanoModel plano = new PlanoModel();
            const int cnpjOrdinal = 1;
            const int planoOrdinal = 2;
            const int validadeOrdinal = 3;
            const string myConn = @"Data Source=hardnet-aws.cm8pvtainpgb.sa-east-1.rds.amazonaws.com,1433;Initial Catalog=control_epiPlanos;User Id=HardNet; Password=Hnsi@CL113; MultipleActiveResultSets=True;";
            var cnpjCrypt = Encryptor.Encrypt(cnpj);

            //var cnpjCriptografado = Encryptor.Encrypt(cnpj);

            var str = "SELECT * FROM [controlepi_planos].[schema_control] WHERE schema_reg = '" + cnpjCrypt + "';";

            using (var con = new SqlConnection(myConn))
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    return null;
                    throw new Exception(ex.Message);
                }
                try
                {
                    using (var oCommand = new SqlCommand(str, con))
                    {
                        using (var oReader = oCommand.ExecuteReader())
                        {
                            while (oReader.Read())
                            {
                                var cnpjCrypto = oReader.GetString(cnpjOrdinal);
                                var cnpjDescrypto = Encryptor.Decrypt(cnpjCrypto);

                                plano = Db.Plano.FirstOrDefault(p => p.CnpjCriptografado == cnpjCrypto);

                                plano.Cnpj = cnpjDescrypto;
                                plano.NivelPlano = (Plano?)oReader.GetInt32(planoOrdinal);
                                var date = oReader.GetString(validadeOrdinal);
                                var dataD = Encryptor.Decrypt(date);
                                plano.Validade = DateTime.Parse(dataD);
                                Update(plano);
                                return plano;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }

            return plano;
        }

        public bool UpdatePlanoServidor(PlanoModel plano)
        {
            const string myConn = @"Data Source=hardnet-aws.cm8pvtainpgb.sa-east-1.rds.amazonaws.com,1433;Initial Catalog=control_epiPlanos;User Id=HardNet; Password=Hnsi@CL113; MultipleActiveResultSets=True;";
            var str = String.Format("UPDATE [controlepi_planos].[schema_control] SET schema_type = @type,schema_val = @validade ,schema_inf = @lastupdated WHERE schema_reg = '{0}'", Encryptor.Encrypt(plano.Cnpj));
            using (var con = new SqlConnection(myConn))
            {
                try
                {

                    con.Open();
                    try
                    {
                        using (var oCommand = new SqlCommand(str, con))
                        {
                            oCommand.CommandText = str;
                            oCommand.Parameters.AddWithValue("@type", plano.NivelPlano);
                            oCommand.Parameters.AddWithValue("@validade", Encryptor.Encrypt(plano.Validade.ToString()));
                            oCommand.Parameters.AddWithValue("@lastupdated", Encryptor.Encrypt(DateTime.Now.ToString()));
                            oCommand.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                        throw new Exception(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                    throw new Exception(ex.Message);
                }
                finally
                {
                    con.Close();

                }

            }
            return true;

        }

        public IEnumerable<PlanoModel> BuscarPorCnpj(string cnpj)
        {
            var cnpjCr = Encryptor.Encrypt(cnpj);
           return Db.Plano.Where(p=>p.CnpjCriptografado.Equals(cnpjCr)).OrderBy(p=>p.CnpjCriptografado);
        }
    }
}