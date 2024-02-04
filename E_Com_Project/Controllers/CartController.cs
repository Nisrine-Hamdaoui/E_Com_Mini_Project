using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using E_Com_Project.Models;
using System.Diagnostics;
using E_Com_Project.Repositories;



namespace E_Com_Project.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartRepository cartRepo, ILogger<CartController> logger)
        {
            _cartRepo = cartRepo;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IActionResult> AddItem(int ProductId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(ProductId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int ProductId)
        {
            var cartCount = await _cartRepo.RemoveItem(ProductId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public async Task<IActionResult> Checkout()
        {

            bool isCheckedOut = await _cartRepo.DoCheckout();
            return RedirectToAction("Index", "Home");

        }
    }
}