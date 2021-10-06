using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BreakAway.Entities
{
    public interface IRepository
    {
        ITable<Activity> Activities { get; }
        ITable<Contact> Contacts { get; }
        ITable<Customer> Customers { get; }
        ITable<Destination> Destinations { get; }
        ITable<Equipment> Equipments { get; }
        ITable<Payment> Payments { get; }
        ITable<Reservation> Reservations { get; }
        ITable<Lodging> Lodgings { get; }
        ITable<Event> Events { get; }
        ITable<Address> Addresses { get; }

        void Save();
    } 

    public interface ITable<T> : IQueryable<T> where T : class
    {
        void Add(T item);

        void Delete(T item);
    }

    public static class QueryableExtension
    {
        public static ITable<T> AsTable<T>(this IQueryable<T> source) where T : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ITable<T>)
            {
                return (ITable<T>)source;
            }

            return new EnumerableTable<T>(source);
        }
    }

    internal class EnumerableTable<T> : ITable<T> where T : class
    {
        private IQueryable<T> _source;

        public EnumerableTable(IQueryable<T> source)
        {
            _source = source;
        }

        public Expression Expression => _source.Expression;

        public Type ElementType => _source.ElementType;

        public IQueryProvider Provider => _source.Provider;

        public void Add(T item)
        {
            var list = _source.ToList();
            list.Add(item);
            _source = list.AsQueryable();
        }

        public void Delete(T item)
        {
            var list = _source.ToList();
            list.Remove(item);
            _source = list.AsQueryable();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }
    }
}
