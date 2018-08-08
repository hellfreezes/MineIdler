using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Нулевой массив хранит MAX_VALUE (по идеи это будет 999,999,999.99 - 999 миллионов)
 * Каждый следующий хранит 999 следующего поколения чисел. Например 999 миллиардов будет
 * хранить первый элемент массива, 999 триллионов второй и так далее. Но у каждого следующего
 * элемента после точки (дробная часть) будут хранится две пары чисел из предыдущей. Например:
 * миллионы 999'999'999.99 - еще копейка и миллиард
 * милларды 1.000'000 - первый 000 - это миллионы, второй 000 - это тысячи
 * триллионы 1.000'000 - перый 000 - это миллиарды, второй 000 - это миллионы
 * 
 * Операции сложения должны определять покаления складываемых чисел и последовательно увеличивать 
 * их начиная с нулевого покаления. Остальные операции по аналогии со сложением
 */
public class Money {
    int step;

    double[] values;

    string[] name = new string[] { "", "миллиардов", "триллионов" };
    const double MAX_VALUE0 = 999999999.99;
    const double MAX_VALUE1 = 999.999999;
    const int MAX_STEP = 1000;

    public double[] Values
    {
        get { return values; }
    }

    public int Step
    {
        get { return step; }
    }

    public Money(double value, int step)
    {
        this.values = new double[MAX_STEP];
        this.step = step;

        this.values[0] = MAX_VALUE0;
        if (step > 0)
        {
            for (int i = 1; i < step; i++)
            {
                this.values[i] = MAX_VALUE1;
            }
        }
        this.values[step] = value;
    }

    public void Plus(Money v)
    {
        double rest = 0;
        for (int i = 0; i < MAX_STEP; i++)
        {
            if (rest == 0 && values[i] == 0 && v.values[i] == 0)
            {
                break;
            }
            values[i] += v.values[i];
            if (i > 0 && values[i] > MAX_VALUE0)
            {
                rest = values[i] - MAX_VALUE0;
                values[i] = MAX_VALUE0;
                //rest = rest / 100000000; неправильно
            }
        }
    }

    //public override string ToString()
    //{
    //    //string pSpecifier = String.Format("{0}{1}", "N", 0);
    //    //return values[step].ToString(pSpecifier) + " " + name[step];
    //}
}
