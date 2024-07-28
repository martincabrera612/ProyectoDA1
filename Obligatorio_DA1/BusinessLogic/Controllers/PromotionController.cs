using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using Domain;
using Persistence;

namespace BusinessLogic.Controllers;

public class PromotionController
{
    private const string AddPromotion = "Promotion created successfully";
    private const string DeletePromotion = "Promotion deleted successfully";
    private const string UpdatePromotion = "Promotion updated successfully";
    private readonly IRepository<Promotion> _promotionRepository;
    private readonly SessionLogic _sessionLogic;

    public PromotionController(IRepository<Promotion> aPromotionRepository, SessionLogic aSessionLogic)
    {
        _promotionRepository = aPromotionRepository;
        _sessionLogic = aSessionLogic;
    }
    public string Create(Promotion aPromotion)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to create a promotion.");
            }
            PromotionValidator.IsValid(aPromotion);
            _promotionRepository.Add(aPromotion);
            return AddPromotion;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
    
    public string Delete(string aPromotionId)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to delete a promotion.");
            }
            _promotionRepository.Delete(aPromotionId);
            return DeletePromotion;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
    
    public string Update(Promotion aPromotion)
    {
        try
        {
            if (!_sessionLogic.IsAdmin())
            {
                throw new BusinessLogicException("You must be an admin to update a promotion.");
            }
            PromotionValidator.IsValid(aPromotion);
            Console.WriteLine("Edited");
            _promotionRepository.Update(aPromotion);
            return UpdatePromotion;
        }
        catch (BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
}
