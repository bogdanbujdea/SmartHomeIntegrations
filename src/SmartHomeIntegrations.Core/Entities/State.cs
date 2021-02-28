using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHomeIntegrations.Core.Entities
{
    [Table("states")]
    public class State
    {
        [Column("state_id")]
        public int StateId { get; set; }

        [Column("state")]
        public string CurrentState { get; set; }

        [Column("last_updated")]
        public DateTime LastUpdated { get; set; }

        [Column("attributes")]
        public string Attributes { get; set; }
        
        [Column("entity_id")]
        public string EntityId { get; set; }

        [Column("old_state_id")]
        public int? OldStateId { get; set; }

        [Column("domain")]
        public string Domain { get; set; }

        [Column("last_changed")]
        public DateTime LastChanged { get; set; }
    }
}
