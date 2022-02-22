using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;

namespace WebSalesMvc.Services
{
    public class SellerService
    {
        private readonly WebSalesMvcContext _context; //DEPENDÊNCIA DO DBCONTEXT;

        public SellerService(WebSalesMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList(); //Retorna do banco todos os vendedores, convertido para uma lista;
        }

        public void Insert(Seller obj)
        {
            obj.Department = _context.Department.First(); //Provisoriamente atribuir o primeeiro departamento ao obj;
            _context.Add(obj); //Adicionar no meu context o obj passado como argumento do método Insert;
            _context.SaveChanges();
        }
    }
}
