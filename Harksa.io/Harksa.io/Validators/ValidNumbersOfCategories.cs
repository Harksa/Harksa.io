using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Harksa.io.Validators
{
    public class ValidNumbersOfCategories : ValidationAttribute
    {
        public override bool IsValid(object value) {
            if (value == null) return true;

            var str = value.ToString();

            if (String.IsNullOrWhiteSpace(str)) {
                return true;
            }

            var arrays = str.Split(';');

            return arrays.Length <= 3;
        }
    }
}
