using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VehicleTrackingSystem.Attributes;
namespace VehicleTrackingSystem.DataModels
{
    public partial class Vehicle
    {
        [Key]
        public int Vid { get; set; }
        [Required(ErrorMessage ="Vehicle type is required.")]
        public byte TypeId { get; set; }
        [Required(ErrorMessage ="Fuel level is required."),Range(0,100,ErrorMessage ="Fuel Level (%) must be between 0 to 100.")]
        public double FuelLevel { get; set; }
        [Required(ErrorMessage = "VIN/HIN is required.")]
        public string Idnumber { get; set; }

        [TireConditionRequiredIfHasTires]
        public byte? ConditionId { get; set; }

        [Required(ErrorMessage ="Tire quantity is required."),Range(0, int.MaxValue, ErrorMessage = "Tire Quantity must not be a negative integer.")]
        public int TireQty { get; set; }

        public TireCondition TireCondition { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
