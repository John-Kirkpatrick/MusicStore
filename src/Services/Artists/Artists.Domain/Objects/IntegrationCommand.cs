#region references

using System;

#endregion

namespace Artists.Domain.Objects {
    /// <summary>
    /// </summary>
    public class IntegrationCommand {
        #region Public Constructors

        public IntegrationCommand() {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        #endregion

        #region Public Properties

        public Guid Id { get; }

        public DateTime CreationDate { get; }

        //place to store previous value that has changed
        public string Info { get; set; }

        //specifies whether the command should call context.SaveChanges()
        public bool SaveChanges { get; set; } = true;

        #endregion

        #region Public Methods

        public void SetInfo(
            string info
        ) {
            Info = info;
        }

        #endregion
    }
}