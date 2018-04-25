/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using System.Text;
using System.Xml;
using Dev2.Common.Interfaces.Data;
using Dev2.Data.Interfaces.Enums;



namespace Dev2.DataList.Contract
{
    public class DefinitionBuilder
    {
        const string _nameAttribute = "Name";
        const string _mapsToAttribute = "MapsTo";
        const string _valueAttribute = "Value";
        const string _recordsetAttribute = "Recordset";
        const string _defaultValueAttribute = "DefaultValue";
        const string _validateTag = "Validator";
        const string _typeAttribute = "Type";
        const string _requiredValue = "Required";
        const string _sourceAttribute = "Source";

        public enDev2ArgumentType ArgumentType { get; set; }

        public IEnumerable<IDev2Definition> Definitions { get; set; }

        /// <summary>
        /// Generates this instance.
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            var result = new StringBuilder();

            var xDoc = new XmlDocument();
            var rootNode = xDoc.CreateElement(string.Concat(ArgumentType.ToString(), "s"));


            foreach (IDev2Definition def in Definitions)
            {
                var tmp = xDoc.CreateElement(ArgumentType.ToString());
                tmp.SetAttribute(_nameAttribute, def.Name);

                tmp.SetAttribute(ArgumentType != enDev2ArgumentType.Input ? _mapsToAttribute : _sourceAttribute, def.MapsTo);

                if(ArgumentType != enDev2ArgumentType.Input)
                {
                    tmp.SetAttribute(_valueAttribute, def.Value);
                }

                tmp.SetAttribute("IsObject", def.IsObject.ToString());

                if(def.RecordSetName.Length > 0)
                {
                    tmp.SetAttribute(_recordsetAttribute, def.RecordSetName);
                }

                if(def.DefaultValue.Length > 0 && ArgumentType == enDev2ArgumentType.Input)
                {
                    tmp.SetAttribute(_defaultValueAttribute, def.DefaultValue);
                }

                if(def.IsRequired && ArgumentType == enDev2ArgumentType.Input)
                {
                    var requiredElm = xDoc.CreateElement(_validateTag);
                    requiredElm.SetAttribute(_typeAttribute, _requiredValue);
                    tmp.AppendChild(requiredElm);
                }
                rootNode.AppendChild(tmp);
            }

            xDoc.AppendChild(rootNode);

            result.Append(xDoc.OuterXml);

            return result.ToString();
        }
    }
}
