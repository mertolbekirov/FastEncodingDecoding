using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        string encodedFilePath = @"C:\Users\merto\Programming\Nethermind\encoded_output.bin"; // Replace with your encoded file path
        string outputFilePath = @"C:\Users\merto\Programming\Nethermind\decoded_output.bin"; // Replace with your output file path

        try
        {
            List<int> numbers = new List<int>();

            using (FileStream fs = new FileStream(encodedFilePath, FileMode.Open))
            {
                byte[] buffer = new byte[4]; // Buffer size for a single integer
                int bytesRead;

                // Read the first number
                bytesRead = fs.Read(buffer, 0, 4);
                if (bytesRead == 4)
                {
                    int firstNumber = BitConverter.ToInt32(buffer, 0);
                    numbers.Add(firstNumber);
                }
                else
                {
                    Console.WriteLine("Failed to read the first number.");
                    return;
                }

                // Read the differential encoded values
                while ((bytesRead = fs.Read(buffer, 0, 4)) > 0)
                {
                    int diff = BitConverter.ToInt32(buffer, 0);
                    int lastNumber = numbers[^1];
                    int number = lastNumber + diff;
                    numbers.Add(number);
                }
            }

            // Write the numbers to the output file
            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (int number in numbers)
                {
                    byte[] bytes = BitConverter.GetBytes(number);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            Console.WriteLine("Decoding complete.");
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
