using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VehicleTrackingSystem.DataModels;
namespace VehicleTrackingSystem.Attributes
{
    public class TireConditionRequiredIfHasTiresAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           Vehicle  vehicle = (Vehicle)validationContext.ObjectInstance;

            if (vehicle.TireQty > 0 && vehicle.ConditionId==null)
            {
                return new ValidationResult("If tire quantity is more than 0, the vehicle tire condition must be required.");
            }
            else if (vehicle.TireQty == 0 && vehicle.ConditionId != null)
            {
                return new ValidationResult("The tire quantity is 0, the vehicle tire condition is not required.");
            }
            return ValidationResult.Success;
        }
    }
}
