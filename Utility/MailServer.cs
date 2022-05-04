using MailKit.Security;
using MimeKit;
using System;
using System.Configuration;

namespace Utility
{
    public class MailServer
    {

        public bool SendMailUsingMailKit(string ToEmailId, string CC, string Attachment, string Subject, string stblr, string Bcc = null, string From = null)
        {
            var email = new MimeMessage();
            try
            {
                email.From.Add(MailboxAddress.Parse(From ?? EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["SenderMailFrom"].ToString())));

                if (!string.IsNullOrEmpty(ToEmailId))
                {
                    string[] EmailId = ToEmailId.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string EmailId2 in EmailId)
                    {
                        email.To.Add(MailboxAddress.Parse(EmailId2));
                    }
                }

                if (!string.IsNullOrEmpty(CC))
                {
                    string[] ccId = CC.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string ccEmailId in ccId)
                    {
                        email.Cc.Add(MailboxAddress.Parse(ccEmailId));
                    }
                }

                //from webconfig
                if (!string.IsNullOrEmpty(Bcc))
                {
                    string[] bccId = Bcc.ToString().Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string bccEmailId in bccId)
                    {
                        email.Bcc.Add(MailboxAddress.Parse(bccEmailId));
                    }
                }

                // email.Bcc.Add(MailboxAddress.Parse("muralimohan@indimmune.com"));
                email.Bcc.Add(MailboxAddress.Parse("j.avinash@indimmune.com"));
                email.Bcc.Add(MailboxAddress.Parse("p.gangadhar@indimmune.com"));

                var bodyBuilder = new BodyBuilder();
                if (!string.IsNullOrEmpty(Attachment))
                {
                    bodyBuilder.Attachments.Add(Attachment);
                }

                email.Subject = Subject;

                bodyBuilder.HtmlBody = stblr;
                email.Body = bodyBuilder.ToMessageBody();

                // send email
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["SenderServer"].ToString()), Convert.ToInt32(EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["SenderPortNo"].ToString())), SecureSocketOptions.StartTls);
                    smtp.Authenticate(EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["SenderMailFrom"].ToString()), EncryptDecrypt.Decrypt(ConfigurationManager.AppSettings["SenderMailPwd"].ToString()));
                    try
                    {
                        smtp.Send(email);
                    }
                    catch (Exception ex)
                    {
                        System.Threading.Thread.Sleep(5000);
                        try
                        {
                            smtp.Send(email);
                        }
                        catch (Exception ex2)
                        {
                            System.Threading.Thread.Sleep(5000);
                            try
                            {
                                smtp.Send(email);
                            }
                            catch (Exception ex3)
                            {
                                System.Threading.Thread.Sleep(5000);
                                smtp.Send(email);
                            }
                        }
                    }
                    smtp.Disconnect(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
