using System.Text;

namespace FinanceTracker;

internal static class ConsoleEncodingSetup
{
    public static void Set()
    {
        try
        {
            var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

            if (Console.OutputEncoding.WebName != "utf-8")
                Console.OutputEncoding = utf8;

            if (Console.InputEncoding.WebName != "utf-8")
                Console.InputEncoding = utf8;

        }
        catch
        {
            // ignored
        }
    }
}