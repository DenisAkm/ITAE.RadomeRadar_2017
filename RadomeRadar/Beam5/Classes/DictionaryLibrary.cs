using SharpDX;
using System.Collections.Generic;

namespace Apparat
{
    static class DictionaryLibrary
    {
        public static Dictionary<string, float> Unit = new Dictionary<string, float>() { { "м", 1f }, { "дм", 1e-1f }, { "см", 1e-2f }, { "мм", 1e-3f } };

        public static Dictionary<int, string> CurveNames = new Dictionary<int, string>(){{0, "TotalAntenna"}, {1, "TotalRadom"}, {2, "ThetaRadom"}, {3, "PhiRadom"}, 
        {4, "TotalReflaction"}, {5, "ThetaReflaction"}, {6, "PhiReflaction"}, {7, "TotalRefInterpolation"}, {8, "ThetaRefInterpolation"}, {9, "PhiRefInterpolation"}, 
        {10, "TotalRadomeCombined"}, {11, "ThetaRadomeCombined"}, {12, "PhiRadomeCombined"}, {13, "TotalRadomeCombinedInterpolation"}, 
        {14, "ThetaRadomeCombinedInterpolation"}, {15, "PhiRadomeCombinedInterpolation"}};

        public static Dictionary<int, string> PolarizationNames = new Dictionary<int, string>() {
            { 0, "Поляризация А" }, { 1, "Поляризация Б" }, { 2, "Круговая поляризация А" }, { 3, "Круговая поляризация Б" }, { 4, "Пользовательская" }
        };

        public static Dictionary<int, string> PolarizationNamesShort = new Dictionary<int, string>() {
            { 0, "А" }, { 1, "Б" }, { 2, "CA" }, { 3, "CБ" }, { 4, "U" }
        };

        public static Dictionary<string, Color> СolorDictionary = new Dictionary<string, Color>
        {
            {"Белый", Color.Transparent},
            {"Синий", Color.Blue},
            {"Красный", Color.Red},
            {"Зелёный", Color.Green},
            {"Жёлтый", Color.Yellow},
            {"Серый", Color.Gray},
            {"Оранжевый", Color.Orange}
        };
    }
}
