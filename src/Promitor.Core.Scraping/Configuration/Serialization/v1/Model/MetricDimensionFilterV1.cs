namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class MetricDimensionFilterV1
    {
        /// <summary>
        /// Property to filter.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Select which dimension values apply to the filter
        /// </summary>
        public string Values { get; set; }
    }
}
