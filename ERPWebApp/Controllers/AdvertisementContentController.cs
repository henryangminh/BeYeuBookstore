using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class AdvertisementContentController : Controller
    {

        IAdvertisementContentService _advertisementContentService;
        IUnitOfWork _unitOfWork;
        public AdvertisementContentController(IAdvertisementContentService advertisementContentService, IUnitOfWork unitOfWork)
        {
            _advertisementContentService = advertisementContentService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}