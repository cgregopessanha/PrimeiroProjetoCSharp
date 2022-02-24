using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSalesMvc.Data;
using WebSalesMvc.Models;
using WebSalesMvc.Services.Exceptions;
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

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync(); //Retorna do banco todos os vendedores, convertido para uma lista;
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj); //Adicionar no meu context o obj passado como argumento do método Insert;
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id); //Linq
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id); //Procuro o id vindo do argumento;
                _context.Seller.Remove(obj); //Removo do DbSet passando o obj
                _context.SaveChanges(); //Entity framework efetiva no banco de dados;
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Can not delete this Seller bacause He/She has sales.");
            }
        }

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id Not Found!");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }

        }
    }
}
