using E_Com_Project.Data;
using E_Com_Project.Models;
using E_Com_Project.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace E_Com_Project.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();

            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    // Utilisateur non connecté, gérer l'erreur en conséquence
                    throw new Exception("User is not logged in");
                }

                using (var transaction = _db.Database.BeginTransaction())
                {
                    var cart = await GetCart(userId);

                    if (cart == null)
                    {
                        // Si le panier n'existe pas, le créer
                        cart = new Cart
                        {
                            UserName = userId
                        };
                        _db.Carts.Add(cart);
                        _db.SaveChanges();
                    }

                    // Détail du panier
                    var cartItem = _db.CartLines
                        .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);

                    if (cartItem != null)
                    {
                        // Mettre à jour la quantité si l'article est déjà dans le panier
                        cartItem.Quantity += qty;
                    }
                    else
                    {
                        // Ajouter un nouvel élément au panier
                        var product = _db.Products.Find(productId);

                        if (product != null)
                        {
                            cartItem = new CartLine
                            {
                                ProductId = productId,
                                CartId = cart.Id,
                                Quantity = qty,
                                UnitPrice = product.Prix
                            };
                            _db.CartLines.Add(cartItem);
                            _db.SaveChanges();
                        }
                        else
                        {
                            // Le produit n'a pas été trouvé, gérer l'erreur
                            throw new Exception("Product not found");
                        }
                    }

                    // Tout s'est bien passé, valider la transaction
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception (journalisation, etc.)
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Retourner le nombre d'articles dans le panier après l'opération
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<int> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartLines
                                  .FirstOrDefault(a => a.CartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartLines.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<Cart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var Cart = await _db.Carts
                                  .Include(a => a.CartLines)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserName == userId).FirstOrDefaultAsync();
            return Cart;

        }
        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(x => x.UserName == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (!string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Carts
                              join cartLine in _db.CartLines
                              on cart.Id equals cartLine.CartId
                              select new { cartLine.Id }
                        ).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartLine = _db.CartLines
                                    .Where(a => a.CartId == cart.Id).ToList();
                if (cartLine.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserName = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartLine)
                {
                    var orderDetail = new OrderDetails
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CartLines.RemoveRange(cartLine);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public async Task<int> GetCartItemCount()
        {
            string userId = GetUserId();
            return await GetCartItemCount(userId);
        }


    }
}
