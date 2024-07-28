using BusinessLogic.Validators;

namespace UnitTests.BusinessLogicTests;

[TestClass]
public class UuidTests
{
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidateInvalidUuid()
    {
        const string invalidUuid = "invalid-uuid";

        UuidValidator.IsValidUuid(invalidUuid);
    }
}
