using System;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.Getbasket(userName);

            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart basket)
        {
            // TODO : Comunicar com Discount.grpc e calcular os preços atuais
            // dos produtos no carrinho de compras.
            // Fazer uma comunicação entre os serviços Basket.API e Discount.Grpc
            // Basket.API será o client de Discount.Grpc(está definido como server only)
            // Para isso é preciso utilizar o ConnectedServices para criar um cliente para
            // para o discount grpc server e o basket api
            
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{username}", Name = "DeleteBasket")]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var basket = await _repository.Getbasket(userName);
            if (basket is null) return NotFound();
            
            await _repository.DeleteBasket(userName);
            return Ok();
        }

    }
}
