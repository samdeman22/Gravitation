using UnityEngine;
using System.Collections;

public class sfloat{

    public float mantissa, exponent;

    public sfloat (float mantissa, float exponent)
    {
        this.mantissa = mantissa;
        this.exponent = exponent;
    }

    // a*10^b + c*10^d = (a + c * 10 ^ (b-d))*10 ^ b
    public static sfloat operator +(sfloat a, sfloat b)
    {
        //assign the largest exponent as the new exponent
        if (Mathf.Abs(a.exponent) >= Mathf.Abs(b.exponent))
        {
            return new sfloat(a.mantissa + b.mantissa * Mathf.Pow(10, a.exponent - b.exponent), a.exponent);
        }
        else
        {
            return new sfloat(a.mantissa * Mathf.Pow(10, b.exponent - a.exponent) + b.mantissa, b.exponent);
        }
    }

    public static sfloat operator -(sfloat a, sfloat b)
    {
        //assign the largest exponent as the new exponent
        if (Mathf.Abs(a.exponent) >= Mathf.Abs(b.exponent))
        {
            return new sfloat(a.mantissa - b.mantissa * Mathf.Pow(10, a.exponent - b.exponent), a.exponent);
        }
        else
        {
            return new sfloat(a.mantissa * Mathf.Pow(10, b.exponent - a.exponent) - b.mantissa, b.exponent);
        }
    }

    public static sfloat operator *(sfloat a, sfloat b)
    {
        return new sfloat(a.mantissa * b.mantissa, a.exponent + b.exponent);
    }

    public static sfloat operator /(sfloat a, sfloat b)
    {
        return new sfloat(a.mantissa / b.mantissa, a.exponent - b.exponent);
    }

    // (a*10^b) ^ (c) = (a^c)*10^(b*c)
    public static sfloat operator ^(sfloat a, float b)
    {
        return new sfloat(Mathf.Pow(a.mantissa, b), a.exponent * b);
    }

    public string ToSting()
    {
        return mantissa + " * 10 ^ " + exponent;
    }
}
