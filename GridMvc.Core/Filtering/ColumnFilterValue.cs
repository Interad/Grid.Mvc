using System.Net;
using System.Runtime.Serialization;

namespace GridMvc.Core.Filtering
{
    /// <summary>
    ///     Structure that specifies filter settings for each column
    /// </summary>
    [DataContract]
    public struct ColumnFilterValue
    {
        //[DataMember(Name = "columnName")]
        public string ColumnName;

        [DataMember(Name = "filterType")]
        public GridFilterType FilterType;

        public string FilterValue;

        [DataMember(Name = "filterValue")]
        internal string FilterValueEncoded
        {
            get { return WebUtility.UrlEncode(FilterValue); }
            set { FilterValue = value; }
        }

        public static ColumnFilterValue Null
        {
            get { return default(ColumnFilterValue); }
        }

        public override bool Equals(object obj)
        {
            return obj is ColumnFilterValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ColumnName != null ? ColumnName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)FilterType;
                hashCode = (hashCode * 397) ^ (FilterValue != null ? FilterValue.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ColumnFilterValue a, ColumnFilterValue b)
        {
            return a.ColumnName == b.ColumnName && a.FilterType == b.FilterType && a.FilterValue == b.FilterValue;
        }

        public static bool operator !=(ColumnFilterValue a, ColumnFilterValue b)
        {
            return a.ColumnName != b.ColumnName || a.FilterType != b.FilterType || a.FilterValue != b.FilterValue;
        }
    }
}