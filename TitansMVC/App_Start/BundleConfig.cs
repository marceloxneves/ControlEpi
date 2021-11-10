using System.Web;
using System.Web.Optimization;

namespace TitansMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signaturepad").Include(
                        "~/Scripts//SignaturePad/flashcanvas.js",
                        "~/Scripts//SignaturePad/jquery.signaturepad.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.8.24.min.js",
                        "~/Scripts/datepicker-pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/bootstrap-switch.min.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/moment-with-locales.js",
                        "~/Scripts/bootstrap-filestyle.min.js",
                        "~/Scripts/bootstrap-datetimepicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(  
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/json").Include(
                        "~/Scripts/json.js"));

            bundles.Add(new ScriptBundle("~/bundles/mascaras").Include(
                        "~/Scripts/jquery.maskedinput-1.4.1.pack.js",
                        "~/Scripts/jquery.maskMoney.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jszipcompr").Include(
                        "~/Scripts/jszip.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/util").Include(
                        "~/Scripts/util.js"));

            bundles.Add(new ScriptBundle("~/bundles/validacao").Include(
                        "~/Scripts/valida-cpf-cnpj.js"));
            

            // Validações no padrão brasileiro 
            bundles.Add(new ScriptBundle("~/bundles/jqueryfixes").Include(
                        "~/Scripts/jQueryFixes.js"));
            bundles.Add(
                new ScriptBundle("~/bundles/validations_pt-br")
                    .Include(
                        "~/Scripts/jquery.validate.zcustom.pt-br.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/registro").Include(
                        "~/Scripts//cadastros/CadRegistro.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/colaborador").Include(
                        "~/Scripts//cadastros/CadColaborador.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/empresa").Include(
                        "~/Scripts//cadastros/CadEmpresa.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/epi").Include(
                       "~/Scripts//cadastros/CadEpi.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/uniforme").Include(
                       "~/Scripts//cadastros/CadUniforme.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/centroCusto").Include(
                       "~/Scripts//cadastros/CadCentroCusto.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/setor").Include(
                       "~/Scripts//cadastros/CadSetor.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/lbc").Include(
                       "~/Scripts//cadastros/CadLbc.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/familiaEpi").Include(
                       "~/Scripts//cadastros/cadFamiliaEpi.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/tipoEpi").Include(
                       "~/Scripts//cadastros/cadTipoEpi.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/tipoUniforme").Include(
                       "~/Scripts//cadastros/cadTipoUniforme.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/usuario").Include(
                       "~/Scripts//cadastros/CadUsuario.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/plano").Include(
                       "~/Scripts//cadastros/cadPlano.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/relEpiCol").Include(
                        "~/Scripts//cadastros/RelEpi.js"));

            bundles.Add(new ScriptBundle("~/bundles/cadastros/relUniformeCol").Include(
                        "~/Scripts//cadastros/RelUniforme.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-datetimepicker.css",
                      "~/Content//bootstrap-switch//bootstrap3/bootstrap-switch.min.css",
                      "~/Content/site.css",
                      "~/Content/jquery.signaturepad.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content//themes//base/jquery.ui.core.css",
                        "~/Content//themes//base/jquery.ui.resizable.css",
                        "~/Content//themes//base/jquery.ui.selectable.css",
                        "~/Content//themes//base/jquery.ui.accordion.css",
                        "~/Content//themes//base/jquery.ui.autocomplete.css",
                        "~/Content//themes//base/jquery.ui.button.css",
                        "~/Content//themes//base/jquery.ui.dialog.css",
                        "~/Content//themes//base/jquery.ui.slider.css",
                        "~/Content//themes//base/jquery.ui.tabs.css",
                        "~/Content//themes//base/datepicker.css",
                        "~/Content//themes//base/jquery.ui.progressbar.css",
                        "~/Content//themes/jquery-ui.theme.min.css"));
            
            BundleTable.EnableOptimizations = false;
        }
    }
}
