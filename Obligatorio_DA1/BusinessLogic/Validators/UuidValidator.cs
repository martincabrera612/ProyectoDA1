using System.Text.RegularExpressions;

namespace BusinessLogic.Validators
{
    public static class UuidValidator
    {
        public static void IsValidUuid(string uuid)
        {
            if (!Regex.IsMatch(uuid, @"^[A-Fa-f0-9]{8}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{4}-[A-Fa-f0-9]{12}$"))
            {
                throw new InvalidOperationException("The Uuid has an invalid format");
            }
        }
    }
}