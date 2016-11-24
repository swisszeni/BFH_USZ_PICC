﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFH_USZ_PICC.Models
{
    public enum MicroClaveChangementReason
    {
        Routine,
        Pollution
    }

    /// <summary>
    /// Extends the JournalEntry class with one parameters (MicroClaveChangementReason) to handle special events for the MicroClave changing procedure.
    /// </summary>
    class MicroClaveChangingEntry : JournalEntry
    {
        public MicroClaveChangementReason Reason { get; set; }        

        public MicroClaveChangingEntry(DateTime creationalDateTime, DateTime procedureDateTime, HealthInstitution institution, HealthPerson person, MicroClaveChangementReason reason)
        {
            CreationDateTime = creationalDateTime;
            ProcedureDateTime = procedureDateTime;
            Institution = institution;
            Person = person;
            Reason = reason;
           
        }

    }
}