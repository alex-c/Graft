using Graft.Primitives;
using System.Collections.Generic;

namespace Graft.Default
{
    public class Vertex<T> : IVertex<T>
    {
        private Dictionary<string, object> Attributes { get; }

        public T Value { get; set; }

        public Vertex(T value)
        {
            Value = value;
            Attributes = new Dictionary<string, object>();
        }

        #region Attributes

        public bool HasAttribute(string attribute) => Attributes.ContainsKey(attribute);

        public object GetAttribute(string attribute)
        {
            if (Attributes.TryGetValue(attribute, out object value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"There is no attribute '{attribute}' set on this vertex.");
            }
        }

        public TAttr GetAttribute<TAttr>(string attribute)
        {
            if (Attributes.TryGetValue(attribute, out object value))
            {
                return (TAttr)value;
            }
            else
            {
                throw new KeyNotFoundException($"There is no attribute '{attribute}' set on this vertex.");
            }
        }

        public void SetAttribute(string attribute, object value)
        {
            Attributes[attribute] = value;
        }

        public bool TryGetAttribute(string attribute, out object value) => Attributes.TryGetValue(attribute, out value);

        public bool TryGetAttribute<TAttr>(string attribute, out TAttr value)
        {
            if (Attributes.TryGetValue(attribute, out object ovalue))
            {
                value = (TAttr)ovalue;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        #endregion
    }
}
