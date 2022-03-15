using System.ComponentModel;

namespace FoodShades.Web.Models.Enums
{
    public enum ProductSizes
    {
        [Description("Small")]
        Small = 1,

        [Description("Medium")]
        Medium = 2,

        [Description("Large")]
        Large = 3,

        [Description("Extra Large")]
        ExtraLarge = 4,
    }
}
