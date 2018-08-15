using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VehicleTrackingSystem.DataModels
{
    public partial class TireCondition
    {
        public TireCondition()
        {
            Vehicle = new HashSet<Vehicle>();
        }

        public byte ConditionId { get; set; }
        [Required(ErrorMessage ="Tire condition is required.")]
        public string Condition { get; set; }

        public ICollection<Vehicle> Vehicle { get; set; }
    }
}
