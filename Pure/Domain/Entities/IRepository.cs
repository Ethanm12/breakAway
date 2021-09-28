using System.Linq;

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
}
