/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using Dev2.Common.Interfaces.Core.DynamicServices;
using Dev2.Common.Interfaces.ToolBase;
using Dev2.Common.Interfaces.ToolBase.Email;
using System;

namespace Dev2.Common.Interfaces.Core
{
    public class EmailServiceSourceDefinition : ISmtpSource, IEquatable<EmailServiceSourceDefinition>
    {
        public Guid ResourceID { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public enSourceType Type { get; set; }
        public string ResourceType { get; set; }
        public int Timeout { get; set; }
        public string EmailTo { get; set; }
        public string Path { get; set; }
        public Guid Id { get; set; }
        public string ResourceName { get; set; }
        public int Port { get; set; }
        public string EmailFrom { get; set; }
        public bool EnableSSL { get; set; }

        #region Equality members

        public bool Equals(EmailServiceSourceDefinition other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Host, other.Host) && string.Equals(UserName, other.UserName) && string.Equals(Password, other.Password) 
                && EnableSSL == other.EnableSSL && Port == other.Port && Timeout == other.Timeout;
        }

        public bool Equals(ISmtpSource other)
        {
            return Equals(other as EmailServiceSourceDefinition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((EmailServiceSourceDefinition)obj);
        }

        public bool Equals(ToolBase.IEmailSource other)
        {
            return Equals(other as EmailServiceSourceDefinition);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Host?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (UserName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Password?.GetHashCode() ?? 0);
                return hashCode;
            }
        }

        #endregion
    }
}
