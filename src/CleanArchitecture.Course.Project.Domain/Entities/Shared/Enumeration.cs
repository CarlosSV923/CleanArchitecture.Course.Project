using System.Reflection;

namespace CleanArchitecture.Course.Project.Domain.Entities.Shared
{
    public abstract class Enumeration<TEnum>(int id, string name) : IEquatable<Enumeration<TEnum>> where TEnum : Enumeration<TEnum>
    {
        private static readonly Dictionary<int, TEnum> _enumerations = CreateEnumerations();
        public string Name { get; protected init; } = name;
        public int Id { get; protected init; } = id;

        public bool Equals(Enumeration<TEnum>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id && Name == other.Name && GetType() == other.GetType();
        }

        public override bool Equals(object? obj)
        {
            return obj is Enumeration<TEnum> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public override string ToString()
        {
            return Name;
        }

        public static TEnum? FromValue(int id)
        {
            return _enumerations.TryGetValue(id, out var result) ? result : null;
        }

        public static TEnum? FromName(string name)
        {
            return _enumerations.Values.FirstOrDefault(e => e.Name == name);
        }

        public static List<TEnum> GetValues()
        {
            return [.. _enumerations.Values];
        }

        public static Dictionary<int, TEnum> CreateEnumerations()
        {
            var enumerationType = typeof(TEnum);

            var fieldsForType = enumerationType.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly |
                    BindingFlags.FlattenHierarchy
                )
                .Where(f => enumerationType.IsAssignableFrom(f.FieldType))
                .Select(f => (TEnum)f.GetValue(default)!);

            return fieldsForType.ToDictionary(e => e.Id);
        }
    }
}