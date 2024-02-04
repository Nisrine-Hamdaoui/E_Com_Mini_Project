﻿using E_Com_Project.Models;

namespace E_Com_Project.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int productId, int qty);
        Task<int> RemoveItem(int productId);
        Task<Cart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task< Cart> GetCart(string userId);
        Task<bool> DoCheckout();
        Task<int> GetCartItemCount();
    }
}