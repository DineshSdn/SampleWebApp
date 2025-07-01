using System.Collections.Generic;

namespace App.Common.Validators.Models
{
    public sealed class ValidationResultDto
    {
        public bool IsValid
        {
            get
            {
                return (Errors?.Count ?? 0) == 0;
            }
        }

        public List<string> Errors { get; init; } = new List<string>();
    }
}
