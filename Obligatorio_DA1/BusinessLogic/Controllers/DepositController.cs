using BusinessLogic.Exceptions;
using Domain;
using Persistence;

namespace BusinessLogic.Controllers;

public class DepositController
{
    private const string AddDeposit = "Deposit created successfully";
    private const string DeleteDeposit = "Deposit deleted successfully";
    private readonly IRepository<Deposit?> _depositRepository;
    private readonly SessionLogic _sessionLogic;

    public DepositController(IRepository<Deposit> aDepositRepository, SessionLogic aSessionLogic)
    {
        _depositRepository = aDepositRepository;
        _sessionLogic = aSessionLogic;
    }
    public string Create(Deposit aDeposit)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to create a deposit.");
            }
            _depositRepository.Add(aDeposit);
            return AddDeposit;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
        
    public string Delete(string aDepositId)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to delete a deposit.");
            }
            _depositRepository.Delete(aDepositId);
            return DeleteDeposit;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
    
    public string Update(Deposit aDeposit)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to update a deposit.");
            }
            _depositRepository.Update(aDeposit);
            return AddDeposit;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
}