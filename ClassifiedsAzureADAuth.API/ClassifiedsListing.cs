using System;

namespace ClassifiedsAzureADAuth.API
{
    public class ClassifiedsListing
    {
        public DateTime Date { get; set; }

        public int Sold { get; set; }

        public int InStock => 100 - Sold;

        public string Summary { get; set; }
    }
}
