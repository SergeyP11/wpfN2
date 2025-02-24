using System;
using System.IO;
using System.Text;
using System.Windows;
using CompilerApp; 

public class Interpreter
{
    private readonly Registers _registers;
    private StringBuilder _output = new StringBuilder();

    public Interpreter(Registers registers)
    {
        _registers = registers ?? throw new ArgumentNullException(nameof(registers));
    }

    public string Execute(MemoryStream codeStream)
    {
        _output.Clear();
        using (BinaryReader reader = new BinaryReader(codeStream))
        {
            while (codeStream.Position < codeStream.Length)
            {
                int instruction = reader.ReadInt32();
                ExecuteInstruction(instruction);
            }
        }
        return _output.ToString();
    }

    private void ExecuteInstruction(int instruction)
    {
        int opcode = instruction & 0x1F; // Младшие 5 бит
        int operand1 = (instruction >> 5) & 0x1FF; // Следующие 9 бит
        int operand2 = (instruction >> 14) & 0x1FF; // Следующие 9 бит
        int operand3 = (instruction >> 23) & 0x1FF; // Следующие 9 бит

        try
        {
            switch ((Operation)opcode)
            {
                case Operation.PrintRegisters:
                    PrintRegisters(operand1);
                    break;
                case Operation.BitwiseInversion:
                    BitwiseInversion(operand1, operand3);
                    break;
                case Operation.Disjunction:
                    Disjunction(operand1, operand2, operand3);
                    break;
                case Operation.Conjunction:
                    Conjunction(operand1, operand2, operand3);
                    break;
                case Operation.Xor:
                    Xor(operand1, operand2, operand3);
                    break;
                case Operation.Implication:
                    Implication(operand1, operand2, operand3);
                    break;
                case Operation.Coimplication:
                    Coimplication(operand1, operand2, operand3);
                    break;
                case Operation.Equivalence:
                    Equivalence(operand1, operand2, operand3);
                    break;
                case Operation.PierceArrow:
                    PierceArrow(operand1, operand2, operand3);
                    break;
                case Operation.ShefferStroke:
                    ShefferStroke(operand1, operand2, operand3);
                    break;
                case Operation.Addition:
                    Addition(operand1, operand2, operand3);
                    break;
                case Operation.Subtraction:
                    Subtraction(operand1, operand2, operand3);
                    break;
                case Operation.Multiplication:
                    Multiplication(operand1, operand2, operand3);
                    break;
                case Operation.Division:
                    Division(operand1, operand2, operand3);
                    break;
                case Operation.Modulo:
                    Modulo(operand1, operand2, operand3);
                    break;
                case Operation.Swap:
                    Swap(operand1, operand2);
                    break;
                case Operation.SetByte:
                    SetByte(operand1, operand2, operand3);
                    break;
                case Operation.PrintOperand:
                    PrintOperand(operand1, operand2);
                    break;
                case Operation.InputOperand:
                    InputOperand(operand1, operand2);
                    break;
                case Operation.MaxPowerOfTwo:
                    MaxPowerOfTwo(operand1, operand3);
                    break;
                case Operation.ShiftLeft:
                    ShiftLeft(operand1, operand2, operand3);
                    break;
                case Operation.ShiftRight:
                    ShiftRight(operand1, operand2, operand3);
                    break;
                case Operation.RotateLeft:
                    RotateLeft(operand1, operand2, operand3);
                    break;
                case Operation.RotateRight:
                    RotateRight(operand1, operand2, operand3);
                    break;
                case Operation.Copy:
                    Copy(operand1, operand2);
                    break;
                default:
                    _output.AppendLine($"Error: Unknown opcode {opcode}");
                    break;
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _output.AppendLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            _output.AppendLine($"Error during instruction execution: {ex.Message}");
        }
    }

    private void PrintRegisters(int baseValue)
    {
        if (baseValue < 2 || baseValue > 36)
        {
            _output.AppendLine("Error during instruction execution: Invalid Base.");
            return;
        }

        var registers = _registers.GetAllRegisters();
        _output.AppendLine("Register Values:");
        for (int i = 0; i < registers.Length; i++)
        {
            try
            {
                _output.AppendLine($"R{i}: {Convert.ToString(registers[i], baseValue)}");
            }
            catch (ArgumentException)
            {
                _output.AppendLine($"R{i}: Error during instruction execution: Invalid Base.");
            }
        }
    }

    private void BitwiseInversion(int operand1, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        _registers.SetRegister(operand3, ~value);
    }

    private void Disjunction(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 | value2);
    }

