﻿using System;

public class StartUp
{
    public static void Main()
    {
        string[] phoneNumbers = Console.ReadLine().Split(new[] { ' ' });
        string[] webUrls = Console.ReadLine().Split(new[] { ' ' });

        Smartphone phone = new Smartphone(phoneNumbers, webUrls);
        Console.Write(phone.ToString());
    }
}