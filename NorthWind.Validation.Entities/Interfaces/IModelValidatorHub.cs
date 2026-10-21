using NorthWind.Validation.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Validation.Entities.Interfaces;

public interface IModelValidatorHub<T>
{
    Task<bool> Validate(T model);
    IEnumerable<ValidationError> Errors { get; }
}