using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Models;
using WebSalesMvc.Services;

namespace WebSalesMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        //INJEÇÃO DE DEPENDÊNCIA
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); //Retorna uma lista de Seller

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]//Anotação Indicando que o método é post
        [ValidateAntiForgeryToken] //Anotação de segurança para evitar injeção durante sessão de conexão.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //Redirecionar para o index;
        }
    }
}
