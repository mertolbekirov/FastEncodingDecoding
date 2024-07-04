using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\merto\Programming\Nethermind\decoded_output.bin"; // Replace with your binary file path
        string outputFilePath = @"C:\Users\merto\Programming\Nethermind\decoded_output.txt"; // Replace with your text file path

        try
        {
            List<int> numbers = new List<int>();
            byte[] buffer = new byte[100 * 4]; // Buffer size for 100 integers

            using (FileStream fs = new FileStream(inputFilePath, FileMode.Open))
            {
                int bytesRead;

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i += 4)
                    {
                        int number = BitConverter.ToInt32(buffer, i);
                        numbers.Add(number);
                    }
                }
            }

            using (StreamWriter sw = new StreamWriter(outputFilePath))
            {
                foreach (int num in numbers)
                {
                    sw.WriteLine(num);
                }
            }

            Console.WriteLine("Numbers have been written to the text file.");
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred while reading or writing the file: {e.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
