using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Services;
using BeYeuBookstore.Utilities.Constants;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BeYeuBookstore.Controllers
{
    public class BookController : Controller
    {
        IRatingDetailService _ratingDetailService;
        IBookService _bookService;
        IBookCategoryService _bookCategoryService;
        IAuthorizationService _authorizationService;
        private readonly IEmailService _emailService;
        IMerchantService _merchantService;
        private readonly IHostingEnvironment _hostingEnvironment;
        IUnitOfWork _unitOfWork;
        public BookController(IRatingDetailService ratingDetailService,IEmailService emailService, IAuthorizationService authorizationService,IHostingEnvironment hostingEnvironment,IMerchantService merchantService ,IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _ratingDetailService = ratingDetailService;
            _emailService = emailService;
            _authorizationService = authorizationService;
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork =unitOfWork;
            _merchantService = merchantService;
            _hostingEnvironment = hostingEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            var temp = Task.Run(() => _authorizationService.AuthorizeAsync(User, Const_FunctionId.Book, Operations.Read));
            temp.Wait();
            //check truy cập
            if (temp.Result.Succeeded == false)
                return new RedirectResult("/Home/Index");
            return View();
        }
        #region AJAX API
        [Authorize]
        [HttpPost]
        public IActionResult SaveEntity(BookViewModel bookVm)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var model = new MerchantViewModel();
            if (Guid.TryParse(userid, out var guid))
            {
                model = _merchantService.GetBysId(userid);
                
            }
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (model.KeyId != 0)
                {
                    if (bookVm.KeyId == 0)
                    {

                        _bookService.Add(bookVm);
                    }
                    else
                    {
                        _bookService.Update(bookVm);
                    }
                    _bookCategoryService.Save();
                    return new OkObjectResult(bookVm);
                }
                else
                {
                    return new BadRequestResult();
                }
            }
        }

        [HttpGet]
        public IActionResult GetAllPaging(int mId, string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M=_merchantService.GetBysId(userid);
            var model = _bookService.GetAllPaging(mId, M.KeyId, fromdate, todate, keyword, bookcategoryid, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllBookCategory()
        {
            var model = _bookCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult ConfirmEmail(string toEmailAddress, string subject, string content)
        {
            try
            {
                string MailContent = System.IO.File.ReadAllText(@"./Helpers/template.html");
                //MailContent = MailContent.Replace("{{Code}}", "Active");

                new MailHelper().SendMail(toEmailAddress, "Register Code", MailContent);
                return new OkObjectResult("true");
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _bookService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult UpdateBookRatingById(int id)
        {
            double rating = _ratingDetailService.CalculateBookRatingByBookId(id);
            _bookService.UpdateBookRating(rating, id);
            _bookService.Save();
            return new OkObjectResult("true");
        }

        [HttpGet]
        public IActionResult GetMerchantInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            if (Guid.TryParse(userid, out var guid))
            {
                var model = _merchantService.GetBysId(userid);
                return new OkObjectResult(model);
            }
            return new BadRequestResult();
        }
        [HttpPost]
        public IActionResult UpdateBookStatus(int id, int status)
        {
            _bookService.UpdateBookStatus(id, status);
            _bookService.Save();
            return new OkObjectResult("true");
        }
        
        [HttpGet]
        public IActionResult GetAllMerchantInfo()
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M = _merchantService.GetBysId(userid);
            var model = _merchantService.GetAllByBook(M.KeyId);
                return new OkObjectResult(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ImportFiles(IList<IFormFile> files)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var merchant = new MerchantViewModel();
            if (Guid.TryParse(userid, out var guid))
            {
                merchant = _merchantService.GetBysId(userid);
            }
          
            if (files != null && files.Count > 0)
            {
                List<string> fileName = new List<string>();
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    var extension = Path.GetExtension(file.FileName);
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss");
                    filename = (filename + extension).ToString();
                    string folder = _hostingEnvironment.WebRootPath + $@"\images\merchant\"+merchant.MerchantCompanyName+"\\books";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, filename);
                    fileName.Add(Path.Combine($@"\images\merchant\"+merchant.MerchantCompanyName +"\\books", filename).Replace($@"\", $@"/"));

                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
                _bookService.Save();
                return new OkObjectResult(fileName);
            }
            return new NoContentResult();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                _bookService.Delete(id);
                _bookService.Save();
                return new OkObjectResult(id);

            }


        }
    }
}
#endregion