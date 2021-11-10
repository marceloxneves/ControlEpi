using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TitansMVC.Models;
using TitansMVC.Models.Enums;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;
using TitansMVC.Utils;

namespace TitansMVC.Controllers
{
    [Authorize(Roles = "role_admin, role_master")]
    public class ImportFromToExcelController : BaseController
    {
        public ActionResult Upload()
        {
            return View();
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase uploadFile)
        {
            StringBuilder strValidations = new StringBuilder(string.Empty);
            try
            {
                if (uploadFile.ContentLength > 0)
                {
                    var arquivoNome = Path.GetFileName(uploadFile.FileName);
                    var caminho = HttpContext.Server.MapPath("../Uploads");                    
                    string filePath = Path.Combine(caminho,arquivoNome
                        );
                    uploadFile.SaveAs(filePath);

                    var package = new ExcelPackage(uploadFile.InputStream);
                    ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                    DataTable table = new DataTable();

                    //DataSet ds = new DataSet();
                    //A 32-bit provider which enables the use of

                    //string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";

                    //using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
                    //{
                    //    conn.Open();
                    //    using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
                    //    {
                    //        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    //        string query = "SELECT * FROM [" + sheetName + "]";
                    //        OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
                    //        //DataSet ds = new DataSet();
                    //        adapter.Fill(ds, "Items");

                    //        if (ds.Tables.Count > 0)
                    //        {
                    //            if (sheetName.ToLower().StartsWith("setor"))
                    //            {
                    //                if (ImportarExcel.ImportarSetor(ds.Tables[0]))
                    //                {
                    //                    Success(String.Format("Setores importados com sucesso!"), true);
                    //                }
                    //                else
                    //                {
                    //                    Danger(String.Format("Falha ao importar Colaboradores!"), true);
                    //                }
                    //            }else if (sheetName.ToLower().StartsWith("colaborador"))
                    //            {
                    //                if (ImportarExcel.ImportarColaborador(ds.Tables[0]))
                    //                {
                    //                    Success(String.Format("Colaboradores importados com sucesso!"), true);
                    //                }
                    //                else
                    //                {
                    //                    Danger(String.Format("Falha ao importar ou limite de Colaboradores atingido!"), true);
                    //                }
                    //            }
                    //            else if (sheetName.ToLower().StartsWith("epi"))
                    //            {
                    //                if (ImportarExcel.ImportarEpi(ds.Tables[0]))
                    //                {
                    //                    Success(String.Format("EPI's importados com sucesso!"), true);
                    //                }
                    //                else
                    //                {
                    //                    Danger(String.Format("Falha ao importar EPI's!"), true);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
                    {
                        table.Columns.Add(firstRowCell.Text);
                    }
                    for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                    {
                        var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                        var newRow = table.NewRow();
                        foreach (var cell in row)
                        {
                            newRow[cell.Start.Column - 1] = cell.Text;
                        }
                        table.Rows.Add(newRow);
                    }

                    if (table.Rows.Count > 0)
                    {
                        if (workSheet.Name.ToLower().StartsWith("setor"))
                        {
                            if (ImportarExcel.ImportarSetor(table))
                            {
                                Success(String.Format("Setores importados com sucesso!"), true);
                            }
                            else
                            {
                                Danger(String.Format("Falha ao importar Colaboradores!"), true);
                            }
                        }
                        else if (workSheet.Name.ToLower().StartsWith("colaborador"))
                        {
                            if (ImportarExcel.ImportarColaborador(table))
                            {
                                Success(String.Format("Colaboradores importados com sucesso!"), true);
                            }
                            else
                            {
                                Danger(String.Format("Falha ao importar ou limite de Colaboradores atingido!"), true);
                            }
                        }
                        else if (workSheet.Name.ToLower().StartsWith("epi"))
                        {
                            if (ImportarExcel.ImportarEpi(table))
                            {
                                Success(String.Format("EPI's importados com sucesso!"), true);
                            }
                            else
                            {
                                Danger(String.Format("Falha ao importar EPI's!"), true);
                            }
                        }
                        else if (workSheet.Name.ToLower().StartsWith("centrodecusto"))
                        {
                            if (ImportarExcel.ImportarCentroCusto(table))
                            {
                                Success(String.Format("Centros de Custo importados com sucesso!"), true);
                            }
                            else
                            {
                                Danger(String.Format("Falha ao importar Centros de Custo!"), true);
                            }
                        }
                        else if (workSheet.Name.ToLower().StartsWith("unidade_negocio"))
                        {
                            try
                            {
                                if (ImportarExcel.ImportarUnidadesNegocio(table))
                                {
                                    Success(String.Format("Unidades de Negócio importadas com sucesso!"), true);
                                }
                                else
                                {
                                    Danger(String.Format("Falha ao importar Unidades de Negócio!"), true);
                                }
                            }catch(Exception ex)
                            {
                                while (ex != null)
                                {
                                    Warning(ex.Message, true);
                                    ex = ex.InnerException;
                                }
                            }
                        }
                        else if (workSheet.Name.ToLower().StartsWith("uniforme"))
                        {
                            if (ImportarExcel.ImportarUniforme(table))
                            {
                                Success(String.Format("Uniforme importados com sucesso!"), true);
                            }
                            else
                            {
                                Danger(String.Format("Falha ao importar Uniformes!"), true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);

                Danger(String.Format("Falha ao importar ou Formato de arquivo inválido! " + ex.Message), true);
            }

            return RedirectToAction("Upload");
        }
    }
}