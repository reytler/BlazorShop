﻿using BlazorShop.Api.Context;
using BlazorShop.Api.Entities;
using BlazorShop.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Api.Repositories
{
    public class CarrinhoCompraRepository : ICarrinhoCompraRepository
    {
        private readonly AppDbContext _context;

        public CarrinhoCompraRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CarrinhoItem> AdicionaItem(CarrinhoItemAdicionaDto carrinhoItemAdicionaDto)
        {
            if (!await CarrinhoItemJaExiste(carrinhoItemAdicionaDto.CarrinhoId, 
                carrinhoItemAdicionaDto.ProdutoId))
            {
                var item = await (from produto in _context.Produtos
                                  where produto.Id == carrinhoItemAdicionaDto.ProdutoId
                                  select new CarrinhoItem
                                  {
                                      CarrinhoId = carrinhoItemAdicionaDto.CarrinhoId,
                                      ProdutoId = produto.Id,
                                      Quantidade = carrinhoItemAdicionaDto.Quantidade
                                  }).SingleOrDefaultAsync();
                if(item is not null)
                {
                    var resultado = await _context.CarrinhoItems.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return resultado.Entity;
                }
            }

            return null;
        }

        public async Task<CarrinhoItem> AtualizaQuantidade(int id, CarrinhoItemAtualizaQuantidadeDto carrinhoItemAtualizaQuantidadeDto)
        {
            var carrinhoItem = await _context.CarrinhoItems.FindAsync(id);

            if(carrinhoItem is not null)
            {
                carrinhoItem.Quantidade = carrinhoItemAtualizaQuantidadeDto.Quantidade;
                await _context.SaveChangesAsync();
                return carrinhoItem;
            }

            return null;
        }

        public async Task<CarrinhoItem> DeletaItem(int id)
        {
            var item = await _context.CarrinhoItems.FindAsync(id);
            if(item is not null)
            {
                _context.CarrinhoItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return item;
        }

        public async Task<CarrinhoItem> GetItem(int id)
        {
            return await (from carrinho in _context.Carrinhos
                          join carrinhoItem in _context.CarrinhoItems
                          on carrinho.Id equals carrinhoItem.CarrinhoId
                          where carrinhoItem.Id == id
                          select new CarrinhoItem
                          {
                              Id = carrinhoItem.Id,
                              ProdutoId = carrinhoItem.ProdutoId,
                              Quantidade = carrinhoItem.Quantidade,
                              CarrinhoId = carrinhoItem.CarrinhoId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CarrinhoItem>> GetItems(string usuarioId)
        {
            return await (from carrinho  in _context.Carrinhos
                          join carrinhoItem in _context.CarrinhoItems
                          on carrinho.Id equals carrinhoItem.CarrinhoId
                          where carrinho.UsuarioId == usuarioId
                          select new CarrinhoItem
                          {
                              Id = carrinhoItem.Id,
                              ProdutoId = carrinhoItem.ProdutoId,
                              Quantidade = carrinhoItem.Quantidade,
                              CarrinhoId = carrinhoItem.CarrinhoId
                          }).ToListAsync();
        }

        private async Task<bool> CarrinhoItemJaExiste(int carrinhoId, int produtoId)
        {
            return await _context.CarrinhoItems.AnyAsync(c => c.CarrinhoId == carrinhoId && 
                                                                c.ProdutoId == produtoId);
        }
    }
}
