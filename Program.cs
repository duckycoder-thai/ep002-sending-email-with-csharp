using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

var message = new MimeMessage();
message.From.Add(new  MailboxAddress("Ducky", "ducky@user.com"));
message.To.Add(new MailboxAddress("Coder", "coder@user.com"));
message.Subject = "สวัสดี, Coder!";

var attachment = new MimePart("image", "gif")
{
    Content = new MimeContent(File.OpenRead("C:\\Users\\ducky\\Downloads\\nyan_cat.gif")),
    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
    ContentTransferEncoding = ContentEncoding.Base64,
    FileName = Path.GetFileName("C:\\Users\\ducky\\Downloads\\nyan_cat.gif")
};

var bodyBuilder = new BodyBuilder();
var image = bodyBuilder.LinkedResources.Add("C:\\Users\\ducky\\Downloads\\nyan_cat.gif");
image.ContentId = MimeUtils.GenerateMessageId();

bodyBuilder.HtmlBody = @"<p>เขียนโค้ดสนุกมาก</p><h1>โดยเฉพาะ C#</h1>" +
$"<img src='cid:{image.ContentId}' alt='Nyan Cat' />";
bodyBuilder.TextBody = @"เขียนโค้ดสนุกมาก
โดยเฉพาะ C#";

var multipart = new Multipart("mixed");
multipart.Add(bodyBuilder.ToMessageBody());
multipart.Add(attachment);
message.Body = multipart;

using var client = new SmtpClient();
client.Connect("localhost", 25, false);
client.Send(message);
client.Disconnect(true);
