namespace CompilerApp;

using System;

public class Registers
{
    private readonly int[] _registers = new int[512];

    public int GetRegister(int index)
    {
        if (index < 0 || index >= _registers.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Register index out of range.");
        }
        return _registers[index];
    }

    public void SetRegister(int index, int value)
    {
        if (index < 0 || index >= _registers.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Register index out of range.");
        }
        _registers[index] = value;
    }

    public int[] GetAllRegisters()
    {
        return _registers;
    }

    public void ClearRegisters()
    {
        Array.Clear(_registers, 0, _registers.Length);
    }
}
