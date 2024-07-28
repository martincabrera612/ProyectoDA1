using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using Domain;
using Domain.Enums;
using Persistence;

namespace BusinessLogic.Services;

public class AvailabilityService
{
    private readonly IRepository<Deposit> _depositRepository;
    private readonly SessionLogic _sessionLogic;

    public AvailabilityService(IRepository<Deposit> aDepositRepository, SessionLogic aSessionLogic)
    {
        _depositRepository = aDepositRepository;
        _sessionLogic = aSessionLogic;
    }

    public bool IsAvailable(Deposit deposit, DateTime from, DateTime to)
    {
        var availability = deposit.Availabilities
            .Any(a => a.From <= from && a.To >= to);

        if (!availability)
        {
            return false;
        }

        var overlap = deposit.Bookings
            .Any(b => b.Status == Status.Approved && (b.From < to && b.To > from));

        return !overlap;
    }
    
    public IList<Deposit> GetAvailableDeposits(DateTime from, DateTime to)
    {
        var deposits = _depositRepository.FindAll();
        return deposits
            .Where(d => IsAvailable(d, from, to))
            .ToList();
    }

    public void AddAvailability(string depositId, DateTime from, DateTime to)
    {
        var deposit = GetDeposit(depositId);
        if (IsAvailable(deposit, from, to))
        {
            throw new BusinessLogicException("Availability already exists in this time frame.");
        }
        var availability = BuildAvailability(from, to);
        _sessionLogic.HasAdminPrivileges();
        AvailabilityValidator.Validate(availability);
        deposit.Availabilities.Add(availability);
        _depositRepository.Update(deposit);
    }
    
    private Availability BuildAvailability(DateTime from, DateTime to)
    {
        return new Availability
        {
            From = from,
            To = to
        };
    }

    private Deposit GetDeposit(string depositId)
    {
        var deposit = _depositRepository.Find(d => d.Id == depositId);
        if (deposit == null)
        {
            throw new BusinessLogicException("Deposit not found.");
        }

        return deposit;
    }
}