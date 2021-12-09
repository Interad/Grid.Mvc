﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GridMvc.Sorting;
using GridMvc.Utility;

namespace GridMvc.Columns
{
    /// <summary>
    ///     Collection of columns
    /// </summary>
    public class GridColumnCollection<T> : KeyedCollection<string, IGridColumn>, IGridColumnCollection<T>
    {
        private readonly IColumnBuilder<T> _columnBuilder;
        private readonly IGridSortSettings _sortSettings;

        public GridColumnCollection(IColumnBuilder<T> columnBuilder, IGridSortSettings sortSettings)
        {
            _columnBuilder = columnBuilder;
            _sortSettings = sortSettings;
        }

        #region IGridColumnCollection<T> Members

        public IGridColumn<T> Add()
        {
            return Add(false);
        }

        public IGridColumn<T> Add(bool hidden)
        {
            return Add((Expression<Func<T, string>>)null, hidden);
        }

        public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint)
        {
            return Add(constraint, false);
        }

        public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, string columnName)
        {
            IGridColumn<T> newColumn = CreateColumn(constraint, false, columnName);
            return Add(newColumn);
        }

        public IGridColumn<T> Add<TKey>(Expression<Func<T, TKey>> constraint, bool hidden)
        {
            IGridColumn<T> newColumn = CreateColumn(constraint, hidden, string.Empty);
            return Add(newColumn);
        }

        public IGridColumn<T> Add(PropertyInfo pi)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateColumn(pi);
            if (newColumn == null) return null;
            return Add(newColumn);
        }

        public IGridColumn<T> AddJsonColumn<TKey>(Expression<Func<T, TKey>> constraint, string propertyName, bool hidden)
        {
            IGridColumn<T> newColumn = CreateJsonValueColumn(constraint, propertyName, hidden, string.Empty);
            return Add(newColumn);
        }

        public IGridColumn<T> AddJsonColumn<TKey>(Expression<Func<T, TKey>> constraint, string propertyName)
        {
            IGridColumn<T> newColumn = CreateJsonValueColumn(constraint, propertyName, false, string.Empty);
            return Add(newColumn);
        }

        public IGridColumn<T> AddJsonColumn<TKey>(Expression<Func<T, TKey>> constraint, string propertyName, string columnName)
        {
            IGridColumn<T> newColumn = CreateJsonValueColumn(constraint, propertyName, false, columnName);
            return Add(newColumn);
        }

        public IGridColumn<T> AddJsonColumn(PropertyInfo pi, string propertyName)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateJsonValueColumn(pi, propertyName);
            if (newColumn == null)
                return null;
            return Add(newColumn);
        }

        public IGridColumn<T> Add(IGridColumn<T> column)
        {
            if (column == null)
                throw new ArgumentNullException("column");

            try
            {
                base.Add(column);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException(string.Format("Column '{0}' already exist in the grid", column.Name));
            }
            UpdateColumnsSorting();
            return column;
        }

        public IGridColumn<T> Insert(int position, IGridColumn<T> column)
        {
            base.Insert(position, column);
            UpdateColumnsSorting();
            return column;
        }

        public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint)
        {
            return Insert(position, constraint, false);
        }

        public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, string columnName)
        {
            IGridColumn<T> newColumn = CreateColumn(constraint, false, columnName);
            return Insert(position, newColumn);
        }

        public IGridColumn<T> Insert<TKey>(int position, Expression<Func<T, TKey>> constraint, bool hidden)
        {
            IGridColumn<T> newColumn = CreateColumn(constraint, hidden, string.Empty);
            return Insert(position, newColumn);
        }

        public new IEnumerator<IGridColumn> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        public IGridColumn<T> Get<TKey>(Expression<Func<T, TKey>> constraint)
        {
            var expr = constraint.Body as MemberExpression;
            if (expr == null)
                throw new ArgumentException(
                    string.Format("Expression '{0}' must be a member expression", constraint), "constraint");

            var name = PropertiesHelper.BuildColumnNameFromMemberExpression(expr);
            return this.FirstOrDefault(c => !string.IsNullOrEmpty(c.Name) && String.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase)) as IGridColumn<T>;
        }

        public IGridColumn GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            return this.FirstOrDefault(c => !string.IsNullOrEmpty(c.Name) && String.Equals(c.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion

        protected override string GetKeyForItem(IGridColumn item)
        {
            return item.Name;
        }

        private IGridColumn<T> CreateColumn<TKey>(Expression<Func<T, TKey>> constraint, bool hidden, string columnName)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateColumn(constraint, hidden);
            if (!string.IsNullOrEmpty(columnName))
                newColumn.Name = columnName;
            return newColumn;
        }

        private IGridColumn<T> CreateJsonValueColumn<TKey>(Expression<Func<T, TKey>> constraint, string propertyName, bool hidden, string columnName)
        {
            IGridColumn<T> newColumn = _columnBuilder.CreateJsonValueColumn(constraint, propertyName, hidden);
            if (!string.IsNullOrEmpty(columnName))
                newColumn.Name = columnName;
            else
                newColumn.Name = propertyName;

            return newColumn;
        }

        internal void UpdateColumnsSorting()
        {
            if (!string.IsNullOrEmpty(_sortSettings.ColumnName))
            {
                foreach (IGridColumn gridColumn in this)
                {
                    gridColumn.IsSorted = gridColumn.Name == _sortSettings.ColumnName;
                    if (gridColumn.Name == _sortSettings.ColumnName)
                        gridColumn.Direction = _sortSettings.Direction;
                    else
                        gridColumn.Direction = null;
                }
            }
        }
    }
}