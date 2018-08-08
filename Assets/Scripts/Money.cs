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

    float[] values;

    string[] name = new string[] { "", "миллиардов", "триллионов" };
    const float MAX_VALUE = 999.999999f;
    const int MAX_STEP = 1000;

    public float[] Values
    {
        get { return values; }
    }

    public int Step
    {
        get { return step; }
    }

    public Money(float value, int step)
    {
        Debug.LogError("Неправильно создаются объекты. При создании если step больше нуля надо дробную часть откидывать в другое поколение");
        if (value > MAX_VALUE)
        {
            Debug.LogError("Значение поколения не может превышать " + MAX_VALUE + ". Присвоено максимальное значение.");
            value = MAX_VALUE;
        }

        this.values = new float[MAX_STEP];
        this.step = step;

        this.values[step] = value;
    }

    public void Mult(Money v)
    {
        float rest = 0;
        int maxStep = Mathf.Max(step, v.Step);
        for (int i = 0; i <= maxStep; i++)
        {
            step = i; // динамическое увеличение покаления

            values[i] += v.values[i] + rest; // складываем одинаковые поколения чисел и прибавляем остаток с предыдущего поколения
            rest = 0; // Обнуляем остаток
            if (values[i] > MAX_VALUE) // Формируем остаток если поколение вышло за пределы допустимого
            {
                rest = Mathf.Floor(values[i] / Mathf.Pow(10, 3)); // этот остаток пойдет в новое поколение
                values[i] = values[i] - MAX_VALUE; // это значение остается в текущем поколении
            }
            if (rest != 0 && i == maxStep) // Если обработали все поколения складываемых объектов, а остаток еще есть
                maxStep++; // Увеличиваем поколение объекта, чтобы перенести в него остаток
        }
    }

    public void Subt(Money v)
    {
        int maxStep = Mathf.Max(step, v.Step);
        for (int i = maxStep; i >= 0; i--)
        {
            values[i] -= v.Values[i];
            if (values[i] < 0)
            {
                values[i + 1]--;
                values[i] = 1000 - Mathf.Abs(values[i]);
                if (values[i + 1] == 0)
                    step--;
            }
        }
    }

    public void Add(float index)
    {
        float rest = 0;
        int maxStep = step;
        for (int i = 0; i <= maxStep; i++)
        {
            step = i; // динамическое увеличение покаления

            values[i] = (values[i] * index) + rest; // умножаем последовательно каждое поколение и прибавляем остаток с предыдущего поколения
            rest = 0; // Обнуляем остаток
            if (values[i] > MAX_VALUE) // Формируем остаток если поколение вышло за пределы допустимого
            {
                rest = Mathf.Floor(values[i] / Mathf.Pow(10, 3)); // этот остаток пойдет в новое поколение
                values[i] = GetMaxValueInStep(values[i]); // это значение остается в текущем поколении
            }
            if (rest != 0 && i == maxStep) // Если обработали все поколения складываемых объектов, а остаток еще есть
                maxStep++; // Увеличиваем поколение объекта, чтобы перенести в него остаток
        }
    }

    public void Div(float index)
    {
        int maxStep = step;
        float rest = 0;
        float un = 0;
        for (int i = maxStep; i >= 0; i--)
        {
            values[i] = (values[i]/index) + rest;
            un = (float)Math.Truncate(values[i]);
            rest = values[i] - un;
            if (rest > 0) // Нужно не на ноль проверять а на остаток после запятой!!!!
            {
                rest *= 1000;
                values[i] = un;

                if (un == 0)
                {
                    step--;
                }
            }
        }
    }

    float GetMaxValueInStep(float val)
    {
        string s = val.ToString();
        int i = s.IndexOf(".");
        i = i == -1 ? s.Length : i;
        s = s.Remove(0, i - 3);
        return float.Parse(s);
    }

    public string GetValue()
    {
        double v = values[Step];
        if (Step > 0)
            v += values[Step - 1] / Mathf.Pow(10, 3);
        if (Step > 1)
            v += values[Step - 2] / Mathf.Pow(10, 6);
        return v.ToString() + " поколение №" + Step; 
    }

    //public override string ToString()
    //{
    //    //string pSpecifier = String.Format("{0}{1}", "N", 0);
    //    //return values[step].ToString(pSpecifier) + " " + name[step];
    //}
}
