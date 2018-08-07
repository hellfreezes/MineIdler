using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money {
    int step;

    float[] values;

    const int MAX = 999999999;

    public float[] Values
    {
        get { return values; }
    }

    public int Step
    {
        get { return Step; }
    }

    public Money(float value, int step)
    {
        this.values = new float[1000];
        this.values[step] = value;
        this.step = step;
        if (step > 0)
        {
            for (int i = 0; i < step; i++)
            {
                this.values[i] = MAX;
            }
        }
    }

    public void Plus(float v)
    {
        float result = values[step] + v;
        if (result > MAX)
        {
            values[step] = MAX;
            values[++step] = result - MAX;
        }
    }

    public void Plus(Money v)
    {
        int counter = 0;
        float r = 0;
        float a = 0;
        while (counter <= Mathf.Max(step, v.Step))
        {
            a = 0;
            values[counter] += r;
            if (values[counter] > MAX)
            {
                a = MAX - values[counter];
            }
            r = (MAX - values[counter]) + (MAX - v.Values[counter]) + a;
            counter++;
            if (counter > step && r > 0)
                step++;
        }

    }
}
