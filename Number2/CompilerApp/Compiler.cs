// Compiler.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Compiler
{
    public MemoryStream Compile(string code)
    {
        MemoryStream stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream); // Remove using block
        {
            string[] lines = code.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("//"))
                {
                    continue; // Skip empty lines and comments
                }

                try
                {
                    int instruction = ParseInstruction(trimmedLine);
                    writer.Write(instruction);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error compiling line: {line}. {ex.Message}");
                }
            }
        }
        writer.Flush();
        stream.Position = 0;
        return stream;
    }

    private int ParseInstruction(string line)
    {
        string[] parts = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            throw new ArgumentException("Empty instruction");
        }

        if (!Enum.TryParse(parts[0], out Operation opcode))
        {
            throw new ArgumentException($"Invalid opcode: {parts[0]}");
        }

        int operand1 = 0, operand2 = 0, operand3 = 0;

        switch (opcode)
        {
            case Operation.PrintRegisters:
                if (parts.Length > 1)
                {
                    operand1 = int.Parse(parts[1]); // Base for PrintRegisters
                }
                break;
            //case Operation.PrintRegisters:
            case Operation.PrintOperand:
            case Operation.InputOperand:
                if (parts.Length > 1)
                {
                    if(opcode != Operation.PrintRegisters)
                    {
                        operand1 = ParseOperand(parts[1]);
                    }

                    if (parts.Length > 2)
                    {
                        operand2 = int.Parse(parts[2]); // Base for PrintRegisters, PrintOperand, InputOperand
                    }
                }

                break;
            case Operation.BitwiseInversion:
            case Operation.MaxPowerOfTwo:
                if (parts.Length > 1) operand1 = ParseOperand(parts[1]);
                if (parts.Length > 2) operand3 = ParseOperand(parts[2]);
                break;
            case Operation.Swap:
            case Operation.Copy:
                if (parts.Length > 1) operand1 = ParseOperand(parts[1]);
                if (parts.Length > 2) operand2 = ParseOperand(parts[2]);
                break;
            case Operation.SetByte:
            case Operation.ShiftLeft:
            case Operation.ShiftRight:
            case Operation.RotateLeft:
            case Operation.RotateRight:
            case Operation.Disjunction:
            case Operation.Conjunction:
            case Operation.Xor:
            case Operation.Implication:
            case Operation.Coimplication:
            case Operation.Equivalence:
            case Operation.PierceArrow:
            case Operation.ShefferStroke:
            case Operation.Addition:
            case Operation.Subtraction:
            case Operation.Multiplication:
            case Operation.Division:
            case Operation.Modulo:
                if (parts.Length > 1) operand1 = ParseOperand(parts[1]);
                if (parts.Length > 2) operand2 = ParseOperand(parts[2]);
                if (parts.Length > 3) operand3 = ParseOperand(parts[3]);
                break;

        }

        return EncodeInstruction((int)opcode, operand1, operand2, operand3);
    }


    private int ParseOperand(string operand)
    {
        if (operand.StartsWith("R", StringComparison.OrdinalIgnoreCase))
        {
            if (int.TryParse(operand.Substring(1), out int registerIndex))
            {
                if (registerIndex >= 0 && registerIndex < 512)
                {
                    return registerIndex;
                }
                else
                {
                    throw new ArgumentException($"Register index {registerIndex} out of range (0-511)");
                }
            }
            else
            {
                throw new ArgumentException($"Invalid register index: {operand}");
            }
        }
        else
        {
            throw new ArgumentException($"Invalid operand: {operand}");
        }
    }


    private int EncodeInstruction(int opcode, int operand1, int operand2, int operand3)
    {
        if (opcode < 0 || opcode > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(opcode), "Opcode out of range (0-31).");
        }

        if (operand1 < 0 || operand1 > 511)
        {
            throw new ArgumentOutOfRangeException(nameof(operand1), "Operand1 out of range (0-511).");
        }

        if (operand2 < 0 || operand2 > 511)
        {
            throw new ArgumentOutOfRangeException(nameof(operand2), "Operand2 out of range (0-511).");
        }

        if (operand3 < 0 || operand3 > 511)
        {
            throw new ArgumentOutOfRangeException(nameof(operand3), "Operand3 out of range (0-511).");
        }

        return opcode | (operand1 << 5) | (operand2 << 14) | (operand3 << 23);
    }
}