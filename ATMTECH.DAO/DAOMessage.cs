﻿using System.Collections.Generic;
using ATMTECH.DAO.Database;
using ATMTECH.DAO.Interface;
using ATMTECH.Entities;

namespace ATMTECH.DAO
{
    public class DAOMessage : BaseDao<Message, int>, IDAOMessage
    {
        public Message GetMessage(string innerId, string language)
        {
            Criteria criteriaEnterprise = new Criteria { Column = Message.INNER_ID, Operator = DatabaseOperator.OPERATOR_EQUAL, Value = innerId };
            IList<Criteria> criterias = new List<Criteria>();
            criterias.Add(criteriaEnterprise);
            criterias.Add(IsActive());
            SetLanguage(criterias, language);
            IList<Message> byCriteria = GetByCriteria(criterias);
            return byCriteria == null || byCriteria.Count == 0 ? null : byCriteria[0];
        }
    }
}
