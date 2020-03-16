using System;

public class GBReg
{
    public UInt32 address;
    public byte msb;
    public byte lsb;
    public UInt16 count;
    public UInt32 defaultvalue;
    public string accesstype;

    public GBReg(UInt32 address, byte msb, byte lsb, UInt16 count, UInt32 defaultvalue, string accesstype)
    {
        this.address = address;
        this.msb = msb;
        this.lsb = lsb;
        this.count = count;
        this.defaultvalue = defaultvalue;
        this.accesstype = accesstype;
    }

    public void write(UInt32 value)
    {
        UInt32 newvalue = BitConverter.ToUInt32(GBRegs.data, (int)address);
        UInt32 filter = ~((((UInt32)Math.Pow(2, msb + 1)) - 1) - (((UInt32)Math.Pow(2, lsb)) - 1));
        newvalue = newvalue & filter;
        newvalue |= value << lsb;

        GBReg_common.write_reg(newvalue, this);
    }

    public UInt32 read()
    {
        UInt32 value = BitConverter.ToUInt32(GBRegs.data, (int)address);
        UInt32 filter = ((((UInt32)Math.Pow(2, msb + 1)) - 1) - (((UInt32)Math.Pow(2, lsb)) - 1));
        value = (value & filter) >> lsb;

        return value;
    }

    public bool on()
    {
        UInt32 value = BitConverter.ToUInt32(GBRegs.data, (int)address);
        return (((value >> lsb) & 1) == 1);
    }

    public void mask(UInt32 mask)
    {
        UInt32 value = BitConverter.ToUInt32(GBRegs.data, (int)address);
        GBReg_common.write_reg(value & mask, this);
    }

}

public static class GBReg_common
{
    public static void write_reg(UInt32 value, GBReg reg)
    {
        if (reg.msb < 8)
        {
            GBRegs.data[reg.address] = (byte)value;
        }
        else if (reg.msb < 16)
        {
            GBRegs.data[reg.address] = (byte)(value & 0xFF);
            GBRegs.data[reg.address + 1] = (byte)((value >> 8) & 0xFF);
        }
        else
        {
            GBRegs.data[reg.address] = (byte)(value & 0xFF);
            GBRegs.data[reg.address + 1] = (byte)((value >> 8) & 0xFF);
            GBRegs.data[reg.address + 2] = (byte)((value >> 16) & 0xFF);
            GBRegs.data[reg.address + 3] = (byte)((value >> 24) & 0xFF);
        }
    }

}
