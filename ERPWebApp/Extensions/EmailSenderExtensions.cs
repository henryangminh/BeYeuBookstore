using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BeYeuBookstore.Services;

namespace BeYeuBookstore.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Xác nhận tài khoản Bé Yêu Bookstore",
                $"Hãy click vào đường dẫn dưới đây để xác nhận tài khoản: <a href='{HtmlEncoder.Default.Encode(link)}'></a>");
        }
    }
}