    private void Conjunction(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 & value2);
    }

    private void Xor(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 ^ value2);
    }

    private void Implication(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, (~value1) | value2);
    }

    private void Coimplication(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 | (~value2));
    }

    private void Equivalence(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, (~value1 & ~value2) | (value1 & value2));
    }

    private void PierceArrow(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, ~(value1 | value2));
    }

    private void ShefferStroke(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, ~(value1 & value2));
    }

    private void Addition(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 + value2);
    }

    private void Subtraction(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 - value2);
    }

    private void Multiplication(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value1 * value2);
    }

    private void Division(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        if (value2 == 0)
        {
            _output.AppendLine("Error: Division by zero.");
            return;
        }
        _registers.SetRegister(operand3, value1 / value2);
    }

    private void Modulo(int operand1, int operand2, int operand3)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        if (value2 == 0)
        {
            _output.AppendLine("Error: Division by zero.");
            return;
        }
        _registers.SetRegister(operand3, value1 % value2);
    }

    private void Swap(int operand1, int operand2)
    {
        int value1 = _registers.GetRegister(operand1);
        int value2 = _registers.GetRegister(operand2);
        _registers.SetRegister(operand1, value2);
        _registers.SetRegister(operand2, value1);
    }

    private void SetByte(int operand1, int operand2, int operand3)
    {
        int registerValue = _registers.GetRegister(operand1);
        int byteIndex = _registers.GetRegister(operand2);
        int byteValue = _registers.GetRegister(operand3);

        if (byteIndex < 0 || byteIndex > 3)
        {
            _output.AppendLine("Error: Byte index out of range (0-3).");
            return;
        }

        if (byteValue < 0 || byteValue > 255)
        {
            _output.AppendLine("Error: Byte value out of range (0-255).");
            return;
        }

        byte[] bytes = BitConverter.GetBytes(registerValue);
        bytes[byteIndex] = (byte)byteValue;
        _registers.SetRegister(operand1, BitConverter.ToInt32(bytes, 0));
    }

    private void PrintOperand(int operand1, int baseValue)
    {
        int value = _registers.GetRegister(operand1);
        _output.AppendLine($"R{operand1}: {Convert.ToString(value, baseValue)}");
    }

    private void InputOperand(int operand1, int baseValue)
    {
        string answer = null;
        Application.Current.Dispatcher.Invoke(() =>
        {
            InputBox inputBox = new InputBox($"Enter value for R{operand1} in base {baseValue}:", "Input");
            if (inputBox.ShowDialog() == true)
            {
                answer = inputBox.Answer;
            }
            else
            {
                _output.AppendLine("Input cancelled.");
            }
        });

        
        if (answer != null)
        {
            if (int.TryParse(answer, System.Globalization.NumberStyles.Integer, null, out int value))
            {
                try
                {
                    _registers.SetRegister(operand1, Convert.ToInt32(answer, baseValue));
                }
                catch (Exception ex)
                {
                    _output.AppendLine($"Error: Invalid input - {ex.Message}");
                }
            }
            else
            {
                _output.AppendLine("Error: Invalid input.");
            }
        }
    }

    private void MaxPowerOfTwo(int operand1, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        if (value <= 0)
        {
            _registers.SetRegister(operand3, 0);
            return;
        }

        int power = 1;
        while ((power * 2) <= value)
        {
            power *= 2;
        }
        _registers.SetRegister(operand3, power);
    }

    private void ShiftLeft(int operand1, int operand2, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        int shift = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value << shift);
    }

    private void ShiftRight(int operand1, int operand2, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        int shift = _registers.GetRegister(operand2);
        _registers.SetRegister(operand3, value >> shift);
    }

    private void RotateLeft(int operand1, int operand2, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        int shift = _registers.GetRegister(operand2) % 32; 
        _registers.SetRegister(operand3, (value << shift) | (value >>> (32 - shift)));
    }

    private void RotateRight(int operand1, int operand2, int operand3)
    {
        int value = _registers.GetRegister(operand1);
        int shift = _registers.GetRegister(operand2) % 32; //shift is within 0-31
        _registers.SetRegister(operand3, (value >>> shift) | (value << (32 - shift)));
    }

    private void Copy(int operand1, int operand2)
    {
        int value = _registers.GetRegister(operand2);
        _registers.SetRegister(operand1, value);
    }
}