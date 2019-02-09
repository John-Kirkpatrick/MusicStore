#region references

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Artists.Domain.Objects
{
    /// <summary>
    /// </summary>
    public class Command
    {
        #region Public Constructors

        public Command(IntegrationCommand command)
        {
            CommandId = command.Id;
            CreationTime = command.CreationDate;
            EventTypeName = command.GetType().FullName;
            Content = JsonConvert.SerializeObject(command);
            State = (int)EventStateEnum.NotPublished;
            Info = command.Info;
        }

        #endregion

        #region Private Constructors

        public Command()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the command identifier.
        /// </summary>
        /// <value>
        ///     The command identifier.
        /// </value>
        public Guid CommandId { get; set; }

        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        [Required]
        public string Content { get; set; }

        /// <summary>
        ///     Gets or sets the creation time.
        /// </summary>
        /// <value>
        ///     The creation time.
        /// </value>
        [Column(TypeName = "datetime")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        ///     Gets or sets the name of the event type.
        /// </summary>
        /// <value>
        ///     The name of the event type.
        /// </value>
        [Required]
        [StringLength(200)]
        public string EventTypeName { get; set; }

        /// <summary>
        ///     Gets or sets the state.
        /// </summary>
        /// <value>
        ///     The state.
        /// </value>
        public int State { get; set; }

        [StringLength(255)]
        public string Info { get; set; }

        #endregion
    }

    public enum EventStateEnum
    {
        NotPublished = 0, Published = 1, PublishedFailed = 2
    }
}