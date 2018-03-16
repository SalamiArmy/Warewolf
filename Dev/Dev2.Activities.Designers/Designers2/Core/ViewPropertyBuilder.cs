using System;
using System.Collections.Generic;
using Dev2.Activities.Designers2.Core.ActionRegion;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.DB;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.ToolBase;

namespace Dev2.Activities.Designers2.Core
{
    public class ViewPropertyBuilder : IViewPropertyBuilder
    {

        public List<KeyValuePair<string, string>> BuildProperties(IDbActionToolRegion<IDbAction> actionToolRegion, ISourceToolRegion<IDbSource> sourceToolRegion, string type)
        {
            var properties = new List<KeyValuePair<string, string>>();
            var sourceName = sourceToolRegion?.SelectedSource == null ? "" : sourceToolRegion.SelectedSource.Name;
            var procedureName = actionToolRegion?.SelectedAction == null ? "" : actionToolRegion.SelectedAction.Name;
			var sqlQuery = actionToolRegion?.SelectedAction == null ? "" : actionToolRegion.SelectedAction.Name;
			if (!string.IsNullOrEmpty(sourceName))
            {
                properties.Add(new KeyValuePair<string, string>("Source :", sourceName));
            }
            
            if (!string.IsNullOrEmpty(type))
            {
                properties.Add(new KeyValuePair<string, string>("Type :", type));
            }
			
			if (string.IsNullOrEmpty(sqlQuery))
			{
				var dbActionRegion = (DbActionRegion)actionToolRegion;
				if (dbActionRegion != null)
				{
					try
					{
						sqlQuery = dbActionRegion.SqlQuery;
						properties.Add(new KeyValuePair<string, string>("SqlQuery :", sqlQuery));
					}
					catch (Exception)
					{
						//
					}

				}

			}
			else
			{
				properties.Add(new KeyValuePair<string, string>("SqlQuery :", sqlQuery));
			}
			if (string.IsNullOrEmpty(procedureName))
            {
                var dbActionRegion = (DbActionRegion) actionToolRegion;
                if (dbActionRegion != null)
                {
                    try
                    {
                        procedureName = dbActionRegion.ProcedureName;
                        properties.Add(new KeyValuePair<string, string>("Procedure :", procedureName));
                    }
                    catch (Exception)
                    {
                       //
                    }
                   
                }
               
            }
            else
            {
                properties.Add(new KeyValuePair<string, string>("Procedure :", procedureName));
            }

            return properties;

        }

		public List<string> BuildProperties(string commandText, string type)
		{
			var properties = new List<string>();

			if (!string.IsNullOrEmpty(commandText))
			{
				properties.Add("CommandText :" + commandText);
			}
			if (!string.IsNullOrEmpty(type))
			{
				properties.Add("Type :" + type);
			}

			return properties;
		}
	}
}