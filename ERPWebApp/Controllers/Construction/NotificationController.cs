using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces.Boq;
using BeYeuBookstore.Application.ViewModels.Boq;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers.Construction
{
    public class NotificationController : Controller
    {
        ISO_NotificationGeneralService _notificationGeneralService;
        ISO_NotificationGeneralDetailService _notificationGeneralDetailService;

        public NotificationController(ISO_NotificationGeneralService notificationGeneralService, ISO_NotificationGeneralDetailService notificationGeneralDetailService)
        {
            _notificationGeneralService = notificationGeneralService;
            _notificationGeneralDetailService = notificationGeneralDetailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region Ajax API
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _notificationGeneralService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            Guid id;
            bool userid = Guid.TryParse(_generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key), out id);
            var model = _notificationGeneralDetailService.GetAllPaging(keyword, page, pageSize,id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _notificationGeneralService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(SO_NotificationGeneralViewModel VendorVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (VendorVm.KeyId == 0)
                {
                    _notificationGeneralService.Add(VendorVm);
                }
                else
                {
                    _notificationGeneralService.Update(VendorVm);
                }
                _notificationGeneralService.Save();
                return new OkObjectResult(VendorVm);
            }
        }
        [HttpGet]
        public IActionResult GetByUserId()
        {
            Guid id;
            bool userid = Guid.TryParse(_generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key),out id);
            var model = _notificationGeneralDetailService.getNotitficationByUserId(id);
            return new OkObjectResult(model);
        }
        [HttpPost]
        public IActionResult UpdateStatus(bool status)
        {
            var CurrentUserId = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);
            _notificationGeneralDetailService.UpdateStatus(CurrentUserId);
            if(_notificationGeneralDetailService.Save())
                return new OkResult();
            return new BadRequestResult();
        }
        #endregion
    }
}