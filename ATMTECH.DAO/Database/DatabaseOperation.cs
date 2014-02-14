﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using ATMTECH.Entities;

namespace ATMTECH.DAO.Database
{
    public class DatabaseOperation<TModel, TId>
    {
        public SQLite<TModel, TId> SqLite { get { return new SQLite<TModel, TId>(); } }
        public MsSql<TModel, TId> MsSql { get { return new MsSql<TModel, TId>(); } }
        public Model<TModel, TId> Model { get { return new Model<TModel, TId>(); } }

        public DatabaseVendor.DatabaseVendorType CurrentDatabaseVendor
        {
            get { return DatabaseVendor.GetCurrentDatabaseVendorType(); }
        }

        public bool IsColumnExist(string column, DataColumnCollection dataColumnCollection)
        {
            foreach (DataColumn dataColumn in dataColumnCollection)
            {
                if (dataColumn.ColumnName == column)
                {
                    return true;
                }
            }

            return false;
        }
        public DataColumnCollection ReturnAllColumnNameFromTable(string table)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnAllColumnNameFromTable(table);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.ReturnAllColumnNameFromTable(table);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public string ConstructSqlPaging(PagingOperation pagingOperation)
        {
            string sql = string.Empty;
            if (pagingOperation.PageSize != 0)
            {
                switch (CurrentDatabaseVendor)
                {
                    case DatabaseVendor.DatabaseVendorType.Sqlite:
                        sql = string.Format(SQLite<TModel, TId>.SQL_PAGING, pagingOperation.PageIndex, pagingOperation.PageSize);
                        break;
                    case DatabaseVendor.DatabaseVendorType.MsSql:
                        sql = string.Format(MsSql<TModel, TId>.SQL_PAGING, pagingOperation.PageIndex, pagingOperation.PageSize);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            return sql;
        }
        public string ExtractColumnsFromDatabase(Type type)
        {
            string columns = string.Empty;
            DataColumnCollection dataColumnCollection = ReturnAllColumnNameFromTable(ReturnTableName(type));
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (IsColumnExist(propertyInfo.Name, dataColumnCollection))
                {
                    columns += "[" + propertyInfo.Name + "],";
                }
            }

            // remove last ,
            columns = columns.Remove(columns.Length - 1, 1);
            return columns;
        }
        public string ConstructSqlOrderBy(OrderOperation orderOperation)
        {
            string sql = string.Empty;
            if (orderOperation.OrderByType == OrderBy.Type.Ascending)
            {
                switch (CurrentDatabaseVendor)
                {
                    case DatabaseVendor.DatabaseVendorType.Sqlite:
                        sql = string.Format(SQLite<TModel, TId>.SQL_ORDER_BY, orderOperation.OrderByColumn, "ASC");
                        break;
                    case DatabaseVendor.DatabaseVendorType.MsSql:
                        sql = string.Format(MsSql<TModel, TId>.SQL_ORDER_BY, orderOperation.OrderByColumn, "ASC");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }
            else
            {
                if (orderOperation.OrderByType == OrderBy.Type.Descending)
                {
                    switch (CurrentDatabaseVendor)
                    {
                        case DatabaseVendor.DatabaseVendorType.Sqlite:
                            sql = string.Format(SQLite<TModel, TId>.SQL_ORDER_BY, orderOperation.OrderByColumn, "DESC");
                            break;
                        case DatabaseVendor.DatabaseVendorType.MsSql:
                            sql = string.Format(MsSql<TModel, TId>.SQL_ORDER_BY, orderOperation.OrderByColumn, "DESC");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return sql;
        }
        public string ReturnTableName(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (fieldInfo.Name == "EST_UN_ALIAS_DE")
                {
                    return fieldInfo.GetValue(type).ToString();
                }
            }
            return type.Name;
        }
        public string ConstructSqlWhere(string @where, Type type, string columns)
        {
            string sql;
            string tableName = ReturnTableName(type);

            if (!String.IsNullOrEmpty(where))
            {
                switch (CurrentDatabaseVendor)
                {
                    case DatabaseVendor.DatabaseVendorType.Sqlite:
                        sql = String.Format(SQLite<TModel, TId>.SQL_SELECT_WHERE, columns, tableName, where);
                        break;
                    case DatabaseVendor.DatabaseVendorType.MsSql:
                        sql = String.Format(MsSql<TModel, TId>.SQL_SELECT_WHERE, columns, tableName, where);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (CurrentDatabaseVendor)
                {
                    case DatabaseVendor.DatabaseVendorType.Sqlite:
                        sql = String.Format(SQLite<TModel, TId>.SQL_SELECT, columns, tableName);
                        break;
                    case DatabaseVendor.DatabaseVendorType.MsSql:
                        sql = String.Format(MsSql<TModel, TId>.SQL_SELECT, columns, tableName);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return sql;
        }
        public string ConstructSqlFromModel(string @where, PagingOperation pagingOperation, OrderOperation orderOperation, Type type)
        {
            string columns = ExtractColumnsFromDatabase(type);
            string sql = ConstructSqlWhere(@where, type, columns);
            sql += ConstructSqlOrderBy(orderOperation);
            sql += ConstructSqlPaging(pagingOperation);
            return sql;
        }
        public string ReturnSetClauseUpdate(TModel model, string id)
        {
            Type type = model.GetType();
            string tableName = ReturnTableName(type);
            DataColumnCollection dataColumnCollection = ReturnAllColumnNameFromTable(tableName);
            PropertyInfo[] properties = type.GetProperties();

            string setClause = null;
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.Name != id)
                {
                    if (propertyInfo.PropertyType.Name.ToLower() != "ilist`1")
                    {
                        if (IsColumnExist(propertyInfo.Name, dataColumnCollection))
                        {
                            setClause += "[" + propertyInfo.Name + "]= @" + propertyInfo.Name + ",";
                        }
                    }
                }
            }

            if (setClause != null) setClause = setClause.Remove(setClause.Length - 1, 1);

            return setClause;
        }
        public string ReturnColumnInsert(TModel model)
        {
            Type type = model.GetType();
            string tableName = ReturnTableName(type);
            DataColumnCollection dataColumnCollection = ReturnAllColumnNameFromTable(tableName);
            PropertyInfo[] properties = type.GetProperties();

            string columnInsert = null;
            foreach (var propertyInfo in properties)
            {
                if (IsColumnExist(propertyInfo.Name, dataColumnCollection))
                {
                    if (propertyInfo.Name != Model.GetIdKeyColumnFromModel())
                    {
                        columnInsert += "[" + propertyInfo.Name + "],";
                    }
                }
            }

            if (columnInsert != null) columnInsert = columnInsert.Remove(columnInsert.Length - 1, 1);

            return columnInsert;
        }
        public string ReturnValueInsert(TModel model)
        {
            Type type = model.GetType();
            string tableName = ReturnTableName(type);
            DataColumnCollection dataColumnCollection = ReturnAllColumnNameFromTable(tableName);
            PropertyInfo[] properties = type.GetProperties();

            string valueInsert = null;
            foreach (var propertyInfo in properties)
            {
                if (IsColumnExist(propertyInfo.Name, dataColumnCollection))
                {
                    if (propertyInfo.Name != Model.GetIdKeyColumnFromModel())
                    {
                        valueInsert += "@" + propertyInfo.Name + ",";
                    }
                }
            }

            if (valueInsert != null) valueInsert = valueInsert.Remove(valueInsert.Length - 1, 1);

            return valueInsert;
        }

        public DataSet ReturnDataSetCount()
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnDataSetCount();
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.ReturnDataSetCount();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public DataSet ReturnDataSetMax(string columnName)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnDataSetMax(columnName);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.ReturnDataSetMax(columnName);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DataSet ReturnDataSet(PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnDataSet(pagingOperation, orderOperation);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.ReturnDataSet(pagingOperation, orderOperation);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DataSet ReturnDataset(string sql)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnDataSet(sql);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    throw new AccessViolationException("Le retour par SQL non supporté encore pour MSSQL");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DataSet ReturnDataSet(string where, IList<Criteria> criterias, PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.ReturnDataSet(where, criterias, pagingOperation, orderOperation);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.ReturnDataSet(where, criterias, pagingOperation, orderOperation);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void ExecuteSql(string sql)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    SqLite.ExecuteSql(sql);
                    break;
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    MsSql.ExecuteSql(sql);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public void UpdateSql(TModel model, string id)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    SqLite.UpdateSql(model, id);
                    break;
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    MsSql.UpdateSql(model, id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public int InsertSql(TModel model)
        {
            switch (CurrentDatabaseVendor)
            {
                case DatabaseVendor.DatabaseVendorType.Sqlite:
                    return SqLite.InsertSql(model);
                case DatabaseVendor.DatabaseVendorType.MsSql:
                    return MsSql.InsertSql(model);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public class DatabaseOperator
    {
        public const int NO_PAGING = 0;
        public const string OPERATOR_EQUAL = "=";
        public const string OPERATOR_LIKE = "Like";
        public const string OPERATOR_NOT_EQUAL = "<>";
        public const string OPERATOR_GREATER_THAN = ">";
        public const string OPERATOR_GREATER_EQUAL_THAN = ">=";
        public const string OPERATOR_LOWER_THAN = "<";
        public const string OPERATOR_LOWER_EQUAL_THAN = "<=";
    }
}