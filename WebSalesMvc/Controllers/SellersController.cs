using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Models;
using WebSalesMvc.Services;
using WebSalesMvc.Models.ViewModels;
using WebSalesMvc.Services.Exceptions;

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
            /*Esse if é para, caso o JavaScript esteja desabilitado, o APP não aceitar gravações em branco*/
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //Redirecionar para o index;
        }
        public IActionResult Delete(int? id)
        {
            //Testando se o id é null
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" });
            }

            var obj = _sellerService.FindById(id.Value);
            //testando se o obj existe
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" });
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

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" }); ;
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" }); ;
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" }); ;
            }

            var obj = _sellerService.FindById(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" });
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            /*Esse if é para, caso o JavaScript esteja desabilitado, o APP não aceitar gravações em branco*/
            if (!ModelState.IsValid)
            {
                var departments = _departmentService.FindAll();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Mismatch!" });
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
