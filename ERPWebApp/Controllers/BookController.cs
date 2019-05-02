using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.ViewModels;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class BookController : Controller
    {
        IBookService _bookService;
        IBookCategoryService _bookCategoryService;
        IMerchantService _merchantService;
        private readonly IHostingEnvironment _hostingEnvironment;
        IUnitOfWork _unitOfWork;
        public BookController(IHostingEnvironment hostingEnvironment,IMerchantService merchantService ,IBookCategoryService bookCategoryService, IBookService bookService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _unitOfWork =unitOfWork;
            _merchantService = merchantService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region AJAX API
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
                if (model != null)
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
        public IActionResult GetAllPaging( string fromdate, string todate, string keyword, int bookcategoryid, int page, int pageSize)
        {
            var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            var M=_merchantService.GetBysId(userid);
            var model = _bookService.GetAllPaging(M.KeyId, fromdate, todate, keyword, bookcategoryid, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllBookCategory()
        {
            var model = _bookCategoryService.GetAll();
            return new OkObjectResult(model);
        }


        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _bookService.GetById(id);
            return new OkObjectResult(model);
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
                    var filename = ContentDispositionHeaderValue
                                       .Parse(file.ContentDisposition)
                                       .FileName
                                       .Trim('"');

                    string folder = _hostingEnvironment.WebRootPath + $@"\images\"+merchant.MerchantCompanyName+"\\books";
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string filePath = Path.Combine(folder, filename);
                    fileName.Add(Path.Combine($@"\images\"+merchant.MerchantCompanyName +"\\books", filename).Replace($@"\", $@"/"));

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