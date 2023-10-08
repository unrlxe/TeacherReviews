﻿using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TeacherReviews.Domain.Exceptions;

public class EntityValidationException : BaseApiException
{
    public new const int DefaultCode = StatusCodes.Status400BadRequest;

    public EntityValidationException(ModelStateDictionary modelStateDictionary) : base(
        DefaultCode,
        SerializeErrors(modelStateDictionary)
    )
    {
    }

    private static List<string> SerializeErrors(ModelStateDictionary modelStateDictionary)
    {
        List<string> errors = new();
        foreach (var (_, value) in modelStateDictionary)
        {
            errors.AddRange(value.Errors.Select(error => error.ErrorMessage));
        }

        return errors;
    }
}