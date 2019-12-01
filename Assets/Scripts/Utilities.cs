using System;
using System.Collections;

public static class Utilities
{
    public static int Modulo(float a, float b) {
        return (int) (a - b * Math.Floor(a / b));
    }
}