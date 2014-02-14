﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using ATMTECH.DAO.Database;
using ATMTECH.Entities;

namespace ATMTECH.DAO
{
    public class BaseDao<TModel, TId>
    {
        public DatabaseOperation<TModel, TId> DatabaseOperation { get { return new DatabaseOperation<TModel, TId>(); } }
        public DatabaseBackup<TModel, TId> DatabaseBackup { get { return new DatabaseBackup<TModel, TId>(); } }
        public Model<TModel, TId> Model { get { return new Model<TModel, TId>(); } }


        public Criteria IsActive()
        {
            return new Criteria { Column = BaseEntity.IS_ACTIVE, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = "1" };
        }

        public TModel GetById(TId id)
        {
            if (id.ToString() != "0")
            {
                PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
                OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = OrderBy.Type.Ascending };

                DataSet data = DatabaseOperation.ReturnDataSet("[" + Model.GetIdKeyColumnFromModel() + "] = " + id, null, pagingOperation, orderOperation);

                if (data.Tables[0].Rows.Count > 0)
                {
                    return Model.FillModel(data.Tables[0].Rows[0]);
                }
                else
                {
                    TModel model2 = default(TModel);
                    return model2;
                }


            }
            else
            {
                TModel model2 = default(TModel);
                return model2;
            }
        }

        public IList<TModel> GetBySql(string sql)
        {
            return Model.FillModelFromDataSet(DatabaseOperation.ReturnDataset(sql));
        }

