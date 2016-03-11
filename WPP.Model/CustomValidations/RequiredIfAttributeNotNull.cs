using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPP.Model.CustomValidations
{
    public class RequiredIfAttributeNotNull : RequiredAttribute
    {
        private String PropertyName { get; set; }
        private String DependentPropertyName { get; set; }


        public RequiredIfAttributeNotNull(String propertyName, String dependentPropertyName)
        {
            PropertyName = propertyName;
            DependentPropertyName = dependentPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            Object instance = context.ObjectInstance;
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);
            Object dependentValue = type.GetProperty(DependentPropertyName).GetValue(instance, null);

            if (dependentValue != null)
            {
                if (proprtyvalue == null)
                {
                    ValidationResult result = new ValidationResult(getResourse());
                    return result;
                }
            }
            return ValidationResult.Success;
        }


        private string getResourse()
        {
            return "El campo es obligatorio";

        }
    }
}
