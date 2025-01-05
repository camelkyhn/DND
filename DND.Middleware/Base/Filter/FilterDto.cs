using DND.Middleware.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace DND.Middleware.Base.Filter
{
    public class FilterDto
    {
        private int _pageSize;

        public int PageSize
        {
            get => _pageSize <= 0 ? 5 : _pageSize;
            set => _pageSize = value <= 0 ? 5 : value;
        }

        private int _pageNumber;

        public int PageNumber
        {
            get => _pageNumber <= 0 ? 1 : _pageNumber;
            set => _pageNumber = value <= 0 ? 1 : value;
        }

        public int TotalCount { get; set; }

        public bool IsAllData { get; set; }

        public DateTime? CreatedBeforeDate { get; set; }

        public DateTime? CreatedAfterDate { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string CreatorUserEmail { get; set; }

        public DateTime? ModifiedBeforeDate { get; set; }

        public DateTime? ModifiedAfterDate { get; set; }

        [StringLength(MaxLengths.LongText)]
        public string ModifierUserEmail { get; set; }

        public bool? IsDeleted { get; set; }

        public string SortBy { get; set; }
    }
}
