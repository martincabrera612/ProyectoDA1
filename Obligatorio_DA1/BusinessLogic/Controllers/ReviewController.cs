using BusinessLogic.Exceptions;
using BusinessLogic.Validators;
using Domain;
using Persistence;

namespace BusinessLogic.Controllers;

public class ReviewController
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly SessionLogic _sessionLogic;
    private const string AddReview = "Review created successfully";

    public ReviewController(IRepository<Review> aReviewRepository, SessionLogic aSessionLogic)
    {
        _reviewRepository = aReviewRepository;
        _sessionLogic = aSessionLogic;
    }
    
    public string Create(Review aReview)
    {
        try
        {
            aReview.User = _sessionLogic.CurrentUser;
            ReviewValidator.Validate(aReview);
            _reviewRepository.Add(aReview);
            return AddReview;
        }catch(BusinessLogicException exception)
        {
            return exception.Message;
        }
    }
}