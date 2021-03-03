using System.Collections.Generic;
using System.Windows.Data;

namespace SoundLevelCalculator
{
    public static class StaticResources
    {
        private static readonly XmlDataProvider elementsData;
        private static readonly List<string> diametersList;
        private static readonly IDictionary<string, List<string>> rectangularList;

        static StaticResources()
        {
            elementsData = System.Windows.Application.Current.FindResource("ElementsListViewData") as XmlDataProvider;
            diametersList = new List<string>()
            {
                "80", "100", "125", "140", "150", "160", "180", "200", "224", "250", "280", "300",
                "315", "355", "400", "450", "500", "560", "600" , "630" , "710", "800", "900",
                "1000", "1120", "1250", "1400", "1600"
            };
            rectangularList = new Dictionary<string, List<string>>()
            {
                { "100", new List<string>() { "200", "250", "300", "400", "500", "600" } },
                { "150", new List<string>() { "200", "250", "300", "400", "500", "600" } },
                { "200", new List<string>() { "100", "150", "200", "250", "300", "400", "500", "600", "800" } },
                { "250", new List<string>() { "100", "150", "200", "250", "300", "400", "500", "600", "800", "1000" } },
                { "300", new List<string>() { "100", "150", "200", "250", "300", "400", "500", "600", "800", "1000", "1200" } },
                { "400", new List<string>() { "100", "150", "200", "250", "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600" } },
                { "500", new List<string>() { "150", "200", "250", "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600", "1800", "2000" } },
                { "600", new List<string>() { "150", "200", "250", "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600", "1800", "2000" } },
                { "800", new List<string>() { "200", "250", "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600", "1800", "2000" } },
                { "1000", new List<string>() { "250", "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600", "1800", "2000" } },
                { "1200", new List<string>() { "300", "400", "500", "600", "800", "1000", "1200", "1400", "1600", "1800", "2000" } },
                { "1400", new List<string>() { "400", "500", "600", "800", "1000", "1200" } },
                { "1600", new List<string>() { "400", "500", "600", "800", "1000", "1200" } },
                { "1800", new List<string>() { "500", "600", "800", "1000", "1200" } },
                { "2000", new List<string>() { "500", "600", "800", "1000", "1200" } },
            };
        }

        public static XmlDataProvider ElementsList
        {
            get
            {
                return elementsData;
            }
        }

        public static List<string> DiametersList
        {
            get
            {
                return diametersList;
            }
        }

        public static IDictionary<string, List<string>> RectangularList
        {
            get
            {
                return rectangularList;
            }
        }
    }
}
