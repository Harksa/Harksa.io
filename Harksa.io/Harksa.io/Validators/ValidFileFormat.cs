using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Harksa.io.Validators
{
    public class ValidFileFormat : ValidationAttribute
    {
        public string AcceptedFormats { get; set; }

        public override bool IsValid(object value) {
            if (!(value is IFormFile)) return false;
            
            var file = (IFormFile) value;

            string uploadedFileFormat = System.IO.Path.GetExtension(file.FileName);

            var acceptedFormatList = AcceptedFormats.Split(",");

            return acceptedFormatList.Contains(uploadedFileFormat);
        }
    }
}
