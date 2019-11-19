using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparat
{
    public class Solution
    {
        SolutionElement[] solutions;
        public SolutionElement this[int index]
        {
            get
            {
                return solutions[index];
            }
        }
        public Solution(int count)
        {
            solutions = new SolutionElement[count];
            for (int i = 0; i < count; i++)
            {
                solutions[i] = new SolutionElement();
            }
        }
        public int Count
        {
            get
            {
                return solutions.Length;
            }
        }
    }

    public class SolutionElement
    {
        public FarFieldC ffantenna;
        public FarFieldC ffradome;
        public FarFieldC ffreflactionWithoutTransition;
        public FarFieldC ffradomeAndReflactionWithoutTransSum;
        public FarFieldC ffradomeAndReflactionWithTransSum;
        public FarFieldC ffreflactionWithTransition;

        public string stenkaName = "";
        public string requestName = "";
        public string sourceName = "";
        public double frequency = 0;

        public string Name
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName);
            }
        }
        public string FFantenaName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Поле от апертуры");
            }
        }
        public string FFradomeName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Поле от апертуры через обтекатель");
            }
        }
        public string FFreflactionWithoutTransitionName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Отражённое поле (без прохождения)");
            }
        }
        public string FFradomeAndReflactionWithoutTransSumName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Сумма поля через обтекатель и отражённого поля (без прохождения)");
            }
        }
        public string FFreflactionWithTransSumName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Отражённое поле");
            }
        }
        public string FFradomeAndReflactionWithTransSumName
        {
            get
            {
                return String.Concat(frequency / 1e9, "ГГц_", requestName, "_", stenkaName, "_", sourceName, "_", "Сумма поля через обтекатель и отражённого поля");
            }
        }

        public int CountRequest
        {
            get
            {
                int k = 0;
                if (ffantenna != null)
                {
                    k++;
                }
                if (ffradome != null)
                {
                    k++;
                }
                if (ffreflactionWithoutTransition != null)
                {
                    k++;
                }
                if (ffradomeAndReflactionWithoutTransSum != null)
                {
                    k++;
                }
                if (ffreflactionWithTransition != null)
                {
                    k++;
                }
                if (ffradomeAndReflactionWithTransSum != null)
                {
                    k++;
                }

                return k;
            }
        }
    }
}
