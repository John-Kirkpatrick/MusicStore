#region references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Artists.Domain.Application.Helpers {
    public abstract class Enumeration : IComparable {
        #region Private Constructors

        protected Enumeration() {
        }

        protected Enumeration(
            int id,
            string name
        ) {
            Id = id;
            Name = name;
        }

        #endregion

        #region Public Properties

        public string Name { get; }

        public int Id { get; }

        #endregion

        #region Public Methods

        public override string ToString() {
            return Name;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration {
            FieldInfo[] fields =
                typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(
            object obj
        ) {
            Enumeration otherValue = obj as Enumeration;
            if (otherValue == null) {
                return false;
            }

            bool typeMatches = GetType().Equals(obj.GetType());
            bool valueMatches = Id.Equals(otherValue.Id);
            return typeMatches && valueMatches;
        }

        public int CompareTo(
            object other
        ) {
            return Id.CompareTo(((Enumeration) other).Id);
        }

        #endregion
    }
}