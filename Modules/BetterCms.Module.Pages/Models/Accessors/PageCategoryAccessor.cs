﻿using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Pages.Models.Accessors
{
    public class PageCategoryAccessor : ICategoryAccessor
    {
        public string Name
        {
            get
            {
                return PageProperties.CategorizableItemKeyForPages;
            }
        }

        public IFutureValue<int> CheckIsUsed(IRepository repository, CategoryTree categoryTree)
        {
            var query = repository.AsQueryable<PageCategory>().Where(p => p.Page is PageProperties && p.Category.CategoryTree == categoryTree);
            return query.ToRowCountFutureValue();
        }

        public IEnumerable<IEntityCategory> QueryEntityCategories(IRepository repository, ICategory category)
        {
            return repository.AsQueryable<PageCategory>().Where(m => m.Page is PageProperties && m.Category.Id == category.Id).ToFuture();
        }
    }
}