using HCSRunTimeLibrary;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Utilities.Encoders;
using System; 
using System.Collections.Generic;
using System.ComponentModel; 
using System.Data.Common;
using System.Globalization; 
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks; 

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    internal class Strings
    {

        public static string Mid(string str, int Start, int Length)
        {
            string str1;
            if (Start <= 0)
                throw new ArgumentException("Start");

            if (Length < 0)
                throw new ArgumentException("Length");

            if (Length == 0 || str == null)
            {
                str1 = "";
            } else
            {
                int length = str.Length;
                if (Start <= length)
                {
                    str1 = Start + Length <= length ? str.Substring(Start - 1, Length) : str.Substring(Start - 1);
                } else
                {
                    str1 = "";
                }
            }
            return str1;
        }


        public static int Len(bool Expression)
        {
            return 2;
        }

        public static int Len(sbyte Expression)
        {
            return 1;
        }

        public static int Len(byte Expression)
        {
            return 1;
        }

        public static int Len(short Expression)
        {
            return 2;
        }

        public static int Len(ushort Expression)
        {
            return 2;
        }

        public static int Len(int Expression)
        {
            return 4;
        }

        public static int Len(uint Expression)
        {
            return 4;
        }

        public static int Len(long Expression)
        {
            return 8;
        }

        public static int Len(ulong Expression)
        {
            return 8;
        }

        public static int Len(decimal Expression)
        {
            return 8;
        }

        public static int Len(float Expression)
        {
            return 4;
        }

        public static int Len(double Expression)
        {
            return 8;
        }

        public static int Len(DateTime Expression)
        {
            return 8;
        }

        public static int Len(char Expression)
        {
            return 2;
        }

        public static int Len(string Expression)
        {
            return (Expression != null ? Expression.Length : 0);
        }


    }

    internal class Operators
    {
        private enum CompareClass
        {
            Less = -1,
            Equal = 0,
            Greater = 1,
            Unordered = 2,
            UserDefined = 3,
            Undefined = 4
        }

        private static Operators.CompareClass CompareSingle(float Left, float Right)
        {
            Operators.CompareClass compareClass;
            if (Left == Right)
            {
                compareClass = Operators.CompareClass.Equal;
            } else if (Left >= Right)
            {
                compareClass = (Left <= Right ? Operators.CompareClass.Unordered : Operators.CompareClass.Greater);
            } else
            {
                compareClass = Operators.CompareClass.Less;
            }
            return compareClass;
        }

        public static int CompareString(string Left, string Right, bool TextCompare)
        {
            int num;
            int num1;
            if ((object)Left == (object)Right)
            {
                num = 0;
            } else if (Left == null)
            {
                num = (Right.Length != 0 ? -1 : 0);
            } else if (Right != null)
            {
                num1 = (!TextCompare ? string.CompareOrdinal(Left, Right) : Utils.GetCultureInfo().CompareInfo.Compare(Left, Right, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth));
                if (num1 != 0)
                {
                    num = (num1 <= 0 ? -1 : 1);
                } else
                {
                    num = 0;
                }
            } else
            {
                num = (Left.Length != 0 ? 1 : 0);
            }
            return num;
        }

        public static object ConcatenateObject(object Left, object Right)
        {
            object left = null;
            TypeCode typeCode;
            TypeCode typeCode1;
            IConvertible convertible = Left as IConvertible;
            if (convertible != null)
            {
                typeCode = convertible.GetTypeCode();
            } else
            {
                typeCode = (Left != null ? TypeCode.Object : TypeCode.Empty);
            }
            IConvertible right = Right as IConvertible;
            if (right != null)
            {
                typeCode1 = right.GetTypeCode();
            } else
            {
                typeCode1 = (Right != null ? TypeCode.Object : TypeCode.Empty);
            }
            if (typeCode == TypeCode.Object && Left is char[])
            {
                typeCode = TypeCode.String;
            }
            if (typeCode1 == TypeCode.Object && Right is char[])
            {
                typeCode1 = TypeCode.String;
            } else
            {
                bool flag = typeCode == TypeCode.DBNull;
                bool flag1 = typeCode1 == TypeCode.DBNull;
                if (!(flag & flag1))
                {
                    if (flag & !flag1)
                    {
                        Left = "";
                    } else if (flag1 & !flag)
                    {
                        Right = "";
                    }
                    left = string.Concat(Conversions.ToString(Left), Conversions.ToString(Right));
                } else
                {
                    left = Left;
                }
            }
            return left;
        }

    }

    internal class Conversions
    {
        public static string ToString(object Value)
        {
            string str;
            if (Value != null)
            {
                string value = Value as string;
                if (value == null)
                {
                    IConvertible convertible = Value as IConvertible;
                    if (convertible == null)
                    {
                        char[] chrArray = Value as char[];
                        if (chrArray == null)
                        {
                            throw new InvalidCastException();
                        }
                        str = new string(chrArray);
                        return str;
                    } else
                    {
                        switch (convertible.GetTypeCode())
                        {
                            case TypeCode.Boolean:
                                {
                                    str = Conversions.ToString(convertible.ToBoolean(null));
                                    return str;
                                }
                            case TypeCode.Char:
                                {
                                    str = Conversions.ToString(convertible.ToChar(null));
                                    return str;
                                }
                            case TypeCode.SByte:
                                {
                                    str = Conversions.ToString(convertible.ToSByte(null));
                                    return str;
                                }
                            case TypeCode.Byte:
                                {
                                    str = Conversions.ToString(convertible.ToByte(null));
                                    return str;
                                }
                            case TypeCode.Int16:
                                {
                                    str = Conversions.ToString((int)convertible.ToInt16(null));
                                    return str;
                                }
                            case TypeCode.UInt16:
                                {
                                    str = Conversions.ToString(convertible.ToUInt16(null));
                                    return str;
                                }
                            case TypeCode.Int32:
                                {
                                    str = Conversions.ToString(convertible.ToInt32(null));
                                    return str;
                                }
                            case TypeCode.UInt32:
                                {
                                    str = Conversions.ToString(convertible.ToUInt32(null));
                                    return str;
                                }
                            case TypeCode.Int64:
                                {
                                    str = Conversions.ToString(convertible.ToInt64(null));
                                    return str;
                                }
                            case TypeCode.UInt64:
                                {
                                    str = Conversions.ToString(convertible.ToUInt64(null));
                                    return str;
                                }
                            case TypeCode.Single:
                                {
                                    str = Conversions.ToString(convertible.ToSingle(null));
                                    return str;
                                }
                            case TypeCode.Double:
                                {
                                    str = Conversions.ToString(convertible.ToDouble(null));
                                    return str;
                                }
                            case TypeCode.Decimal:
                                {
                                    str = Conversions.ToString(convertible.ToDecimal(null));
                                    return str;
                                }
                            case TypeCode.DateTime:
                                {
                                    str = Conversions.ToString(convertible.ToDateTime(null));
                                    return str;
                                }
                            case TypeCode.String:
                                {
                                    str = convertible.ToString(null);
                                    return str;
                                }
                        }
                    }
                    throw new InvalidCastException();
                } else
                {
                    str = value;
                }
            } else
            {
                str = null;
            }
            return str;
        }

        public static bool ToBoolean(object Value)
        {
            bool flag;
            if (Value != null)
            {
                IConvertible value = Value as IConvertible;
                if (value != null)
                {
                    switch (value.GetTypeCode())
                    {
                        case TypeCode.Boolean:
                            {
                                if (!(Value is bool))
                                {
                                    flag = value.ToBoolean(null);
                                    return flag;
                                } else
                                {
                                    flag = (bool)Value;
                                    return flag;
                                }
                            }
                        case TypeCode.SByte:
                            {
                                if (!(Value is sbyte))
                                {
                                    flag = value.ToSByte(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (sbyte)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.Byte:
                            {
                                if (!(Value is byte))
                                {
                                    flag = value.ToByte(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (byte)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.Int16:
                            {
                                if (!(Value is short))
                                {
                                    flag = value.ToInt16(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (short)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.UInt16:
                            {
                                if (!(Value is ushort))
                                {
                                    flag = value.ToUInt16(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (ushort)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.Int32:
                            {
                                if (!(Value is int))
                                {
                                    flag = value.ToInt32(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (int)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.UInt32:
                            {
                                if (!(Value is uint))
                                {
                                    flag = value.ToUInt32(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (uint)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.Int64:
                            {
                                if (!(Value is long))
                                {
                                    flag = value.ToInt64(null) != (long)0;
                                    return flag;
                                } else
                                {
                                    flag = (long)Value != (long)0;
                                    return flag;
                                }
                            }
                        case TypeCode.UInt64:
                            {
                                if (!(Value is ulong))
                                {
                                    flag = value.ToUInt64(null) != (long)0;
                                    return flag;
                                } else
                                {
                                    flag = (ulong)Value != (long)0;
                                    return flag;
                                }
                            }
                        case TypeCode.Single:
                            {
                                if (!(Value is float))
                                {
                                    flag = value.ToSingle(null) != 0f;
                                    return flag;
                                } else
                                {
                                    flag = (float)Value != 0f;
                                    return flag;
                                }
                            }
                        case TypeCode.Double:
                            {
                                if (!(Value is double))
                                {
                                    flag = value.ToDouble(null) != 0;
                                    return flag;
                                } else
                                {
                                    flag = (double)Value != 0;
                                    return flag;
                                }
                            }
                        case TypeCode.Decimal:
                            {
                                if (!(Value is decimal))
                                {
                                    flag = Convert.ToBoolean(value.ToDecimal(null));
                                    return flag;
                                } else
                                {
                                    flag = value.ToBoolean(null);
                                    return flag;
                                }
                            }
                        case TypeCode.String:
                            {
                                string str = Value as string;
                                if (str == null)
                                {
                                    flag = Conversions.ToBoolean(value.ToString(null));
                                    return flag;
                                } else
                                {
                                    flag = Conversions.ToBoolean(str);
                                    return flag;
                                }
                            }
                    }
                }
                throw new InvalidCastException();
            } else
            {
                flag = false;
            }
            return flag;
        }
    }

    internal class Conversion
    {
        public static string Hex(byte Number)
        {
            return Number.ToString("X");
        }
    }

    internal class Interaction
    {
        public static object IIf(bool Expression, object TruePart, object FalsePart)
        {
            return Expression ? TruePart : FalsePart;
        }

        public static object Switch(params object[] VarExpr)
        {
            object varExpr;
            if (VarExpr != null)
            {
                int length = (int)VarExpr.Length;
                int num = 0;
                if (length % 2 != 0)
                {
                    throw new ArgumentException("VarExpr");
                }
                while (length > 0)
                {
                    if (!Conversions.ToBoolean(VarExpr[num]))
                    {
                        num = checked(num + 2);
                        length = checked(length - 2);
                    } else
                    {
                        varExpr = VarExpr[checked(num + 1)];
                        return varExpr;
                    }
                }
                varExpr = null;
            } else
            {
                varExpr = null;
            }
            return varExpr;
        }
    }

    internal class Information
    {
        public static int UBound(System.Array Array, int Rank = 1)
        {
            if (Array == null)
                throw new ArgumentNullException("Array");
            if (Rank < 1 || Rank > Array.Rank)
                throw new ArgumentException("无效值", "Rank");

            return Array.GetUpperBound(checked(Rank - 1));
        }
    }

    internal class Utils
    {
        public static Array CopyArray(Array arySrc, Array aryDest)
        {
            Array arrays;
            if (arySrc != null)
            {
                int length = arySrc.Length;
                if (length != 0)
                {
                    if (length > aryDest.Length)
                    {
                        length = aryDest.Length;
                    }
                    if (arySrc.Rank <= 1)
                    {
                        Array.Copy(arySrc, aryDest, length);
                    } else
                    {
                        int num = arySrc.Rank;
                        int length1 = arySrc.GetLength(checked(num - 1));
                        int num1 = aryDest.GetLength(checked(num - 1));
                        if (num1 == 0)
                        {
                            arrays = aryDest;
                            return arrays;
                        }
                        int num2 = Math.Min(length1, num1);
                        int length2 = checked(arySrc.Length / length1 - 1);
                        for (int j = 0; j <= length2; j = checked(j + 1))
                        {
                            Array.Copy(arySrc, checked(j * length1), aryDest, checked(j * num1), num2);
                        }
                    }
                    arrays = aryDest;
                } else
                {
                    arrays = aryDest;
                }
            } else
            {
                arrays = aryDest;
            }
            return arrays;
        }

        public static System.Globalization.CultureInfo GetCultureInfo()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture;
        }

        public static Encoding GetFileIOEncoding()
        {
            return Encoding.Default;
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    public class Sm4
    {
        public const int SM4_ENCRYPT = 1;
        public const int SM4_DECRYPT = 0;

        private long GET_ULONG_BE(byte[] b, int i)
        {
            long n = (long)(b[i] & 0xff) << 24 | (long)((b[i + 1] & 0xff) << 16) | (long)((b[i + 2] & 0xff) << 8) | (long)(b[i + 3] & 0xff) & 0xffffffffL;
            return n;
        }
        private void PUT_ULONG_BE(long n, byte[] b, int i)
        {
            b[i] = (byte)(int)(0xFF & n >> 24);
            b[i + 1] = (byte)(int)(0xFF & n >> 16);
            b[i + 2] = (byte)(int)(0xFF & n >> 8);
            b[i + 3] = (byte)(int)(0xFF & n);
        }
        private long SHL(long x, int n)
        {
            return (x & 0xFFFFFFFF) << n;
        }

        private long ROTL(long x, int n)
        {
            return SHL(x, n) | x >> (32 - n);
        }

        private void SWAP(long[] sk, int i)
        {
            long t = sk[i];
            sk[i] = sk[(31 - i)];
            sk[(31 - i)] = t;
        }

        public byte[] SboxTable = new byte[] { (byte) 0xd6, (byte) 0x90, (byte) 0xe9, (byte) 0xfe,
            (byte) 0xcc, (byte) 0xe1, 0x3d, (byte) 0xb7, 0x16, (byte) 0xb6,
            0x14, (byte) 0xc2, 0x28, (byte) 0xfb, 0x2c, 0x05, 0x2b, 0x67,
            (byte) 0x9a, 0x76, 0x2a, (byte) 0xbe, 0x04, (byte) 0xc3,
            (byte) 0xaa, 0x44, 0x13, 0x26, 0x49, (byte) 0x86, 0x06,
            (byte) 0x99, (byte) 0x9c, 0x42, 0x50, (byte) 0xf4, (byte) 0x91,
            (byte) 0xef, (byte) 0x98, 0x7a, 0x33, 0x54, 0x0b, 0x43,
            (byte) 0xed, (byte) 0xcf, (byte) 0xac, 0x62, (byte) 0xe4,
            (byte) 0xb3, 0x1c, (byte) 0xa9, (byte) 0xc9, 0x08, (byte) 0xe8,
            (byte) 0x95, (byte) 0x80, (byte) 0xdf, (byte) 0x94, (byte) 0xfa,
            0x75, (byte) 0x8f, 0x3f, (byte) 0xa6, 0x47, 0x07, (byte) 0xa7,
            (byte) 0xfc, (byte) 0xf3, 0x73, 0x17, (byte) 0xba, (byte) 0x83,
            0x59, 0x3c, 0x19, (byte) 0xe6, (byte) 0x85, 0x4f, (byte) 0xa8,
            0x68, 0x6b, (byte) 0x81, (byte) 0xb2, 0x71, 0x64, (byte) 0xda,
            (byte) 0x8b, (byte) 0xf8, (byte) 0xeb, 0x0f, 0x4b, 0x70, 0x56,
            (byte) 0x9d, 0x35, 0x1e, 0x24, 0x0e, 0x5e, 0x63, 0x58, (byte) 0xd1,
            (byte) 0xa2, 0x25, 0x22, 0x7c, 0x3b, 0x01, 0x21, 0x78, (byte) 0x87,
            (byte) 0xd4, 0x00, 0x46, 0x57, (byte) 0x9f, (byte) 0xd3, 0x27,
            0x52, 0x4c, 0x36, 0x02, (byte) 0xe7, (byte) 0xa0, (byte) 0xc4,
            (byte) 0xc8, (byte) 0x9e, (byte) 0xea, (byte) 0xbf, (byte) 0x8a,
            (byte) 0xd2, 0x40, (byte) 0xc7, 0x38, (byte) 0xb5, (byte) 0xa3,
            (byte) 0xf7, (byte) 0xf2, (byte) 0xce, (byte) 0xf9, 0x61, 0x15,
            (byte) 0xa1, (byte) 0xe0, (byte) 0xae, 0x5d, (byte) 0xa4,
            (byte) 0x9b, 0x34, 0x1a, 0x55, (byte) 0xad, (byte) 0x93, 0x32,
            0x30, (byte) 0xf5, (byte) 0x8c, (byte) 0xb1, (byte) 0xe3, 0x1d,
            (byte) 0xf6, (byte) 0xe2, 0x2e, (byte) 0x82, 0x66, (byte) 0xca,
            0x60, (byte) 0xc0, 0x29, 0x23, (byte) 0xab, 0x0d, 0x53, 0x4e, 0x6f,
            (byte) 0xd5, (byte) 0xdb, 0x37, 0x45, (byte) 0xde, (byte) 0xfd,
            (byte) 0x8e, 0x2f, 0x03, (byte) 0xff, 0x6a, 0x72, 0x6d, 0x6c, 0x5b,
            0x51, (byte) 0x8d, 0x1b, (byte) 0xaf, (byte) 0x92, (byte) 0xbb,
            (byte) 0xdd, (byte) 0xbc, 0x7f, 0x11, (byte) 0xd9, 0x5c, 0x41,
            0x1f, 0x10, 0x5a, (byte) 0xd8, 0x0a, (byte) 0xc1, 0x31,
            (byte) 0x88, (byte) 0xa5, (byte) 0xcd, 0x7b, (byte) 0xbd, 0x2d,
            0x74, (byte) 0xd0, 0x12, (byte) 0xb8, (byte) 0xe5, (byte) 0xb4,
            (byte) 0xb0, (byte) 0x89, 0x69, (byte) 0x97, 0x4a, 0x0c,
            (byte) 0x96, 0x77, 0x7e, 0x65, (byte) 0xb9, (byte) 0xf1, 0x09,
            (byte) 0xc5, 0x6e, (byte) 0xc6, (byte) 0x84, 0x18, (byte) 0xf0,
            0x7d, (byte) 0xec, 0x3a, (byte) 0xdc, 0x4d, 0x20, 0x79,
            (byte) 0xee, 0x5f, 0x3e, (byte) 0xd7, (byte) 0xcb, 0x39, 0x48 };
        public uint[] FK = { 0xa3b1bac6, 0x56aa3350, 0x677d9197, 0xb27022dc };
        public uint[] CK = { 0x00070e15,0x1c232a31,0x383f464d,0x545b6269,
                                        0x70777e85,0x8c939aa1,0xa8afb6bd,0xc4cbd2d9,
                                        0xe0e7eef5,0xfc030a11,0x181f262d,0x343b4249,
                                        0x50575e65,0x6c737a81,0x888f969d,0xa4abb2b9,
                                        0xc0c7ced5,0xdce3eaf1,0xf8ff060d,0x141b2229,
                                        0x30373e45,0x4c535a61,0x686f767d,0x848b9299,
                                        0xa0a7aeb5,0xbcc3cad1,0xd8dfe6ed,0xf4fb0209,
                                        0x10171e25,0x2c333a41,0x484f565d,0x646b7279 };
        private byte sm4Sbox(byte inch)
        {
            int i = inch & 0xFF;
            byte retVal = SboxTable[i];
            return retVal;
        }
        private long sm4Lt(long ka)
        {
            long bb = 0L;
            long c = 0L;
            byte[] a = new byte[4];
            byte[] b = new byte[4];
            PUT_ULONG_BE(ka, a, 0);
            b[0] = sm4Sbox(a[0]);
            b[1] = sm4Sbox(a[1]);
            b[2] = sm4Sbox(a[2]);
            b[3] = sm4Sbox(a[3]);
            bb = GET_ULONG_BE(b, 0);
            c = bb ^ ROTL(bb, 2) ^ ROTL(bb, 10) ^ ROTL(bb, 18) ^ ROTL(bb, 24);
            return c;
        }
        private long sm4F(long x0, long x1, long x2, long x3, long rk)
        {
            return x0 ^ sm4Lt(x1 ^ x2 ^ x3 ^ rk);
        }

        private long sm4CalciRK(long ka)
        {
            long bb = 0L;
            long rk = 0L;
            byte[] a = new byte[4];
            byte[] b = new byte[4];
            PUT_ULONG_BE(ka, a, 0);
            b[0] = sm4Sbox(a[0]);
            b[1] = sm4Sbox(a[1]);
            b[2] = sm4Sbox(a[2]);
            b[3] = sm4Sbox(a[3]);
            bb = GET_ULONG_BE(b, 0);
            rk = bb ^ ROTL(bb, 13) ^ ROTL(bb, 23);
            return rk;
        }

        private void sm4_setkey(long[] SK, byte[] key)
        {
            long[] MK = new long[4];
            long[] k = new long[36];
            int i = 0;
            MK[0] = GET_ULONG_BE(key, 0);
            MK[1] = GET_ULONG_BE(key, 4);
            MK[2] = GET_ULONG_BE(key, 8);
            MK[3] = GET_ULONG_BE(key, 12);
            k[0] = MK[0] ^ (long)FK[0];
            k[1] = MK[1] ^ (long)FK[1];
            k[2] = MK[2] ^ (long)FK[2];
            k[3] = MK[3] ^ (long)FK[3];
            for (; i < 32; i++)
            {
                k[(i + 4)] = (k[i] ^ sm4CalciRK(k[(i + 1)] ^ k[(i + 2)] ^ k[(i + 3)] ^ (long)CK[i]));
                SK[i] = k[(i + 4)];
            }
        }

        private void sm4_one_round(long[] sk, byte[] input, byte[] output)
        {
            int i = 0;
            long[] ulbuf = new long[36];
            ulbuf[0] = GET_ULONG_BE(input, 0);
            ulbuf[1] = GET_ULONG_BE(input, 4);
            ulbuf[2] = GET_ULONG_BE(input, 8);
            ulbuf[3] = GET_ULONG_BE(input, 12);
            while (i < 32)
            {
                ulbuf[(i + 4)] = sm4F(ulbuf[i], ulbuf[(i + 1)], ulbuf[(i + 2)], ulbuf[(i + 3)], sk[i]);
                i++;
            }
            PUT_ULONG_BE(ulbuf[35], output, 0);
            PUT_ULONG_BE(ulbuf[34], output, 4);
            PUT_ULONG_BE(ulbuf[33], output, 8);
            PUT_ULONG_BE(ulbuf[32], output, 12);
        }

        private byte[] padding(byte[] input, int mode)
        {
            if (input == null)
            {
                return null;
            }

            byte[] ret = (byte[])null;
            if (mode == SM4_ENCRYPT)
            {
                if (input.Length % 16 != 0)
                {
                    int p = 16 - input.Length % 16;
                    ret = new byte[input.Length + p];
                    Array.Copy(input, 0, ret, 0, input.Length);
                    for (int i = 0; i < p; i++)
                    {
                        ret[input.Length + i] = (byte)p;
                    }
                } else
                {
                    return input;
                }
            } else
            {
                int p = input[input.Length - 1];
                ret = new byte[input.Length - p];
                Array.Copy(input, 0, ret, 0, input.Length - p);
            }
            return ret;
        }

        public void sm4_setkey_enc(Sm4Context ctx, byte[] key)
        {
            ctx.mode = SM4_ENCRYPT;
            sm4_setkey(ctx.sk, key);
        }

        public void sm4_setkey_dec(Sm4Context ctx, byte[] key)
        {
            int i = 0;
            ctx.mode = SM4_DECRYPT;
            sm4_setkey(ctx.sk, key);
            for (i = 0; i < 16; i++)
            {
                SWAP(ctx.sk, i);
            }
        }

        public byte[] sm4_crypt_ecb(Sm4Context ctx, byte[] input)
        {
            if ((ctx.isPadding) && (ctx.mode == SM4_ENCRYPT))
            {
                input = padding(input, SM4_ENCRYPT);
            }

            int length = input.Length;
            byte[] bins = new byte[length];
            Array.Copy(input, 0, bins, 0, length);
            byte[] bous = new byte[length];
            for (int i = 0; length > 0; length -= 16, i++)
            {
                byte[] inBytes = new byte[16];
                byte[] outBytes = new byte[16];
                Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
                sm4_one_round(ctx.sk, inBytes, outBytes);
                Array.Copy(outBytes, 0, bous, i * 16, length > 16 ? 16 : length);
            }

            if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
            {
                bous = padding(bous, SM4_DECRYPT);
            }
            return bous;
        }







        public byte[] sm4_crypt_cbc(Sm4Context ctx, byte[] iv, byte[] input)
        {
            if (ctx.isPadding && ctx.mode == SM4_ENCRYPT)
            {
                input = padding(input, SM4_ENCRYPT);
            }

            int i = 0;
            int length = input.Length;
            byte[] bins = new byte[length];
            Array.Copy(input, 0, bins, 0, length);
            byte[] bous = null;
            List<byte> bousList = new List<byte>();
            if (ctx.mode == SM4_ENCRYPT)
            {
                for (int j = 0; length > 0; length -= 16, j++)
                {
                    byte[] inBytes = new byte[16];
                    byte[] outBytes = new byte[16];
                    byte[] out1 = new byte[16];
                    Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
                    for (i = 0; i < 16; i++)
                    {
                        outBytes[i] = ((byte)(inBytes[i] ^ iv[i]));
                    }
                    sm4_one_round(ctx.sk, outBytes, out1);
                    Array.Copy(out1, 0, iv, 0, 16);
                    for (int k = 0; k < 16; k++)
                    {
                        bousList.Add(out1[k]);
                    }
                }
            } else
            {
                byte[] temp = new byte[16];
                for (int j = 0; length > 0; length -= 16, j++)
                {
                    byte[] inBytes = new byte[16];
                    byte[] outBytes = new byte[16];
                    byte[] out1 = new byte[16];


                    Array.Copy(bins, i * 16, inBytes, 0, length > 16 ? 16 : length);
                    Array.Copy(inBytes, 0, temp, 0, 16);
                    sm4_one_round(ctx.sk, inBytes, outBytes);
                    for (i = 0; i < 16; i++)
                    {
                        out1[i] = ((byte)(outBytes[i] ^ iv[i]));
                    }
                    Array.Copy(temp, 0, iv, 0, 16);
                    for (int k = 0; k < 16; k++)
                    {
                        bousList.Add(out1[k]);
                    }
                }
            }

            if (ctx.isPadding && ctx.mode == SM4_DECRYPT)
            {
                bous = padding(bousList.ToArray(), SM4_DECRYPT);
                return bous;
            } else
            {
                return bousList.ToArray();
            }
        }
    }

    public class Sm4Context
    {
        public int mode;
        public long[] sk;
        public bool isPadding;
        public Sm4Context()
        {
            this.mode = 1;
            this.isPadding = true;
            this.sk = new long[32];
        }
    }

    public class MainSm4
    {
        /// <summary>
        /// 加密ECB模式
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="hexString">明文是否是十六进制</param>
        /// <param name="plainText">明文</param>
        /// <returns>返回密文</returns>
        public String Encrypt_ECB(String secretKey, bool hexString, String plainText)
        {
            Sm4Context ctx = new Sm4Context();
            ctx.isPadding = true;
            ctx.mode = Sm4.SM4_ENCRYPT;
            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            } else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }
            Sm4 sm4 = new Sm4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] bType = Encoding.Default.GetBytes(plainText);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, bType);
            String cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
        }


        /// <summary>
        /// 加密ECB模式
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="hexString">明文是否是十六进制</param>
        /// <param name="plainText">明文</param>
        /// <returns>返回密文</returns>
        public String EncryptZlhisPass(String secretKey, bool hexString, String plainText)
        {
            if (secretKey == "" || plainText == "") return "";

            Sm4Context ctx = new Sm4Context();
            ctx.isPadding = true;
            ctx.mode = Sm4.SM4_ENCRYPT;
            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            } else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }
            Sm4 sm4 = new Sm4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] bType = Encoding.Default.GetBytes(plainText);
            int lngLenBef = bType.Length;


            //判断得到的数组的长度，若不是16的整数倍，则补空格或:Chr(0)
            int lngLenAft = 16;
            if (lngLenBef > 16 && lngLenBef <= 32) lngLenAft = 32;
            if (lngLenBef > 32 && lngLenBef <= 48) lngLenAft = 48;
            if (lngLenBef < lngLenAft)
            {
                int intLen = lngLenAft - lngLenBef;
                string strTmp = "";
                for (int i = 0; i < intLen; i++)
                {
                    strTmp = strTmp + "\0";
                }
                plainText = plainText + strTmp;
                bType = Encoding.Default.GetBytes(plainText);
            }



            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, bType);
            String cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
        }



        /// <summary>
        /// 解密ECB模式
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="hexString">明文是否是十六进制</param>
        /// <param name="cipherText">密文</param>
        /// <returns>返回明文</returns>
        public String Decrypt_ECB(String secretKey, bool hexString, String cipherText)
        {
            Sm4Context ctx = new Sm4Context();
            ctx.isPadding = true;
            ctx.mode = Sm4.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
            } else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }

            Sm4 sm4 = new Sm4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, Hex.Decode(cipherText));
            return Encoding.Default.GetString(decrypted);
        }

        /// <summary>
        /// 加密CBC模式
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="hexString">明文是否是十六进制</param>
        /// <param name="iv"></param>
        /// <param name="plainText">明文</param>
        /// <returns>返回密文</returns>
        public String Encrypt_CBC(String secretKey, bool hexString, string iv, String plainText)
        {
            Sm4Context ctx = new Sm4Context();
            ctx.isPadding = true;
            ctx.mode = Sm4.SM4_ENCRYPT;
            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            } else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }
            Sm4 sm4 = new Sm4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Encoding.Default.GetBytes(plainText));
            String cipherText = Encoding.Default.GetString(Hex.Encode(encrypted));
            return cipherText;
        }

        /// <summary>
        /// 解密CBC模式
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="hexString">明文是否是十六进制</param>
        /// <param name="iv"></param>
        /// <param name="cipherText">密文</param>
        /// <returns>返回明文</returns>
        public String Decrypt_CBC(String secretKey, bool hexString, string iv, String cipherText)
        {
            Sm4Context ctx = new Sm4Context();
            ctx.isPadding = true;
            ctx.mode = Sm4.SM4_DECRYPT;
            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = Hex.Decode(secretKey);
                ivBytes = Hex.Decode(iv);
            } else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }
            Sm4 sm4 = new Sm4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, Hex.Decode(cipherText));
            return Encoding.Default.GetString(decrypted);
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
    internal class SQ
    {
        internal static byte[] A4 = new byte[6732]
    {
        209,
        208,
        136,
        218,
        221,
        192,
        243,
        216,
        203,
        199,
        128,
        156,
        134,
        220,
        148,
        216,
        150,
        155,
        205,
        215,
        223,
        210,
        217,
        157,
        143,
        147,
        203,
        128,
        203,
        155,
        148,
        212,
        233,
        232,
        231,
        252,
        224,
        251,
        172,
        176,
        162,
        248,
        178,
        252,
        170,
        167,
        236,
        236,
        233,
        196,
        237,
        234,
        251,
        237,
        195,
        244,
        246,
        179,
        173,
        177,
        237,
        164,
        233,
        185,
        202,
        131,
        129,
        154,
        177,
        154,
        159,
        136,
        144,
        188,
        131,
        142,
        130,
        130,
        196,
        216,
        218,
        128,
        204,
        132,
        210,
        223,
        148,
        148,
        129,
        172,
        133,
        130,
        147,
        133,
        171,
        140,
        168,
        168,
        167,
        173,
        171,
        239,
        241,
        237,
        185,
        246,
        189,
        237,
        230,
        33,
        92,
        106,
        63,
        75,
        126,
        60,
        88,
        90,
        59,
        96,
        124,
        52,
        126,
        112,
        49,
        71,
        82,
        48,
        187,
        179,
        8,
        20,
        14,
        84,
        26,
        80,
        14,
        3,
        198,
        185,
        137,
        194,
        180,
        131,
        223,
        149,
        166,
        221,
        135,
        159,
        219,
        169,
        173,
        19,
        13,
        17,
        77,
        0,
        73,
        21,
        119,
        118,
        80,
        59,
        72,
        73,
        74,
        75,
        68,
        69,
        70,
        68,
        64,
        65,
        66,
        67,
        92,
        93,
        94,
        95,
        88,
        89,
        90,
        91,
        84,
        85,
        86,
        87,
        80,
        81,
        82,
        83,
        44,
        45,
        46,
        47,
        40,
        41,
        42,
        43,
        36,
        37,
        38,
        39,
        86,
        87,
        84,
        85,
        74,
        75,
        72,
        73,
        56,
        57,
        58,
        59,
        52,
        53,
        54,
        55,
        48,
        49,
        50,
        51,
        12,
        13,
        14,
        15,
        8,
        9,
        10,
        11,
        4,
        5,
        6,
        4,
        0,
        1,
        2,
        3,
        28,
        29,
        30,
        31,
        24,
        25,
        26,
        27,
        20,
        21,
        22,
        23,
        16,
        17,
        18,
        19,
        236,
        237,
        238,
        239,
        232,
        233,
        234,
        235,
        228,
        229,
        230,
        231,
        150,
        151,
        148,
        149,
        138,
        139,
        136,
        137,
        248,
        249,
        250,
        251,
        244,
        245,
        246,
        247,
        240,
        241,
        242,
        243,
        204,
        205,
        206,
        202,
        188,
        183,
        201,
        180,
        196,
        194,
        185,
        196,
        191,
        195,
        189,
        195,
        175,
        222,
        171,
        173,
        170,
        219,
        169,
        220,
        171,
        214,
        164,
        211,
        213,
        209,
        162,
        160,
        218,
        210,
        169,
        222,
        168,
        220,
        213,
        218,
        218,
        218,
        166,
        212,
        215,
        210,
        165,
        167,
        194,
        189,
        193,
        203,
        186,
        187,
        190,
        190,
        176,
        183,
        196,
        192,
        194,
        179,
        205,
        193,
        250,
        142,
        241,
        250,
        136,
        137,
        138,
        139,
        132,
        133,
        134,
        132,
        128,
        129,
        130,
        131,
        156,
        157,
        158,
        159,
        152,
        153,
        154,
        155,
        148,
        149,
        150,
        151,
        144,
        145,
        146,
        147,
        108,
        109,
        110,
        111,
        25,
        29,
        28,
        30,
        102,
        101,
        22,
        99,
        20,
        22,
        103,
        19,
        10,
        14,
        10,
        123,
        11,
        12,
        126,
        127,
        116,
        7,
        0,
        8,
        5,
        14,
        112,
        0,
        62,
        58,
        58,
        58,
        61,
        61,
        79,
        57,
        67,
        70,
        50,
        66,
        55,
        65,
        53,
        60,
        34,
        42,
        41,
        32,
        43,
        89,
        37,
        36,
        34,
        39,
        36,
        39,
        32,
        86,
        39,
        44,
        41,
        82,
        81,
        93,
        86,
        41,
        41,
        94,
        82,
        33,
        34,
        39,
        32,
        85,
        82,
        83,
        74,
        57,
        61,
        72,
        73,
        78,
        73,
        60,
        70,
        70,
        72,
        72,
        69,
        68,
        64,
        54,
        125,
        127,
        11,
        126,
        12,
        12,
        127,
        122,
        113,
        117,
        1,
        115,
        0,
        115,
        2,
        115,
        109,
        108,
        97,
        26,
        107,
        102,
        30,
        25,
        17,
        22,
        21,
        98,
        96,
        21,
        98,
        108,
        152,
        154,
        157,
        154,
        234,
        159,
        237,
        148,
        154,
        148,
        151,
        226,
        229,
        145,
        150,
        228,
        142,
        140,
        140,
        137,
        142,
        141,
        248,
        251,
        129,
        129,
        245,
        132,
        132,
        134,
        135,
        140,
        204,
        187,
        201,
        185,
        142,
        212,
        104,
        53,
        2,
        174,
        105,
        63,
        35,
        218,
        195,
        199,
        168,
        168,
        169,
        171,
        121,
        11,
        52,
        123,
        26,
        36,
        117,
        1,
        27,
        114,
        59,
        19,
        13,
        75,
        105,
        13,
        86,
        98,
        10,
        64,
        65,
        4,
        65,
        79,
        1,
        115,
        76,
        3,
        114,
        76,
        29,
        105,
        115,
        26,
        83,
        123,
        21,
        83,
        113,
        21,
        78,
        122,
        18,
        88,
        105,
        44,
        105,
        103,
        33,
        115,
        86,
        41,
        120,
        79,
        38,
        109,
        103,
        33,
        107,
        81,
        63,
        116,
        97,
        60,
        115,
        71,
        57,
        65,
        122,
        54,
        103,
        127,
        51,
        107,
        86,
        175,
        70,
        88,
        71,
        79,
        90,
        98,
        65,
        76,
        87,
        73,
        104,
        105,
        85,
        68,
        108,
        66,
        82,
        111,
        121,
        10,
        113,
        122,
        12,
        95,
        64,
        67,
        98,
        0,
        5,
        79,
        31,
        93,
        78,
        127,
        81,
        92,
        75,
        57,
        101,
        34,
        58,
        112,
        104,
        78,
        75,
        104,
        50,
        99,
        74,
        114,
        89,
        94,
        91,
        40,
        37,
        76,
        84,
        91,
        66,
        117,
        65,
        79,
        63,
        120,
        51,
        7,
        93,
        70,
        0,
        43,
        11,
        66,
        18,
        49,
        25,
        3,
        37,
        85,
        93,
        47,
        76,
        62,
        20,
        12,
        20,
        20,
        78,
        39,
        26,
        22,
        33,
        76,
        75,
        3,
        38,
        67,
        32,
        19,
        35,
        11,
        35,
        54,
        1,
        124,
        46,
        16,
        14,
        16,
        54,
        62,
        112,
        1,
        34,
        2,
        9,
        100,
        99,
        111,
        108,
        31,
        98,
        106,
        17,
        16,
        18,
        19,
        108,
        17,
        236,
        232,
        152,
        237,
        236,
        236,
        238,
        152,
        155,
        224,
        149,
        151,
        227,
        150,
        151,
        227,
        130,
        253,
        142,
        136,
        253,
        249,
        136,
        248,
        244,
        132,
        132,
        133,
        135,
        244,
        245,
        140,
        191,
        205,
        204,
        189,
        184,
        182,
        200,
        206,
        196,
        197,
        180,
        192,
        179,
        182,
        179,
        193,
        169,
        168,
        171,
        168,
        174,
        172,
        223,
        173,
        166,
        214,
        211,
        169,
        174,
        213,
        173,
        173,
        174,
        173,
        221,
        208,
        168,
        218,
        173,
        221,
        166,
        167,
        209,
        213,
        162,
        212,
        208,
        160,
        205,
        184,
        189,
        201,
        201,
        202,
        191,
        197,
        202,
        182,
        193,
        196,
        197,
        195,
        181,
        176,
        136,
        142,
        241,
        140,
        249,
        141,
        138,
        253,
        246,
        246,
        248,
        248,
        133,
        255,
        252,
        245,
        236,
        158,
        225,
        225,
        231,
        230,
        157,
        232,
        144,
        145,
        147,
        226,
        146,
        224,
        236,
        226,
        26,
        104,
        31,
        27,
        22,
        110,
        111,
        26,
        22,
        19,
        22,
        19,
        96,
        22,
        96,
        17,
        121,
        120,
        122,
        15,
        9,
        12,
        127,
        5,
        118,
        114,
        113,
        117,
        112,
        4,
        118,
        115,
        57,
        61,
        75,
        48,
        55,
        55,
        78,
        52,
        65,
        52,
        65,
        69,
        52,
        63,
        50,
        54,
        42,
        89,
        44,
        88,
        42,
        94,
        37,
        47,
        33,
        247,
        173,
        174,
        241,
        131,
        188,
        243,
        252,
        219,
        141,
        198,
        232,
        136,
        204,
        236,
        133,
        206,
        246,
        134,
        243,
        194,
        128,
        216,
        197,
        156,
        236,
        209,
        150,
        255,
        253,
        152,
        221,
        245,
        151,
        209,
        247,
        144,
        217,
        227,
        173,
        222,
        237,
        102,
        116,
        126,
        121,
        127,
        115,
        107,
        4,
        4,
        21,
        4,
        22,
        12,
        10,
        15,
        17,
        22,
        16,
        98,
        116,
        28,
        22,
        23,
        2,
        20,
        5,
        4,
        105,
        125,
        250,
        249,
        231,
        253,
        225,
        236,
        227,
        225,
        159,
        247,
        227,
        241,
        143,
        143,
        236,
        234,
        233,
        239,
        133,
        144,
        150,
        239,
        243,
        239,
        230,
        142,
        153,
        152,
        158,
        244,
        251,
        251,
        196,
        206,
        203,
        221,
        209,
        203,
        205,
        217,
        195,
        190,
        168,
        210,
        195,
        213,
        210,
        204,
        217,
        222,
        199,
        215,
        223,
        210,
        217,
        160,
        187,
        186,
        185,
        117,
        43,
        40,
        115,
        1,
        66,
        218,
        216,
        199,
        221,
        218,
        194,
        222,
        210,
        6,
        105,
        108,
        1,
        125,
        96,
        0,
        85,
        125,
        31,
        89,
        127,
        24,
        82,
        106,
        20,
        64,
        101,
        209,
        19,
        71,
        105,
        29,
        101,
        94,
        44,
        116,
        113,
        40,
        88,
        101,
        39,
        108,
        70,
        38,
        102,
        70,
        35,
        107,
        77,
        61,
        107,
        76,
        236,
        255,
        57,
        109,
        79,
        59,
        127,
        68,
        50,
        106,
        107,
        50,
        190,
        131,
        205,
        134,
        168,
        200,
        140,
        172,
        197,
        141,
        183,
        199,
        149,
        178,
        107,
        73,
        94,
        27,
        124,
        88,
        74,
        94,
        28,
        110,
        93,
        70,
        66,
        82,
        83,
        10,
        79,
        5,
        119,
        48,
        88,
        102,
        97,
        99,
        101,
        99,
        101,
        62,
        102,
        96,
        106,
        116,
        97,
        62,
        79,
        104,
        125,
        107,
        62,
        86,
        88,
        32,
        105,
        34,
        109,
        42,
        70,
        118,
        103,
        102,
        61,
        4,
        26,
        13,
        83,
        77,
        23,
        95,
        31,
        65,
        91,
        49,
        3,
        21,
        23,
        12,
        9,
        15,
        88,
        42,
        27,
        28,
        9,
        15,
        27,
        7,
        9,
        81,
        63,
        25,
        18,
        26,
        119,
        13,
        41,
        37,
        61,
        42,
        119,
        62,
        39,
        47,
        37,
        34,
        50,
        103,
        48,
        55,
        47,
        53,
        59,
        113,
        45,
        38,
        47,
        57,
        51,
        39,
        53,
        124,
        56,
        33,
        56,
        125,
        199,
        202,
        208,
        129,
        75,
        34,
        43,
        69,
        29,
        34,
        70,
        54,
        16,
        78,
        51,
        17,
        147,
        151,
        203,
        192,
        205,
        219,
        221,
        201,
        215,
        158,
        132,
        152,
        159,
        151,
        208,
        212,
        243,
        248,
        168,
        239,
        252,
        224,
        225,
        173,
        248,
        239,
        245,
        241,
        225,
        245,
        229,
        225,
        255,
        250,
        244,
        237,
        251,
        237,
        188,
        232,
        225,
        246,
        226,
        177,
        182,
        254,
        240,
        240,
        132,
        159,
        129,
        143,
        135,
        138,
        136,
        205,
        128,
        154,
        192,
        195,
        196,
        215,
        210,
        212,
        176,
        201,
        176,
        202,
        199,
        203,
        187,
        200,
        183,
        162,
        199,
        201,
        174,
        187,
        181,
        166,
        136,
        136,
        140,
        155,
        136,
        130,
        133,
        131,
        137,
        140,
        153,
        145,
        146,
        146,
        146,
        159,
        141,
        129,
        148,
        145,
        151,
        140,
        157,
        140,
        147,
        226,
        226,
        226,
        140,
        155,
        135,
        131,
        24,
        17,
        123,
        76,
        66,
        74,
        79,
        89,
        2,
        196,
        169,
        169,
        192,
        187,
        136,
        192,
        181,
        140,
        24,
        127,
        76,
        80,
        81,
        29,
        72,
        95,
        99,
        72,
        69,
        67,
        81,
        88,
        121,
        43,
        95,
        97,
        107,
        125,
        105,
        45,
        229,
        191,
        150,
        228,
        137,
        176,
        36,
        56,
        58,
        42,
        40,
        41,
        62,
        74,
        114,
        116,
        125,
        125,
        48,
        80,
        122,
        123,
        52,
        70,
        15,
        7,
        13,
        10,
        26,
        79,
        139,
        228,
        234,
        133,
        252,
        205,
        131,
        232,
        211,
        69,
        60,
        9,
        23,
        20,
        94,
        5,
        16,
        46,
        11,
        0,
        4,
        20,
        27,
        4,
        84,
        34,
        34,
        46,
        58,
        44,
        110,
        168,
        240,
        219,
        167,
        204,
        247,
        97,
        123,
        103,
        118,
        112,
        106,
        107,
        120,
        24,
        48,
        59,
        124,
        19,
        61,
        39,
        112,
        20,
        46,
        62,
        39,
        33,
        217,
        131,
        251,
        204,
        194,
        202,
        207,
        217,
        130,
        68,
        41,
        41,
        64,
        59,
        8,
        64,
        53,
        12,
        152,
        255,
        204,
        208,
        209,
        157,
        200,
        223,
        227,
        200,
        197,
        195,
        209,
        216,
        249,
        171,
        223,
        225,
        235,
        253,
        233,
        173,
        101,
        63,
        22,
        100,
        9,
        48,
        164,
        184,
        186,
        170,
        168,
        169,
        183,
        174,
        172,
        179,
        161,
        166,
        161,
        161,
        184,
        164,
        161,
        187,
        218,
        184,
        156,
        136,
        156,
        155,
        160,
        136,
        140,
        132,
        148,
        137,
        176,
        134,
        150,
        160,
        130,
        139,
        138,
        184,
        140,
        141,
        157,
        132,
        20,
        100,
        80,
        23,
        99,
        127,
        17,
        117,
        118,
        153,
        169,
        167,
        165,
        41,
        91,
        109,
        36,
        86,
        72,
        38,
        92,
        67,
        33,
        106,
        92,
        60,
        120,
        88,
        178,
        176,
        187,
        186,
        183,
        161,
        150,
        176,
        181,
        163,
        187,
        167,
        83,
        66,
        70,
        64,
        90,
        127,
        77,
        95,
        81,
        66,
        80,
        81,
        72,
        70,
        73,
        64,
        91,
        75,
        72,
        119,
        95,
        82,
        89,
        84,
        81,
        92,
        94,
        65,
        87,
        67,
        92,
        92,
        105,
        100,
        102,
        89,
        111,
        123,
        100,
        34,
        100,
        98,
        118,
        104,
        101,
        104,
        106,
        43,
        115,
        120,
        119,
        105,
        107,
        125,
        112,
        116,
        97,
        123,
        116,
        112,
        98,
        114,
        100,
        96,
        8,
        7,
        1,
        26,
        6,
        48,
        8,
        12,
        22,
        6,
        22,
        4,
        20,
        20,
        13,
        10,
        20,
        31,
        11,
        58,
        17,
        27,
        25,
        152,
        221,
        245,
        151,
        209,
        247,
        158,
        225,
        202,
        175,
        241,
        238,
        172,
        241,
        202,
        165,
        236,
        249,
        166,
        228,
        230,
        162,
        253,
        202,
        163,
        210,
        205,
        191,
        244,
        215,
        187,
        230,
        211,
        183,
        252,
        214,
        182,
        246,
        214,
        176,
        237,
        39,
        78,
        45,
        40,
        70,
        1,
        20,
        73,
        26,
        45,
        71,
        53,
        14,
        65,
        44,
        18,
        95,
        43,
        53,
        94,
        37,
        7,
        89,
        45,
        62,
        85,
        56,
        39,
        81,
        44,
        12,
        81,
        54,
        55,
        109,
        38,
        8,
        104,
        44,
        12,
        106,
        60,
        7,
        101,
        60,
        9,
        99,
        43,
        26,
        126,
        21,
        12,
        123,
        48,
        26,
        122,
        50,
        18,
        120,
        22,
        37,
        114,
        36,
        4,
        2,
        77,
        105,
        12,
        98,
        106,
        9,
        125,
        73,
        7,
        88,
        75,
        3,
        74,
        115,
        2,
        86,
        93,
        156,
        144,
        153,
        150,
        136,
        27,
        103,
        67,
        21,
        92,
        97,
        148,
        156,
        148,
        184,
        170,
        171,
        189,
        171,
        189,
        41,
        96,
        85,
        36,
        108,
        103,
        182,
        178,
        170,
        166,
        174,
        174,
        185,
        173,
        183,
        176,
        178,
        59,
        114,
        84,
        55,
        83,
        111,
        48,
        120,
        115,
        207,
        164,
        159,
        204,
        129,
        169,
        203,
        141,
        163,
        203,
        167,
        146,
        195,
        151,
        181,
        205,
        156,
        186,
        221,
        181,
        187,
        218,
        172,
        150,
        214,
        139,
        176,
        213,
        142,
        157,
        215,
        181,
        139,
        238,
        154,
        133,
        235,
        160,
        138,
        234,
        162,
        130,
        229,
        187,
        146,
        227,
        188,
        139,
        252,
        140,
        191,
        252,
        177,
        153,
        251,
        189,
        147,
        244,
        139,
        169,
        243,
        160,
        186,
        253,
        237,
        216,
        141,
        217,
        255,
        98,
        102,
        136,
        205,
        229,
        135,
        193,
        231,
        131,
        220,
        232,
        157,
        215,
        222,
        156,
        238,
        247,
        148,
        219,
        243,
        149,
        193,
        243,
        153,
        203,
        238,
        147,
        214,
        225,
        161,
        201,
        212,
        167,
        243,
        202,
        167,
        236,
        198,
        166,
        230,
        198,
        163,
        232,
        204,
        188,
        205,
        252,
        183,
        245,
        208,
        181,
        253,
        210,
        179,
        209,
        212,
        36,
        49,
        57,
        207,
        200,
        220,
        137,
        203,
        215,
        220,
        196,
        208,
        218,
        255,
        197,
        199,
        211,
        193,
        133,
        220,
        201,
        215,
        212,
        158,
        202,
        207,
        216,
        192,
        236,
        197,
        194,
        211,
        197,
        199,
        229,
        235,
        248,
        251,
        254,
        225,
        253,
        232,
        173,
        238,
        230,
        238,
        230,
        242,
        239,
        164,
        233,
        255,
        232,
        235,
        185,
        234,
        247,
        253,
        243,
        178,
        200,
        161,
        188,
        175,
        202,
        207,
        165,
        199,
        210,
        181,
        195,
        190,
        142,
        159,
        158,
        149,
        140,
        146,
        133,
        198,
        148,
        133,
        136,
        159,
        219,
        153,
        138,
        222,
        144,
        142,
        221,
        129,
        154,
        157,
        152,
        154,
        150,
        134,
        213,
        190,
        164,
        232,
        188,
        189,
        170,
        190,
        157,
        163,
        176,
        179,
        182,
        169,
        181,
        160,
        229,
        174,
        180,
        183,
        249,
        173,
        182,
        177,
        173,
        190,
        182,
        128,
        176,
        165,
        164,
        163,
        186,
        88,
        79,
        8,
        1,
        93,
        71,
        67,
        88,
        78,
        71,
        92,
        76,
        83,
        84,
        80,
        12,
        26,
        95,
        81,
        95,
        88,
        90,
        78,
        29,
        80,
        74,
        16,
        80,
        66,
        23,
        88,
        80,
        107,
        120,
        124,
        41,
        85,
        62,
        33,
        52,
        95,
        88,
        48,
        44,
        63,
        90,
        46,
        37,
        121,
        115,
        121,
        107,
        127,
        124,
        104,
        120,
        96,
        96,
        64,
        112,
        101,
        100,
        99,
        122,
        24,
        15,
        72,
        65,
        29,
        7,
        3,
        24,
        14,
        7,
        28,
        12,
        19,
        20,
        16,
        76,
        90,
        24,
        23,
        23,
        10,
        30,
        21,
        19,
        82,
        18,
        4,
        81,
        26,
        18,
        21,
        6,
        62,
        107,
        39,
        39,
        43,
        111,
        23,
        17,
        30,
        30,
        106,
        26,
        26,
        52,
        25,
        111,
        114,
        63,
        49,
        62,
        55,
        43,
        32,
        62,
        58,
        50,
        34,
        48,
        53,
        35,
        49,
        39,
        214,
        219,
        221,
        199,
        205,
        219,
        217,
        204,
        214,
        202,
        207,
        207,
        143,
        247,
        197,
        214,
        201,
        204,
        215,
        203,
        218,
        159,
        148,
        206,
        218,
        220,
        197,
        221,
        210,
        203,
        217,
        192,
        249,
        255,
        161,
        169,
        234,
        230,
        234,
        235,
        231,
        241,
        160,
        231,
        244,
        232,
        233,
        165,
        238,
        243,
        253,
        185,
        197,
        195,
        192,
        192,
        184,
        200,
        204,
        226,
        203,
        189,
        251,
        249,
        142,
        203,
        152,
        136,
        157,
        156,
        155,
        130,
        144,
        135,
        192,
        131,
        159,
        199,
        133,
        145,
        218,
        151,
        157,
        152,
        141,
        139,
        220,
        166,
        195,
        222,
        201,
        172,
        173,
        199,
        217,
        204,
        151,
        225,
        232,
        170,
        166,
        174,
        190,
        172,
        161,
        183,
        165,
        179,
        181,
        189,
        168,
        182,
        191,
        175,
        172,
        176,
        176,
        184,
        242,
        190,
        189,
        189,
        182,
        184,
        177,
        249,
        190,
        166,
        69,
        69,
        123,
        76,
        92,
        89,
        69,
        78,
        71,
        109,
        65,
        76,
        67,
        114,
        86,
        73,
        121,
        84,
        92,
        92,
        120,
        80,
        78,
        111,
        87,
        85,
        99,
        84,
        68,
        65,
        93,
        86,
        111,
        49,
        93,
        123,
        98,
        76,
        99,
        105,
        103,
        46,
        122,
        109,
        117,
        116,
        107,
        224,
        146,
        134,
        253,
        190,
        149,
        250,
        144,
        139,
        245,
        168,
        143,
        245,
        174,
        151,
        243,
        140,
        209,
        142,
        213,
        252,
        134,
        193,
        200,
        133,
        205,
        226,
        133,
        230,
        220,
        142,
        240,
        252,
        149,
        199,
        226,
        159,
        226,
        213,
        154,
        244,
        204,
        150,
        248,
        193,
        159,
        242,
        249,
        146,
        247,
        229,
        174,
        223,
        201,
        171,
        247,
        251,
        96,
        57,
        44,
        50,
        35,
        51,
        48,
        44,
        52,
        60,
        118,
        58,
        49,
        49,
        58,
        52,
        53,
        125,
        58,
        34,
        57,
        57,
        118,
        186,
        22,
        42,
        77,
        25,
        40,
        75,
        20,
        32,
        70,
        31,
        58,
        68,
        54,
        8,
        67,
        49,
        18,
        92,
        3,
        38,
        90,
        7,
        60,
        90,
        43,
        8,
        85,
        12,
        35,
        95,
        26,
        17,
        98,
        36,
        9,
        96,
        47,
        58,
        111,
        13,
        0,
        102,
        8,
        28,
        99,
        32,
        15,
        96,
        22,
        13,
        127,
        34,
        1,
        123,
        36,
        29,
        117,
        10,
        43,
        116,
        43,
        2,
        124,
        59,
        78,
        3,
        71,
        104,
        11,
        104,
        86,
        4,
        118,
        122,
        15,
        93,
        106,
        2,
        84,
        99,
        30,
        67,
        117,
        29,
        66,
        101,
        25,
        109,
        93,
        20,
        100,
        89,
        17,
        76,
        107,
        17,
        114,
        75,
        47,
        80,
        117,
        42,
        113,
        88,
        42,
        109,
        100,
        41,
        105,
        70,
        45,
        100,
        111,
        56,
        88,
        91,
        241,
        190,
        172,
        180,
        253,
        158,
        177,
        191,
        183,
        176,
        177,
        250,
        109,
        78,
        92,
        121,
        79,
        92,
        95,
        90,
        77,
        81,
        68,
        113,
        73,
        75,
        77,
        70,
        67,
        104,
        76,
        88,
        74,
        90,
        72,
        79,
        71,
        86,
        216,
        191,
        129,
        210,
        187,
        163,
        239,
        164,
        142,
        238,
        174,
        142,
        235,
        160,
        148,
        228,
        149,
        164,
        227,
        151,
        171,
        226,
        142,
        179,
        255,
        147,
        168,
        249,
        156,
        156,
        247,
        183,
        161,
        249,
        162,
        178,
        110,
        121,
        25,
        24,
        7,
        68,
        27,
        29,
        0,
        139,
        254,
        201,
        132,
        218,
        232,
        128,
        223,
        250,
        158,
        195,
        248,
        159,
        235,
        207,
        154,
        240,
        220,
        149,
        202,
        225,
        158,
        249,
        195,
        144,
        197,
        221,
        173,
        193,
        254,
        20,
        54,
        33,
        49,
        48,
        47,
        108,
        51,
        53,
        40,
        24,
        189,
        193,
        220,
        191,
        203,
        239,
        186,
        208,
        252,
        181,
        234,
        193,
        190,
        249,
        234,
        178,
        23,
        5,
        253,
        251,
        226,
        73,
        48,
        7,
        70,
        24,
        46,
        70,
        29,
        56,
        64,
        29,
        58,
        93,
        45,
        9,
        88,
        50,
        18,
        91,
        8,
        35,
        88,
        63,
        1,
        82,
        59,
        35,
        111,
        3,
        56,
        210,
        244,
        227,
        255,
        254,
        237,
        174,
        245,
        243,
        234,
        218,
        99,
        31,
        30,
        192,
        205,
        203,
        210,
        194,
        207,
        232,
        241,
        240,
        245,
        226,
        229,
        211,
        245,
        225,
        139,
        191,
        135,
        130,
        139,
        129,
        4,
        74,
        65,
        5,
        126,
        113,
        1,
        115,
        76,
        3,
        114,
        76,
        28,
        70,
        95,
        25,
        125,
        82,
        23,
        87,
        65,
        25,
        66,
        82,
        157,
        145,
        171,
        168,
        171,
        166,
        187,
        161,
        184,
        163,
        163,
        174,
        165,
        160,
        165,
        164,
        171,
        176,
        180,
        175,
        135,
        188,
        166,
        171,
        185,
        179,
        182,
        182,
        168,
        165,
        179,
        185,
        176,
        165,
        66,
        68,
        70,
        76,
        75,
        66,
        77,
        68,
        78,
        74,
        83,
        64,
        66,
        74,
        77,
        75,
        88,
        82,
        86,
        93,
        83,
        94,
        85,
        83,
        83,
        70,
        68,
        89,
        87,
        66,
        64,
        93,
        99,
        111,
        107,
        124,
        124,
        125,
        105,
        99,
        118,
        92,
        97,
        98,
        101,
        104,
        113,
        107,
        110,
        120,
        109,
        107,
        108,
        122,
        114,
        105,
        77,
        114,
        115,
        114,
        121,
        98,
        122,
        97,
        53,
        14,
        16,
        29,
        11,
        1,
        8,
        32,
        43,
        42,
        34,
        40,
        12,
        38,
        42,
        39,
        29,
        16,
        9,
        17,
        21,
        22,
        59,
        68,
        5,
        67,
        50,
        48,
        39,
        50,
        50,
        52,
        11,
        4,
        11,
        8,
        31,
        119,
        13,
        0,
        11,
        10,
        2,
        2,
        33,
        12,
        7,
        4,
        11,
        30,
        25,
        108,
        13,
        103,
        54,
        100,
        19,
        5,
        96,
        103,
        98,
        120,
        1,
        22,
        210,
        204,
        252,
        231,
        155,
        152,
        251,
        200,
        239,
        140,
        216,
        244,
        237,
        195,
        220,
        203,
        233,
        242,
        235,
        200,
        248,
        222,
        242,
        228,
        222,
        248,
        210,
        128,
        196,
        128,
        236,
        210,
        235,
        204,
        231,
        208,
        229,
        187,
        219,
        222,
        177,
        214,
        244,
        194,
        245,
        223,
        195,
        220,
        248,
        207,
        170,
        223,
        169,
        170,
        249,
        200,
        215,
        212,
        232,
        225,
        207,
        165,
        194,
        211,
        144,
        143,
        129,
        158,
        132,
        223,
        171,
        129,
        213,
        142,
        167,
        215,
        169,
        146,
        133,
        145,
        202,
        150,
        178,
        200,
        204,
        143,
        158,
        169,
        189,
        170,
        132,
        168,
        174,
        186,
        179,
        143,
        186,
        252,
        250,
        128,
        136,
        137,
        173,
        156,
        139,
        183,
        167,
        144,
        136,
        146,
        245,
        167,
        184,
        240,
        157,
        143,
        135,
        176,
        185,
        152,
        181,
        165,
        161,
        150,
        167,
        162,
        158,
        236,
        1,
        115,
        92,
        69,
        122,
        100,
        66,
        75,
        86,
        122,
        90,
        98,
        84,
        99,
        112,
        115,
        82,
        11,
        109,
        84,
        122,
        114,
        4,
        87,
        75,
        92,
        7,
        102,
        5,
        109,
        100,
        116,
        94,
        81,
        125,
        78,
        66,
        68,
        86,
        60,
        70,
        102,
        74,
        67,
        111,
        95,
        102,
        54,
        89,
        107,
        55,
        94,
        74,
        84,
        110,
        104,
        104,
        89,
        118,
        70,
        110,
        68,
        110,
        64,
        5,
        56,
        59,
        95,
        39,
        1,
        47,
        25,
        41,
        48,
        84,
        44,
        11,
        43,
        86,
        1,
        3,
        58,
        75,
        32,
        73,
        12,
        14,
        75,
        10,
        67,
        52,
        70,
        26,
        46,
        77,
        62,
        32,
        63,
        12,
        32,
        55,
        4,
        103,
        41,
        117,
        19,
        113,
        12,
        34,
        33,
        107,
        117,
        21,
        24,
        111,
        55,
        27,
        27,
        5,
        9,
        28,
        63,
        26,
        48,
        100,
        18,
        23,
        56,
        196,
        237,
        131,
        243,
        212,
        235,
        207,
        154,
        251,
        232,
        153,
        233,
        141,
        151,
        244,
        194,
        139,
        212,
        225,
        195,
        196,
        201,
        205,
        218,
        218,
        139,
        196,
        194,
        247,
        230,
        209,
        194,
        250,
        186,
        230,
        189,
        218,
        219,
        250,
        184,
        179,
        226,
        199,
        212,
        247,
        229,
        175,
        178,
        200,
        216,
        173,
        232,
        177,
        166,
        212,
        165,
        202,
        170,
        217,
        232,
        213,
        188,
        249,
        172,
        175,
        187,
        156,
        184,
        188,
        166,
        181,
        131,
        179,
        170,
        164,
        160,
        183,
        166,
        166,
        132,
        147,
        148,
        139,
        138,
        145,
        154,
        132,
        141,
        155,
        128,
        131,
        144,
        131,
        147,
        151,
        157,
        163,
        165,
        173,
        186,
        171,
        161,
        173,
        160,
        167,
        179,
        161,
        178,
        181,
        176,
        171,
        183,
        190,
        174,
        171,
        188,
        172,
        177,
        189,
        176,
        183,
        160,
        179,
        142,
        179,
        165,
        166,
        186,
        88,
        116,
        192,
        167,
        153,
        202,
        163,
        187,
        199,
        171,
        144,
        198,
        188,
        163,
        195,
        177,
        146,
        221,
        176,
        142,
        218,
        128,
        157,
        219,
        179,
        156,
        212,
        137,
        140,
        208,
        157,
        143,
        65,
        78,
        81,
        64,
        64,
        73,
        67,
        120,
        113,
        102,
        114,
        94,
        111,
        99,
        77,
        65,
        253,
        167,
        142,
        252,
        145,
        168,
        251,
        132,
        169,
        246,
        173,
        132,
        242,
        173,
        174,
        240,
        251,
        243,
        141,
        206,
        253,
        138,
        252,
        224,
        135,
        196,
        243,
        132,
        246,
        234,
        41,
        44,
        62,
        1,
        20,
        17,
        23,
        12,
        35,
        16,
        27,
        23,
        152,
        255,
        193,
        146,
        251,
        227,
        7,
        2,
        12,
        172,
        201,
        245,
        165,
        217,
        219,
        172,
        252,
        219,
        174,
        224,
        231,
        163,
        196,
        203,
        191,
        205,
        246,
        185,
        212,
        234,
        182,
        236,
        241,
        183,
        215,
        248,
        177,
        210,
        16,
        66,
        60,
        48,
        65,
        19,
        54,
        242,
        212,
        194,
        204,
        254,
        205,
        194,
        221,
        211,
        219,
        215,
        205,
        220,
        205,
        218,
        208,
        216,
        209,
        199,
        144,
        235,
        218,
        208,
        209,
        193,
        255,
        229,
        225,
        253,
        251,
        252,
        233,
        255,
        235,
        237,
        230,
        238,
        244,
        175,
        234,
        240,
        246,
        247,
        177,
        185,
        248,
        237,
        243,
        240,
        178,
        247,
        229,
        240,
        250,
        115,
        47,
        27,
        176,
        167,
        160,
        160,
        189,
        10,
        86,
        126,
        10,
        76,
        91,
        4,
        105,
        113,
        1,
        105,
        65,
        31,
        71,
        100,
        22,
        75,
        83,
        27,
        82,
        116,
        20,
        78,
        87,
        17,
        117,
        90,
        47,
        75,
        116,
        33,
        105,
        108,
        42,
        83,
        82,
        38,
        71,
        123,
        47,
        83,
        93,
        229,
        230,
        246,
        248,
        60,
        82,
        100,
        53,
        68,
        112,
        54,
        64,
        92,
        49,
        112,
        100,
        48,
        166,
        144,
        193,
        176,
        140,
        202,
        160,
        150,
        198,
        156,
        189,
        198,
        154,
        177,
        195,
        133,
        187,
        220,
        131,
        166,
        218,
        135,
        188,
        219,
        167,
        131,
        214,
        188,
        152,
        209,
        142,
        165,
        239,
        187,
        146,
        239,
        146,
        165,
        233,
        133,
        159,
        230,
        167,
        138,
        227,
        139,
        146,
        226,
        161,
        132,
        252,
        161,
        158,
        249,
        137,
        173,
        244,
        158,
        190,
        247,
        172,
        135,
        241,
        165,
        240,
        141,
        244,
        195,
        139,
        231,
        241,
        136,
        197,
        232,
        133,
        237,
        240,
        136,
        216,
        233,
        146,
        212,
        207,
        156,
        251,
        247,
        148,
        205,
        241,
        148,
        228,
        217,
        86,
        51,
        7,
        52,
        45,
        46,
        38,
        61,
        8,
        46,
        47,
        57,
        45,
        49,
        57,
        111,
        15,
        41,
        55,
        49,
        59,
        53,
        59,
        60,
        112,
        22,
        50,
        52,
        38,
        123,
        121,
        113,
        179,
        208,
        233,
        179,
        63,
        27,
        64,
        22,
        53,
        71,
        13,
        33,
        71,
        43,
        61,
        68,
        1,
        44,
        65,
        41,
        44,
        88,
        56,
        59,
        228,
        243,
        239,
        238,
        253,
        236,
        234,
        253,
        254,
        254,
        231,
        93,
        37,
        60,
        109,
        12,
        6,
        103,
        60,
        14,
        101,
        23,
        40,
        251,
        234,
        239,
        237,
        246,
        219,
        238,
        236,
        241,
        176,
        214,
        242,
        244,
        230,
        118,
        24,
        12,
        115,
        48,
        31,
        112,
        102,
        125,
        14,
        127,
        87,
        9,
        95,
        120,
        7,
        115,
        110,
        14,
        90,
        107,
        1,
        99,
        119,
        19,
        72,
        122,
        25,
        107,
        84,
        27,
        100,
        74,
        22,
        66,
        99,
        17,
        125,
        82,
        34,
        106,
        68,
        33,
        64,
        120,
        41,
        66,
        84,
        153,
        140,
        137,
        143,
        148,
        44,
        122,
        68,
        61,
        86,
        124,
        58,
        96,
        125,
        59,
        83,
        124,
        52,
        105,
        108,
        48,
        125,
        111,
        194,
        132,
        159,
        205,
        147,
        144,
        203,
        185,
        138,
        196,
        155,
        190,
        194,
        159,
        164,
        205,
        128,
        144,
        220,
        130,
        131,
        215,
        146,
        153,
        218,
        156,
        177,
        215,
        160,
        142,
        209,
        137,
        133,
        236,
        145,
        178,
        235,
        178,
        153,
        234,
        179,
        184,
        229,
        159,
        141,
        226,
        170,
        155,
        254,
        162,
        184,
        254,
        138,
        128,
        244,
        169,
        180,
        246,
        159,
        166,
        241,
        142,
        175,
        240,
        215,
        254,
        128,
        221,
        200,
        138,
        227,
        218,
        135,
        235,
        203,
        132,
        246,
        234,
        23,
        0,
        22,
        30,
        27,
        13,
        94,
        78,
        92,
        54,
        55,
        42,
        92,
        16,
        88,
        30,
        16,
        89,
        43,
        101,
        175,
        245,
        216,
        170,
        195,
        250,
        110,
        34,
        110,
        164,
        225,
        212,
        161,
        213,
        215,
        119,
        58,
        119,
        185,
        203,
        244,
        187,
        218,
        228,
        181,
        193,
        219,
        123,
        115,
        114,
        138,
        226,
        230,
        239,
        225,
        143,
        234,
        223,
        205,
        206,
        128,
        69,
        28,
        29,
        65,
        52,
        34,
        83,
        25,
        17,
        158,
        222,
        144,
        157,
        86,
        11,
        58,
        87,
        42,
        13,
        80,
        15,
        48,
        110,
        25,
        17,
        102,
        46,
        36,
        173,
        224,
        163,
        215,
        233,
        227,
        245,
        225,
        165,
        251,
        181,
        209,
        253,
        190,
        162,
        188,
        255,
        188,
        119,
        42,
        43,
        115,
        6,
        12,
        252,
        142,
        203,
        169,
        135,
        138,
        207,
        142,
        195,
        5,
        119,
        72,
        7,
        110,
        80,
        1,
        117,
        119,
        198,
        194,
        169,
        207,
        223,
        137,
        147,
        155,
        156,
        158,
        209,
        151,
        155,
        152,
        213,
        185,
        174,
        164,
        172,
        173,
        187,
        236,
        255,
        238,
        243,
        236,
        230,
        225,
        235,
        227,
        226,
        246,
        252,
        255,
        245,
        132,
        179,
        187,
        184,
        166,
        166,
        190,
        184,
        162,
        162,
        167,
        176,
        88,
        66,
        70,
        79,
        65,
        93,
        4,
        23,
        114,
        18,
        9,
        1,
        64,
        85,
        75,
        72,
        26,
        95,
        77,
        88,
        82,
        5,
        108,
        12,
        214,
        136,
        190,
        107,
        122,
        127,
        125,
        102,
        239,
        177,
        155,
        225,
        128,
        184,
        233,
        130,
        148,
        231,
        186,
        187,
        227,
        150,
        156,
        225,
        165,
        186,
        254,
        152,
        177,
        63,
        32,
        48,
        50,
        251,
        164,
        183,
        243,
        152,
        163,
        241,
        210,
        230,
        128,
        234,
        211,
        139,
        212,
        215,
        133,
        202,
        218,
        132,
        201,
        225,
        131,
        197,
        251,
        159,
        192,
        244,
        150,
        252,
        193,
        153,
        202,
        201,
        151,
        216,
        204,
        146,
        196,
        243,
        172,
        208,
        252,
        175,
        216,
        255,
        169,
        197,
        233,
        166,
        208,
        204,
        162,
        255,
        254,
        166,
        218,
        203,
        187,
        217,
        207,
        184,
        198,
        217,
        186,
        231,
        246,
        180,
        217,
        224,
        183,
        213,
        58,
        72,
        40,
        56,
        75,
        0,
        42,
        74,
        2,
        34,
        213,
        209,
        194,
        198,
        208,
        192,
        154,
        94,
        22,
        39,
        90,
        6,
        28,
        89,
        8,
        9,
        85,
        32,
        46,
        95,
        21,
        29,
        170,
        248,
        237,
        253,
        174,
        106,
        35,
        11,
        101,
        35,
        1,
        188,
        188,
        215,
        181,
        165,
        237,
        243,
        253,
        235,
        251,
        191,
        123,
        9,
        58,
        117,
        24,
        38,
        115,
        7,
        25,
        168,
        208,
        187,
        218,
        211,
        190,
        221,
        10,
        118,
        86,
        5,
        118,
        81,
        3,
        73,
        122,
        1,
        67,
        91,
        31,
        109,
        97,
        26,
        83,
        123,
        21,
        83,
        113,
        20,
        88,
        105,
        16,
        76,
        106,
        47,
        114,
        115,
        43,
        94,
        84,
        37,
        99,
        107,
        38,
        90,
        114,
        33,
        82,
        117,
        63,
        116,
        94,
        62,
        126,
        94,
        57,
        121,
        99,
        59,
        100,
        116,
        57,
        107,
        78,
        60,
        173,
        166,
        207,
        148,
        128,
        202,
        148,
        189,
        199,
        172,
        151,
        198,
        188,
        163,
        193,
        138,
        188,
        220,
        152,
        184,
        219,
        184,
        134,
        212,
        166,
        170,
        69,
        65,
        82,
        86,
        64,
        80,
        42,
        239,
        176,
        131,
        232,
        147,
        182,
        233,
        184,
        185,
        229,
        144,
        158,
        239,
        165,
        173,
        58,
        104,
        125,
        109,
        62,
        248,
        133,
        166,
        247,
        174,
        133,
        244,
        185,
        145,
        243,
        181,
        235,
        86,
        82,
        57,
        95,
        79,
        27,
        5,
        7,
        17,
        5,
        65,
        129,
        243,
        204,
        131,
        242,
        204,
        157,
        233,
        243,
        66,
        70,
        45,
        64,
        149,
        235,
        197,
        144,
        225,
        196,
        145,
        242,
        193,
        174,
        213,
        244,
        171,
        246,
        247,
        167,
        210,
        216,
        169,
        231,
        239,
        161,
        234,
        220,
        188,
        248,
        216,
        186,
        231,
        214,
        187,
        206,
        233,
        180,
        235,
        236,
        178,
        197,
        205,
        66,
        10,
        0,
        79,
        53,
        27,
        74,
        59,
        18,
        70,
        15,
        39,
        65,
        7,
        37,
        64,
        30,
        10,
        80,
        13,
        27,
        80,
        0,
        39,
        84,
        16,
        48,
        87,
        41,
        18,
        238,
        249,
        194,
        194,
        219,
        110,
        7,
        7,
        106,
        17,
        46,
        101,
        24,
        46,
        99,
        23,
        34,
        96,
        62,
        60,
        124,
        35,
        16,
        174,
        172,
        179,
        161,
        166,
        190,
        160,
        162,
        167,
        186,
        165,
        219,
        216,
        217,
        15,
        78,
        78,
        5,
        71,
        110,
        153,
        140,
        137,
        143,
        148,
        3,
        108,
        114,
        29,
        100,
        85,
        27,
        67,
        126,
        24,
        74,
        75,
        31,
        77,
        122,
        31,
        91,
        78,
        47,
        68,
        94,
        147,
        130,
        135,
        133,
        158,
        37,
        74,
        72,
        39,
        90,
        107,
        43,
        121,
        64,
        52,
        100,
        67,
        56,
        74,
        108,
        59,
        95,
        125,
        54,
        107,
        70,
        245,
        246,
        49,
        146,
        166,
        205,
        132,
        182,
        202,
        176,
        133,
        119,
        80,
        69,
        83,
        6,
        110,
        96,
        117,
        91,
        72,
        75,
        78,
        81,
        77,
        88,
        117,
        125,
        96,
        100,
        97,
        121,
        101,
        96,
        102,
        79,
        89,
        94,
        64,
        77,
        74,
        83,
        67,
        67,
        78,
        69,
        35,
        226,
        184,
        165,
        227,
        155,
        180,
        252,
        161,
        147,
        249,
        177,
        190,
        245,
        178,
        190,
        49,
        59,
        55,
        103,
        112,
        6,
        14,
        11,
        29,
        78,
        138,
        227,
        239,
        132,
        246,
        208,
        132,
        230,
        219,
        68,
        3,
        8,
        20,
        21,
        89,
        4,
        19,
        44,
        28,
        0,
        18,
        29,
        20,
        2,
        18,
        6,
        6,
        106,
        60,
        32,
        44,
        60,
        42,
        108,
        168,
        205,
        193,
        166,
        212,
        246,
        162,
        212,
        200,
        103,
        124,
        189,
        246,
        226,
        183,
        212,
        247,
        183,
        220,
        224,
        180,
        234,
        237,
        177,
        221,
        28,
        76,
        60,
        1,
        72,
        39,
        27,
        72,
        42,
        8,
        69,
        49,
        43,
        64,
        61,
        30,
        95,
        6,
        45,
        158,
        158,
        222,
        210,
        217,
        146,
        84,
        3,
        10,
        81,
        12,
        43,
        149,
        227,
        248,
        168,
        231,
        251,
        227,
        224,
        173,
        227,
        237,
        228,
        161,
        96,
        47,
        37,
        96,
        7,
        12,
        184,
        240,
        237,
        191,
        242,
        232,
        254,
        255,
        118,
        14,
        51,
        127,
        59,
        55,
        176,
        167,
        160,
        160,
        189,
        10,
        86,
        126,
        7,
        108,
        98,
        7,
        115,
        87,
        198,
        0,
        70,
        65,
        29,
        113,
        72,
        27,
        65,
        66,
        21,
        103,
        88,
        20,
        126,
        92,
        17,
        101,
        71,
        233,
        45,
        73,
        114,
        41,
        83,
        104,
        42,
        108,
        98,
        155,
        138,
        143,
        141,
        150,
        63,
        97,
        75,
        60,
        81,
        93,
        58,
        72,
        98,
        241,
        53,
        109,
        108,
        50,
        92,
        99,
        206,
        150,
        151,
        206,
        186,
        135,
        201,
        165,
        137,
        198,
        176,
        172,
        4,
        194,
        164,
        153,
        223,
        188,
        130,
        208,
        170,
        166,
        211,
        129,
        190,
        214,
        128,
        183,
        223,
        176,
        179,
        210,
        158,
        163,
        239,
        181,
        180,
        232,
        144,
        140,
        231,
        131,
        188,
        238,
        186,
        157,
        224,
        189,
        151,
        254,
        164,
        163,
        251,
        151,
        170,
        250,
        134,
        187,
        245,
        153,
        189,
        242,
        132,
        152,
        141,
        242,
        211,
        140,
        211,
        250,
        143,
        237,
        224,
        133,
        244,
        215,
        131,
        239,
        212,
        130,
        193,
        228,
        156,
        193,
        254,
        153,
        233,
        205,
        148,
        254,
        222,
        151,
        204,
        231,
        145,
        250,
        210,
        174,
        196,
        223,
        167,
        207,
        214,
        170,
        221,
        230,
        175,
        253,
        202,
        163,
        249,
        195,
        188,
        206,
        232,
        191,
        211,
        241,
        186,
        231,
        194,
        180,
        236,
        199,
        177,
        247,
        213,
        177,
        18,
        17,
        79,
        0,
        20,
        76,
        44,
        47,
        71,
        19,
        38,
        73,
        17,
        20,
        76,
        26,
        61,
        95,
        0,
        52,
        91,
        62,
        38,
        88,
        22,
        55,
        87,
        33,
        48,
        84,
        52,
        55,
        108,
        30,
        56,
        111,
        3,
        33,
        106,
        55,
        18,
        108,
        60,
        27,
        188,
        175,
        205,
        193,
        160,
        178,
        126,
        13,
        40,
        122,
        20,
        45,
        117,
        40,
        15,
        117,
        46,
        23,
        114,
        0,
        90,
        13,
        101,
        71,
        8,
        85,
        124,
        8,
        109,
        123,
        5,
        109,
        112,
        14,
        100,
        127,
        29,
        100,
        93,
        22,
        66,
        101,
        222,
        223,
        26,
        76,
        110,
        23,
        120,
        82,
        16,
        74,
        107,
        45,
        73,
        102,
        43,
        64,
        84,
        40,
        78,
        85,
        47,
        125,
        74,
        34,
        116,
        67,
        63,
        92,
        94,
        60,
        122,
        88,
        57,
        92,
        72,
        53,
        75,
        101,
        48,
        65,
        100,
        54,
        170,
        169,
        206,
        189,
        152,
        202,
        164,
        157,
        197,
        152,
        191,
        197,
        158,
        167,
        194,
        176,
        138,
        221,
        181,
        151,
        216,
        133,
        172,
        216,
        189,
        171,
        213,
        189,
        160,
        222,
        180,
        175,
        237,
        148,
        173,
        230,
        178,
        131,
        232,
        176,
        132,
        230,
        164,
        133,
        225,
        151,
        130,
        227,
        141,
        173,
        253,
        158,
        164,
        246,
        136,
        132,
        253,
        175,
        156,
        244,
        166,
        145,
        241,
        170,
        215,
        140,
        253,
        204,
        134,
        192,
        201,
        132,
        246,
        250,
        136,
        206,
        201,
        130,
        221,
        211,
        157,
        192,
        223,
        158,
        197,
        210,
        148,
        194,
        226,
        155,
        209,
        253,
        153,
        203,
        245,
        157,
        228,
        245,
        175,
        244,
        224,
        170,
        240,
        247,
        167,
        203,
        246,
        164,
        206,
        236,
        161,
        213,
        215,
        188,
        193,
        226,
        187,
        226,
        201,
        187,
        197,
        229,
        11,
        12,
        176,
        203,
        254,
        189,
        4,
        21,
        79,
        20,
        0,
        74,
        36,
        6,
        71,
        51,
        45,
        78,
        26,
        43,
        64,
        29,
        55,
        94,
        61,
        56,
        86,
        17,
        4,
        90,
        43,
        8,
        85,
        12,
        35,
        95,
        26,
        17,
        98,
        36,
        9,
        108,
        42,
        62,
        100,
        57,
        39,
        108,
        60,
        27,
        97,
        19,
        44,
        99,
        18,
        44,
        125,
        9,
        19,
        122,
        51,
        27,
        117,
        51,
        17,
        117,
        46,
        26,
        114,
        56,
        73,
        12,
        73,
        71,
        157,
        138,
        128,
        136,
        129,
        151,
        192,
        168,
        162,
        203,
        3,
        89,
        108,
        30,
        119,
        78,
        210,
        26,
        91,
        110,
        23,
        99,
        125,
        221,
        19,
        88,
        114,
        18,
        106,
        74,
        232,
        157,
        139,
        151,
        152,
        225,
        39,
        108,
        124,
        36,
        126,
        79,
        141,
        129,
        250,
        139,
        145,
        157,
        254,
        185,
        174,
        178,
        191,
        243,
        53,
        127,
        72,
        51,
        109,
        117,
        206,
        145,
        146,
        204,
        191,
        183,
        196,
        140,
        138,
        3,
        87,
        73,
        67,
        85,
        65,
        5,
        221,
        175,
        144,
        223,
        182,
        136,
        217,
        173,
        191,
        14,
        10,
        97,
        7,
        23,
        85,
        91,
        110,
        43,
        238,
        155,
        170,
        233,
        173,
        174,
        228,
        148,
        182,
        232,
        145,
        179,
        58,
        118,
        99,
        104,
        124,
        120,
        106,
        122,
        72,
        88,
        74,
        71,
        67,
        116,
        122,
        114,
        119,
        97,
        74,
        90,
        72,
        47,
        28,
        0,
        1,
        77,
        134,
        219,
        234,
        135,
        250,
        221,
        128,
        223,
        192,
        158,
        233,
        225,
        150,
        222,
        212,
        93,
        37,
        27,
        21,
        3,
        19,
        87,
        144,
        207,
        240,
        174,
        217,
        209,
        39,
        43,
        113,
        119,
        18,
        114,
        16,
        8,
        2,
        175,
        234,
        225,
        178,
        244,
        217,
        188,
        250,
        238,
        180,
        233,
        247,
        188,
        236,
        203,
        179,
        249,
        202,
        177,
        19,
        11,
        79,
        61,
        49,
        74,
        3,
        17,
        71,
        27,
        40,
        70,
        44,
        17,
        66,
        37,
        59,
        95,
        0,
        52,
        88,
        18,
        31,
        90,
        19,
        29,
        195,
        212,
        218,
        210,
        215,
        193,
        170,
        186,
        168,
        239,
        252,
        224,
        225,
        173,
        103,
        45,
        30,
        101,
        63,
        39,
        96,
        63,
        32,
        126,
        9,
        1,
        118,
        34,
        48,
        120,
        21,
        41,
        120,
        48,
        62,
        183,
        227,
        253,
        143,
        153,
        141,
        201,
        11,
        65,
        114,
        9,
        91,
        67,
        4,
        91,
        92,
        2,
        117,
        125,
        147,
        159,
        197,
        195,
        174,
        206,
        220,
        156,
        156,
        151,
        208,
        22,
        124,
        65,
        18,
        117,
        75,
        246,
        249,
        33,
        97,
        116,
        41,
        66,
        84,
        38,
        110,
        95,
        34,
        126,
        100,
        33,
        96,
        97,
        61,
        72,
        70,
        59,
        99,
        124,
        52,
        82,
        127,
        55,
        65,
        97,
        59,
        105,
        166,
        195,
        166,
        158,
        203,
        160,
        186,
        200,
        174,
        152,
        201,
        184,
        132,
        194,
        168,
        158,
        222,
        132,
        165,
        209,
        138,
        144,
        218,
        157,
        181,
        215,
        143,
        144,
        208,
        182,
        155,
        208,
        141,
        177,
        225,
        157,
        151,
        224,
        176,
        151,
        228,
        163,
        161,
        232,
        172,
        139,
        225,
        171,
        132,
        255,
        161,
        185,
        249,
        139,
        131,
        245,
        166,
        181,
        245,
        158,
        161,
        254,
        149,
        146,
        143,
        227,
        216,
        128,
        250,
        246,
        132,
        194,
        205,
        140,
        220,
        251,
        70,
        74,
        63,
        56,
        9,
        30,
        20,
        28,
        29,
        11,
        92,
        153,
        200,
        201,
        149,
        224,
        238,
        159,
        213,
        221,
        100,
        174,
        239,
        218,
        171,
        223,
        193,
        97,
        166,
        251,
        202,
        167,
        218,
        253,
        160,
        255,
        224,
        190,
        201,
        193,
        182,
        254,
        244,
        115,
        181,
        202,
        235,
        180,
        235,
        194,
        177,
        250,
        44,
        76,
        8,
        40,
        142,
        201,
        222,
        194,
        207,
        131,
        68,
        25,
        44,
        65,
        56,
        31,
        94,
        1,
        2,
        92,
        47,
        39,
        84,
        28,
        26,
        159,
        84,
        11,
        12,
        82,
        37,
        45,
        98,
        42,
        32,
        169,
        249,
        231,
        233,
        255,
        231,
        163,
        100,
        57,
        12,
        97,
        24,
        63,
        126,
        33,
        34,
        124,
        15,
        7,
        116,
        60,
        58,
        189,
        119,
        5,
        62,
        113,
        28,
        34,
        15,
        123,
        101,
        212,
        187,
        188,
        169,
        191,
        194,
        130,
        142,
        133,
        198,
        3,
        92,
        111,
        28,
        103,
        66,
        29,
        68,
        69,
        25,
        108,
        106,
        27,
        81,
        89,
        216,
        19,
        78,
        79,
        47,
        90,
        80,
        128,
        138,
        242,
        40,
        119,
        120,
        38,
        81,
        89,
        46,
        102,
        108,
        235,
        179,
        191,
        63,
        64,
        101,
        58,
        97,
        72,
        55,
        124,
        86,
        54,
        118,
        86,
        51,
        65,
        130,
        205,
        160,
        158,
        203,
        191,
        161,
        200,
        141,
        165,
        199,
        129,
        167,
        195,
        156,
        168,
        220,
        150,
        155,
        222,
        159,
        145,
        103,
        16,
        126,
        90,
        82,
        67,
        87,
        69,
        77,
        208,
        182,
        137,
        237,
        177,
        182,
        224,
        176,
        151,
        47,
        94,
        229,
        174,
        128,
        224,
        164,
        132,
        252,
        187,
        185,
        240,
        180,
        147,
        248,
        165,
        159,
        246,
        156,
        168,
        255,
        146,
        153,
        61,
        16,
        7,
        0,
        0,
        29,
        70,
        132,
        195,
        198,
        139,
        207,
        224,
        131,
        195,
        213,
        141,
        206,
        222,
        35,
        36,
        83,
        51,
        21,
        31,
        0,
        18,
        2,
        8,
        147,
        203,
        246,
        144,
        242,
        243,
        167,
        245,
        212,
        168,
        213,
        246,
        167,
        254,
        213,
        169,
        232,
        227,
        172,
        234,
        219,
        190,
        252,
        232,
        182,
        235,
        249,
        46,
        55,
        63,
        53,
        50,
        34,
        119,
        179,
        193,
        2,
        77,
        32,
        30,
        75,
        63,
        33,
        141,
        196,
        209,
        207,
        204,
        134,
        64,
        48,
        13,
        92,
        51,
        15,
        92,
        54,
        20,
        89,
        45,
        63,
        147,
        199,
        217,
        211,
        197,
        209,
        149,
        111,
        3,
        35,
        108,
        30,
        2,
        177,
        183,
        210,
        178,
        100,
        58,
        8,
        221,
        200,
        205,
        211,
        200,
        125,
        35,
        13,
        121,
        3,
        56,
        122,
        60,
        50,
        116,
        56,
        9,
        125,
        12,
        111,
        3,
        92,
        79,
        11,
        96,
        91,
        11,
        125,
        70,
        8,
        78,
        68,
        189,
        168,
        173,
        179,
        168,
        29,
        67,
        109,
        24,
        102,
        121,
        208,
        20,
        100,
        89,
        16,
        127,
        67,
        16,
        66,
        96,
        45,
        89,
        67,
        237,
        36,
        108,
        106,
        38,
        71,
        123,
        47,
        83,
        93,
        42,
        102,
        87,
        61,
        105,
        88,
        58,
        99,
        96,
        53,
        70,
        117,
        52,
        94,
        124,
        49,
        69,
        167,
        200,
        168,
        171,
        202,
        144,
        130,
        203,
        143,
        128,
        207,
        157,
        188,
        195,
        156,
        175,
        220,
        167,
        130,
        221,
        132,
        133,
        217,
        172,
        170,
        219,
        145,
        153,
        109,
        106,
        211,
        172,
        177,
        238,
        181,
        156,
        235,
        160,
        138,
        234,
        162,
        130,
        229,
        165,
        183,
        239,
        176,
        160,
        252,
        147,
        136,
        252,
        148,
        128,
        249,
        152,
        174,
        246,
        190,
        168,
        242,
        168,
        186,
        243,
        199,
        200,
        141,
        198,
        232,
        136,
        204,
        236,
        135,
        223,
        226,
        132,
        222,
        223,
        63,
        56,
        87,
        158,
        255,
        195,
        151,
        235,
        229,
        146,
        206,
        233,
        151,
        223,
        215,
        144,
        228,
        243,
        162,
        255,
        238,
        172,
        193,
        248,
        164,
        227,
        252,
        164,
        253,
        239,
        162,
        255,
        201,
        163,
        247,
        248,
        191,
        248,
        240,
        114,
        109,
        109,
        99,
        100,
        181,
        214,
        236,
        190,
        192,
        204,
        238,
        202,
        220,
        200,
        142,
        252,
        195,
        216,
        208,
        192,
        197,
        156,
        221,
        151,
        217,
        158,
        234,
        212,
        215,
        213,
        215,
        209,
        219,
        128,
        198,
        193,
        197,
        212,
        141,
        226,
        199,
        208,
        248,
        171,
        193,
        205,
        179,
        244,
        189,
        240,
        185,
        211,
        225,
        242,
        245,
        208,
        235,
        247,
        254,
        166,
        186,
        226,
        172,
        226,
        190,
        166,
        194,
        246,
        226,
        226,
        255,
        228,
        224,
        181,
        185,
        142,
        139,
        156,
        156,
        134,
        152,
        148,
        194,
        170,
        142,
        135,
        137,
        218,
        162,
        132,
        150,
        136,
        157,
        194,
        178,
        144,
        155,
        148,
        156,
        220,
        165,
        130,
        147,
        133,
        219,
        161,
        165,
        160,
        173,
        167,
        241,
        172,
        163,
        169,
        167,
        254,
        40,
        79,
        113,
        34,
        75,
        83,
        61,
        66,
        99,
        60,
        99,
        74,
        52,
        105,
        116,
        54,
        95,
        102,
        51,
        80,
        110,
        60,
        190,
        178,
        207,
        176,
        149,
        202,
        145,
        184,
        198,
        153,
        154,
        196,
        183,
        191,
        193,
        169,
        129,
        223,
        135,
        164,
        217,
        131,
        170,
        218,
        146,
        178,
        69,
        66,
        83,
        69,
        71,
        70,
        101,
        84,
        124,
        102,
        101,
        106,
        98,
        94,
        79,
        48,
        67,
        66,
        51,
        48,
        52,
        64,
        92,
        46,
        43,
        44,
        46,
        44,
        94,
        88,
        83,
        38,
        36,
        41,
        32,
        33,
        32,
        32,
        93,
        45,
        43,
        40,
        88,
        89,
        85,
        85,
        80,
        87,
        80,
        85,
        82,
        4,
        86,
        84,
        27,
        77,
        29,
        79,
        28,
        78,
        73,
        73,
        74,
        75,
        67,
        21,
        67,
        21,
        16,
        19,
        120,
        46,
        127,
        113,
        123,
        46,
        120,
        122,
        122,
        114,
        118,
        121,
        36,
        117,
        37,
        36,
        98,
        111,
        59,
        107,
        61,
        105,
        61,
        100,
        107,
        107,
        52,
        104,
        50,
        111,
        100,
        96,
        153,
        156,
        153,
        157,
        157,
        205,
        148,
        207,
        198,
        151,
        196,
        145,
        194,
        159,
        197,
        148,
        143,
        221,
        143,
        220,
        138,
        138,
        133,
        223,
        130,
        209,
        131,
        136,
        134,
        130,
        131,
        135,
        232,
        184,
        176,
        237,
        186,
        238,
        237,
        185,
        186,
        225,
        182,
        180,
        190,
        225,
        179,
        178,
        173,
        175,
        170,
        250,
        174,
        166,
        170,
        168,
        243,
        165,
        245,
        247,
        244,
        174,
        240,
        243,
        139,
        211,
        137,
        216,
        216,
        214,
        220,
        222,
        219,
        219,
        210,
        210
    };

        internal static string[] A5 = new string[272];

        private static string A6(int P_0, int P_1, int P_2)
        {
            string @string = Encoding.UTF8.GetString(SQ.A4, P_1, P_2);
            SQ.A5[P_0] = @string;
            return @string;
        }

        public static string A()
        {
            return SQ.A5[0] ?? SQ.A6(0, 0, 162);
        }

        public static string a()
        {
            return SQ.A5[1] ?? SQ.A6(1, 162, 2);
        }

        public static string B()
        {
            return SQ.A5[2] ?? SQ.A6(2, 164, 64);
        }

        public static string b()
        {
            return SQ.A5[3] ?? SQ.A6(3, 228, 64);
        }

        public static string C()
        {
            return SQ.A5[4] ?? SQ.A6(4, 292, 64);
        }

        public static string c()
        {
            return SQ.A5[5] ?? SQ.A6(5, 356, 64);
        }

        public static string D()
        {
            return SQ.A5[6] ?? SQ.A6(6, 420, 64);
        }

        public static string d()
        {
            return SQ.A5[7] ?? SQ.A6(7, 484, 64);
        }

        public static string E()
        {
            return SQ.A5[8] ?? SQ.A6(8, 548, 0);
        }

        public static string e()
        {
            return SQ.A5[9] ?? SQ.A6(9, 548, 1);
        }

        public static string F()
        {
            return SQ.A5[10] ?? SQ.A6(10, 549, 9);
        }

        public static string f()
        {
            return SQ.A5[11] ?? SQ.A6(11, 558, 6);
        }

        public static string G()
        {
            return SQ.A5[12] ?? SQ.A6(12, 564, 24);
        }

        public static string g()
        {
            return SQ.A5[13] ?? SQ.A6(13, 588, 51);
        }

        public static string H()
        {
            return SQ.A5[14] ?? SQ.A6(14, 639, 6);
        }

        public static string h()
        {
            return SQ.A5[15] ?? SQ.A6(15, 645, 64);
        }

        public static string I()
        {
            return SQ.A5[16] ?? SQ.A6(16, 709, 24);
        }

        public static string i()
        {
            return SQ.A5[17] ?? SQ.A6(17, 733, 24);
        }

        public static string J()
        {
            return SQ.A5[18] ?? SQ.A6(18, 757, 66);
        }

        public static string j()
        {
            return SQ.A5[19] ?? SQ.A6(19, 823, 130);
        }

        public static string K()
        {
            return SQ.A5[20] ?? SQ.A6(20, 953, 21);
        }

        public static string k()
        {
            return SQ.A5[21] ?? SQ.A6(21, 974, 21);
        }

        public static string L()
        {
            return SQ.A5[22] ?? SQ.A6(22, 995, 1);
        }

        public static string l()
        {
            return SQ.A5[23] ?? SQ.A6(23, 996, 1);
        }

        public static string M()
        {
            return SQ.A5[24] ?? SQ.A6(24, 997, 4);
        }

        public static string m()
        {
            return SQ.A5[25] ?? SQ.A6(25, 1001, 42);
        }

        public static string N()
        {
            return SQ.A5[26] ?? SQ.A6(26, 1043, 7);
        }

        public static string n()
        {
            return SQ.A5[27] ?? SQ.A6(27, 1050, 30);
        }

        public static string O()
        {
            return SQ.A5[28] ?? SQ.A6(28, 1080, 3);
        }

        public static string o()
        {
            return SQ.A5[29] ?? SQ.A6(29, 1083, 33);
        }

        public static string P()
        {
            return SQ.A5[30] ?? SQ.A6(30, 1116, 26);
        }

        public static string p()
        {
            return SQ.A5[31] ?? SQ.A6(31, 1142, 28);
        }

        public static string Q()
        {
            return SQ.A5[32] ?? SQ.A6(32, 1170, 85);
        }

        public static string q()
        {
            return SQ.A5[33] ?? SQ.A6(33, 1255, 74);
        }

        public static string R()
        {
            return SQ.A5[34] ?? SQ.A6(34, 1329, 11);
        }

        public static string r()
        {
            return SQ.A5[35] ?? SQ.A6(35, 1340, 16);
        }

        public static string S()
        {
            return SQ.A5[36] ?? SQ.A6(36, 1356, 1);
        }

        public static string s()
        {
            return SQ.A5[37] ?? SQ.A6(37, 1357, 1);
        }

        public static string T()
        {
            return SQ.A5[38] ?? SQ.A6(38, 1358, 1);
        }

        public static string t()
        {
            return SQ.A5[39] ?? SQ.A6(39, 1359, 1);
        }

        public static string U()
        {
            return SQ.A5[40] ?? SQ.A6(40, 1360, 1);
        }

        public static string u()
        {
            return SQ.A5[41] ?? SQ.A6(41, 1361, 1);
        }

        public static string V()
        {
            return SQ.A5[42] ?? SQ.A6(42, 1362, 1);
        }

        public static string v()
        {
            return SQ.A5[43] ?? SQ.A6(43, 1363, 1);
        }

        public static string W()
        {
            return SQ.A5[44] ?? SQ.A6(44, 1364, 1);
        }

        public static string w()
        {
            return SQ.A5[45] ?? SQ.A6(45, 1365, 1);
        }

        public static string X()
        {
            return SQ.A5[46] ?? SQ.A6(46, 1366, 1);
        }

        public static string x()
        {
            return SQ.A5[47] ?? SQ.A6(47, 1367, 1);
        }

        public static string Y()
        {
            return SQ.A5[48] ?? SQ.A6(48, 1368, 1);
        }

        public static string y()
        {
            return SQ.A5[49] ?? SQ.A6(49, 1369, 1);
        }

        public static string Z()
        {
            return SQ.A5[50] ?? SQ.A6(50, 1370, 1);
        }

        public static string z()
        {
            return SQ.A5[51] ?? SQ.A6(51, 1371, 1);
        }

        public static string aA()
        {
            return SQ.A5[52] ?? SQ.A6(52, 1372, 1);
        }

        public static string aa()
        {
            return SQ.A5[53] ?? SQ.A6(53, 1373, 1);
        }

        public static string aB()
        {
            return SQ.A5[54] ?? SQ.A6(54, 1374, 1);
        }

        public static string ab()
        {
            return SQ.A5[55] ?? SQ.A6(55, 1375, 1);
        }

        public static string aC()
        {
            return SQ.A5[56] ?? SQ.A6(56, 1376, 1);
        }

        public static string ac()
        {
            return SQ.A5[57] ?? SQ.A6(57, 1377, 1);
        }

        public static string aD()
        {
            return SQ.A5[58] ?? SQ.A6(58, 1378, 1);
        }

        public static string ad()
        {
            return SQ.A5[59] ?? SQ.A6(59, 1379, 1);
        }

        public static string aE()
        {
            return SQ.A5[60] ?? SQ.A6(60, 1380, 1);
        }

        public static string ae()
        {
            return SQ.A5[61] ?? SQ.A6(61, 1381, 1);
        }

        public static string aF()
        {
            return SQ.A5[62] ?? SQ.A6(62, 1382, 1);
        }

        public static string af()
        {
            return SQ.A5[63] ?? SQ.A6(63, 1383, 1);
        }

        public static string aG()
        {
            return SQ.A5[64] ?? SQ.A6(64, 1384, 1);
        }

        public static string ag()
        {
            return SQ.A5[65] ?? SQ.A6(65, 1385, 1);
        }

        public static string aH()
        {
            return SQ.A5[66] ?? SQ.A6(66, 1386, 1);
        }

        public static string ah()
        {
            return SQ.A5[67] ?? SQ.A6(67, 1387, 1);
        }

        public static string aI()
        {
            return SQ.A5[68] ?? SQ.A6(68, 1388, 1);
        }

        public static string ai()
        {
            return SQ.A5[69] ?? SQ.A6(69, 1389, 1);
        }

        public static string aJ()
        {
            return SQ.A5[70] ?? SQ.A6(70, 1390, 1);
        }

        public static string aj()
        {
            return SQ.A5[71] ?? SQ.A6(71, 1391, 1);
        }

        public static string aK()
        {
            return SQ.A5[72] ?? SQ.A6(72, 1392, 1);
        }

        public static string ak()
        {
            return SQ.A5[73] ?? SQ.A6(73, 1393, 5);
        }

        public static string aL()
        {
            return SQ.A5[74] ?? SQ.A6(74, 1398, 3);
        }

        public static string al()
        {
            return SQ.A5[75] ?? SQ.A6(75, 1401, 3);
        }

        public static string aM()
        {
            return SQ.A5[76] ?? SQ.A6(76, 1404, 6);
        }

        public static string am()
        {
            return SQ.A5[77] ?? SQ.A6(77, 1410, 179);
        }

        public static string aN()
        {
            return SQ.A5[78] ?? SQ.A6(78, 1589, 5);
        }

        public static string an()
        {
            return SQ.A5[79] ?? SQ.A6(79, 1594, 7);
        }

        public static string aO()
        {
            return SQ.A5[80] ?? SQ.A6(80, 1601, 5);
        }

        public static string ao()
        {
            return SQ.A5[81] ?? SQ.A6(81, 1606, 6);
        }

        public static string aP()
        {
            return SQ.A5[82] ?? SQ.A6(82, 1612, 7);
        }

        public static string ap()
        {
            return SQ.A5[83] ?? SQ.A6(83, 1619, 5);
        }

        public static string aQ()
        {
            return SQ.A5[84] ?? SQ.A6(84, 1624, 9);
        }

        public static string aq()
        {
            return SQ.A5[85] ?? SQ.A6(85, 1633, 4);
        }

        public static string aR()
        {
            return SQ.A5[86] ?? SQ.A6(86, 1637, 15);
        }

        public static string ar()
        {
            return SQ.A5[87] ?? SQ.A6(87, 1652, 13);
        }

        public static string aS()
        {
            return SQ.A5[88] ?? SQ.A6(88, 1665, 8);
        }

        public static string @as()
        {
            return SQ.A5[89] ?? SQ.A6(89, 1673, 7);
        }

        public static string aT()
        {
            return SQ.A5[90] ?? SQ.A6(90, 1680, 7);
        }

        public static string at()
        {
            return SQ.A5[91] ?? SQ.A6(91, 1687, 8);
        }

        public static string aU()
        {
            return SQ.A5[92] ?? SQ.A6(92, 1695, 8);
        }

        public static string au()
        {
            return SQ.A5[93] ?? SQ.A6(93, 1703, 12);
        }

        public static string aV()
        {
            return SQ.A5[94] ?? SQ.A6(94, 1715, 11);
        }

        public static string av()
        {
            return SQ.A5[95] ?? SQ.A6(95, 1726, 12);
        }

        public static string aW()
        {
            return SQ.A5[96] ?? SQ.A6(96, 1738, 7);
        }

        public static string aw()
        {
            return SQ.A5[97] ?? SQ.A6(97, 1745, 6);
        }

        public static string aX()
        {
            return SQ.A5[98] ?? SQ.A6(98, 1751, 33);
        }

        public static string ax()
        {
            return SQ.A5[99] ?? SQ.A6(99, 1784, 42);
        }

        public static string aY()
        {
            return SQ.A5[100] ?? SQ.A6(100, 1826, 18);
        }

        public static string ay()
        {
            return SQ.A5[101] ?? SQ.A6(101, 1844, 21);
        }

        public static string aZ()
        {
            return SQ.A5[102] ?? SQ.A6(102, 1865, 9);
        }

        public static string az()
        {
            return SQ.A5[103] ?? SQ.A6(103, 1874, 5);
        }

        public static string BA()
        {
            return SQ.A5[104] ?? SQ.A6(104, 1879, 6);
        }

        public static string Ba()
        {
            return SQ.A5[105] ?? SQ.A6(105, 1885, 9);
        }

        public static string BB()
        {
            return SQ.A5[106] ?? SQ.A6(106, 1894, 6);
        }

        public static string Bb()
        {
            return SQ.A5[107] ?? SQ.A6(107, 1900, 11);
        }

        public static string BC()
        {
            return SQ.A5[108] ?? SQ.A6(108, 1911, 12);
        }

        public static string Bc()
        {
            return SQ.A5[109] ?? SQ.A6(109, 1923, 27);
        }

        public static string BD()
        {
            return SQ.A5[110] ?? SQ.A6(110, 1950, 3);
        }

        public static string Bd()
        {
            return SQ.A5[111] ?? SQ.A6(111, 1953, 3);
        }

        public static string BE()
        {
            return SQ.A5[112] ?? SQ.A6(112, 1956, 33);
        }

        public static string Be()
        {
            return SQ.A5[113] ?? SQ.A6(113, 1989, 2);
        }

        public static string BF()
        {
            return SQ.A5[114] ?? SQ.A6(114, 1991, 54);
        }

        public static string Bf()
        {
            return SQ.A5[115] ?? SQ.A6(115, 2045, 34);
        }

        public static string BG()
        {
            return SQ.A5[116] ?? SQ.A6(116, 2079, 37);
        }

        public static string Bg()
        {
            return SQ.A5[117] ?? SQ.A6(117, 2116, 35);
        }

        public static string BH()
        {
            return SQ.A5[118] ?? SQ.A6(118, 2151, 19);
        }

        public static string Bh()
        {
            return SQ.A5[119] ?? SQ.A6(119, 2170, 64);
        }

        public static string BI()
        {
            return SQ.A5[120] ?? SQ.A6(120, 2234, 83);
        }

        public static string Bi()
        {
            return SQ.A5[121] ?? SQ.A6(121, 2317, 96);
        }

        public static string BJ()
        {
            return SQ.A5[122] ?? SQ.A6(122, 2413, 21);
        }

        public static string Bj()
        {
            return SQ.A5[123] ?? SQ.A6(123, 2434, 11);
        }

        public static string BK()
        {
            return SQ.A5[124] ?? SQ.A6(124, 2445, 34);
        }

        public static string Bk()
        {
            return SQ.A5[125] ?? SQ.A6(125, 2479, 122);
        }

        public static string BL()
        {
            return SQ.A5[126] ?? SQ.A6(126, 2601, 75);
        }

        public static string Bl()
        {
            return SQ.A5[127] ?? SQ.A6(127, 2676, 34);
        }

        public static string BM()
        {
            return SQ.A5[128] ?? SQ.A6(128, 2710, 4);
        }

        public static string Bm()
        {
            return SQ.A5[129] ?? SQ.A6(129, 2714, 36);
        }

        public static string BN()
        {
            return SQ.A5[130] ?? SQ.A6(130, 2750, 9);
        }

        public static string Bn()
        {
            return SQ.A5[131] ?? SQ.A6(131, 2759, 59);
        }

        public static string BO()
        {
            return SQ.A5[132] ?? SQ.A6(132, 2818, 3);
        }

        public static string Bo()
        {
            return SQ.A5[133] ?? SQ.A6(133, 2821, 49);
        }

        public static string BP()
        {
            return SQ.A5[134] ?? SQ.A6(134, 2870, 7);
        }

        public static string Bp()
        {
            return SQ.A5[135] ?? SQ.A6(135, 2877, 4);
        }

        public static string BQ()
        {
            return SQ.A5[136] ?? SQ.A6(136, 2881, 5);
        }

        public static string Bq()
        {
            return SQ.A5[137] ?? SQ.A6(137, 2886, 24);
        }

        public static string BR()
        {
            return SQ.A5[138] ?? SQ.A6(138, 2910, 2);
        }

        public static string Br()
        {
            return SQ.A5[139] ?? SQ.A6(139, 2912, 7);
        }

        public static string BS()
        {
            return SQ.A5[140] ?? SQ.A6(140, 2919, 4);
        }

        public static string Bs()
        {
            return SQ.A5[141] ?? SQ.A6(141, 2923, 14);
        }

        public static string BT()
        {
            return SQ.A5[142] ?? SQ.A6(142, 2937, 6);
        }

        public static string Bt()
        {
            return SQ.A5[143] ?? SQ.A6(143, 2943, 5);
        }

        public static string BU()
        {
            return SQ.A5[144] ?? SQ.A6(144, 2948, 5);
        }

        public static string Bu()
        {
            return SQ.A5[145] ?? SQ.A6(145, 2953, 7);
        }

        public static string BV()
        {
            return SQ.A5[146] ?? SQ.A6(146, 2960, 8);
        }

        public static string Bv()
        {
            return SQ.A5[147] ?? SQ.A6(147, 2968, 4);
        }

        public static string BW()
        {
            return SQ.A5[148] ?? SQ.A6(148, 2972, 6);
        }

        public static string Bw()
        {
            return SQ.A5[149] ?? SQ.A6(149, 2978, 15);
        }

        public static string BX()
        {
            return SQ.A5[150] ?? SQ.A6(150, 2993, 22);
        }

        public static string Bx()
        {
            return SQ.A5[151] ?? SQ.A6(151, 3015, 392);
        }

        public static string BY()
        {
            return SQ.A5[152] ?? SQ.A6(152, 3407, 3);
        }

        public static string By()
        {
            return SQ.A5[153] ?? SQ.A6(153, 3410, 3);
        }

        public static string BZ()
        {
            return SQ.A5[154] ?? SQ.A6(154, 3413, 3);
        }

        public static string Bz()
        {
            return SQ.A5[155] ?? SQ.A6(155, 3416, 3);
        }

        public static string bA()
        {
            return SQ.A5[156] ?? SQ.A6(156, 3419, 3);
        }

        public static string ba()
        {
            return SQ.A5[157] ?? SQ.A6(157, 3422, 11);
        }

        public static string bB()
        {
            return SQ.A5[158] ?? SQ.A6(158, 3433, 8);
        }

        public static string bb()
        {
            return SQ.A5[159] ?? SQ.A6(159, 3441, 8);
        }

        public static string bC()
        {
            return SQ.A5[160] ?? SQ.A6(160, 3449, 2);
        }

        public static string bc()
        {
            return SQ.A5[161] ?? SQ.A6(161, 3451, 7);
        }

        public static string bD()
        {
            return SQ.A5[162] ?? SQ.A6(162, 3458, 30);
        }

        public static string bd()
        {
            return SQ.A5[163] ?? SQ.A6(163, 3488, 3);
        }

        public static string bE()
        {
            return SQ.A5[164] ?? SQ.A6(164, 3491, 4);
        }

        public static string be()
        {
            return SQ.A5[165] ?? SQ.A6(165, 3495, 7);
        }

        public static string bF()
        {
            return SQ.A5[166] ?? SQ.A6(166, 3502, 2);
        }

        public static string bf()
        {
            return SQ.A5[167] ?? SQ.A6(167, 3504, 6);
        }

        public static string bG()
        {
            return SQ.A5[168] ?? SQ.A6(168, 3510, 18);
        }

        public static string bg()
        {
            return SQ.A5[169] ?? SQ.A6(169, 3528, 6);
        }

        public static string bH()
        {
            return SQ.A5[170] ?? SQ.A6(170, 3534, 3);
        }

        public static string bh()
        {
            return SQ.A5[171] ?? SQ.A6(171, 3537, 9);
        }

        public static string bI()
        {
            return SQ.A5[172] ?? SQ.A6(172, 3546, 18);
        }

        public static string bi()
        {
            return SQ.A5[173] ?? SQ.A6(173, 3564, 27);
        }

        public static string bJ()
        {
            return SQ.A5[174] ?? SQ.A6(174, 3591, 5);
        }

        public static string bj()
        {
            return SQ.A5[175] ?? SQ.A6(175, 3596, 3);
        }

        public static string bK()
        {
            return SQ.A5[176] ?? SQ.A6(176, 3599, 5);
        }

        public static string bk()
        {
            return SQ.A5[177] ?? SQ.A6(177, 3604, 41);
        }

        public static string bL()
        {
            return SQ.A5[178] ?? SQ.A6(178, 3645, 35);
        }

        public static string bl()
        {
            return SQ.A5[179] ?? SQ.A6(179, 3680, 19);
        }

        public static string bM()
        {
            return SQ.A5[180] ?? SQ.A6(180, 3699, 12);
        }

        public static string bm()
        {
            return SQ.A5[181] ?? SQ.A6(181, 3711, 18);
        }

        public static string bN()
        {
            return SQ.A5[182] ?? SQ.A6(182, 3729, 30);
        }

        public static string bn()
        {
            return SQ.A5[183] ?? SQ.A6(183, 3759, 101);
        }

        public static string bO()
        {
            return SQ.A5[184] ?? SQ.A6(184, 3860, 11);
        }

        public static string bo()
        {
            return SQ.A5[185] ?? SQ.A6(185, 3871, 68);
        }

        public static string bP()
        {
            return SQ.A5[186] ?? SQ.A6(186, 3939, 29);
        }

        public static string bp()
        {
            return SQ.A5[187] ?? SQ.A6(187, 3968, 39);
        }

        public static string bQ()
        {
            return SQ.A5[188] ?? SQ.A6(188, 4007, 6);
        }

        public static string bq()
        {
            return SQ.A5[189] ?? SQ.A6(189, 4013, 9);
        }

        public static string bR()
        {
            return SQ.A5[190] ?? SQ.A6(190, 4022, 6);
        }

        public static string br()
        {
            return SQ.A5[191] ?? SQ.A6(191, 4028, 18);
        }

        public static string bS()
        {
            return SQ.A5[192] ?? SQ.A6(192, 4046, 199);
        }

        public static string bs()
        {
            return SQ.A5[193] ?? SQ.A6(193, 4245, 3);
        }

        public static string bT()
        {
            return SQ.A5[194] ?? SQ.A6(194, 4248, 29);
        }

        public static string bt()
        {
            return SQ.A5[195] ?? SQ.A6(195, 4277, 4);
        }

        public static string bU()
        {
            return SQ.A5[196] ?? SQ.A6(196, 4281, 18);
        }

        public static string bu()
        {
            return SQ.A5[197] ?? SQ.A6(197, 4299, 18);
        }

        public static string bV()
        {
            return SQ.A5[198] ?? SQ.A6(198, 4317, 21);
        }

        public static string bv()
        {
            return SQ.A5[199] ?? SQ.A6(199, 4338, 15);
        }

        public static string bW()
        {
            return SQ.A5[200] ?? SQ.A6(200, 4353, 9);
        }

        public static string bw()
        {
            return SQ.A5[201] ?? SQ.A6(201, 4362, 57);
        }

        public static string bX()
        {
            return SQ.A5[202] ?? SQ.A6(202, 4419, 3);
        }

        public static string bx()
        {
            return SQ.A5[203] ?? SQ.A6(203, 4422, 21);
        }

        public static string bY()
        {
            return SQ.A5[204] ?? SQ.A6(204, 4443, 36);
        }

        public static string by()
        {
            return SQ.A5[205] ?? SQ.A6(205, 4479, 27);
        }

        public static string bZ()
        {
            return SQ.A5[206] ?? SQ.A6(206, 4506, 63);
        }

        public static string bz()
        {
            return SQ.A5[207] ?? SQ.A6(207, 4569, 27);
        }

        public static string CA()
        {
            return SQ.A5[208] ?? SQ.A6(208, 4596, 36);
        }

        public static string Ca()
        {
            return SQ.A5[209] ?? SQ.A6(209, 4632, 43);
        }

        public static string CB()
        {
            return SQ.A5[210] ?? SQ.A6(210, 4675, 46);
        }

        public static string Cb()
        {
            return SQ.A5[211] ?? SQ.A6(211, 4721, 3);
        }

        public static string CC()
        {
            return SQ.A5[212] ?? SQ.A6(212, 4724, 10);
        }

        public static string Cc()
        {
            return SQ.A5[213] ?? SQ.A6(213, 4734, 10);
        }

        public static string CD()
        {
            return SQ.A5[214] ?? SQ.A6(214, 4744, 7);
        }

        public static string Cd()
        {
            return SQ.A5[215] ?? SQ.A6(215, 4751, 8);
        }

        public static string CE()
        {
            return SQ.A5[216] ?? SQ.A6(216, 4759, 4);
        }

        public static string Ce()
        {
            return SQ.A5[217] ?? SQ.A6(217, 4763, 4);
        }

        public static string CF()
        {
            return SQ.A5[218] ?? SQ.A6(218, 4767, 12);
        }

        public static string Cf()
        {
            return SQ.A5[219] ?? SQ.A6(219, 4779, 19);
        }

        public static string CG()
        {
            return SQ.A5[220] ?? SQ.A6(220, 4798, 124);
        }

        public static string Cg()
        {
            return SQ.A5[221] ?? SQ.A6(221, 4922, 43);
        }

        public static string CH()
        {
            return SQ.A5[222] ?? SQ.A6(222, 4965, 100);
        }

        public static string Ch()
        {
            return SQ.A5[223] ?? SQ.A6(223, 5065, 100);
        }

        public static string CI()
        {
            return SQ.A5[224] ?? SQ.A6(224, 5165, 4);
        }

        public static string Ci()
        {
            return SQ.A5[225] ?? SQ.A6(225, 5169, 1);
        }

        public static string CJ()
        {
            return SQ.A5[226] ?? SQ.A6(226, 5170, 37);
        }

        public static string Cj()
        {
            return SQ.A5[227] ?? SQ.A6(227, 5207, 43);
        }

        public static string CK()
        {
            return SQ.A5[228] ?? SQ.A6(228, 5250, 93);
        }

        public static string Ck()
        {
            return SQ.A5[229] ?? SQ.A6(229, 5343, 28);
        }

        public static string CL()
        {
            return SQ.A5[230] ?? SQ.A6(230, 5371, 34);
        }

        public static string Cl()
        {
            return SQ.A5[231] ?? SQ.A6(231, 5405, 39);
        }

        public static string CM()
        {
            return SQ.A5[232] ?? SQ.A6(232, 5444, 114);
        }

        public static string Cm()
        {
            return SQ.A5[233] ?? SQ.A6(233, 5558, 4);
        }

        public static string CN()
        {
            return SQ.A5[234] ?? SQ.A6(234, 5562, 48);
        }

        public static string Cn()
        {
            return SQ.A5[235] ?? SQ.A6(235, 5610, 3);
        }

        public static string CO()
        {
            return SQ.A5[236] ?? SQ.A6(236, 5613, 45);
        }

        public static string Co()
        {
            return SQ.A5[237] ?? SQ.A6(237, 5658, 73);
        }

        public static string CP()
        {
            return SQ.A5[238] ?? SQ.A6(238, 5731, 69);
        }

        public static string Cp()
        {
            return SQ.A5[239] ?? SQ.A6(239, 5800, 36);
        }

        public static string CQ()
        {
            return SQ.A5[240] ?? SQ.A6(240, 5836, 3);
        }

        public static string Cq()
        {
            return SQ.A5[241] ?? SQ.A6(241, 5839, 1);
        }

        public static string CR()
        {
            return SQ.A5[242] ?? SQ.A6(242, 5840, 162);
        }

        public static string Cr()
        {
            return SQ.A5[243] ?? SQ.A6(243, 6002, 12);
        }

        public static string CS()
        {
            return SQ.A5[244] ?? SQ.A6(244, 6014, 25);
        }

        public static string Cs()
        {
            return SQ.A5[245] ?? SQ.A6(245, 6039, 17);
        }

        public static string CT()
        {
            return SQ.A5[246] ?? SQ.A6(246, 6056, 1);
        }

        public static string Ct()
        {
            return SQ.A5[247] ?? SQ.A6(247, 6057, 29);
        }

        public static string CU()
        {
            return SQ.A5[248] ?? SQ.A6(248, 6086, 13);
        }

        public static string Cu()
        {
            return SQ.A5[249] ?? SQ.A6(249, 6099, 18);
        }

        public static string CV()
        {
            return SQ.A5[250] ?? SQ.A6(250, 6117, 18);
        }

        public static string Cv()
        {
            return SQ.A5[251] ?? SQ.A6(251, 6135, 51);
        }

        public static string CW()
        {
            return SQ.A5[252] ?? SQ.A6(252, 6186, 29);
        }

        public static string Cw()
        {
            return SQ.A5[253] ?? SQ.A6(253, 6215, 61);
        }

        public static string CX()
        {
            return SQ.A5[254] ?? SQ.A6(254, 6276, 25);
        }

        public static string Cx()
        {
            return SQ.A5[255] ?? SQ.A6(255, 6301, 13);
        }

        public static string CY()
        {
            return SQ.A5[256] ?? SQ.A6(256, 6314, 6);
        }

        public static string Cy()
        {
            return SQ.A5[257] ?? SQ.A6(257, 6320, 6);
        }

        public static string CZ()
        {
            return SQ.A5[258] ?? SQ.A6(258, 6326, 25);
        }

        public static string Cz()
        {
            return SQ.A5[259] ?? SQ.A6(259, 6351, 2);
        }

        public static string cA()
        {
            return SQ.A5[260] ?? SQ.A6(260, 6353, 41);
        }

        public static string ca()
        {
            return SQ.A5[261] ?? SQ.A6(261, 6394, 6);
        }

        public static string cB()
        {
            return SQ.A5[262] ?? SQ.A6(262, 6400, 84);
        }

        public static string cb()
        {
            return SQ.A5[263] ?? SQ.A6(263, 6484, 5);
        }

        public static string cC()
        {
            return SQ.A5[264] ?? SQ.A6(264, 6489, 17);
        }

        public static string cc()
        {
            return SQ.A5[265] ?? SQ.A6(265, 6506, 24);
        }

        public static string cD()
        {
            return SQ.A5[266] ?? SQ.A6(266, 6530, 24);
        }

        public static string cd()
        {
            return SQ.A5[267] ?? SQ.A6(267, 6554, 4);
        }

        public static string cE()
        {
            return SQ.A5[268] ?? SQ.A6(268, 6558, 9);
        }

        public static string ce()
        {
            return SQ.A5[269] ?? SQ.A6(269, 6567, 3);
        }

        public static string cF()
        {
            return SQ.A5[270] ?? SQ.A6(270, 6570, 32);
        }

        public static string cf()
        {
            return SQ.A5[271] ?? SQ.A6(271, 6602, 130);
        }

        static SQ()
        {
            for (int i = 0; i < SQ.A4.Length; i++)
            {
                SQ.A4[i] = (byte)(SQ.A4[i] ^ i ^ 0xAA);
            }
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    public class _S
    {
        private static byte[] IP;

        private static byte[] IP_1;

        private static byte[] E;

        private static byte[] p;

        private static byte[,] s1;

        private static byte[,] s2;

        private static byte[,] s3;

        private static byte[,] S4;

        private static byte[,] S5;

        private static byte[,] S6;

        private static byte[,] S7;

        private static byte[,] S8;

        private static byte[] PC_1;

        private static byte[] PC_2;

        private static byte[] Lsi;

        static _S()
        {
            _S.IP = new byte[64];
            _S.IP_1 = new byte[64];
            _S.E = new byte[48];
            _S.p = new byte[32];
            _S.s1 = new byte[4, 16];
            _S.s2 = new byte[4, 16];
            _S.s3 = new byte[4, 16];
            _S.S4 = new byte[4, 16];
            _S.S5 = new byte[4, 16];
            _S.S6 = new byte[4, 16];
            _S.S7 = new byte[4, 16];
            _S.S8 = new byte[4, 16];
            _S.PC_1 = new byte[56];
            _S.PC_2 = new byte[48];
            _S.Lsi = new byte[17];
        }

        internal _S()
        {
        }


        public static void GE(byte[] sCode, ref byte[] bReturn, string sKey)
        {
            byte[] BinKey = new byte[64];
            byte[] KeyPC_1 = new byte[56];
            byte[] tempCode = new byte[8];
            byte[] tempReturn = new byte[8];
            byte[] BinCode = new byte[64];
            byte[] CodeIP = new byte[64];
            byte[] CodeE = new byte[48];
            byte[] CodeP = new byte[32];
            byte[] RetS = new byte[48];
            byte[] S = new byte[8];
            byte[] CodeS1 = new byte[6];
            byte[] CodeS2 = new byte[6];
            byte[] CodeS3 = new byte[6];
            byte[] CodeS4 = new byte[6];
            byte[] CodeS5 = new byte[6];
            byte[] CodeS6 = new byte[6];
            byte[] CodeS7 = new byte[6];
            byte[] CodeS8 = new byte[6];
            byte[] L0 = new byte[32];
            byte[] R0 = new byte[32];
            byte[] l1 = new byte[32];
            byte[] R1 = new byte[32];
            byte[] L2 = new byte[32];
            byte[] R2 = new byte[32];
            byte[] L3 = new byte[32];
            byte[] R3 = new byte[32];
            byte[] L4 = new byte[32];
            byte[] R4 = new byte[32];
            byte[] L5 = new byte[32];
            byte[] R5 = new byte[32];
            byte[] L6 = new byte[32];
            byte[] R6 = new byte[32];
            byte[] L7 = new byte[32];
            byte[] R7 = new byte[32];
            byte[] L8 = new byte[32];
            byte[] R8 = new byte[32];
            byte[] L9 = new byte[32];
            byte[] R9 = new byte[32];
            byte[] L10 = new byte[32];
            byte[] R10 = new byte[32];
            byte[] L11 = new byte[32];
            byte[] R11 = new byte[32];
            byte[] L12 = new byte[32];
            byte[] R12 = new byte[32];
            byte[] L13 = new byte[32];
            byte[] R13 = new byte[32];
            byte[] L14 = new byte[32];
            byte[] R14 = new byte[32];
            byte[] L15 = new byte[32];
            byte[] R15 = new byte[32];
            byte[] L16 = new byte[32];
            byte[] R16 = new byte[32];
            byte[] C0 = new byte[28];
            byte[] D0 = new byte[28];
            byte[] C1 = new byte[28];
            byte[] D1 = new byte[28];
            byte[] C2 = new byte[28];
            byte[] D2 = new byte[28];
            byte[] C3 = new byte[28];
            byte[] D3 = new byte[28];
            byte[] C4 = new byte[28];
            byte[] D4 = new byte[28];
            byte[] C5 = new byte[28];
            byte[] D5 = new byte[28];
            byte[] C6 = new byte[28];
            byte[] D6 = new byte[28];
            byte[] C7 = new byte[28];
            byte[] D7 = new byte[28];
            byte[] C8 = new byte[28];
            byte[] D8 = new byte[28];
            byte[] C9 = new byte[28];
            byte[] D9 = new byte[28];
            byte[] C10 = new byte[28];
            byte[] D10 = new byte[28];
            byte[] C11 = new byte[28];
            byte[] D11 = new byte[28];
            byte[] C12 = new byte[28];
            byte[] D12 = new byte[28];
            byte[] C13 = new byte[28];
            byte[] D13 = new byte[28];
            byte[] C14 = new byte[28];
            byte[] D14 = new byte[28];
            byte[] C15 = new byte[28];
            byte[] D15 = new byte[28];
            byte[] C16 = new byte[28];
            byte[] D16 = new byte[28];
            byte[] C_D = new byte[56];
            byte[] K1 = new byte[48];
            byte[] K2 = new byte[48];
            byte[] K3 = new byte[48];
            byte[] K4 = new byte[48];
            byte[] K5 = new byte[48];
            byte[] K6 = new byte[48];
            byte[] K7 = new byte[48];
            byte[] K8 = new byte[48];
            byte[] K9 = new byte[48];
            byte[] K10 = new byte[48];
            byte[] K11 = new byte[48];
            byte[] K12 = new byte[48];
            byte[] K13 = new byte[48];
            byte[] K14 = new byte[48];
            byte[] K15 = new byte[48];
            byte[] K16 = new byte[48];
            _S.Init();
            byte[] tempKey = GetBytes(sKey); //Encoding.UTF8.GetBytes(sKey);
            tempKey = (byte[])Utils.CopyArray(tempKey, new byte[8]);
            int i = 0;
            do
            {
                BinKey[checked(checked(i * 8) + 0)] = checked((byte)((tempKey[i] & 128) / 128));
                BinKey[checked(checked(i * 8) + 1)] = checked((byte)((tempKey[i] & 64) / 64));
                BinKey[checked(checked(i * 8) + 2)] = checked((byte)((tempKey[i] & 32) / 32));
                BinKey[checked(checked(i * 8) + 3)] = checked((byte)((tempKey[i] & 16) / 16));
                BinKey[checked(checked(i * 8) + 4)] = checked((byte)((tempKey[i] & 8) / 8));
                BinKey[checked(checked(i * 8) + 5)] = checked((byte)((tempKey[i] & 4) / 4));
                BinKey[checked(checked(i * 8) + 6)] = checked((byte)((tempKey[i] & 2) / 2));
                BinKey[checked(checked(i * 8) + 7)] = checked((byte)((tempKey[i] & 1) / 1));
                i = checked(i + 1);
            }
            while (i <= 7);
            i = 0;
            do
            {
                KeyPC_1[i] = BinKey[_S.PC_1[i]];
                i = checked(i + 1);
            }
            while (i <= 55);
            i = 0;
            do
            {
                C0[i] = KeyPC_1[i];
                D0[i] = KeyPC_1[checked(i + 28)];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                C1[i] = C0[checked(i + _S.Lsi[1])];
                D1[i] = D0[checked(i + _S.Lsi[1])];
                i = checked(i + 1);
            }
            while (i <= 26);
            C1[27] = C0[0];
            D1[27] = D0[0];
            i = 0;
            do
            {
                C_D[i] = C1[i];
                C_D[checked(i + 28)] = D1[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K1[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C2[i] = C1[checked(i + _S.Lsi[2])];
                D2[i] = D1[checked(i + _S.Lsi[2])];
                i = checked(i + 1);
            }
            while (i <= 26);
            C2[27] = C1[0];
            D2[27] = D1[0];
            i = 0;
            do
            {
                C_D[i] = C2[i];
                C_D[checked(i + 28)] = D2[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K2[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C3[i] = C2[checked(i + _S.Lsi[3])];
                D3[i] = D2[checked(i + _S.Lsi[3])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C3[26] = C2[0];
            D3[26] = D2[0];
            C3[27] = C2[1];
            D3[27] = D2[1];
            i = 0;
            do
            {
                C_D[i] = C3[i];
                C_D[checked(i + 28)] = D3[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K3[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C4[i] = C3[checked(i + _S.Lsi[4])];
                D4[i] = D3[checked(i + _S.Lsi[4])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C4[26] = C3[0];
            D4[26] = D3[0];
            C4[27] = C3[1];
            D4[27] = D3[1];
            i = 0;
            do
            {
                C_D[i] = C4[i];
                C_D[checked(i + 28)] = D4[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K4[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C5[i] = C4[checked(i + _S.Lsi[5])];
                D5[i] = D4[checked(i + _S.Lsi[5])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C5[26] = C4[0];
            D5[26] = D4[0];
            C5[27] = C4[1];
            D5[27] = D4[1];
            i = 0;
            do
            {
                C_D[i] = C5[i];
                C_D[checked(i + 28)] = D5[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K5[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C6[i] = C5[checked(i + _S.Lsi[6])];
                D6[i] = D5[checked(i + _S.Lsi[6])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C6[26] = C5[0];
            D6[26] = D5[0];
            C6[27] = C5[1];
            D6[27] = D5[1];
            i = 0;
            do
            {
                C_D[i] = C6[i];
                C_D[checked(i + 28)] = D6[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K6[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C7[i] = C6[checked(i + _S.Lsi[7])];
                D7[i] = D6[checked(i + _S.Lsi[7])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C7[26] = C6[0];
            D7[26] = D6[0];
            C7[27] = C6[1];
            D7[27] = D6[1];
            i = 0;
            do
            {
                C_D[i] = C7[i];
                C_D[checked(i + 28)] = D7[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K7[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C8[i] = C7[checked(i + _S.Lsi[8])];
                D8[i] = D7[checked(i + _S.Lsi[8])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C8[26] = C7[0];
            D8[26] = D7[0];
            C8[27] = C7[1];
            D8[27] = D7[1];
            i = 0;
            do
            {
                C_D[i] = C8[i];
                C_D[checked(i + 28)] = D8[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K8[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C9[i] = C8[checked(i + _S.Lsi[9])];
                D9[i] = D8[checked(i + _S.Lsi[9])];
                i = checked(i + 1);
            }
            while (i <= 26);
            C9[27] = C8[0];
            D9[27] = D8[0];
            i = 0;
            do
            {
                C_D[i] = C9[i];
                C_D[checked(i + 28)] = D9[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K9[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C10[i] = C9[checked(i + _S.Lsi[10])];
                D10[i] = D9[checked(i + _S.Lsi[10])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C10[26] = C9[0];
            D10[26] = D9[0];
            C10[27] = C9[1];
            D10[27] = D9[1];
            i = 0;
            do
            {
                C_D[i] = C10[i];
                C_D[checked(i + 28)] = D10[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K10[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C11[i] = C10[checked(i + _S.Lsi[11])];
                D11[i] = D10[checked(i + _S.Lsi[11])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C11[26] = C10[0];
            D11[26] = D10[0];
            C11[27] = C10[1];
            D11[27] = D10[1];
            i = 0;
            do
            {
                C_D[i] = C11[i];
                C_D[checked(i + 28)] = D11[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K11[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C12[i] = C11[checked(i + _S.Lsi[12])];
                D12[i] = D11[checked(i + _S.Lsi[12])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C12[26] = C11[0];
            D12[26] = D11[0];
            C12[27] = C11[1];
            D12[27] = D11[1];
            i = 0;
            do
            {
                C_D[i] = C12[i];
                C_D[checked(i + 28)] = D12[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K12[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C13[i] = C12[checked(i + _S.Lsi[13])];
                D13[i] = D12[checked(i + _S.Lsi[13])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C13[26] = C12[0];
            D13[26] = D12[0];
            C13[27] = C12[1];
            D13[27] = D12[1];
            i = 0;
            do
            {
                C_D[i] = C13[i];
                C_D[checked(i + 28)] = D13[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K13[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C14[i] = C13[checked(i + _S.Lsi[14])];
                D14[i] = D13[checked(i + _S.Lsi[14])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C14[26] = C13[0];
            D14[26] = D13[0];
            C14[27] = C13[1];
            D14[27] = D13[1];
            i = 0;
            do
            {
                C_D[i] = C14[i];
                C_D[checked(i + 28)] = D14[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K14[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C15[i] = C14[checked(i + _S.Lsi[15])];
                D15[i] = D14[checked(i + _S.Lsi[15])];
                i = checked(i + 1);
            }
            while (i <= 25);
            C15[26] = C14[0];
            D15[26] = D14[0];
            C15[27] = C14[1];
            D15[27] = D14[1];
            i = 0;
            do
            {
                C_D[i] = C15[i];
                C_D[checked(i + 28)] = D15[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K15[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            i = 0;
            do
            {
                C16[i] = C15[checked(i + _S.Lsi[16])];
                D16[i] = D15[checked(i + _S.Lsi[16])];
                i = checked(i + 1);
            }
            while (i <= 26);
            C16[27] = C15[0];
            D16[27] = D15[0];
            i = 0;
            do
            {
                C_D[i] = C16[i];
                C_D[checked(i + 28)] = D16[i];
                i = checked(i + 1);
            }
            while (i <= 27);
            i = 0;
            do
            {
                K16[i] = C_D[_S.PC_2[i]];
                i = checked(i + 1);
            }
            while (i <= 47);
            if ((checked(Information.UBound(sCode, 1) + 1)) % 8 > 0)
            {
                sCode = (byte[])Utils.CopyArray(sCode, new byte[checked(checked(checked((checked((checked(Information.UBound(sCode, 1) + 1)) / 8 + 1)) * 8) - 1) + 1)]);
            }
            bReturn = new byte[checked(Information.UBound(sCode, 1) + 1)];
            int num = Information.UBound(sCode, 1);
            for (int j = 0; j <= num; j = checked(j + 8))
            {
                Array.Copy(sCode, j, tempCode, 0, 8);
                i = 0;
                do
                {
                    BinCode[checked(checked(i * 8) + 0)] = checked((byte)((tempCode[i] & 128) / 128));
                    BinCode[checked(checked(i * 8) + 1)] = checked((byte)((tempCode[i] & 64) / 64));
                    BinCode[checked(checked(i * 8) + 2)] = checked((byte)((tempCode[i] & 32) / 32));
                    BinCode[checked(checked(i * 8) + 3)] = checked((byte)((tempCode[i] & 16) / 16));
                    BinCode[checked(checked(i * 8) + 4)] = checked((byte)((tempCode[i] & 8) / 8));
                    BinCode[checked(checked(i * 8) + 5)] = checked((byte)((tempCode[i] & 4) / 4));
                    BinCode[checked(checked(i * 8) + 6)] = checked((byte)((tempCode[i] & 2) / 2));
                    BinCode[checked(checked(i * 8) + 7)] = checked((byte)((tempCode[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeIP[i] = BinCode[_S.IP[i]];
                    i = checked(i + 1);
                }
                while (i <= 63);
                i = 0;
                do
                {
                    L0[i] = CodeIP[i];
                    R0[i] = CodeIP[checked(i + 32)];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R0[_S.E[i]];
                    CodeE[i] ^= K1[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R1[i] = (byte)(L0[i] ^ CodeP[i]);
                    l1[i] = R0[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R1[_S.E[i]];
                    CodeE[i] ^= K2[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R2[i] = (byte)(l1[i] ^ CodeP[i]);
                    L2[i] = R1[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R2[_S.E[i]];
                    CodeE[i] ^= K3[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R3[i] = (byte)(L2[i] ^ CodeP[i]);
                    L3[i] = R2[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R3[_S.E[i]];
                    CodeE[i] ^= K4[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R4[i] = (byte)(L3[i] ^ CodeP[i]);
                    L4[i] = R3[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R4[_S.E[i]];
                    CodeE[i] ^= K5[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R5[i] = (byte)(L4[i] ^ CodeP[i]);
                    L5[i] = R4[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R5[_S.E[i]];
                    CodeE[i] ^= K6[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R6[i] = (byte)(L5[i] ^ CodeP[i]);
                    L6[i] = R5[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R6[_S.E[i]];
                    CodeE[i] ^= K7[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R7[i] = (byte)(L6[i] ^ CodeP[i]);
                    L7[i] = R6[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R7[_S.E[i]];
                    CodeE[i] ^= K8[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R8[i] = (byte)(L7[i] ^ CodeP[i]);
                    L8[i] = R7[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R8[_S.E[i]];
                    CodeE[i] ^= K9[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R9[i] = (byte)(L8[i] ^ CodeP[i]);
                    L9[i] = R8[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R9[_S.E[i]];
                    CodeE[i] ^= K10[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R10[i] = (byte)(L9[i] ^ CodeP[i]);
                    L10[i] = R9[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R10[_S.E[i]];
                    CodeE[i] ^= K11[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R11[i] = (byte)(L10[i] ^ CodeP[i]);
                    L11[i] = R10[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R11[_S.E[i]];
                    CodeE[i] ^= K12[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R12[i] = (byte)(L11[i] ^ CodeP[i]);
                    L12[i] = R11[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R12[_S.E[i]];
                    CodeE[i] ^= K13[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R13[i] = (byte)(L12[i] ^ CodeP[i]);
                    L13[i] = R12[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R13[_S.E[i]];
                    CodeE[i] ^= K14[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R14[i] = (byte)(L13[i] ^ CodeP[i]);
                    L14[i] = R13[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R14[_S.E[i]];
                    CodeE[i] ^= K15[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R15[i] = (byte)(L14[i] ^ CodeP[i]);
                    L15[i] = R14[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeE[i] = R15[_S.E[i]];
                    CodeE[i] ^= K16[i];
                    i = checked(i + 1);
                }
                while (i <= 47);
                i = 0;
                do
                {
                    CodeS1[i] = CodeE[i];
                    CodeS2[i] = CodeE[checked(i + 6)];
                    CodeS3[i] = CodeE[checked(i + 12)];
                    CodeS4[i] = CodeE[checked(i + 18)];
                    CodeS5[i] = CodeE[checked(i + 24)];
                    CodeS6[i] = CodeE[checked(i + 30)];
                    CodeS7[i] = CodeE[checked(i + 36)];
                    CodeS8[i] = CodeE[checked(i + 42)];
                    i = checked(i + 1);
                }
                while (i <= 5);
                S[0] = _S.s1[checked(CodeS1[5] + checked(CodeS1[0] * 2)), checked(checked(checked(CodeS1[4] + checked(CodeS1[3] * 2)) + checked(CodeS1[2] * 4)) + checked(CodeS1[1] * 8))];
                S[1] = _S.s2[checked(CodeS2[5] + checked(CodeS2[0] * 2)), checked(checked(checked(CodeS2[4] + checked(CodeS2[3] * 2)) + checked(CodeS2[2] * 4)) + checked(CodeS2[1] * 8))];
                S[2] = _S.s3[checked(CodeS3[5] + checked(CodeS3[0] * 2)), checked(checked(checked(CodeS3[4] + checked(CodeS3[3] * 2)) + checked(CodeS3[2] * 4)) + checked(CodeS3[1] * 8))];
                S[3] = _S.S4[checked(CodeS4[5] + checked(CodeS4[0] * 2)), checked(checked(checked(CodeS4[4] + checked(CodeS4[3] * 2)) + checked(CodeS4[2] * 4)) + checked(CodeS4[1] * 8))];
                S[4] = _S.S5[checked(CodeS5[5] + checked(CodeS5[0] * 2)), checked(checked(checked(CodeS5[4] + checked(CodeS5[3] * 2)) + checked(CodeS5[2] * 4)) + checked(CodeS5[1] * 8))];
                S[5] = _S.S6[checked(CodeS6[5] + checked(CodeS6[0] * 2)), checked(checked(checked(CodeS6[4] + checked(CodeS6[3] * 2)) + checked(CodeS6[2] * 4)) + checked(CodeS6[1] * 8))];
                S[6] = _S.S7[checked(CodeS7[5] + checked(CodeS7[0] * 2)), checked(checked(checked(CodeS7[4] + checked(CodeS7[3] * 2)) + checked(CodeS7[2] * 4)) + checked(CodeS7[1] * 8))];
                S[7] = _S.S8[checked(CodeS8[5] + checked(CodeS8[0] * 2)), checked(checked(checked(CodeS8[4] + checked(CodeS8[3] * 2)) + checked(CodeS8[2] * 4)) + checked(CodeS8[1] * 8))];
                i = 0;
                do
                {
                    RetS[checked(checked(i * 4) + 0)] = checked((byte)((S[i] & 8) / 8));
                    RetS[checked(checked(i * 4) + 1)] = checked((byte)((S[i] & 4) / 4));
                    RetS[checked(checked(i * 4) + 2)] = checked((byte)((S[i] & 2) / 2));
                    RetS[checked(checked(i * 4) + 3)] = checked((byte)((S[i] & 1) / 1));
                    i = checked(i + 1);
                }
                while (i <= 7);
                i = 0;
                do
                {
                    CodeP[i] = RetS[_S.p[i]];
                    R16[i] = (byte)(L15[i] ^ CodeP[i]);
                    L16[i] = R15[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    BinCode[i] = L16[i];
                    BinCode[checked(i + 32)] = R16[i];
                    i = checked(i + 1);
                }
                while (i <= 31);
                i = 0;
                do
                {
                    CodeIP[i] = BinCode[_S.IP_1[i]];
                    i = checked(i + 1);
                }
                while (i <= 63);
                i = 0;
                do
                {
                    tempReturn[i] = checked((byte)(checked(CodeIP[checked(checked(i * 8) + 0)] * 128)));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 1)] * 64))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 2)] * 32))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 3)] * 16))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 4)] * 8))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 5)] * 4))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 6)] * 2))));
                    tempReturn[i] = checked((byte)(checked(tempReturn[i] + checked(CodeIP[checked(checked(i * 8) + 7)] * 1))));
                    i = checked(i + 1);
                }
                while (i <= 7);
                Array.Copy(tempReturn, 0, bReturn, j, 8);
            }
        }

        private static void Init()
        {
            _S.IP[0] = 57;
            _S.IP[1] = 49;
            _S.IP[2] = 41;
            _S.IP[3] = 33;
            _S.IP[4] = 25;
            _S.IP[5] = 17;
            _S.IP[6] = 9;
            _S.IP[7] = 1;
            _S.IP[8] = 59;
            _S.IP[9] = 51;
            _S.IP[10] = 43;
            _S.IP[11] = 35;
            _S.IP[12] = 27;
            _S.IP[13] = 19;
            _S.IP[14] = 11;
            _S.IP[15] = 3;
            _S.IP[16] = 61;
            _S.IP[17] = 53;
            _S.IP[18] = 45;
            _S.IP[19] = 37;
            _S.IP[20] = 29;
            _S.IP[21] = 21;
            _S.IP[22] = 13;
            _S.IP[23] = 5;
            _S.IP[24] = 63;
            _S.IP[25] = 55;
            _S.IP[26] = 47;
            _S.IP[27] = 39;
            _S.IP[28] = 31;
            _S.IP[29] = 23;
            _S.IP[30] = 15;
            _S.IP[31] = 7;
            _S.IP[32] = 56;
            _S.IP[33] = 48;
            _S.IP[34] = 40;
            _S.IP[35] = 32;
            _S.IP[36] = 24;
            _S.IP[37] = 16;
            _S.IP[38] = 8;
            _S.IP[39] = 0;
            _S.IP[40] = 58;
            _S.IP[41] = 50;
            _S.IP[42] = 42;
            _S.IP[43] = 34;
            _S.IP[44] = 26;
            _S.IP[45] = 18;
            _S.IP[46] = 10;
            _S.IP[47] = 2;
            _S.IP[48] = 60;
            _S.IP[49] = 52;
            _S.IP[50] = 44;
            _S.IP[51] = 36;
            _S.IP[52] = 28;
            _S.IP[53] = 20;
            _S.IP[54] = 12;
            _S.IP[55] = 4;
            _S.IP[56] = 62;
            _S.IP[57] = 54;
            _S.IP[58] = 46;
            _S.IP[59] = 38;
            _S.IP[60] = 30;
            _S.IP[61] = 22;
            _S.IP[62] = 14;
            _S.IP[63] = 6;
            _S.IP_1[0] = 39;
            _S.IP_1[1] = 7;
            _S.IP_1[2] = 47;
            _S.IP_1[3] = 15;
            _S.IP_1[4] = 55;
            _S.IP_1[5] = 23;
            _S.IP_1[6] = 63;
            _S.IP_1[7] = 31;
            _S.IP_1[8] = 38;
            _S.IP_1[9] = 6;
            _S.IP_1[10] = 46;
            _S.IP_1[11] = 14;
            _S.IP_1[12] = 54;
            _S.IP_1[13] = 22;
            _S.IP_1[14] = 62;
            _S.IP_1[15] = 30;
            _S.IP_1[16] = 37;
            _S.IP_1[17] = 5;
            _S.IP_1[18] = 45;
            _S.IP_1[19] = 13;
            _S.IP_1[20] = 53;
            _S.IP_1[21] = 21;
            _S.IP_1[22] = 61;
            _S.IP_1[23] = 29;
            _S.IP_1[24] = 36;
            _S.IP_1[25] = 4;
            _S.IP_1[26] = 44;
            _S.IP_1[27] = 12;
            _S.IP_1[28] = 52;
            _S.IP_1[29] = 20;
            _S.IP_1[30] = 60;
            _S.IP_1[31] = 28;
            _S.IP_1[32] = 35;
            _S.IP_1[33] = 3;
            _S.IP_1[34] = 43;
            _S.IP_1[35] = 11;
            _S.IP_1[36] = 51;
            _S.IP_1[37] = 19;
            _S.IP_1[38] = 59;
            _S.IP_1[39] = 27;
            _S.IP_1[40] = 34;
            _S.IP_1[41] = 2;
            _S.IP_1[42] = 42;
            _S.IP_1[43] = 10;
            _S.IP_1[44] = 50;
            _S.IP_1[45] = 18;
            _S.IP_1[46] = 58;
            _S.IP_1[47] = 26;
            _S.IP_1[48] = 33;
            _S.IP_1[49] = 1;
            _S.IP_1[50] = 41;
            _S.IP_1[51] = 9;
            _S.IP_1[52] = 49;
            _S.IP_1[53] = 17;
            _S.IP_1[54] = 57;
            _S.IP_1[55] = 25;
            _S.IP_1[56] = 32;
            _S.IP_1[57] = 0;
            _S.IP_1[58] = 40;
            _S.IP_1[59] = 8;
            _S.IP_1[60] = 48;
            _S.IP_1[61] = 16;
            _S.IP_1[62] = 56;
            _S.IP_1[63] = 24;
            _S.E[0] = 31;
            int i = 1;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 1)));
                i = checked(i + 1);
            }
            while (i <= 5);
            i = 6;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 3)));
                i = checked(i + 1);
            }
            while (i <= 11);
            i = 12;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 5)));
                i = checked(i + 1);
            }
            while (i <= 17);
            i = 18;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 7)));
                i = checked(i + 1);
            }
            while (i <= 23);
            i = 24;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 9)));
                i = checked(i + 1);
            }
            while (i <= 29);
            i = 30;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 11)));
                i = checked(i + 1);
            }
            while (i <= 35);
            i = 36;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 13)));
                i = checked(i + 1);
            }
            while (i <= 41);
            i = 42;
            do
            {
                _S.E[i] = checked((byte)(checked(i - 15)));
                i = checked(i + 1);
            }
            while (i <= 46);
            _S.E[47] = 30;
            _S.p[0] = 15;
            _S.p[1] = 6;
            _S.p[2] = 19;
            _S.p[3] = 20;
            _S.p[4] = 28;
            _S.p[5] = 11;
            _S.p[6] = 27;
            _S.p[7] = 16;
            _S.p[8] = 0;
            _S.p[9] = 14;
            _S.p[10] = 22;
            _S.p[11] = 25;
            _S.p[12] = 4;
            _S.p[13] = 17;
            _S.p[14] = 30;
            _S.p[15] = 9;
            _S.p[16] = 1;
            _S.p[17] = 7;
            _S.p[18] = 23;
            _S.p[19] = 13;
            _S.p[20] = 31;
            _S.p[21] = 26;
            _S.p[22] = 2;
            _S.p[23] = 8;
            _S.p[24] = 18;
            _S.p[25] = 12;
            _S.p[26] = 29;
            _S.p[27] = 5;
            _S.p[28] = 21;
            _S.p[29] = 10;
            _S.p[30] = 3;
            _S.p[31] = 24;
            _S.s1[0, 0] = 14;
            _S.s1[0, 1] = 4;
            _S.s1[0, 2] = 13;
            _S.s1[0, 3] = 1;
            _S.s1[0, 4] = 2;
            _S.s1[0, 5] = 15;
            _S.s1[0, 6] = 11;
            _S.s1[0, 7] = 8;
            _S.s1[0, 8] = 3;
            _S.s1[0, 9] = 10;
            _S.s1[0, 10] = 6;
            _S.s1[0, 11] = 12;
            _S.s1[0, 12] = 5;
            _S.s1[0, 13] = 9;
            _S.s1[0, 14] = 0;
            _S.s1[0, 15] = 7;
            _S.s1[1, 0] = 0;
            _S.s1[1, 1] = 15;
            _S.s1[1, 2] = 7;
            _S.s1[1, 3] = 4;
            _S.s1[1, 4] = 14;
            _S.s1[1, 5] = 2;
            _S.s1[1, 6] = 13;
            _S.s1[1, 7] = 1;
            _S.s1[1, 8] = 10;
            _S.s1[1, 9] = 6;
            _S.s1[1, 10] = 12;
            _S.s1[1, 11] = 11;
            _S.s1[1, 12] = 9;
            _S.s1[1, 13] = 5;
            _S.s1[1, 14] = 3;
            _S.s1[1, 15] = 8;
            _S.s1[2, 0] = 4;
            _S.s1[2, 1] = 1;
            _S.s1[2, 2] = 14;
            _S.s1[2, 3] = 8;
            _S.s1[2, 4] = 13;
            _S.s1[2, 5] = 6;
            _S.s1[2, 6] = 2;
            _S.s1[2, 7] = 11;
            _S.s1[2, 8] = 15;
            _S.s1[2, 9] = 12;
            _S.s1[2, 10] = 9;
            _S.s1[2, 11] = 7;
            _S.s1[2, 12] = 3;
            _S.s1[2, 13] = 10;
            _S.s1[2, 14] = 5;
            _S.s1[2, 15] = 0;
            _S.s1[3, 0] = 15;
            _S.s1[3, 1] = 12;
            _S.s1[3, 2] = 8;
            _S.s1[3, 3] = 2;
            _S.s1[3, 4] = 4;
            _S.s1[3, 5] = 9;
            _S.s1[3, 6] = 1;
            _S.s1[3, 7] = 7;
            _S.s1[3, 8] = 5;
            _S.s1[3, 9] = 11;
            _S.s1[3, 10] = 3;
            _S.s1[3, 11] = 14;
            _S.s1[3, 12] = 10;
            _S.s1[3, 13] = 0;
            _S.s1[3, 14] = 6;
            _S.s1[3, 15] = 13;
            _S.s2[0, 0] = 15;
            _S.s2[0, 1] = 1;
            _S.s2[0, 2] = 8;
            _S.s2[0, 3] = 14;
            _S.s2[0, 4] = 6;
            _S.s2[0, 5] = 11;
            _S.s2[0, 6] = 3;
            _S.s2[0, 7] = 4;
            _S.s2[0, 8] = 9;
            _S.s2[0, 9] = 7;
            _S.s2[0, 10] = 2;
            _S.s2[0, 11] = 13;
            _S.s2[0, 12] = 12;
            _S.s2[0, 13] = 0;
            _S.s2[0, 14] = 5;
            _S.s2[0, 15] = 10;
            _S.s2[1, 0] = 3;
            _S.s2[1, 1] = 13;
            _S.s2[1, 2] = 4;
            _S.s2[1, 3] = 7;
            _S.s2[1, 4] = 15;
            _S.s2[1, 5] = 2;
            _S.s2[1, 6] = 8;
            _S.s2[1, 7] = 14;
            _S.s2[1, 8] = 12;
            _S.s2[1, 9] = 0;
            _S.s2[1, 10] = 1;
            _S.s2[1, 11] = 10;
            _S.s2[1, 12] = 6;
            _S.s2[1, 13] = 9;
            _S.s2[1, 14] = 11;
            _S.s2[1, 15] = 5;
            _S.s2[2, 0] = 0;
            _S.s2[2, 1] = 14;
            _S.s2[2, 2] = 7;
            _S.s2[2, 3] = 11;
            _S.s2[2, 4] = 10;
            _S.s2[2, 5] = 4;
            _S.s2[2, 6] = 13;
            _S.s2[2, 7] = 1;
            _S.s2[2, 8] = 5;
            _S.s2[2, 9] = 8;
            _S.s2[2, 10] = 12;
            _S.s2[2, 11] = 6;
            _S.s2[2, 12] = 9;
            _S.s2[2, 13] = 3;
            _S.s2[2, 14] = 2;
            _S.s2[2, 15] = 15;
            _S.s2[3, 0] = 13;
            _S.s2[3, 1] = 8;
            _S.s2[3, 2] = 10;
            _S.s2[3, 3] = 1;
            _S.s2[3, 4] = 3;
            _S.s2[3, 5] = 15;
            _S.s2[3, 6] = 4;
            _S.s2[3, 7] = 2;
            _S.s2[3, 8] = 11;
            _S.s2[3, 9] = 6;
            _S.s2[3, 10] = 7;
            _S.s2[3, 11] = 12;
            _S.s2[3, 12] = 0;
            _S.s2[3, 13] = 5;
            _S.s2[3, 14] = 14;
            _S.s2[3, 15] = 9;
            _S.s3[0, 0] = 10;
            _S.s3[0, 1] = 0;
            _S.s3[0, 2] = 9;
            _S.s3[0, 3] = 14;
            _S.s3[0, 4] = 6;
            _S.s3[0, 5] = 3;
            _S.s3[0, 6] = 15;
            _S.s3[0, 7] = 5;
            _S.s3[0, 8] = 1;
            _S.s3[0, 9] = 13;
            _S.s3[0, 10] = 12;
            _S.s3[0, 11] = 7;
            _S.s3[0, 12] = 11;
            _S.s3[0, 13] = 4;
            _S.s3[0, 14] = 2;
            _S.s3[0, 15] = 8;
            _S.s3[1, 0] = 13;
            _S.s3[1, 1] = 7;
            _S.s3[1, 2] = 0;
            _S.s3[1, 3] = 9;
            _S.s3[1, 4] = 3;
            _S.s3[1, 5] = 4;
            _S.s3[1, 6] = 6;
            _S.s3[1, 7] = 10;
            _S.s3[1, 8] = 2;
            _S.s3[1, 9] = 8;
            _S.s3[1, 10] = 5;
            _S.s3[1, 11] = 14;
            _S.s3[1, 12] = 12;
            _S.s3[1, 13] = 11;
            _S.s3[1, 14] = 15;
            _S.s3[1, 15] = 1;
            _S.s3[2, 0] = 13;
            _S.s3[2, 1] = 6;
            _S.s3[2, 2] = 4;
            _S.s3[2, 3] = 9;
            _S.s3[2, 4] = 8;
            _S.s3[2, 5] = 15;
            _S.s3[2, 6] = 3;
            _S.s3[2, 7] = 0;
            _S.s3[2, 8] = 11;
            _S.s3[2, 9] = 1;
            _S.s3[2, 10] = 2;
            _S.s3[2, 11] = 12;
            _S.s3[2, 12] = 5;
            _S.s3[2, 13] = 10;
            _S.s3[2, 14] = 14;
            _S.s3[2, 15] = 7;
            _S.s3[3, 0] = 1;
            _S.s3[3, 1] = 10;
            _S.s3[3, 2] = 13;
            _S.s3[3, 3] = 0;
            _S.s3[3, 4] = 6;
            _S.s3[3, 5] = 9;
            _S.s3[3, 6] = 8;
            _S.s3[3, 7] = 7;
            _S.s3[3, 8] = 4;
            _S.s3[3, 9] = 15;
            _S.s3[3, 10] = 14;
            _S.s3[3, 11] = 3;
            _S.s3[3, 12] = 11;
            _S.s3[3, 13] = 5;
            _S.s3[3, 14] = 2;
            _S.s3[3, 15] = 12;
            _S.S4[0, 0] = 7;
            _S.S4[0, 1] = 13;
            _S.S4[0, 2] = 14;
            _S.S4[0, 3] = 3;
            _S.S4[0, 4] = 0;
            _S.S4[0, 5] = 6;
            _S.S4[0, 6] = 9;
            _S.S4[0, 7] = 10;
            _S.S4[0, 8] = 1;
            _S.S4[0, 9] = 2;
            _S.S4[0, 10] = 8;
            _S.S4[0, 11] = 5;
            _S.S4[0, 12] = 11;
            _S.S4[0, 13] = 12;
            _S.S4[0, 14] = 4;
            _S.S4[0, 15] = 15;
            _S.S4[1, 0] = 13;
            _S.S4[1, 1] = 8;
            _S.S4[1, 2] = 11;
            _S.S4[1, 3] = 5;
            _S.S4[1, 4] = 6;
            _S.S4[1, 5] = 15;
            _S.S4[1, 6] = 0;
            _S.S4[1, 7] = 3;
            _S.S4[1, 8] = 4;
            _S.S4[1, 9] = 7;
            _S.S4[1, 10] = 2;
            _S.S4[1, 11] = 12;
            _S.S4[1, 12] = 1;
            _S.S4[1, 13] = 10;
            _S.S4[1, 14] = 14;
            _S.S4[1, 15] = 9;
            _S.S4[2, 0] = 10;
            _S.S4[2, 1] = 6;
            _S.S4[2, 2] = 9;
            _S.S4[2, 3] = 0;
            _S.S4[2, 4] = 12;
            _S.S4[2, 5] = 11;
            _S.S4[2, 6] = 7;
            _S.S4[2, 7] = 13;
            _S.S4[2, 8] = 15;
            _S.S4[2, 9] = 1;
            _S.S4[2, 10] = 3;
            _S.S4[2, 11] = 14;
            _S.S4[2, 12] = 5;
            _S.S4[2, 13] = 2;
            _S.S4[2, 14] = 8;
            _S.S4[2, 15] = 4;
            _S.S4[3, 0] = 3;
            _S.S4[3, 1] = 15;
            _S.S4[3, 2] = 0;
            _S.S4[3, 3] = 6;
            _S.S4[3, 4] = 10;
            _S.S4[3, 5] = 1;
            _S.S4[3, 6] = 13;
            _S.S4[3, 7] = 8;
            _S.S4[3, 8] = 9;
            _S.S4[3, 9] = 4;
            _S.S4[3, 10] = 5;
            _S.S4[3, 11] = 11;
            _S.S4[3, 12] = 12;
            _S.S4[3, 13] = 7;
            _S.S4[3, 14] = 2;
            _S.S4[3, 15] = 14;
            _S.S5[0, 0] = 2;
            _S.S5[0, 1] = 12;
            _S.S5[0, 2] = 4;
            _S.S5[0, 3] = 1;
            _S.S5[0, 4] = 7;
            _S.S5[0, 5] = 10;
            _S.S5[0, 6] = 11;
            _S.S5[0, 7] = 6;
            _S.S5[0, 8] = 8;
            _S.S5[0, 9] = 5;
            _S.S5[0, 10] = 3;
            _S.S5[0, 11] = 15;
            _S.S5[0, 12] = 13;
            _S.S5[0, 13] = 0;
            _S.S5[0, 14] = 14;
            _S.S5[0, 15] = 9;
            _S.S5[1, 0] = 14;
            _S.S5[1, 1] = 11;
            _S.S5[1, 2] = 2;
            _S.S5[1, 3] = 12;
            _S.S5[1, 4] = 4;
            _S.S5[1, 5] = 7;
            _S.S5[1, 6] = 13;
            _S.S5[1, 7] = 1;
            _S.S5[1, 8] = 5;
            _S.S5[1, 9] = 0;
            _S.S5[1, 10] = 15;
            _S.S5[1, 11] = 10;
            _S.S5[1, 12] = 3;
            _S.S5[1, 13] = 9;
            _S.S5[1, 14] = 8;
            _S.S5[1, 15] = 6;
            _S.S5[2, 0] = 4;
            _S.S5[2, 1] = 2;
            _S.S5[2, 2] = 1;
            _S.S5[2, 3] = 11;
            _S.S5[2, 4] = 10;
            _S.S5[2, 5] = 13;
            _S.S5[2, 6] = 7;
            _S.S5[2, 7] = 8;
            _S.S5[2, 8] = 15;
            _S.S5[2, 9] = 9;
            _S.S5[2, 10] = 12;
            _S.S5[2, 11] = 5;
            _S.S5[2, 12] = 6;
            _S.S5[2, 13] = 3;
            _S.S5[2, 14] = 0;
            _S.S5[2, 15] = 14;
            _S.S5[3, 0] = 11;
            _S.S5[3, 1] = 8;
            _S.S5[3, 2] = 12;
            _S.S5[3, 3] = 7;
            _S.S5[3, 4] = 1;
            _S.S5[3, 5] = 14;
            _S.S5[3, 6] = 2;
            _S.S5[3, 7] = 13;
            _S.S5[3, 8] = 6;
            _S.S5[3, 9] = 15;
            _S.S5[3, 10] = 0;
            _S.S5[3, 11] = 9;
            _S.S5[3, 12] = 10;
            _S.S5[3, 13] = 4;
            _S.S5[3, 14] = 5;
            _S.S5[3, 15] = 3;
            _S.S6[0, 0] = 12;
            _S.S6[0, 1] = 1;
            _S.S6[0, 2] = 10;
            _S.S6[0, 3] = 15;
            _S.S6[0, 4] = 9;
            _S.S6[0, 5] = 2;
            _S.S6[0, 6] = 6;
            _S.S6[0, 7] = 8;
            _S.S6[0, 8] = 0;
            _S.S6[0, 9] = 13;
            _S.S6[0, 10] = 3;
            _S.S6[0, 11] = 4;
            _S.S6[0, 12] = 14;
            _S.S6[0, 13] = 7;
            _S.S6[0, 14] = 5;
            _S.S6[0, 15] = 11;
            _S.S6[1, 0] = 10;
            _S.S6[1, 1] = 15;
            _S.S6[1, 2] = 4;
            _S.S6[1, 3] = 2;
            _S.S6[1, 4] = 7;
            _S.S6[1, 5] = 12;
            _S.S6[1, 6] = 9;
            _S.S6[1, 7] = 5;
            _S.S6[1, 8] = 6;
            _S.S6[1, 9] = 1;
            _S.S6[1, 10] = 13;
            _S.S6[1, 11] = 14;
            _S.S6[1, 12] = 0;
            _S.S6[1, 13] = 11;
            _S.S6[1, 14] = 3;
            _S.S6[1, 15] = 8;
            _S.S6[2, 0] = 9;
            _S.S6[2, 1] = 14;
            _S.S6[2, 2] = 15;
            _S.S6[2, 3] = 5;
            _S.S6[2, 4] = 2;
            _S.S6[2, 5] = 8;
            _S.S6[2, 6] = 12;
            _S.S6[2, 7] = 3;
            _S.S6[2, 8] = 7;
            _S.S6[2, 9] = 0;
            _S.S6[2, 10] = 4;
            _S.S6[2, 11] = 10;
            _S.S6[2, 12] = 1;
            _S.S6[2, 13] = 13;
            _S.S6[2, 14] = 11;
            _S.S6[2, 15] = 6;
            _S.S6[3, 0] = 4;
            _S.S6[3, 1] = 3;
            _S.S6[3, 2] = 2;
            _S.S6[3, 3] = 12;
            _S.S6[3, 4] = 9;
            _S.S6[3, 5] = 5;
            _S.S6[3, 6] = 15;
            _S.S6[3, 7] = 10;
            _S.S6[3, 8] = 11;
            _S.S6[3, 9] = 14;
            _S.S6[3, 10] = 1;
            _S.S6[3, 11] = 7;
            _S.S6[3, 12] = 6;
            _S.S6[3, 13] = 0;
            _S.S6[3, 14] = 8;
            _S.S6[3, 15] = 13;
            _S.S7[0, 0] = 4;
            _S.S7[0, 1] = 11;
            _S.S7[0, 2] = 2;
            _S.S7[0, 3] = 14;
            _S.S7[0, 4] = 15;
            _S.S7[0, 5] = 0;
            _S.S7[0, 6] = 8;
            _S.S7[0, 7] = 13;
            _S.S7[0, 8] = 3;
            _S.S7[0, 9] = 12;
            _S.S7[0, 10] = 9;
            _S.S7[0, 11] = 7;
            _S.S7[0, 12] = 5;
            _S.S7[0, 13] = 10;
            _S.S7[0, 14] = 6;
            _S.S7[0, 15] = 1;
            _S.S7[1, 0] = 13;
            _S.S7[1, 1] = 0;
            _S.S7[1, 2] = 11;
            _S.S7[1, 3] = 7;
            _S.S7[1, 4] = 4;
            _S.S7[1, 5] = 9;
            _S.S7[1, 6] = 1;
            _S.S7[1, 7] = 10;
            _S.S7[1, 8] = 14;
            _S.S7[1, 9] = 3;
            _S.S7[1, 10] = 5;
            _S.S7[1, 11] = 12;
            _S.S7[1, 12] = 2;
            _S.S7[1, 13] = 15;
            _S.S7[1, 14] = 8;
            _S.S7[1, 15] = 6;
            _S.S7[2, 0] = 1;
            _S.S7[2, 1] = 4;
            _S.S7[2, 2] = 11;
            _S.S7[2, 3] = 13;
            _S.S7[2, 4] = 12;
            _S.S7[2, 5] = 3;
            _S.S7[2, 6] = 7;
            _S.S7[2, 7] = 14;
            _S.S7[2, 8] = 10;
            _S.S7[2, 9] = 15;
            _S.S7[2, 10] = 6;
            _S.S7[2, 11] = 8;
            _S.S7[2, 12] = 0;
            _S.S7[2, 13] = 5;
            _S.S7[2, 14] = 9;
            _S.S7[2, 15] = 2;
            _S.S7[3, 0] = 6;
            _S.S7[3, 1] = 11;
            _S.S7[3, 2] = 13;
            _S.S7[3, 3] = 8;
            _S.S7[3, 4] = 1;
            _S.S7[3, 5] = 4;
            _S.S7[3, 6] = 10;
            _S.S7[3, 7] = 7;
            _S.S7[3, 8] = 9;
            _S.S7[3, 9] = 5;
            _S.S7[3, 10] = 0;
            _S.S7[3, 11] = 15;
            _S.S7[3, 12] = 14;
            _S.S7[3, 13] = 2;
            _S.S7[3, 14] = 3;
            _S.S7[3, 15] = 12;
            _S.S8[0, 0] = 13;
            _S.S8[0, 1] = 2;
            _S.S8[0, 2] = 8;
            _S.S8[0, 3] = 4;
            _S.S8[0, 4] = 6;
            _S.S8[0, 5] = 15;
            _S.S8[0, 6] = 11;
            _S.S8[0, 7] = 1;
            _S.S8[0, 8] = 10;
            _S.S8[0, 9] = 9;
            _S.S8[0, 10] = 3;
            _S.S8[0, 11] = 14;
            _S.S8[0, 12] = 5;
            _S.S8[0, 13] = 0;
            _S.S8[0, 14] = 12;
            _S.S8[0, 15] = 7;
            _S.S8[1, 0] = 1;
            _S.S8[1, 1] = 15;
            _S.S8[1, 2] = 13;
            _S.S8[1, 3] = 8;
            _S.S8[1, 4] = 10;
            _S.S8[1, 5] = 3;
            _S.S8[1, 6] = 7;
            _S.S8[1, 7] = 4;
            _S.S8[1, 8] = 12;
            _S.S8[1, 9] = 5;
            _S.S8[1, 10] = 6;
            _S.S8[1, 11] = 11;
            _S.S8[1, 12] = 0;
            _S.S8[1, 13] = 14;
            _S.S8[1, 14] = 9;
            _S.S8[1, 15] = 2;
            _S.S8[2, 0] = 7;
            _S.S8[2, 1] = 11;
            _S.S8[2, 2] = 4;
            _S.S8[2, 3] = 1;
            _S.S8[2, 4] = 9;
            _S.S8[2, 5] = 12;
            _S.S8[2, 6] = 14;
            _S.S8[2, 7] = 2;
            _S.S8[2, 8] = 0;
            _S.S8[2, 9] = 6;
            _S.S8[2, 10] = 10;
            _S.S8[2, 11] = 13;
            _S.S8[2, 12] = 15;
            _S.S8[2, 13] = 3;
            _S.S8[2, 14] = 5;
            _S.S8[2, 15] = 8;
            _S.S8[3, 0] = 2;
            _S.S8[3, 1] = 1;
            _S.S8[3, 2] = 14;
            _S.S8[3, 3] = 7;
            _S.S8[3, 4] = 4;
            _S.S8[3, 5] = 10;
            _S.S8[3, 6] = 8;
            _S.S8[3, 7] = 13;
            _S.S8[3, 8] = 15;
            _S.S8[3, 9] = 12;
            _S.S8[3, 10] = 9;
            _S.S8[3, 11] = 0;
            _S.S8[3, 12] = 3;
            _S.S8[3, 13] = 5;
            _S.S8[3, 14] = 6;
            _S.S8[3, 15] = 11;
            _S.PC_1[0] = 56;
            _S.PC_1[1] = 48;
            _S.PC_1[2] = 40;
            _S.PC_1[3] = 32;
            _S.PC_1[4] = 24;
            _S.PC_1[5] = 16;
            _S.PC_1[6] = 8;
            _S.PC_1[7] = 0;
            _S.PC_1[8] = 57;
            _S.PC_1[9] = 49;
            _S.PC_1[10] = 41;
            _S.PC_1[11] = 33;
            _S.PC_1[12] = 25;
            _S.PC_1[13] = 17;
            _S.PC_1[14] = 9;
            _S.PC_1[15] = 1;
            _S.PC_1[16] = 58;
            _S.PC_1[17] = 50;
            _S.PC_1[18] = 42;
            _S.PC_1[19] = 34;
            _S.PC_1[20] = 26;
            _S.PC_1[21] = 18;
            _S.PC_1[22] = 10;
            _S.PC_1[23] = 2;
            _S.PC_1[24] = 59;
            _S.PC_1[25] = 51;
            _S.PC_1[26] = 43;
            _S.PC_1[27] = 35;
            _S.PC_1[28] = 62;
            _S.PC_1[29] = 54;
            _S.PC_1[30] = 46;
            _S.PC_1[31] = 38;
            _S.PC_1[32] = 30;
            _S.PC_1[33] = 22;
            _S.PC_1[34] = 14;
            _S.PC_1[35] = 6;
            _S.PC_1[36] = 61;
            _S.PC_1[37] = 53;
            _S.PC_1[38] = 45;
            _S.PC_1[39] = 37;
            _S.PC_1[40] = 29;
            _S.PC_1[41] = 21;
            _S.PC_1[42] = 13;
            _S.PC_1[43] = 5;
            _S.PC_1[44] = 60;
            _S.PC_1[45] = 52;
            _S.PC_1[46] = 44;
            _S.PC_1[47] = 36;
            _S.PC_1[48] = 28;
            _S.PC_1[49] = 20;
            _S.PC_1[50] = 12;
            _S.PC_1[51] = 4;
            _S.PC_1[52] = 27;
            _S.PC_1[53] = 19;
            _S.PC_1[54] = 11;
            _S.PC_1[55] = 3;
            _S.PC_2[0] = 13;
            _S.PC_2[1] = 16;
            _S.PC_2[2] = 10;
            _S.PC_2[3] = 23;
            _S.PC_2[4] = 0;
            _S.PC_2[5] = 4;
            _S.PC_2[6] = 2;
            _S.PC_2[7] = 27;
            _S.PC_2[8] = 14;
            _S.PC_2[9] = 5;
            _S.PC_2[10] = 20;
            _S.PC_2[11] = 9;
            _S.PC_2[12] = 22;
            _S.PC_2[13] = 18;
            _S.PC_2[14] = 11;
            _S.PC_2[15] = 3;
            _S.PC_2[16] = 25;
            _S.PC_2[17] = 7;
            _S.PC_2[18] = 15;
            _S.PC_2[19] = 6;
            _S.PC_2[20] = 26;
            _S.PC_2[21] = 19;
            _S.PC_2[22] = 12;
            _S.PC_2[23] = 1;
            _S.PC_2[24] = 40;
            _S.PC_2[25] = 51;
            _S.PC_2[26] = 30;
            _S.PC_2[27] = 36;
            _S.PC_2[28] = 46;
            _S.PC_2[29] = 54;
            _S.PC_2[30] = 29;
            _S.PC_2[31] = 39;
            _S.PC_2[32] = 50;
            _S.PC_2[33] = 44;
            _S.PC_2[34] = 32;
            _S.PC_2[35] = 47;
            _S.PC_2[36] = 43;
            _S.PC_2[37] = 48;
            _S.PC_2[38] = 38;
            _S.PC_2[39] = 55;
            _S.PC_2[40] = 33;
            _S.PC_2[41] = 52;
            _S.PC_2[42] = 45;
            _S.PC_2[43] = 41;
            _S.PC_2[44] = 49;
            _S.PC_2[45] = 35;
            _S.PC_2[46] = 28;
            _S.PC_2[47] = 31;
            _S.Lsi[1] = 1;
            _S.Lsi[2] = 1;
            _S.Lsi[3] = 2;
            _S.Lsi[4] = 2;
            _S.Lsi[5] = 2;
            _S.Lsi[6] = 2;
            _S.Lsi[7] = 2;
            _S.Lsi[8] = 2;
            _S.Lsi[9] = 1;
            _S.Lsi[10] = 2;
            _S.Lsi[11] = 2;
            _S.Lsi[12] = 2;
            _S.Lsi[13] = 2;
            _S.Lsi[14] = 2;
            _S.Lsi[15] = 2;
            _S.Lsi[16] = 1;
        }



        private static System.Text.RegularExpressions.Regex sReg = new System.Text.RegularExpressions.Regex("[\u4e00-\u9fa5]");
        private static Encoding sEncodingGB2312;

        private static bool IncludeChinese(string str)
        {
            return sReg.IsMatch(str);
        }
        private static byte[] GetBytes(string str)
        {
            try
            {
                if (IncludeChinese(str))
                {
                    if (sEncodingGB2312 == null)
                    {
                        try
                        {
                            //GB2312不是VS默认支持的编码，需要安装编码库。
                            //  添加引用System.Text.Encoding.CodePages
                            //  使用前注册编码库
                            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                            //完成以上才能正常获取到
                            sEncodingGB2312 = Encoding.GetEncoding("GB2312");
                        } catch
                        {
                            sEncodingGB2312 = Encoding.Default;
                        }
                    }
                    return sEncodingGB2312.GetBytes(str);
                }
            } catch { }
            return Encoding.UTF8.GetBytes(str);
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    internal class _AS
    {
        private static byte[] A1(byte[] P_0, string P_1)
        {
            byte[] array = new byte[16];
            if (P_0.Length < 16)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (i >= P_0.Length)
                    {
                        array[i] = 0;
                    } else
                    {
                        array[i] = P_0[i];
                    }
                }
            }
            return array;
        }

        public static string A1(string P_0, string P_1, bool P_2 = true)
        {
            byte[] array = Encoding.UTF8.GetBytes(P_1 ?? SQ.B());
            if (P_2)
            {
                array = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536._AS.A1(array, P_1);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(P_0);
            SymmetricAlgorithm symmetricAlgorithm = Aes.Create();
            symmetricAlgorithm.Key = array;
            symmetricAlgorithm.Mode = CipherMode.ECB;
            symmetricAlgorithm.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor();
            byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray);
        }

        public static string a(string P_0, string P_1, bool P_2 = true)
        {
            byte[] array = Encoding.UTF8.GetBytes(P_1 ?? SQ.B());
            if (P_2)
            {
                array = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536._AS.A1(array, P_1);
            }
            byte[] array2 = Convert.FromBase64String(P_0);
            SymmetricAlgorithm symmetricAlgorithm = Aes.Create();
            symmetricAlgorithm.Key = array;
            symmetricAlgorithm.Mode = CipherMode.ECB;
            symmetricAlgorithm.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateDecryptor();
            byte[] bytes = cryptoTransform.TransformFinalBlock(array2, 0, array2.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536
{
    internal class C
    {
        internal static bool m_A;

        private static bool m_a;

        private static string m_A_1;

        private static string m_a_1;

        private static readonly string B;

        private static readonly string b;

        private static bool m_B;

        private static string m_C;

        private static string c;

        private static string D;

        private static string d;

        private static string E;

        private static string e;

        private static string F;

        internal static string f;

        internal static string G;

        static C()
        {
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A = false;
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a = false;
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A_1 = SQ.E();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 = SQ.E();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.B = SQ.G();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.b = SQ.g();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_B = true;
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_C = SQ.H();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.c = SQ.E();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.D = SQ.E();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.d = SQ.E();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.E = SQ.h();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.e = SQ.I();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.F = SQ.i();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.f = SQ.J();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.G = SQ.j();
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.c = _AS.a(E, m_C);
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.D = _AS.a(e, m_C);
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.d = _AS.a(F, m_C);
        }

        internal static string A()
        {
            //IL_0006: Expected O, but got I4
            return (string)global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A_1;
        }

        internal static OracleConnection A(string P_0, string P_1, string P_2, bool P_3 = true)
        {
            return global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(P_0, P_1, P_2, P_3);
        }

        internal static string TranPwd(string P_1, string P_2)
        {
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A_1 = SQ.K();
            P_2 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_2, P_1);
            return P_2;
        }

        internal static void A(OracleConnection P_0, string P_1, string P_2, bool P_3, short P_4 = 0)
        {
            if (P_3)
            {
                switch (P_4)
                {
                    case 1:
                        P_2 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_2);
                        break;
                    case 2:
                        P_2 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_2, P_1);
                        break;
                    default:
                        if (!global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(P_0))
                        {
                            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A_1 = SQ.K();
                            P_2 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_2, P_1);
                        } else
                        {
                            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A_1 = SQ.k();
                            P_2 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_2);
                        }
                        break;
                }
            }
            global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_1, P_2);
        }

        private static OracleConnection a(string P_0, string P_1, string P_2, bool P_3)
        {
            OracleConnection val = null;
            if (P_3)
            {
                try
                {
                    bool flag = false;
                    val = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_1, P_2, ref flag);
                    if (flag)
                    {
                        try
                        {
                            if (global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(val))
                            {
                                global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(val, P_2, global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_1, P_2));
                            }
                        } catch
                        {
                        }
                    }
                } catch
                {
                    if ((object)val != null)
                    {
                        ((Component)val).Dispose();
                    }
                    throw;
                }
            } else
            {
                val = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_2, P_1);
            }
            return val;
        }

        private static OracleConnection A(string P_0, string P_1, string P_2, ref bool P_3)
        {
            OracleConnection val = null;
            P_3 = false;
            if (P_0.IndexOf(SQ.L()) > 0)
            {
                string[] array = P_0.Split('/', StringSplitOptions.None);
                string text = array[1];
                string text2;
                string text3;
                if (array[0].IndexOf(SQ.l()) > 0)
                {
                    array = array[0].Split(':', StringSplitOptions.None);
                    text2 = array[0];
                    text3 = array[1];
                } else
                {
                    text2 = array[0];
                    text3 = SQ.M();
                }
                P_0 = SQ.m() + text2 + SQ.N() + text3 + SQ.n() + text + SQ.O();
            }
            P_1 = P_1.Trim().ToUpper();
            P_2 = P_2.Trim().ToUpper();
            long num = 0L;
            string text4;
            if (global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A)
            {
                text4 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_1);
                global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 + SQ.o();
            } else
            {
                text4 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(P_1, P_2, true, ref num);
            }
            try
            {
                val = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_2, text4);
            } catch (Exception ex)
            {
                if (!global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A && global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(ex, 1017))
                {
                    if ((object)val != null)
                    {
                        ((Component)val).Dispose();
                    }
                    bool flag = false;
                    if (num > 8)
                    {
                        long num2 = 0L;
                        text4 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(P_1, P_2, false, ref num2);
                        global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 + SQ.P();
                        try
                        {
                            val = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_2, text4);
                            P_3 = true;
                            return val;
                        } catch (Exception ex2)
                        {
                            if ((object)val != null)
                            {
                                ((Component)val).Dispose();
                            }
                            flag = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(ex2, 1017);
                        }
                    }
                    if (flag)
                    {
                        global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_a_1 + SQ.p();
                        text4 = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_1);
                        val = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_2, text4);
                        P_3 = true;
                        return val;
                    }
                }
                throw;
            }
            return val;
        }

        private static OracleConnection A(string P_0, string P_1, string P_2)
        {
            OracleConnection val = new OracleConnection();
            ((DbConnection)val).ConnectionString = string.Format(SQ.Q(), P_0, P_1, P_2);
            ((DbConnection)val).Open();
            return val;
        }

        private static bool A(OracleConnection P_0)
        {
            string commandText = SQ.q();
            OracleCommand val = new OracleCommand();
            object obj = default(object);
            try
            {
                val.Connection = P_0;
                ((DbCommand)val).CommandText = commandText;
                obj = ((DbCommand)val).ExecuteScalar();
            } finally
            {
                if ((object)val != null)
                {
                    ((Component)val).Dispose();
                }
            }
            int num = default(int);
            if (int.TryParse(obj.ToString(), out num))
            {
                return num >= 3;
            }
            return false;
        }

        private static void A(OracleConnection P_0, string P_1, string P_2)
        {
            string commandText = SQ.R() + P_1 + SQ.r() + P_2 + SQ.S();
            Console.WriteLine(commandText);
            OracleCommand val = new OracleCommand();
            try
            {
                val.Connection = P_0;
                ((DbCommand)val).CommandText = commandText;
                ((DbCommand)val).ExecuteNonQuery();
            } finally
            {
                if ((object)val != null)
                {
                    ((Component)val).Dispose();
                }
            }
        }

        private static bool A(Exception P_0, int P_1)
        {
            OracleException val = P_0 as OracleException;
            return val != null && val.Number == P_1;
        }

        private static string A(string P_0, string P_1)
        {
            long num = 0L;
            return global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, P_1, true, ref num);
        }

        private static string A(string P_0, string P_1, bool P_2, ref long P_3)
        {
            P_0 = P_0.Trim().ToUpper();
            P_1 = P_1.Trim().ToUpper();
            return global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(P_0, P_1, P_2, ref P_3);
        }

        private static string A(string P_0)
        {
            string text = SQ.E();
            if (Strings.Len(P_0) != 0)
            {
                int length = P_0.Length;
                for (int i = 1; i <= length; i = checked(i + 1))
                {
                    string text2 = Strings.Mid(P_0, i, 1);
                    switch (i % 3)
                    {
                        case 0:
                            text = Conversions.ToString(Operators.ConcatenateObject(text, Interaction.Switch(Operators.CompareString(text2, SQ.s(), true) == 0, SQ.T(), Operators.CompareString(text2, SQ.t(), true) == 0, SQ.U(), Operators.CompareString(text2, SQ.u(), true) == 0, SQ.V(), Operators.CompareString(text2, SQ.v(), true) == 0, SQ.W(), Operators.CompareString(text2, SQ.w(), true) == 0, SQ.X(), Operators.CompareString(text2, SQ.x(), true) == 0, SQ.Y(), Operators.CompareString(text2, SQ.T(), true) == 0, SQ.y(), Operators.CompareString(text2, SQ.Z(), true) == 0, SQ.t(), Operators.CompareString(text2, SQ.z(), true) == 0, SQ.aA(), Operators.CompareString(text2, SQ.W(), true) == 0, SQ.aa(), Operators.CompareString(text2, SQ.aB(), true) == 0, SQ.ab(), Operators.CompareString(text2, SQ.aC(), true) == 0, SQ.z(), Operators.CompareString(text2, SQ.ac(), true) == 0, SQ.x(), Operators.CompareString(text2, SQ.aD(), true) == 0, SQ.ad(), Operators.CompareString(text2, SQ.Y(), true) == 0, SQ.Z(), Operators.CompareString(text2, SQ.aE(), true) == 0, SQ.ae(), Operators.CompareString(text2, SQ.X(), true) == 0, SQ.v(), Operators.CompareString(text2, SQ.V(), true) == 0, SQ.aB(), Operators.CompareString(text2, SQ.aF(), true) == 0, SQ.af(), Operators.CompareString(text2, SQ.U(), true) == 0, SQ.aE(), Operators.CompareString(text2, SQ.aG(), true) == 0, SQ.ag(), Operators.CompareString(text2, SQ.aa(), true) == 0, SQ.w(), Operators.CompareString(text2, SQ.ae(), true) == 0, SQ.aG(), Operators.CompareString(text2, SQ.af(), true) == 0, SQ.aH(), Operators.CompareString(text2, SQ.ag(), true) == 0, SQ.aD(), Operators.CompareString(text2, SQ.ah(), true) == 0, SQ.u(), Operators.CompareString(text2, SQ.y(), true) == 0, SQ.aI(), Operators.CompareString(text2, SQ.ad(), true) == 0, SQ.ac(), Operators.CompareString(text2, SQ.ab(), true) == 0, SQ.ai(), Operators.CompareString(text2, SQ.aI(), true) == 0, SQ.ah(), Operators.CompareString(text2, SQ.ai(), true) == 0, SQ.aC(), Operators.CompareString(text2, SQ.aJ(), true) == 0, SQ.aj(), Operators.CompareString(text2, SQ.aK(), true) == 0, SQ.s(), Operators.CompareString(text2, SQ.aA(), true) == 0, SQ.aJ(), Operators.CompareString(text2, SQ.aH(), true) == 0, SQ.aF(), Operators.CompareString(text2, SQ.aj(), true) == 0, SQ.aK(), true, text2)));
                            break;
                        case 1:
                            text = Conversions.ToString(Operators.ConcatenateObject(text, Interaction.Switch(Operators.CompareString(text2, SQ.s(), true) == 0, SQ.aK(), Operators.CompareString(text2, SQ.t(), true) == 0, SQ.aF(), Operators.CompareString(text2, SQ.u(), true) == 0, SQ.af(), Operators.CompareString(text2, SQ.v(), true) == 0, SQ.aI(), Operators.CompareString(text2, SQ.w(), true) == 0, SQ.Y(), Operators.CompareString(text2, SQ.x(), true) == 0, SQ.ad(), Operators.CompareString(text2, SQ.T(), true) == 0, SQ.ah(), Operators.CompareString(text2, SQ.Z(), true) == 0, SQ.aa(), Operators.CompareString(text2, SQ.z(), true) == 0, SQ.ai(), Operators.CompareString(text2, SQ.W(), true) == 0, SQ.ae(), Operators.CompareString(text2, SQ.aB(), true) == 0, SQ.V(), Operators.CompareString(text2, SQ.aC(), true) == 0, SQ.aI(), Operators.CompareString(text2, SQ.ac(), true) == 0, SQ.aF(), Operators.CompareString(text2, SQ.aD(), true) == 0, SQ.ag(), Operators.CompareString(text2, SQ.Y(), true) == 0, SQ.aG(), Operators.CompareString(text2, SQ.aE(), true) == 0, SQ.aJ(), Operators.CompareString(text2, SQ.X(), true) == 0, SQ.aB(), Operators.CompareString(text2, SQ.V(), true) == 0, SQ.af(), Operators.CompareString(text2, SQ.aF(), true) == 0, SQ.aE(), Operators.CompareString(text2, SQ.U(), true) == 0, SQ.U(), Operators.CompareString(text2, SQ.aG(), true) == 0, SQ.aC(), Operators.CompareString(text2, SQ.aa(), true) == 0, SQ.ai(), Operators.CompareString(text2, SQ.ae(), true) == 0, SQ.aH(), Operators.CompareString(text2, SQ.af(), true) == 0, SQ.X(), Operators.CompareString(text2, SQ.ag(), true) == 0, SQ.ah(), Operators.CompareString(text2, SQ.ah(), true) == 0, SQ.aK(), Operators.CompareString(text2, SQ.y(), true) == 0, SQ.ad(), Operators.CompareString(text2, SQ.ad(), true) == 0, SQ.ae(), Operators.CompareString(text2, SQ.ab(), true) == 0, SQ.Y(), Operators.CompareString(text2, SQ.aI(), true) == 0, SQ.ab(), Operators.CompareString(text2, SQ.ai(), true) == 0, SQ.aI(), Operators.CompareString(text2, SQ.aJ(), true) == 0, SQ.y(), Operators.CompareString(text2, SQ.aK(), true) == 0, SQ.aa(), Operators.CompareString(text2, SQ.aA(), true) == 0, SQ.aj(), Operators.CompareString(text2, SQ.aH(), true) == 0, SQ.ac(), Operators.CompareString(text2, SQ.aj(), true) == 0, SQ.aA(), true, text2)));
                            break;
                        case 2:
                            text = Conversions.ToString(Operators.ConcatenateObject(text, Interaction.Switch(Operators.CompareString(text2, SQ.s(), true) == 0, SQ.Z(), Operators.CompareString(text2, SQ.t(), true) == 0, SQ.ae(), Operators.CompareString(text2, SQ.u(), true) == 0, SQ.v(), Operators.CompareString(text2, SQ.v(), true) == 0, SQ.aB(), Operators.CompareString(text2, SQ.w(), true) == 0, SQ.af(), Operators.CompareString(text2, SQ.x(), true) == 0, SQ.aE(), Operators.CompareString(text2, SQ.T(), true) == 0, SQ.ag(), Operators.CompareString(text2, SQ.Z(), true) == 0, SQ.w(), Operators.CompareString(text2, SQ.z(), true) == 0, SQ.aG(), Operators.CompareString(text2, SQ.W(), true) == 0, SQ.aH(), Operators.CompareString(text2, SQ.aB(), true) == 0, SQ.T(), Operators.CompareString(text2, SQ.aC(), true) == 0, SQ.U(), Operators.CompareString(text2, SQ.ac(), true) == 0, SQ.V(), Operators.CompareString(text2, SQ.aD(), true) == 0, SQ.W(), Operators.CompareString(text2, SQ.Y(), true) == 0, SQ.X(), Operators.CompareString(text2, SQ.aE(), true) == 0, SQ.Y(), Operators.CompareString(text2, SQ.X(), true) == 0, SQ.y(), Operators.CompareString(text2, SQ.V(), true) == 0, SQ.t(), Operators.CompareString(text2, SQ.aF(), true) == 0, SQ.aI(), Operators.CompareString(text2, SQ.U(), true) == 0, SQ.ac(), Operators.CompareString(text2, SQ.aG(), true) == 0, SQ.ai(), Operators.CompareString(text2, SQ.aa(), true) == 0, SQ.ah(), Operators.CompareString(text2, SQ.ae(), true) == 0, SQ.aC(), Operators.CompareString(text2, SQ.af(), true) == 0, SQ.aj(), Operators.CompareString(text2, SQ.ag(), true) == 0, SQ.s(), Operators.CompareString(text2, SQ.ah(), true) == 0, SQ.aJ(), Operators.CompareString(text2, SQ.y(), true) == 0, SQ.aF(), Operators.CompareString(text2, SQ.ad(), true) == 0, SQ.aK(), Operators.CompareString(text2, SQ.ab(), true) == 0, SQ.aA(), Operators.CompareString(text2, SQ.aI(), true) == 0, SQ.aa(), Operators.CompareString(text2, SQ.ai(), true) == 0, SQ.x(), Operators.CompareString(text2, SQ.aJ(), true) == 0, SQ.ad(), Operators.CompareString(text2, SQ.aK(), true) == 0, SQ.aD(), Operators.CompareString(text2, SQ.aA(), true) == 0, SQ.u(), Operators.CompareString(text2, SQ.aH(), true) == 0, SQ.ab(), Operators.CompareString(text2, SQ.aj(), true) == 0, SQ.z(), true, text2)));
                            break;
                    }
                }
                return text;
            }
            return SQ.E();
        }

        private static string a(string P_0, string P_1, bool P_2, ref long P_3)
        {
            int num = 0;
            byte[] array = null;
            string text = SQ.E();
            string str = SQ.E();
            if (!(Operators.CompareString(P_1, SQ.E(), true) == 0 | (Operators.CompareString(P_1, SQ.ak(), true) == 0 & (Operators.CompareString(P_0, SQ.aL(), true) == 0 | Operators.CompareString(P_0, SQ.al(), true) == 0))))
            {
                global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0, ref text, ref str);
                string result;
                if (Operators.CompareString(text, SQ.E(), true) != 0)
                {
                    num = Strings.Len(text);
                    array = (byte[])Utils.CopyArray(array, new byte[1]);
                    _S.GE(Encoding.UTF8.GetBytes(text), ref array, P_1);
                    text = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(array);
                    if (num > 8 & P_2)
                    {
                        text = global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.a(text);
                    }
                    text = Strings.Mid(text, 1, num);
                    result = str + text;
                } else
                {
                    result = P_0;
                }
                P_3 = num;
                return result;
            }
            return global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.A(P_0);
        }

        /// <summary>
        /// 获取上机人员字段中的密码
        /// </summary>
        /// <param name="P_0"></param>
        /// <returns></returns>
        internal static string GetTableFldPwd(string P_0)
        {
            P_0 = P_0.Trim().ToUpper();
            Sm4Context sm4Context = new Sm4Context();
            sm4Context.isPadding = true;
            sm4Context.mode = 1;
            byte[] key = Hex.Decode(global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.c);
            Sm4 sm = new Sm4();
            sm.sm4_setkey_enc(sm4Context, key);
            byte[] bytes = Encoding.Default.GetBytes(P_0);
            int num = bytes.Length;
            int num2 = 16;
            if (num > 16 && num <= 32)
            {
                num2 = 32;
            }
            if (num > 32 && num <= 48)
            {
                num2 = 48;
            }
            if (num < num2)
            {
                int num3 = num2 - num;
                string text = SQ.E();
                for (int i = 0; i < num3; i++)
                {
                    text += SQ.e();
                }
                P_0 += text;
                bytes = Encoding.Default.GetBytes(P_0);
            }
            byte[] data = sm.sm4_crypt_ecb(sm4Context, bytes);
            string @string = Encoding.Default.GetString(Hex.Encode(data));
            return SQ.aM() + ((@string != null) ? @string.ToUpper() : null);
        }

        private static bool a(OracleConnection P_0)
        {
            if (!global::ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.C.m_A)
            {
                string commandText = SQ.am();
                try
                {
                    OracleCommand val = new OracleCommand();
                    object obj = default(object);
                    try
                    {
                        val.Connection = P_0;
                        ((DbCommand)val).CommandText = commandText;
                        obj = ((DbCommand)val).ExecuteScalar();
                    } finally
                    {
                        if ((object)val != null)
                        {
                            ((Component)val).Dispose();
                        }
                    }
                    if (obj != null)
                    {
                        string text = obj.ToString();
                        if (text.Length >= 5)
                        {
                            return Operators.CompareString(Strings.Mid(text, 1, 5), SQ.aN(), true) < 0 | Operators.CompareString(text, SQ.an(), true) == 0;
                        }
                    }
                } catch
                {
                    return false;
                }
                return false;
            }
            return true;
        }

        private static string a(string P_0)
        {
            int num = (int)Math.Round((double)P_0.Length / 2.0);
            string text = P_0.Substring(0, num);
            string text2 = P_0.Substring(num);
            string text3 = SQ.E();
            for (int i = 0; i < num; i++)
            {
                string str = text3;
                char c = text[i];
                string str2 = c.ToString();
                c = text2[num - i - 1];
                text3 = str + str2 + c.ToString();
            }
            return text3;
        }

        private static string A(byte[] P_0)
        {
            string text = SQ.E();
            int num = (P_0 != null) ? P_0.Length : 0;
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    string text2 = Conversion.Hex(P_0[i]);
                    text = text + ((text2.Length == 1) ? SQ.s() : SQ.E()) + text2;
                }
            }
            return text;
        }

        private static void A(string P_0, ref string P_1, ref string P_2)
        {
            P_1 = SQ.E();
            P_2 = SQ.E();
            for (int i = 0; i < P_0.Length; i++)
            {
                char value = P_0[i];
                int num = Convert.ToInt32(value);
                if ((num > 47 & num < 58) | (num > 64 & num < 91) | (num > 96 & num < 123))
                {
                    P_1 += value.ToString();
                } else
                {
                    P_2 += value.ToString();
                }
            }
        }
    }
}

namespace ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.UserAccount
{
    /// <summary>
    /// ZLHIS 密码修改辅助类,用于得到加密后的密码以及更新ORACLE数据库中密码
    /// </summary>
    public static class zlhisAuthHRS
    {

        /// <summary>
        /// 获取上机人员表-登陆密码 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetTableFldPwd(string password)
        {
            return C.GetTableFldPwd(password);//thePwd为加密后的密码 上机人员表.登陆密码 
        }

        /// <summary>
        /// 获取后端的登陆密码
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public static string GetPLSQLRegPwd(string account, string newPwd)
        {
            return C.TranPwd(account, newPwd);
        }
    }
}


namespace FlowRunCode
{
    public class Fa1a148e33e2e78998737885bd24e5536 : IFlowRun
    {
        public string FlowId => "a1a148e3-3e2e-7899-8737-885bd24e5536";
        public ServiceSettingBase ServiceSetting { get; set; } = new FunctionServiceSetting();
        public Fa1a148e33e2e78998737885bd24e5536()
        {
        }
        public async Task<FlowFeedBackInfo> Call(string bodyNode, Dictionary<string, object> parameter)
        {
            FlowContext context = new FlowContext(FlowId, bodyNode, parameter);
            FlowFeedBackInfo feedBackInfo = new FlowFeedBackInfo();
            try
            {
                await fa1a148e36db57fd5b5473631e963d5e5(context);
                feedBackInfo.ReturnMsg = context.ReturnMsg;
            } catch (Exception ex)
            {
                feedBackInfo.ReturnMsg = ex.Message;
                feedBackInfo.IsError = true;
            }
            feedBackInfo.Parameter = parameter;

            return feedBackInfo;
        }
        private async Task fa1a148e36db57fd5b5473631e963d5e5(FlowContext context)
        {
            await this.fa1a148e38b6c7df681080b8a73739fdf(context);
        }
        private async Task fa1a148e38b6c7df681080b8a73739fdf(FlowContext context)
        {


            object ReadArg(string key)
            {
                try
                {
                    object bodyValue = null;
                    try
                    {
                        bodyValue = context.Body?[key];
                    } catch (Exception exBody)
                    {
                        throw new Exception("读取 context.Body 参数失败，参数名：" + key + "。详细：" + exBody.Message, exBody);
                    }

                    if (bodyValue != null) return bodyValue;

                    try
                    {
                        return context[key];
                    } catch (Exception exCtx)
                    {
                        throw new Exception("读取 context 参数失败，参数名：" + key + "。详细：" + exCtx.Message, exCtx);
                    }
                } catch (Exception ex)
                {
                    throw new Exception("读取入参失败，参数名：" + key + "。详细：" + ex.Message, ex);
                }
            }

            string ToStr(object value)
            {
                return value == null ? null : value.ToString();
            }

            JsonArray ToArray(object value, string fieldName)
            {
                if (value == null)
                {
                    throw new Exception("参数[" + fieldName + "] 不能为空。需要数组。");
                }

                if (value is JsonArray ja) return ja;
                if (value is JsonNode jn && jn is JsonArray jna) return jna;

                string s = value.ToString();
                if (string.IsNullOrWhiteSpace(s))
                {
                    throw new Exception("参数[" + fieldName + "] 不能为空字符串。需要数组。");
                }

                try
                {
                    JsonNode parsed = JsonNode.Parse(s);
                    if (parsed is JsonArray pja) return pja;
                } catch
                {
                }

                throw new Exception("参数[" + fieldName + "] 不是有效数组。");
            }

            string GetObjValue(JsonObject obj, string rawKey)
            {
                if (obj == null || string.IsNullOrWhiteSpace(rawKey)) return "";

                if (obj[rawKey] != null)
                {
                    return obj[rawKey].ToString().Trim();
                }

                string target = rawKey.Trim();
                foreach (KeyValuePair<string, JsonNode> kv in obj)
                {
                    if ((kv.Key ?? "").Trim().Equals(target, StringComparison.OrdinalIgnoreCase))
                    {
                        return kv.Value == null ? "" : kv.Value.ToString().Trim();
                    }
                }

                return "";
            }


            string sql = "";
            string pwd = "123"; ////前端为123   ZLSV2:BADB708754765124CC4F7E321AF849CA
            string bkPwd = "ZLSV2:BADB708754765124CC4F7E321AF849CA";
            string strGrantRole = "ZL_LW测试角色A,ZL_住院医生";
            int userid = 1486057;
            int pwdDays = 30;

            strGrantRole = "";

            string userName = ToStr(ReadArg("user_name"));
            string strOwner = ToStr(ReadArg("sys_user_name"));
            string strDevName = ToStr(ReadArg("dev_name"));
            string strUserid = ToStr(ReadArg("user_id"));
            userid = Convert.ToInt32(strUserid);

            JsonArray roleList = ToArray(ReadArg("role_list"), "role_list");


            for (int i = 0; i < roleList.Count; i++)
            {
                JsonObject row = roleList[i] as JsonObject;
                if (row == null) continue;
                string item = GetObjValue(row, "name");
                if (string.IsNullOrWhiteSpace(item)) continue;
                strGrantRole += item + ",";
            }

            if (string.IsNullOrWhiteSpace(strGrantRole))
            {
                return;
            }

            int type = 1;

            var access = DBHelpeFactory.New(@"sys_inner_zlhis_admin");

            bkPwd = ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.UserAccount.zlhisAuthHRS.GetTableFldPwd(pwd);//得到上机人员中字段的密码
            pwd = ZLSoftHRSFa1a148e33e2e78998737885bd24e5536.UserAccount.zlhisAuthHRS.GetPLSQLRegPwd(userName, pwd);//得到ORACLE后端的密码

            //先删除用户
            sql = $@"drop user {userName} cascade";
            try { await access.ExecuteNonQuery(sql); } catch (Exception) { }

            sql = $@"CREATE USER {userName} IDENTIFIED BY ""{pwd}""
                    DEFAULT TABLESPACE USERS TEMPORARY TABLESPACE TEMP";
            //创建用户
            await access.ExecuteNonQuery(sql);
            //授权
            sql = $@"Grant Connect,Alter Session,Create Session,Create Synonym,Create Table,Create View,Create Sequence,Create Database Link,Create Cluster to {userName}";
            await access.ExecuteNonQuery(sql);

            //更新表空间 
            sql = $@"Alter User {userName} Default Tablespace USERS";
            await access.ExecuteNonQuery(sql);

            sql = $@"Alter User {userName} Temporary Tablespace TEMP";
            await access.ExecuteNonQuery(sql);

            sql = "delete 上机人员表 a where a.用户名=:用户名_In";
            await access.ExecuteNonQuery(sql,
                new CustomSqlParam(":用户名_In", SqlParamDataType.字符串, userName)
                );

            //添加上机人员表
            sql = "zltools.Zlmanuser_Edit";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("Type_In", SqlParamDataType.数字, 1),
                new CustomSqlParam("Owner_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("人员id_In", SqlParamDataType.数字, userid),
                new CustomSqlParam("密码效天_In", SqlParamDataType.数字, pwdDays));

            sql = "update 上机人员表 set 登录密码=:P1 where 用户名=:用户名_In";
            await access.ExecuteNonQuery(sql,
                new CustomSqlParam(":P1", SqlParamDataType.字符串, bkPwd),
                new CustomSqlParam(":用户名_In", SqlParamDataType.字符串, userName)
                );

            //删除角色
            sql = "zltools.Zl_Zluserroles_Del";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("User_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("Role_In", SqlParamDataType.字符串, DBNull.Value));

            //角色授权
            string[] arrGranRole = strGrantRole.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arrGranRole)
            {
                sql = $@"Grant {item} to {userName}";
                await access.ExecuteNonQuery(sql);

                sql = "zltools.Zl_Zluserroles_Add";
                await access.ExecuteProcedure(sql, 0, false,
                    new CustomSqlParam("User_In", SqlParamDataType.字符串, userName),
                    new CustomSqlParam("Role_In", SqlParamDataType.字符串, item));
            }

            string str操作说明 = $@"{userName}";
            if (!string.IsNullOrWhiteSpace(strGrantRole))
            {
                str操作说明 += $@" 添加的角色为：{string.Join(",", arrGranRole)}";
            }

            //记录日志
            sql = "zltools.Zl_Zlauditlog_Insert";
            await access.ExecuteProcedure(sql, 0, false,
                new CustomSqlParam("用户名_In", SqlParamDataType.字符串, strOwner),
                new CustomSqlParam("工作站_In", SqlParamDataType.字符串, strDevName),
                new CustomSqlParam("操作类型_In", SqlParamDataType.数字, 1),
                new CustomSqlParam("系统_In", SqlParamDataType.数字, DBNull.Value),
                new CustomSqlParam("模块_In", SqlParamDataType.字符串, "0402"),
                new CustomSqlParam("功能_In", SqlParamDataType.字符串, "新增用户"),
                new CustomSqlParam("操作内容_In", SqlParamDataType.字符串, userName),
                new CustomSqlParam("操作说明_In", SqlParamDataType.字符串, str操作说明)
            );

            JsonObject outData = new JsonObject { ["result"] = 1 };
            context.SetParamValue("output", outData);
            //返回结果
            context.ReturnMsg = context.GetParamValue("output");

            await this.fa1a148e375a47e3fad8ef087476861da(context);
        }
        private async Task fa1a148e375a47e3fad8ef087476861da(FlowContext context)
        {
        }
    }
}