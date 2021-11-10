using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using TitansMVC.Models;
using TitansMVC.Repository.Implementations;
using TitansMVC.Repository.Interfaces;

namespace TitansMVC.Identity
{
    public class EmailService : IIdentityMessageService
    {
        private IEmailConfirmacaoRegistroRepository _emailConfirmacaoRegistroRepo;

        public Task SendAsync(IdentityMessage message)
        {
            //return ConfigSendGridasync(message);
            return SendMail(message);
        }

        public Task SendEmail(RegisterViewModel model)
        {
            //return ConfigSendGridasync(message);
            return SendMailAdapt(model);
        }

        public Task SendEmailUser(UsuarioModel model)
        {
            //return ConfigSendGridasync(message);
            return SendMailUser(model);
        }

        // Implementação do SendGrid
        //private Task ConfigSendGridasync(IdentityMessage message)
        //{
        //    var myMessage = new SendGridMessage();
        //    myMessage.AddTo(message.Destination);
        //    myMessage.From = new MailAddress("admin@portal.com.br", "Admin do Portal");
        //    myMessage.Subject = message.Subject;
        //    myMessage.Text = message.Body;
        //    myMessage.Html = message.Body;

        //    var credentials = new NetworkCredential(ConfigurationManager.AppSettings["mailAccount"], ConfigurationManager.AppSettings["mailPassword"]);

        //    // Criar um transport web para envio de e-mail
        //    var transportWeb = new Web(credentials);

        //    // Enviar o e-mail
        //    if (transportWeb != null)
        //    {
        //        var x = transportWeb.DeliverAsync(myMessage);
        //        return x;
        //    }
        //    else
        //    {
        //        return Task.FromResult(0);
        //    }
        //}

        // Implementação de e-mail manual 1 (original)
        private Task SendMail(IdentityMessage message)
        {
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                var text = HttpUtility.HtmlEncode(message.Body);

                var msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["ContaDeEmail"], "Admin do Portal");
                msg.To.Add(new MailAddress(message.Destination));
                msg.Subject = message.Subject;
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

                var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["ClienteSmtp"], Convert.ToInt32(ConfigurationManager.AppSettings["PortaSmtp"]));
                var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = ConfigurationManager.AppSettings["Ssl"] == "true";
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }

        // Implementação de e-mail manual 2 (adaptado)
        private Task SendMailAdapt(RegisterViewModel model)
        {
            
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                //var text = HttpUtility.HtmlEncode(message.Body);
                StringBuilder corpoEmail = new StringBuilder();
                corpoEmail.Append(string.Format("*************************<br/>"));
                corpoEmail.Append(string.Format("Foi efetuado um registro pelo site Control Epi.<br/>"));
                corpoEmail.Append(string.Format("Razão Social: {0}<br/>", model.RazaoSocialEmpr));
                corpoEmail.Append(string.Format("CNPJ: {0}<br/>", model.CnpjEmpr));
                corpoEmail.Append(string.Format("Email: {0}<br/>", model.Email));
                corpoEmail.Append(string.Format("Telefone: {0}<br/>", model.TelContato));
                corpoEmail.Append(string.Format("*************************<br/>"));
                //var text = HttpUtility.HtmlEncode(corpoEmail.ToString());
                var text = corpoEmail.ToString();

                var msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["ContaDeEmail"], "Admin do Portal");
                //msg.To.Add(new MailAddress(message.Destination));

                //var emailsDest = new List<EmailConfirmacaoRegistroModel>();
                _emailConfirmacaoRegistroRepo = new EmailConfirmacaoRegistroRepository();

                var emailsDest = _emailConfirmacaoRegistroRepo.GetAll().ToList();

                foreach (var mail in emailsDest)
                {
                    msg.To.Add(new MailAddress(mail.Email));
                }

                //msg.Subject = message.Subject;
                msg.Subject = "Contato Site ControlEpi";

                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

                var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["ClienteSmtp"], Convert.ToInt32(ConfigurationManager.AppSettings["PortaSmtp"]));
                var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = ConfigurationManager.AppSettings["Ssl"] == "true";
                try
                {
                    smtpClient.Send(msg);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    smtpClient.Dispose();
                }               
            }

            return Task.FromResult(0);
        }

        // Implementação de e-mail manual 1 (original)
        private Task SendMailUser(UsuarioModel usuario)
        {
            if (ConfigurationManager.AppSettings["Internet"] == "true")
            {
                //var text = HttpUtility.HtmlEncode(message.Body);
                StringBuilder corpoEmail = new StringBuilder(string.Format("*************************<br/>"));
                corpoEmail.Append(string.Format("Olá!<br/>"));
                corpoEmail.Append(string.Format("Você foi cadastrado no site Control EPI. Fique à vontade para explorar nossos recursos e entrar em contato caso tenha alguma dúvida.<br/>"));
                corpoEmail.Append(string.Format("Seus dados de acesso são:<br/>"));
                corpoEmail.Append(string.Format("Usuário: {0}<br/>", usuario.Email));
                corpoEmail.Append(string.Format("Senha: {0}<br/>", usuario.Password));
                corpoEmail.Append(string.Format("Abraços da equipe!<br/>"));
                corpoEmail.Append(string.Format("<br/>"));
                corpoEmail.Append(string.Format("www.controlepi.com.br<br/>"));
                corpoEmail.Append(string.Format("*************************<br/>"));
                var text = HttpUtility.HtmlEncode(corpoEmail.ToString());

                var msg = new MailMessage();
                msg.From = new MailAddress(ConfigurationManager.AppSettings["ContaDeEmail"], "Admin do Portal");
                msg.To.Add(new MailAddress(usuario.Email));
                msg.Subject = "Registro Control EPI";
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));

                var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["ClienteSmtp"], Convert.ToInt32(ConfigurationManager.AppSettings["PortaSmtp"]));
                var credentials = new NetworkCredential(ConfigurationManager.AppSettings["ContaDeEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = ConfigurationManager.AppSettings["Ssl"] == "true";
                smtpClient.Send(msg);
            }

            return Task.FromResult(0);
        }
    }
}