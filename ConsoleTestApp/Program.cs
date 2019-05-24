﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Testing...");

            TestLoading.Load();
            TestGetFinancialEntities.GetFinancialEntities();

            System.Console.WriteLine("Testing complete!");

            Console.ReadLine();
        }
    }
}