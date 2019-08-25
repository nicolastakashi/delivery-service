namespace DeliveryService.Domain.ValueObject
{
    public abstract class BaseValueObject<T> where T : BaseValueObject<T>
    {
        public static bool operator !=(BaseValueObject<T> objetoA,
            BaseValueObject<T> objetoB) => !(objetoA == objetoB);

        public static bool operator ==(BaseValueObject<T> objetoA, BaseValueObject<T> objetoB)
        {
            if (ReferenceEquals(objetoA, null) && ReferenceEquals(objetoB, null)) return true;

            if (ReferenceEquals(objetoA, null) || ReferenceEquals(objetoB, null)) return false;

            return objetoA.Equals(objetoB);
        }

        public override bool Equals(object valueObject)
        {
            var valueObj = valueObject as T;

            if (ReferenceEquals(valueObj, null)) return false;

            return EqualsCore(valueObj);
        }

        public override int GetHashCode() => GetHashCodeCore();

        protected abstract bool EqualsCore(T other);

        protected abstract int GetHashCodeCore();
    }
}
