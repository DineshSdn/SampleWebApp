using System;
using System.Collections.Generic;
using System.Linq;
using App.Common.Models;

namespace App.Common.Extensions
{
    public static class PageFilterExtensions
    {
        public static PagedResultDto<TModel> GetPage<TModel>(this PageFilterDto filter, IEnumerable<TModel> list, int? totalRecords = null)
        {
            var count = totalRecords ?? list.Count();
            var records = list.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            return new PagedResultDto<TModel>
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalRecords = count,
                Records = records
            };
        }

        public static List<TModel> GetPagedRecords<TModel>(this PageFilterDto filter, IEnumerable<TModel> list)
        {
            return list.Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();
        }
    }
}
