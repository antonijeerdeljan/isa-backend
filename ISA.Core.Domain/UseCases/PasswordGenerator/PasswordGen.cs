namespace ISA.Core.Domain.UseCases.PasswordGenerators;

public static class RandomStringGenerator
{
    private static Random random = new Random();

    public static string GenerateRandomString(int length)
    {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()_+-=[]{}|;:',.<>?";

        if (length < 4)
        {
            throw new ArgumentException("Length must be at least 4 to include uppercase, lowercase, numeric, and special characters.");
        }

        var characters = new List<char>
        {
            upperCase[random.Next(upperCase.Length)], 
            lowerCase[random.Next(lowerCase.Length)], 
            digits[random.Next(digits.Length)],       
            specialChars[random.Next(specialChars.Length)] 
        };

        for (int i = 0; i < length; i++)
        {
            string allChars = upperCase + lowerCase + digits + specialChars;
            characters.Add(allChars[random.Next(allChars.Length)]);
        }

        return new string(characters.OrderBy(x => random.Next()).ToArray());
    }
}