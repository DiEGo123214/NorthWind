using System;
using System.Collections.Generic;
using System.Text;

namespace NorthWind.Validation.Entities.Interfaces;

public interface ICollectionValidationRules<T, TProperty>
{
    ICollectionValidationRules<T, TProperty> SetValidator(IModelValidator<TProperty> validator);
}