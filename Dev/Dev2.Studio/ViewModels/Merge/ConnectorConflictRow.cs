﻿/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2018 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using Dev2.Common.Interfaces;

namespace Dev2.ViewModels.Merge
{
    public class ConnectorConflictRow : ConflictRow, IArmConnectorConflict
    {
        bool _hasConflict;
        public IConnectorConflictItem CurrentArmConnector { get; set; }
        public override IConflictItem Current
        {
            get => CurrentArmConnector;
        }
        public IConnectorConflictItem DifferentArmConnector { get; set; }
        public override IConflictItem Different {
            get => DifferentArmConnector;
        }

        public string Key { get; set; }
        
        public override bool HasConflict
        {
            get => _hasConflict;
            set
            {
                _hasConflict = value;
                OnPropertyChanged(() => HasConflict);
            }
        }
        public override bool IsChecked { get; set; }

        public override Guid UniqueId { get; set; }

        public override bool IsEmptyItemSelected { get; set; }
        
        public bool Equals(IArmConnectorConflict other)
        {
            if (other == null)
            {
                return false;
            }
            var equals = true;
            equals &= (other.CurrentArmConnector?.Equals(CurrentArmConnector)).GetValueOrDefault(false);
            equals &= (other.DifferentArmConnector?.Equals(DifferentArmConnector)).GetValueOrDefault(false);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((IArmConnectorConflict)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = (397) ^ UniqueId.GetHashCode();
            hashCode = (hashCode * 397) ^ (Key != null ? Key.GetHashCode() : 0);
            return hashCode;
        }
    }
}