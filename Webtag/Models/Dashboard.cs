using System.ComponentModel.DataAnnotations.Schema;

namespace Webtag.Models
{
    public enum WidgetType { Links = 1, Search = 2, Weather = 3 }
    public enum SearchType { None = 1, Google = 2, Bing = 3, Yahoo = 4 }
    public enum WeatherType { None = 1, AccuWeather = 2 }

    public class Dashboard
    {
        public Dashboard() { }

        public Dashboard(int userId)
        {
            UserProfileId = userId;
            Name = "Default";
            Order = 1;
        }

        public int Id { get; set; }
        public int UserProfileId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public class Widget
    {
        public int Id { get; set; }

        [ForeignKey("Dashboard")]
        public int DashboardId { get; set; }
        public virtual Dashboard Dashboard { get; set; }

        public int Order { get; set; }
        public WidgetType Type { get; set; }
        public int ObjectId { get; set; }
    }
}