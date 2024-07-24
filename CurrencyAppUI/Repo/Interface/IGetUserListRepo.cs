using CurrencyAppUI.Models;

namespace CurrencyAppUI.Repo.Interface
{
    public interface IGetUserListRepo
    {
        Task<List<UserViewModel>> GetAllUsers();
    }
}