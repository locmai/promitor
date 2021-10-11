namespace Promitor.Core.Scraping.Configuration.Model
{
    /// <summary>
    /// Information about the dimension of an Azure Monitor metric
    /// </summary>
    public class MetricDimensionFilter
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