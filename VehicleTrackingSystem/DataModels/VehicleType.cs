using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VehicleTrackingSystem.DataModels
{
    public partial class VehicleType
    {
        public VehicleType()
        {
            Vehicle = new HashSet<Vehicle>();
        }
        [Key]
        public byte TypeId { get; set; }
        [Required(ErrorMessage ="Vehicle type is required.")]
        public string TypeName { get; set; }

        public ICollection<Vehicle> Vehicle { get; set; }
    }
}
