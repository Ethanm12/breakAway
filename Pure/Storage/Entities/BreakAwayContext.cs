using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace BreakAway.Entities
{
    public interface IBreakAwayContext
    {
        IDbSet<Activity> Activities { get; }
        IDbSet<Contact> Contacts { get; }
        IDbSet<Customer> Customers { get; }
        IDbSet<Destination> Destinations { get; }
        IDbSet<Equipment> Equipments { get; }
        IDbSet<Event> Events { get; }
        IDbSet<Lodging> Lodgings { get; }
        IDbSet<Payment> Payments { get; }
        IDbSet<Reservation> Reservations { get; }
        IDbSet<Address> Addresses { get; }

        int SaveChanges();
    }

    public class BreakAwayContext : DbContext, IBreakAwayContext
    {
        public BreakAwayContext(string connectionString) : base(connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            Activities = Set<Activity>();
            Contacts = Set<Contact>();
            Customers = Set<Customer>();
            Destinations = Set<Destination>();
            Equipments = Set<Equipment>();
            Events = Set<Event>();
            Lodgings = Set<Lodging>();
            Payments = Set<Payment>();
            Reservations = Set<Reservation>();
            Addresses = Set<Address>();
        }

        public IDbSet<Activity> Activities { get; private set; }
        public IDbSet<Contact> Contacts { get; private set; }
        public IDbSet<Customer> Customers { get; private set; }
        public IDbSet<Destination> Destinations { get; private set; }
        public IDbSet<Equipment> Equipments { get; private set; }
        public IDbSet<Event> Events { get; private set; }
        public IDbSet<Lodging> Lodgings { get; private set; }
        public IDbSet<Payment> Payments { get; private set; }
        public IDbSet<Reservation> Reservations { get; private set; }
        public IDbSet<Address> Addresses { get; private set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            MapActivity(modelBuilder.Entity<Activity>());
            MapEquipment(modelBuilder.Entity<Equipment>());
            MapAddress(modelBuilder.Entity<Address>());
            MapContact(modelBuilder.Entity<Contact>());
            MapCustomer(modelBuilder.Entity<Customer>());
            MapEvent(modelBuilder.Entity<Event>());
            MapDestination(modelBuilder.Entity<Destination>());
            MapLodging(modelBuilder.Entity<Lodging>());
            MapPayment(modelBuilder.Entity<Payment>());
            MapReservation(modelBuilder.Entity<Reservation>());
            MapResort(modelBuilder.Entity<Resort>());

            MapAddressMail(modelBuilder.ComplexType<Mail>());
        }

        #region Mappings

        private void MapAddressMail(ComplexTypeConfiguration<Mail> complexType)
        {
            complexType.Property(m => m.Street1).HasColumnName("Street1").HasMaxLength(50);

            complexType.Property(m => m.Street2).HasColumnName("Street2");

            complexType.Property(m => m.City).HasColumnName("City");

            complexType.Property(m => m.StateProvince).HasColumnName("StateProvince");
        }

        private void MapReservation(EntityTypeConfiguration<Reservation> entity)
        {
            entity.HasKey(e => e.Id).ToTable("Reservations");

            entity.Property(e => e.Id).HasColumnName("ReservationID");

            entity.Property(e => e.ReservationDate).HasColumnType("DATETIME");

            entity.Property(e => e.RowVersion).IsRowVersion();

            entity.Property(e => e.CustomerId).HasColumnName("ContactID");

            entity.Property(e => e.EventId).HasColumnName("EventID");

            entity.HasRequired(e => e.Customer).WithMany(c => c.Reservations).HasForeignKey(e => e.CustomerId).WillCascadeOnDelete(true);

            entity.HasOptional(e => e.Event).WithMany().HasForeignKey(e => e.EventId);
        }

        private void MapPayment(EntityTypeConfiguration<Payment> entity)
        {
            entity.HasKey(e => e.Id).ToTable("Payments");

            entity.Property(e => e.Id).HasColumnName("PaymentID");

            entity.Property(e => e.PaymentDate).HasColumnName("PaymentDate").HasColumnType("DATETIME").IsOptional();

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

            entity.Property(e => e.Amount).HasColumnName("Amount").HasColumnType("MONEY").IsOptional();

            entity.Property(e => e.ModifiedDate).HasColumnType("DATETIME");

            entity.Property(e => e.RowVersion).IsRowVersion();

            entity.HasRequired(e => e.Reservation).WithMany(e => e.Payments).HasForeignKey(e => e.ReservationId);
        }

        private void MapLodging(EntityTypeConfiguration<Lodging> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Lodging");

            entity.Property(k => k.Id).HasColumnName("LodgingID");

            entity.Property(k => k.Name).HasColumnName("LodgingName");

            entity.Property(k => k.ContactId).HasColumnName("ContactID");

            entity.Property(k => k.LocationId).HasColumnName("LocationID");

            entity.HasRequired(k => k.Contact).WithMany().HasForeignKey(k => k.ContactId);

            entity.Map<Resort>(m => m.Requires("Resort").HasValue("1"));
        }

        private void MapResort(EntityTypeConfiguration<Resort> entity)
        {
            entity.Property(e => e.ResortChainOwner).HasColumnName("ResortChainOwner");

            entity.Property(e => e.IsLuxuryResort).HasColumnName("LuxuryResort");
        }

        private void MapDestination(EntityTypeConfiguration<Destination> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Locations");

            entity.Property(k => k.Id).HasColumnName("LocationID");

            entity.Property(k => k.Name).HasColumnName("LocationName");
        }

        private void MapEvent(EntityTypeConfiguration<Event> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Events");

            entity.Property(k => k.Id).HasColumnName("EventID");

            entity.Property(k => k.LocationId).HasColumnName("LocationID");

            entity.Property(k => k.LodgingId).HasColumnName("LodgingID");

            entity.Property(k => k.StartDate).HasColumnType("DATETIME");

            entity.Property(k => k.EndDate).HasColumnType("DATETIME");

            entity.Property(k => k.TripCostUSD).IsOptional();

            entity.HasMany(k => k.Activities).WithMany().Map(m => m.MapLeftKey("EventID").MapRightKey("ActivityID").ToTable("EventActivities"));
        }

        private void MapContact(EntityTypeConfiguration<Contact> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Contact");

            entity.Property(k => k.Id).HasColumnName("ContactID").IsRequired();

            entity.Property(k => k.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);

            entity.Property(k => k.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);

            entity.Property(k => k.Title).HasColumnName("Title").HasMaxLength(50);

            entity.Property(k => k.AddDate).HasColumnName("AddDate").HasColumnType("DATETIME").IsRequired();

            entity.Property(k => k.ModifiedDate).HasColumnName("ModifiedDate").HasColumnType("DATETIME").IsRequired();

            entity.Property(k => k.RowVersion).IsRowVersion();

            entity.Map<Customer>(m =>
            {
                m.Properties(c => new
                    {
                        c.CustomerType,
                        c.InitialDate,
                        c.PrimaryActivityId,
                        c.PrimaryDestinationId,
                        c.SecondaryActivityId,
                        c.SecondaryDestinationId,
                        c.Notes,
                        c.CustomerRowVersion
                    });
                m.ToTable("Customers");
            });

            entity.HasMany(k => k.Addresses).WithRequired(k => k.Contact).HasForeignKey(k => k.ContactId).WillCascadeOnDelete(true);

            entity.Ignore(k => k.FullName);
        }

        private void MapCustomer(EntityTypeConfiguration<Customer> entity)
        {
            entity.Property(k => k.CustomerType).HasColumnName("CustomerTypeID");

            entity.Property(k => k.InitialDate).HasColumnType("DATETIME").HasColumnName("InitialDate").IsOptional();

            entity.Property(k => k.PrimaryDestinationId).HasColumnName("PrimaryDesintation");

            entity.Property(k => k.SecondaryDestinationId).HasColumnName("SecondaryDestination");

            entity.Property(k => k.PrimaryActivityId).HasColumnName("PrimaryActivity");

            entity.Property(k => k.SecondaryActivityId).HasColumnName("SecondaryActivity");

            entity.Property(k => k.Notes);

            entity.Property(k => k.CustomerRowVersion).HasColumnName("RowVersion").IsRowVersion().IsConcurrencyToken(false);

            entity.HasOptional(k => k.PrimaryActivity).WithMany().HasForeignKey(k => k.PrimaryActivityId);

            entity.HasOptional(k => k.SecondaryActivity).WithMany().HasForeignKey(k => k.SecondaryActivityId);

            entity.HasOptional(k => k.PrimaryDestination).WithMany().HasForeignKey(k => k.PrimaryDestinationId);

            entity.HasOptional(k => k.SecondaryDestination).WithMany().HasForeignKey(k => k.SecondaryDestinationId);

            entity.Property(k => k.BirthDate).HasColumnType("DATETIME").IsOptional();

            entity.Property(k => k.HeightInches).IsOptional();

            entity.Property(k => k.WeightPounds).IsOptional();

            entity.Property(k => k.DietaryRestrictions).IsOptional();

            entity.HasMany(k => k.Reservations).WithRequired().HasForeignKey(k => k.CustomerId).WillCascadeOnDelete(true);

            entity.Map(m =>
            {
                m.Properties(c => new { c.BirthDate, c.HeightInches, c.WeightPounds, c.DietaryRestrictions });
                m.ToTable("ContactPersonalInfo");
            });
        }

        private void MapAddress(EntityTypeConfiguration<Address> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Address");

            entity.Property(k => k.Id).HasColumnName("AddressID");

            entity.Property(k => k.CountryRegion).HasColumnName("CountryRegion");

            entity.Property(k => k.PostalCode).HasColumnName("PostalCode");

            entity.Property(k => k.AddressType).HasColumnName("AddressType");

            entity.Property(k => k.ContactId).HasColumnName("ContactID");

            entity.Property(k => k.ModifiedDate).HasColumnType("DATETIME");

            //entity.Property(k => k.RowVersion).HasColumnName("TimeStamp").IsRowVersion();

            entity.HasRequired(k => k.Contact).WithMany(c => c.Addresses).HasForeignKey(k => k.ContactId);
        }

        private void MapEquipment(EntityTypeConfiguration<Equipment> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Equipment");

            entity.Property(k => k.Id).HasColumnName("EquipmentID");

            entity.Property(k => k.Name).HasColumnName("Equipment");
        }

        private void MapActivity(EntityTypeConfiguration<Activity> entity)
        {
            entity.HasKey(k => k.Id).ToTable("Activities");

            entity.Property(k => k.Id).HasColumnName("ActivityID");

            entity.Property(k => k.Name).HasColumnName("Activity");

            entity.Property(k => k.ImagePath).HasColumnName("imagepath");

            entity.Property(k => k.Category).HasColumnName("Category");

            entity.HasMany(k => k.Equipments).WithMany().Map(m => m.MapLeftKey("ActivityID").MapRightKey("EquipmentID").ToTable("ActivityEquipment"));
        }

        #endregion
    }
}