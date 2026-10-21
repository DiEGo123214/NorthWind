using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Validation.Entities.Enums;

public enum ValidationConstraint
{
    AlwaysValidate,
    ValidateIfThereAreNoPreviousErrors
}