using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

public class Program
{
    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\merto\Programming\Nethermind\output_sorted_unique_numbers.bin"; // Replace with your binary file path
        string outputFilePath = @"C:\Users\merto\Programming\Nethermind\encoded_output.bin"; // Replace with your encoded output file path

        try
        {
            byte[] buffer = new byte[257 * 4]; // Buffer size for 257 integers
            List<byte> outputBuffer = new List<byte>();

            using (FileStream fs = new FileStream(inputFilePath, FileMode.Open))
            {
                int bytesRead;

                // Read the first number
                bytesRead = fs.Read(buffer, 0, 4);
                if (bytesRead == 4)
                {
                    int firstNumber = BitConverter.ToInt32(buffer, 0);
                    outputBuffer.AddRange(BitConverter.GetBytes(firstNumber));
                }
                else
                {
                    Console.WriteLine("Failed to read the first number.");
                    return;
                }

                fs.Position = 0;

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    int vectorCount = bytesRead / 4; // Number of integers read
                    ReadOnlySpan<int> numbers = MemoryMarshal.Cast<byte, int>(buffer.AsSpan(0, bytesRead));

                    ref int searchspace = ref MemoryMarshal.GetReference(numbers);

                    for (int i = 0; i <= vectorCount - Vector256<int>.Count; i += Vector256<int>.Count)
                    {
                        // Load current vector
                        Vector256<int> currentVector = Vector256.LoadUnsafe(searchspace, (nuint)i);

                        // Load next vector for subtraction
                        Vector256<int> nextVector = Vector256.LoadUnsafe(searchspace, (nuint)(i+1));

                        // Differential encoding
                        Vector256<int> diffVector = Vector256.Subtract(nextVector, currentVector);

                        // Store the differences in the output buffer
                        for (int j = 0; j < Vector256<int>.Count; j++)
                        {
                            int diff = diffVector.GetElement(j);
                            outputBuffer.AddRange(BitConverter.GetBytes(diff));
                        }
                    }

                    fs.Position -= bytesRead != 4 ? 4 : 0;

                    // Handle remaining elements if the number of elements is not a multiple of Vector256<int>.Count
                    int remainingStart = vectorCount - (vectorCount % Vector256<int>.Count) + 1;
                    for (int i = remainingStart; i <= vectorCount - 1; i++)
                    {
                        int diff = numbers[i] - numbers[i - 1];
                        outputBuffer.AddRange(BitConverter.GetBytes(diff));
                    }
                }
            }

            // Write the output buffer to the file
            File.WriteAllBytes(outputFilePath, outputBuffer.ToArray());

            Console.WriteLine("Differential encoding complete.");
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