﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Dev2.Common.Interfaces;

namespace Dev2.Common.DependencyVisualization
{
    public class DependencyVisualizationNode : INotifyPropertyChanged, IDependencyVisualizationNode
    {
        #region Class Members


        const string _errorImagePath = "/Images/Close_Box_Red.png";

        #endregion Class Members

        #region Fields

        double _locationX, _locationY;
        readonly bool _isBroken;

        #endregion Fields

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region Constructor

        public DependencyVisualizationNode(string id, double locationX, double locationY, bool isTarget, bool isBroken)
        {
            NodeDependencies = new List<IDependencyVisualizationNode>();
            CircularDependencies = new List<ICircularDependency>();
            LocationX = locationX;
            LocationY = locationY;
            ID = id;
            IsTargetNode = isTarget;
            _isBroken = isBroken;
        }

        #endregion Constructor

        #region Properties

        public List<ICircularDependency> CircularDependencies { get; }

        public bool HasCircularDependency => CircularDependencies.Any();

        public string ID { get; }

        public bool IsTargetNode { get; set; }
        public string ErrorImagePath => _isBroken ? _errorImagePath : "";

        public double LocationX
        {
            get { return _locationX; }
            set
            {                
                _locationX = value;
                OnPropertyChanged("LocationX");
            }
        }

        public double LocationY
        {
            get { return _locationY; }
            set
            {                
                _locationY = value;
                OnPropertyChanged("LocationY");
            }
        }

        public List<IDependencyVisualizationNode> NodeDependencies { get; }

        public double NodeWidth => 100;

        public double NodeHeight => 50;

        public bool IsBroken => _isBroken;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public new string ToString()
        {
            var result = new StringBuilder(
                $"<node id=\"{ID}\" x=\"{LocationX}\" y=\"{LocationY}\" broken=\"{IsBroken}\">");

            foreach (var nodeDependency in NodeDependencies)
            {
                result.Append($"<dependency id=\"{nodeDependency.ID}\" />");
            }

            result.Append("</node>");

            return result.ToString();
        }

        public List<ICircularDependency> FindCircularDependencies()
        {
            if (NodeDependencies.Count == 0)
            {
                return null;
            }

            var circularDependencies = new List<ICircularDependency>();

            var stack = new Stack<NodeInfo>();
            stack.Push(new NodeInfo(this));

            while (stack.Any())
            {
                var current = stack.Peek().GetNextDependency();
                if (current != null)
                {
                    PushNodeToStack(circularDependencies, stack, current);
                }
                else
                {
                    stack.Pop();
                }
            }

            return circularDependencies;
        }

        private void PushNodeToStack(List<ICircularDependency> circularDependencies, Stack<NodeInfo> stack, NodeInfo current)
        {
            if (current.Node == this)
            {
                var nodes = stack.Select(info => info.Node);
                circularDependencies.Add(new CircularDependency(nodes));
            }
            else
            {
                var visited = stack.Any(info => info.Node == current.Node);
                if (!visited)
                {
                    stack.Push(current);
                }
            }
        }

        class NodeInfo
        {
            public NodeInfo(IDependencyVisualizationNode node)
            {
                Node = node;
                _index = 0;
            }

            public IDependencyVisualizationNode Node { get; }

            public NodeInfo GetNextDependency()
            {
                if (_index < Node.NodeDependencies.Count)
                {
                    var nextNode = Node.NodeDependencies[_index++];
                    return new NodeInfo(nextNode);
                }

                return null;
            }

            int _index;
        }

        #endregion Methods
    }
}
