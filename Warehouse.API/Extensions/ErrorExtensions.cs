﻿using ErrorOr;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Vayosoft.Core.SharedKernel.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace Warehouse.API.Extensions
{
    public static class ErrorExtensions
    {
        public static ErrorOr<T> ToErrorOr<T>(this List<ValidationFailure> failures)
        {
            var errors = failures.ConvertAll(x => Error.Validation(
                code: x.PropertyName,
                description: x.ErrorMessage));

            return ErrorOr<T>.From(errors);
        }

        public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationFailure> failures)
        {
            var errors = failures.ToDictionary(
                p => p.PropertyName,
                v => new []{ v.ErrorMessage });

           return new ValidationProblemDetails(errors);
        }
    }
}
