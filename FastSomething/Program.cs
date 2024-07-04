using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\merto\Programming\Nethermind\ddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef";  // Replace with your input file path
        string outputFilePath = @"C:\Users\merto\Programming\Nethermind\output_sorted_unique_numbers.bin"; // Replace with your desired output file path

        try
        {
            List<int> numbers = new List<int>();
            byte[] buffer = new byte[100 * 4]; // Buffer size for 100 integers

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

            // Remove duplicates
            numbers = numbers.Distinct().ToList();

            // Sort the numbers
            numbers.Sort();

            // Write sorted unique numbers to a new binary file
            using (FileStream fs = new FileStream(outputFilePath, FileMode.Create))
            {
                foreach (int num in numbers)
                {
                    byte[] bytes = BitConverter.GetBytes(num);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            Console.WriteLine("Sorted unique numbers have been written to the output file.");
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
