using System;
using System.Text;
using System.Threading.Tasks;

namespace BreakAway.Entities
{
    public class SqlRepository : IRepository
    {
        private readonly IBreakAwayContext _context;

        public SqlRepository(IBreakAwayContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;

            Activities = new DbSetTable<Activity>(_context.Activities);
            Contacts = new DbSetTable<Contact>(_context.Contacts);
            Customers = new DbSetTable<Customer>(_context.Customers);
            Destinations = new DbSetTable<Destination>(_context.Destinations);
            Events = new DbSetTable<Event>(_context.Events);
            Equipments = new DbSetTable<Equipment>(_context.Equipments);
            Payments = new DbSetTable<Payment>(_context.Payments);
            Reservations = new DbSetTable<Reservation>(_context.Reservations);
            Lodgings = new DbSetTable<Lodging>(_context.Lodgings);
            Addresses = new DbSetTable<Address>(_context.Addresses);
        }

        public ITable<Activity> Activities { get; }

        public ITable<Contact> Contacts { get; }

        public ITable<Customer> Customers { get; }

        public ITable<Destination> Destinations { get; }

        public ITable<Equipment> Equipments { get; }

        public ITable<Event> Events { get; }

        public ITable<Payment> Payments { get; }

        public ITable<Reservation> Reservations { get; }

        public ITable<Lodging> Lodgings { get; }
        public ITable<Address> Addresses { get; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
