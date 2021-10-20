using System;
using System.Linq;
using System.Collections.Generic;
using GuardNet;
using Microsoft.Azure.Management.Monitor.Fluent.Models;

namespace Promitor.Core.Metrics
{
    public class MeasuredMetric
    {
        /// <summary>
        ///     Value of the metric that was found
        /// </summary>
        public double? Value { get; }

        /// <summary>
        ///     Name of dimension for a metric
        /// </summary>
        public string DimensionName { get; set; }

        /// <summary>
        ///     Name of dimension for a metric
        /// </summary>
        public string DimensionValue { get; }

        /// <summary>
        ///     Indication whether or not the metric represents a dimension
        /// </summary>
        public bool IsDimensional { get; }

        /// <summary>
        ///     Dimensions' names and values of a metrics
        /// </summary>
        public Dictionary<string, string> Dimensions { get; }

        /// <summary>
        ///     Indication whether or not the metric represents multiple dimensions
        /// </summary>
        public bool IsMultiDimensional { get; }

        private MeasuredMetric(double? value)
        {
            Value = value;
        }

        private MeasuredMetric(double? value, string dimensionName, string dimensionValue)
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
            Guard.NotNullOrWhitespace(dimensionValue, nameof(dimensionValue));

            Value = value;

            IsDimensional = true;
            DimensionName = dimensionName;
            DimensionValue = dimensionValue;
        }

        private MeasuredMetric(double? value, Dictionary<string, string> dimensions)
        {
            Value = value;

            IsMultiDimensional = true;
            Dimensions = dimensions;
        }

        /// <summary>
        /// Create a measured metric without dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        public static MeasuredMetric CreateWithoutDimension(double? value)
        {
            return new MeasuredMetric(value);
        }

        /// <summary>
        /// Create a measured metric for a given dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionName">Name of dimension that is being scraped</param>
        /// <param name="timeseries">Timeseries representing one of the dimensions</param>
        public static MeasuredMetric CreateForDimension(double? value, string dimensionName, TimeSeriesElement timeseries)
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
            Guard.NotNull(timeseries, nameof(timeseries));
            Guard.For<ArgumentException>(() => timeseries.Metadatavalues.Any() == false);

            var dimensionValue = timeseries.Metadatavalues.Single(metadataValue => metadataValue.Name?.Value.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase) == true);
            return CreateForDimension(value, dimensionName, dimensionValue.Value);
        }

        /// <summary>
        /// Create a measured metric for a given dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionName">Name of dimension that is being scraped</param>
        /// <param name="dimensionValue">Value of the dimension that is being scraped</param>
        public static MeasuredMetric CreateForDimension(double? value, string dimensionName, string dimensionValue)
        {
            Guard.NotNullOrWhitespace(dimensionName, nameof(dimensionName));
            Guard.NotNullOrWhitespace(dimensionValue, nameof(dimensionValue));

            return new MeasuredMetric(value, dimensionName, dimensionValue);
        }

        /// <summary>
        /// Create a measured metric for given dimensions
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensionNames">Names of dimensions those are being scraped</param>
        /// <param name="timeseries">Timeseries representing one of the dimensions</param>
        public static MeasuredMetric CreateForDimensions(double? value, List<string> dimensionNames, TimeSeriesElement timeseries)
        {
            Guard.For<ArgumentException>(() => dimensionNames.Any() == false);
            Guard.NotNull(timeseries, nameof(timeseries));
            Guard.For<ArgumentException>(() => timeseries.Metadatavalues.Any() == false);

            var dimensions = new Dictionary<string, string>();

            foreach (var dimensionName in dimensionNames) {
                var dimensionValue = timeseries.Metadatavalues.Single(metadataValue => metadataValue.Name?.Value.Equals(dimensionName, StringComparison.InvariantCultureIgnoreCase) == true);
                dimensions.Add(dimensionName, dimensionValue.Value);
            }

            return CreateForDimensions(value, dimensions);
        }

        /// <summary>
        /// Create a measured metric for a given dimension
        /// </summary>
        /// <param name="value">Measured metric value</param>
        /// <param name="dimensions">Dictionary contains pairs of Name/Value of dimensions those are being scraped</param>
        public static MeasuredMetric CreateForDimensions(double? value, Dictionary<string,string> dimensions)
        {
            return new MeasuredMetric(value, dimensions);
        }
    }
}
