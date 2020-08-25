using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using MailKit.Net.Smtp;
using MimeKit;

namespace PortalApi.Repository
{
    public class HelperRepository
    {

        public void CriarLog(string NomeArquivo, string Conteudo, string Pasta)
        {

            string Caminho = "C:" + @"\\Logs\\" + Pasta;
            if (!Directory.Exists(Caminho))
            {
                Directory.CreateDirectory(Caminho);
            }

            StreamWriter vWriter = new StreamWriter(Caminho + "\\" + NomeArquivo + ".log", true);

            vWriter.WriteLine("Mesagem: "+Conteudo + " | Data e hora da execução: " + DateTime.Now.ToString());
            vWriter.WriteLine("-------------------------------------------------------------------------------------------------------------------------");
            vWriter.Flush();
            vWriter.Close();
        }

        private void SendEmail(string Texto)
        {
            string User = "msdomingos14@gmail.com";
            string Password = "nynolmeihftecdod";

            var message = new MimeMessage();
            var builder = new BodyBuilder();

            message.From.Add(new MailboxAddress(User));
            message.To.Add(new MailboxAddress("valdanio.alberto@incentea.com"));
            // message.To.Add(new MailboxAddress("carlos.nobre@incentea.com"));
            message.Subject = "Integração de dados Portal de Requisições";

            builder.HtmlBody = Texto;

            message.Body = builder.ToMessageBody();
            var client = new SmtpClient();

            client.Connect("smtp.gmail.com");

            client.Authenticate(User, Password);

            client.Send(message);

        }
    }
}