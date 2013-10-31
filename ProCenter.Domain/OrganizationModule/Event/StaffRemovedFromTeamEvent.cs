﻿namespace ProCenter.Domain.OrganizationModule.Event
{
    #region Using Statements

    using System;
    using CommonModule;

    #endregion

    /// <summary>
    ///     Event for removing staff from team.
    /// </summary>
    public class StaffRemovedFromTeamEvent : CommitEventBase
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StaffRemovedFromTeamEvent" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="version">The version.</param>
        /// <param name="staffKey">The staff key.</param>
        public StaffRemovedFromTeamEvent ( Guid key, int version, Guid staffKey )
            : base ( key, version )
        {
            StaffKey = staffKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the staff key.
        /// </summary>
        /// <value>
        ///     The staff key.
        /// </value>
        public Guid StaffKey { get; private set; }

        #endregion
    }
}