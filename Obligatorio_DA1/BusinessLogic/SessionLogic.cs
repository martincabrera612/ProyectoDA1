using BusinessLogic.Controllers;
using BusinessLogic.Exceptions;
using BusinessLogic.Services;
using Domain;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessLogic;

public class SessionLogic
{
    private readonly UserController _userController;
    private readonly LogService _logService;
    private readonly IMemoryCache _cache;
    public virtual User CurrentUser
    {
        get { return _cache.Get<User>("CurrentUser"); }
        set { _cache.Set("CurrentUser", value); }
    }
    public event Action? OnLogin;
     
    public SessionLogic()
    {
    }
    public SessionLogic(UserController aUserController, LogService aLogService, IMemoryCache cache)
    {
        _userController = aUserController;
        _logService = aLogService;
        _cache = cache;
    }

    public void Login(string email, string password)
    {
        try
        {
            
            User user = _userController.AuthenticateUser(email, password);
            CurrentUser = user;
            _logService.LogLogin(CurrentUser);
            OnLogin?.Invoke();
        }
        catch (BusinessLogicException e)
        {
            throw new BusinessLogicException(e.Message);
        }
    }
    
    public void Logout()
    {
        _logService.LogLogout(CurrentUser);
        _cache.Remove("CurrentUser");
    }
    
    public bool IsLoggedIn()
    {
        return CurrentUser != null;
    }
    
    public virtual bool IsAdmin()
    {
        return CurrentUser != null && CurrentUser.IsAdmin;
    }
    
    public virtual void HasAdminPrivileges()
    {
        if (!IsAdmin())
        {
            throw new BusinessLogicException("You must be an admin to perform this action.");
        }
    }
}