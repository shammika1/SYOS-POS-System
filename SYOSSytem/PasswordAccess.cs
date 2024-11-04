namespace SYOSSytem;

public static class PasswordAccess
{
    private static readonly string correctPassword = "1";

    public static bool Authenticate()
    {
        Console.Write("Enter password: ");
        var enteredPassword = ReadPassword();
        return enteredPassword == correctPassword;
    }

    private static string ReadPassword()
    {
        var password = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}