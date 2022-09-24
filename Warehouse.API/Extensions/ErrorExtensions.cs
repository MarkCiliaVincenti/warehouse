﻿using ErrorOr;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationFailure> failures, string instance)
        {
            var errors = failures.ToDictionary(
                p => p.PropertyName,
                v => new []{ v.ErrorMessage });

           return new ValidationProblemDetails(errors)
           {
               Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
               Title = "One or more validation errors occurred.",
               Status = (int)HttpStatusCode.BadRequest,
               Instance = instance,
           };
        }
    }
}
