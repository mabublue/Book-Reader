using System;
using System.Collections.Generic;
using System.Reflection;

namespace Book_Reader.Domain.BaseTypes
{
    //From https://lostechies.com/jimmybogard/2007/06/25/generic-value-object-equality/
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            Type baseType = typeof(T);

            FieldInfo[] fields = baseType.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                object value1 = field.GetValue(other);
                object value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            T other = obj as T;

            return Equals(other);
        }


        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();
            int startValue = 17;
            int multiplier = 59;
            int hashCode = startValue;


            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }


        private IEnumerable<FieldInfo> GetFields()
        {
            Type t = GetType();
            List<FieldInfo> fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetTypeInfo().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                t = t.GetTypeInfo().BaseType;
            }

            return fields;
        }


        public static bool operator ==(ValueObject<T> x,
                                       ValueObject<T> y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null)) return true;

            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.Equals(y);
        }


        public static bool operator !=(ValueObject<T> x,
                                       ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}
