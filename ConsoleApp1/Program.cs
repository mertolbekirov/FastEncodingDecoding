using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\merto\Programming\Nethermind\output_sorted_unique_numbers.bin"; // Replace with your desired output file path
        string outputFilePath = @"C:\Users\merto\Programming\Nethermind\bindings_copmressed_numbers.bin"; // Replace with your desired output file path


        List<int> numbers = new List<int>();
        byte[] buffer = new byte[100 * 4]; // Buffer size for 100 integers

        Console.WriteLine("Reading from file");

        using (FileStream fs = new FileStream(inputFilePath, FileMode.Open))
        {
            int bytesRead;

            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                // Read integers from buffer
                for (int i = 0; i < bytesRead; i += 4)
                {
                    int number = BitConverter.ToInt32(buffer, i);
                    numbers.Add(number);
                }
            }
        }

        var outputArray = new byte[numbers.Count * 32];

        Console.WriteLine("Before binding call");
        TurboPFor.vbenc32(numbers.ToArray(), numbers.Count, outputArray);
        Console.WriteLine("After binding call");
        File.WriteAllBytes(outputFilePath, outputArray);

    }
}
