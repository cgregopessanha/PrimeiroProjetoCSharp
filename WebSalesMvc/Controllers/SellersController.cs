using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Models.ViewModels;

namespace WebSalesMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        //INJEÇÃO DE DEPENDÊNCIA
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); //Retorna uma lista de Seller

            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); //BUSCA NO bd TODOS OS DEPARTAMENTOS;
            var viewModel = new SellerFormViewModel { Departments = departments }; //INICIO O CONSTRUTOR COM A LISTA DE DEPARTAMENTOS;
            return View(viewModel); //mINHA VIEW RECEBE VIEWMODEL COM OS DEPARTAMENTOS POPULADOS;
        }


        [HttpPost]//Anotação Indicando que o método é post
        [ValidateAntiForgeryToken] //Anotação de segurança para evitar injeção durante sessão de conexão.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //Redirecionar para o index;
        }
        public IActionResult Delete(int? id)
        {
            //Testando se o id é null
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);
            //testando se o obj existe
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj); //Retorno a view, passando o obj como argumento;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
