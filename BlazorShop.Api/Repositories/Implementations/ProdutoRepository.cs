using BlazorShop.Api.Context;
using BlazorShop.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Api.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produto?> GetItem(int id)
        {
            return  await _context.Produtos
                .Include(p => p.Categoria)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetItens()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetItensPorCategoria(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == id).ToListAsync();
        }
    }
}
