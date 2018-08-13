using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
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
public class Money : IXmlSerializable
{
    // -- xml
    int step;
    float[] values;
    // -- xml

    string[] name = new string[] { "", "a", "b", "ab", "bb", "c", "ac", "aac", "bc", "abc", "aabc", "bbc" };
    const float MAX_VALUE = 999.999999f; // максимальное значения числа в поколении
    const int ZEROS = 3; // количество отбрасываемых нулей
    const int MAX_STEP = 1000; // количество поколений

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
        this.values = new float[MAX_STEP];
        this.step = step;

        //Debug.LogError("Неправильно создаются объекты. При создании если step больше нуля надо дробную часть откидывать в другое поколение");
        if (value > MAX_VALUE)
        {
            Debug.LogError("Значение поколения не может превышать " + MAX_VALUE + ". Присвоено максимальное значение.");
            value = MAX_VALUE;
        }

        // Если число больше стартового поколения и у этого числа есть дробная часть
        // То надо перебросить дробную часть на более ранние поколения
        if (step > 0 && (value - Math.Truncate(value))>0)
        {
            float un; // целая часть
            float rest = value; // дробная часть
            int counter = 2; // счетчик перебросов (не более двух поколений)
            for (int i = step; i >= 0; i--)
            {
                un = (float)Math.Truncate(rest);
                rest = rest - un;

                values[i] = un;

                rest = rest * Mathf.Pow(10, ZEROS);

                counter--;
                if (counter == 0)
                    break;
            }
        }
        else // иначе просто присваеваем нулевому поколению значение
        {
            this.values[step] = value;
        }
    }

    public Money Clone()
    {
        Money newM = (new Money(0, 0)).Add(this);
        return newM;
    }

    /// <summary>
    /// Сложение
    /// </summary>
    public Money Add(Money v)
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
                rest = Mathf.Floor(values[i] / Mathf.Pow(10, ZEROS)); // этот остаток пойдет в новое поколение
                values[i] = values[i] - MAX_VALUE; // это значение остается в текущем поколении
            }
            if (rest != 0 && i == maxStep) // Если обработали все поколения складываемых объектов, а остаток еще есть
                maxStep++; // Увеличиваем поколение объекта, чтобы перенести в него остаток
        }

        return this;
    }

    /// <summary>
    /// Сложение с числом
    /// </summary>
    public Money Add(float val)
    {
        float rest = 0;
        int maxStep = 0;
        for (int i = 0; i <= maxStep; i++)
        {
            step = i; // динамическое увеличение покаления

            values[i] += val + rest; // складываем одинаковые поколения чисел и прибавляем остаток с предыдущего поколения
            rest = 0; // Обнуляем остаток
            if (values[i] > MAX_VALUE) // Формируем остаток если поколение вышло за пределы допустимого
            {
                rest = Mathf.Floor(values[i] / Mathf.Pow(10, ZEROS)); // этот остаток пойдет в новое поколение
                values[i] = values[i] - MAX_VALUE; // это значение остается в текущем поколении
            }
            if (rest != 0 && i == maxStep) // Если обработали все поколения складываемых объектов, а остаток еще есть
                maxStep++; // Увеличиваем поколение объекта, чтобы перенести в него остаток
        }

        return this;
    }

    /// <summary>
    /// Вычитание
    /// </summary>
    public Money Subt(Money v)
    {
        int maxStep = Mathf.Max(step, v.Step);
        for (int i = maxStep; i >= 0; i--) // Проверяем все поколения начиная с самого молодого
        {
            values[i] -= v.Values[i]; // Вычитаем из одинаковые поколения
            if (values[i] < 0) // Если получили отрицательное значение, значит нужно уменьшить на 1 более раннее поколение и 
                               // вычесть остаток из текущего раннего поколения
            {
                values[i + 1]--;
                values[i] = Mathf.Pow(10, ZEROS) - Mathf.Abs(values[i]);
                if (values[i + 1] == 0)
                    step--;
            }
        }

        return this;
    }

    /// <summary>
    /// Уможение
    /// </summary>
    public Money Mult(float index)
    {
        float rest = 0;
        int maxStep = step;
        for (int i = 0; i <= maxStep; i++)
        {
            step = i; // динамическое увеличение поколения

            values[i] = (values[i] * index) + rest; // умножаем последовательно каждое поколение и прибавляем остаток с предыдущего поколения
            rest = 0; // Обнуляем остаток
            if (values[i] > MAX_VALUE) // Формируем остаток если поколение вышло за пределы допустимого
            {
                rest = Mathf.Floor(values[i] / Mathf.Pow(10, ZEROS)); // этот остаток пойдет в новое поколение
                values[i] = GetMaxValueInStep(values[i]); // это значение остается в текущем поколении
            }
            if (rest != 0 && i == maxStep) // Если обработали все поколения складываемых объектов, а остаток еще есть
                maxStep++; // Увеличиваем поколение объекта, чтобы перенести в него остаток
        }

        return this;
    }

    /// <summary>
    /// Деление на число
    /// </summary>
    public Money Div(float index)
    {
        int maxStep = step;
        float rest = 0;
        float un = 0;
        for (int i = maxStep; i >= 0; i--)
        {
            values[i] = (values[i]/index) + rest;
            un = (float)Math.Truncate(values[i]);
            rest = values[i] - un;
            if (rest > 0)
            {
                rest *= Mathf.Pow(10, ZEROS);
                values[i] = un;

                if (un == 0)
                {
                    step--;
                }
            }
        }

        return this;
    }

    /// <summary>
    /// Деление на объект
    /// </summary>
    public float Div(Money v)
    {
        int s = step - v.Step; //Разница поколений
        if (s > 0)
        {
            float d2 = GetValueWithPoint(v) / Mathf.Pow(10, ZEROS * s);
            float d1 = GetValueWithPoint(this);

            return d1 / d2;
        } else if (s == 0)
        {
            float d2 = GetValueWithPoint(v);
            float d1 = GetValueWithPoint(this);

            return d1 / d2;
        } else
        {
            float d2 = GetValueWithPoint(v) * Mathf.Pow(10, ZEROS * Mathf.Abs(s));
            float d1 = GetValueWithPoint(this);

            //Debug.Log(d1 + " / " + d2);

            return d1 / d2;
        }
    }

    public bool IsEqual(Money v)
    {
        float d2 = GetValueWithPoint(v);
        float d1 = GetValueWithPoint(this);
        return (step == v.step && d1 == d2);
    }

    public bool IsLessThen(Money v)
    {
        float d2 = GetValueWithPoint(v);
        float d1 = GetValueWithPoint(this);

        return (step < v.Step) || (step == v.Step && d1 < d2); 
    }

    public bool IsGreaterThen(Money v)
    {
        float d2 = GetValueWithPoint(v);
        float d1 = GetValueWithPoint(this);

        return (step > v.Step) || (step == v.Step && d1 > d2);
    }

    /// <summary>
    /// Возвращает максимальную цифру допутимую в поколении, 
    /// отсекая слева всё, что больше для будущего переноса в следующеее поколение
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    float GetMaxValueInStep(float val)
    {
        string s = val.ToString();
        int i = s.IndexOf(".");
        i = i == -1 ? s.Length : i;
        s = s.Remove(0, i - ZEROS);
        return float.Parse(s);
    }

    /// <summary>
    /// Возвращает цифру отформатированную след образом:
    /// Целое число поколение и до двух поколений после запятой
    /// 999.999 999
    /// </summary>
    float GetValueWithPoint(Money val)
    {
        float v = val.values[val.Step];
        if (val.Step > 0)
            v += val.values[val.Step - 1] / Mathf.Pow(10, ZEROS);
        if (val.Step > 1)
            v += val.values[val.Step - 2] / Mathf.Pow(10, ZEROS * 2);
        return v;
    }

    /// <summary>
    /// Возвращает отформатированное значение
    /// </summary>
    /// <returns></returns>
    public string GetValue()
    {
        double v = values[Step];
        if (Step > 0)
            v += values[Step - 1] / Mathf.Pow(10, ZEROS);
        if (Step > 1)
            v += values[Step - 2] / Mathf.Pow(10, ZEROS * 2);
        return v.ToString() + " поколение №" + Step; 
    }

    public override string ToString()
    {
        //string pSpecifier = String.Format("{0}{1}", "N", 0);
        //return values[step].ToString(pSpecifier) + " " + name[step];

        double v = values[Step];
        if (Step > 0)
            v += values[Step - 1] / Mathf.Pow(10, ZEROS);
        if (Step > 1)
            v += values[Step - 2] / Mathf.Pow(10, ZEROS * 2);

        string format = "#.###";

        switch (Step) {
            case 0:
                v *= 1000;
                format = "#.##";
                break;
        }

        return v.ToString(format) + " " + name[Step];// + " поколение №" + Step;
    }

    #region IXmlSerializable
    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        values = new float[MAX_STEP];

        step = int.Parse(reader.GetAttribute("Step"));

        if (reader.ReadToFollowing("Value"))
        {
            do
            {
                int s = int.Parse(reader.GetAttribute("step"));
                values[s] = float.Parse(reader.GetAttribute("value"));

            } while (reader.ReadToNextSibling("Value"));
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Step", Step.ToString());
        writer.WriteStartElement("Values");
        for (int i = Step; i >= 0; i--)
        {
            writer.WriteStartElement("Value");
            writer.WriteAttributeString("step", i.ToString());
            writer.WriteAttributeString("value", values[i].ToString());
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
    #endregion
}
