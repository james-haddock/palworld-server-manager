public class ConsoleWrapper : IConsole
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }
    public ConsoleKeyInfo ReadKey()
    {
        return Console.ReadKey();
    }
}