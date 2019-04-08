using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BeYeuBookstore.Controllers
{
    public class AdvertisementPositionController : Controller
    {
        IAdvertisementPositionService _advertisementPositionService;
        IUnitOfWork _unitOfWork;
        public AdvertisementPositionController(IAdvertisementPositionService advertisementPositionService, IUnitOfWork unitOfWork)
        {
            _advertisementPositionService = advertisementPositionService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}