using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using Microsoft.EntityFrameworkCore;

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
            _context.Add(obj); //Adicionar no meu context o obj passado como argumento do método Insert;
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); //Linq
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id); //Procuro o id vindo do argumento;
            _context.Seller.Remove(obj); //Removo do DbSet passando o obj
            _context.SaveChanges(); //Entity framework efetiva no banco de dados;

        }
    }
}
