using System;
namespace webApiV1.Models.Responses
{
    public class ProductResponds
    {
        public ProductResponds(string name, string category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; private set; }

        public string Category { get; private set; }

    }
}
