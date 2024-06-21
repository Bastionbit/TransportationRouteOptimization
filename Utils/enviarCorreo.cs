using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace ElGatoMensajero.Utilidades
{
  public class enviarCorreo
  {
    public bool SendMail(string email, string asunto, string body)
    {
      MailMessage mailMessage = new MailMessage();
      SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
      smtpClient.Credentials = new NetworkCredential("elgatomensajero@gmail.com", "Hola_123");
      smtpClient.EnableSsl = true;
      try
      {
        smtpClient.Send("elgatomensaje@gmail.com", email, asunto, body);
      }
      catch (Exception ex)
      {
        return false;
      }

      return true;
    }
  }
}