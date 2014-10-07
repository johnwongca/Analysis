﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace Algorithm.Core
{
    public class IndicatorClass
    {
        public static List<IndicatorClass> IndicatorClasses = new List<IndicatorClass>();
        public static void LoadIndicatorClasses()
        {
            Console.WriteLine("Start");
            IndicatorClasses.Clear();
            IndicatorClasses.AddRange(
                    Assembly.GetExecutingAssembly().GetTypes().Where(
                        x =>
                        {
                            if (x.IsSubclassOf(typeof(Indicator)))
                            {
                                return x.GetCustomAttributes(true).FirstOrDefault(y => y is IndicatorAttribute) != null;
                            }
                            return false;
                        }).Select(
                        x =>
                        {
                            IndicatorClass ret = new IndicatorClass() { Class = x, IndicatorName = x.Name.Substring(0, x.Name.Length - 9) };
                            foreach (var attr in x.GetCustomAttributes(true))
                            {
                                if (attr is DescriptionAttribute)
                                {
                                    ret.Description = ((DescriptionAttribute)attr).Description;
                                    break;
                                }
                            }
                            return ret;
                        }
                        ).OrderBy(x => x.IndicatorName).ToArray()
               );
        }
        public string IndicatorName;
        public Type Class;
        public string Description;
    }
}
