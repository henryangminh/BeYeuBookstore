using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BeYeuBookstore.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
      //  private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ERPDbContext _context;

        public EFUnitOfWork(ERPDbContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                //try
                //{
                //    // lưu lại log lỗi
                //    string file = $@"\Logs_test_102";
                //    string filePath = _hostingEnvironment.WebRootPath + file;

                //    if (!Directory.Exists(filePath))
                //    {
                //        Directory.CreateDirectory(filePath);
                //    }
                //    filePath += $@"\ErrorSaveDataToDB.txt";
                //    using (StreamWriter writer = new StreamWriter(filePath, true))
                //    {
                //        writer.WriteLine("-----------------------------------------------------------------------------");
                //        writer.WriteLine("Date : " + DateTime.Now.ToString());
                //        writer.WriteLine();
                //        while (ex != null)
                //        {
                //            writer.WriteLine(ex.GetType().FullName);
                //            writer.WriteLine("Message : " + ex.Message);
                //            writer.WriteLine("StackTrace : " + ex.StackTrace);
                //            ex = ex.InnerException;
                //        }
                //    }
                //}
                //catch { }
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
