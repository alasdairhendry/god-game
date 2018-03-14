using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System;
using System.Collections.Generic;

public static class CustomHelper {

    public static string GetDescription(Enum en)
    {
        Type type = en.GetType();

        MemberInfo[] memInfo = type.GetMember(en.ToString());

        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return en.ToString();
    }

    public static Color NewColor(int R, int G, int B)
    {
        Color _color;

        _color = new Color(R / 255.0f, G / 255.0f, B / 255.0f);

        return _color;
    }

    public static Color NewColor(int R, int G, int B, int A)
    {
        Color _color;

        _color = new Color(R / 255.0f, G / 255.0f, B / 255.0f, A / 255.0f);

        return _color;
    }

    public static void Update(this List<Attribute> attributes, Attribute.AttributeKey key, string value)
    {
        foreach (Attribute a in attributes)
        {
            if (a.Key == key)
            {
                if (a.Type == Attribute.AttributeType.STRING)
                    a.ChangeAttribute(value.ToString());

                if (a.Type == Attribute.AttributeType.INT)
                    a.ChangeAttribute(int.Parse(value));

                if (a.Type == Attribute.AttributeType.FLOAT)
                    a.ChangeAttribute(float.Parse(value));
            }
        }
    }

    public static void Update(this List<Attribute> attributes, Attribute.AttributeKey key, int value)
    {
        foreach (Attribute a in attributes)
        {
            if (a.Key == key)
            {
                if (a.Type == Attribute.AttributeType.STRING)
                    a.ChangeAttribute(value.ToString());

                if (a.Type == Attribute.AttributeType.INT)
                    a.ChangeAttribute(value);

                if (a.Type == Attribute.AttributeType.FLOAT)
                    a.ChangeAttribute((float)value);
            }
        }
    }

    public static void Update(this List<Attribute> attributes, Attribute.AttributeKey key, float value)
    {
        foreach (Attribute a in attributes)
        {
            if (a.Key == key)
            {
                if (a.Type == Attribute.AttributeType.STRING)
                    a.ChangeAttribute(value.ToString());

                if (a.Type == Attribute.AttributeType.INT)
                    a.ChangeAttribute((int)value);

                if (a.Type == Attribute.AttributeType.FLOAT)
                    a.ChangeAttribute(value);
            }
        }
    }

    public static string FindValueByName(this List<Attribute> attributes, string name)
    {
        foreach (Attribute a in attributes)
        {
            if (a.DisplayName == name)
                return a.GetValue();
        }

        return "Attribute Not Found";
    }

    public static Attribute FindAttributeByKey(this List<Attribute> attributes, Attribute.AttributeKey key)
    {
        foreach (Attribute a in attributes)
        {
            if (a.Key == key)
                return a;
        }

        return null;
    }

    public static string PluraliseString(this int value, string singular, string plural)
    {        
        if (value == 1)
            return singular;
        else return plural;
    }

    public static string Commafy<T>(this List<T> list)
    {
        if (list.Count > 0)
        {
            string output = "";

            if (list.Count == 1)
            {
                output += list[0];
            }
            else if (list.Count == 2)
            {
                output += list[0] + ", ";
                output += list[1];
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == list.Count - 1)
                    {
                        output += list[i];
                    }
                    else
                    {
                        output += list[i] + ", ";
                    }
                }
            }

            return output;
        }
        else return "List Empty";
    }
}
