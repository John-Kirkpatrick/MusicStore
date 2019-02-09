#region references

using Artists.Domain.Application.Helpers;

#endregion

namespace Artists.Domain.Objects {
    public abstract class AggregateRoot : Enumeration {
        #region Static Fields

        public static readonly AggregateRoot Constituent = new ConstituentRoot();
        public static readonly AggregateRoot Event = new EventRoot();
        public static readonly AggregateRoot Order = new OrderRoot();

        #endregion

        #region Private Constructors

        private AggregateRoot(
            int id,
            string name
        ) : base(id, name) {
        }

        #endregion

        private class ConstituentRoot : AggregateRoot {
            #region Public Constructors

            public ConstituentRoot() : base(1, $"{nameof(Constituent)}") {
            }

            #endregion
        }

        private class EventRoot : AggregateRoot {
            #region Public Constructors

            public EventRoot() : base(2, $"{nameof(Event)}") {
            }

            #endregion
        }

        private class OrderRoot : AggregateRoot {
            #region Public Constructors

            public OrderRoot() : base(3, $"{nameof(Order)}") {
            }

            #endregion
        }
    }
}