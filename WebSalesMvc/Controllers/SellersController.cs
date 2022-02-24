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

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync(); //Retorna uma lista de Seller

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync(); //BUSCA NO bd TODOS OS DEPARTAMENTOS;
            var viewModel = new SellerFormViewModel { Departments = departments }; //INICIO O CONSTRUTOR COM A LISTA DE DEPARTAMENTOS;
            return View(viewModel); //mINHA VIEW RECEBE VIEWMODEL COM OS DEPARTAMENTOS POPULADOS;
        }


        [HttpPost]//Anotação Indicando que o método é post
        [ValidateAntiForgeryToken] //Anotação de segurança para evitar injeção durante sessão de conexão.
        public async Task<IActionResult> Create(Seller seller)
        {
            /*Esse if é para, caso o JavaScript esteja desabilitado, o APP não aceitar gravações em branco*/
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index)); //Redirecionar para o index;
        }
        public async Task<IActionResult> Delete(int? id)
        {
            //Testando se o id é null
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            //testando se o obj existe
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" });
            }
            return View(obj); //Retorno a view, passando o obj como argumento;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" }); ;
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" }); ;
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Provided!" }); ;
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Not Found!" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            /*Esse if é para, caso o JavaScript esteja desabilitado, o APP não aceitar gravações em branco*/
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id Mismatch!" });
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
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