        public IList<TModel> GetAll()
        {
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = OrderBy.Type.Ascending };
            return GetAll(pagingOperation, orderOperation);
        }
        public IList<TModel> GetAll(PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            return Model.FillModelFromDataSet(DatabaseOperation.ReturnDataSet(pagingOperation, orderOperation));
        }
        public IList<TModel> GetAllActive()
        {
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = DAO.OrderBy.Type.Ascending };
            return GetAllActive(pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllActive(OrderOperation orderOperation)
        {
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            return GetAllActive(pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllActive(PagingOperation pagingOperation)
        {
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = DAO.OrderBy.Type.Ascending };
            return GetAllActive(pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllActive(PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            DataSet dataSet = DatabaseOperation.ReturnDataSet("IsActive = 1", null, pagingOperation, orderOperation);
            return Model.FillModelFromDataSet(dataSet);
        }
        public IList<TModel> GetAllOneCriteria(string columnName, string value)
        {
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = OrderBy.Type.Ascending };
            Criteria criteria = new Criteria() { Column = columnName, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = value };

            return GetAllOneCriteria(criteria, pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllOneCriteria(string columnName, string value, OrderOperation orderOperation)
        {
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            Criteria criteria = new Criteria() { Column = columnName, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = value };
            return GetAllOneCriteria(criteria, pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllOneCriteria(string columnName, string value, PagingOperation pagingOperation)
        {
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = DAO.OrderBy.Type.Ascending };
            Criteria criteria = new Criteria() { Column = columnName, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = value };
            return GetAllOneCriteria(criteria, pagingOperation, orderOperation);
        }
        public IList<TModel> GetAllOneCriteria(Criteria criteria, PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            IList<Criteria> criterias = new List<Criteria>();
            criterias.Add(criteria);
            return GetByCriteria(criterias, pagingOperation, orderOperation);
        }
        public IList<TModel> GetByCriteria(IList<Criteria> criterias)
        {
            OrderOperation orderOperation = new OrderOperation() { OrderByColumn = Model.GetIdKeyColumnFromModel(), OrderByType = OrderBy.Type.Descending };
            PagingOperation pagingOperation = new PagingOperation() { PageIndex = DatabaseOperator.NO_PAGING, PageSize = DatabaseOperator.NO_PAGING };
            return GetByCriteria(criterias, pagingOperation, orderOperation);
        }
        public IList<TModel> GetByCriteria(IList<Criteria> criterias, PagingOperation pagingOperation, OrderOperation orderOperation)
        {
            IList<TModel> listModel = new List<TModel>();
            string where = null;

            foreach (Criteria criteria in criterias)
            {
                if (criteria.Operator == DatabaseOperator.OPERATOR_LIKE)
                {
                    where += "[" + criteria.Column + "] " + criteria.Operator + "(@" + criteria.Column + ") and ";
                }
                else
                {
                    where += "[" + criteria.Column + "]" + criteria.Operator + "@" + criteria.Column + " and ";
                }
            }

            // Remove the last and
            if (!String.IsNullOrEmpty(where))
            {
                where = where.Remove(where.Length - 4, 4);
                listModel = Model.FillModelFromDataSet(DatabaseOperation.ReturnDataSet(where, criterias, pagingOperation, orderOperation));
            }

            return listModel;
        }
        public string GetMax(string columnName)
        {
            DataSet data = DatabaseOperation.ReturnDataSetMax(columnName);
            return data.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        public int GetCount()
        {
            DataSet data = DatabaseOperation.ReturnDataSetCount();
            return Convert.ToInt32(data.Tables[0].Rows[0].ItemArray[0]);
        }
        public int Save(TModel model)
        {
            SetDefaultValueComboboxAndSearch(model);
            int rtn;
            if (Convert.ToInt64(Model.GetValueProperty(Model.GetIdKeyColumnFromModel(), model)) != 0)
            {
                SetDateModified(model);
                DatabaseOperation.UpdateSql(model, Model.GetIdKeyColumnFromModel());
                rtn = (int)Convert.ToInt64(Model.GetValueProperty(Model.GetIdKeyColumnFromModel(), model));
            }
            else
            {
                SetDateCreated(model);
                rtn = DatabaseOperation.InsertSql(model);
            }

            return rtn;
        }

        private void SetDateCreated(TModel model)
        {
            Model.SetValueProperty("DateCreated", DateTime.Now.ToString(), model);
        }

        private void SetDateModified(TModel model)
        {
            Model.SetValueProperty("DateModified", DateTime.Now.ToString(), model);
        }
        private void SetDefaultValueComboboxAndSearch(TModel model)
        {
            Object objSearch = Model.GetValueProperty("SearchUpdate", model);
            Object objcomboboxDescription = Model.GetValueProperty("ComboboxDescriptionUpdate", model);

            if (objSearch != null)
            {
                string searchUpdate = HttpUtility.HtmlDecode(objSearch.ToString());
                Model.SetValueProperty("Search", searchUpdate, model);
            }
            if (objcomboboxDescription != null)
            {
                string comboboxDescriptionUpdate = HttpUtility.HtmlDecode(objcomboboxDescription.ToString());
                Model.SetValueProperty("ComboboxDescription", comboboxDescriptionUpdate, model);
            }
        }


        public void SetLanguage(IList<Criteria> criterias, string language)
        {
            Criteria criteriaLanguage = new Criteria()
            {
                Column = BaseEntity.LANGUAGE,
                Operator = DatabaseOperator.OPERATOR_EQUAL,
                Value = language
            };
            criterias.Add(criteriaLanguage);
        }

        public void ExecuteSql(string sql)
        {
            DatabaseOperation.ExecuteSql(sql);
        }
        public void BackupToXml(string zipFile, bool allTableFromDatabase)
        {
            DatabaseBackup.BackupToXml(zipFile, allTableFromDatabase);
        }
        public void RestoreFromXml(string zipFile)
        {
            DatabaseBackup.RestoreFromXml(zipFile);
        }
    }

    public class Criteria
    {
        public string Column { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public DbType DbType { get; set; }
        public bool ClearText { get; set; }
    }
    public static class OrderBy
    {
        public enum Type
        {
            Ascending = 0,
            Descending = 1
        }
    }
    public class OrderOperation
    {
        public string OrderByColumn { get; set; }
        public OrderBy.Type OrderByType { get; set; }
    }
    public class PagingOperation
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

}