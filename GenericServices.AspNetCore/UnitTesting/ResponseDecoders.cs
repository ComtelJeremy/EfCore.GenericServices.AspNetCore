﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace GenericServices.AspNetCore.UnitTesting
{
    /// <summary>
    /// This contains extention methods to decode the response so that you can unit test/integeration test your Web API code
    /// </summary>
    public static class ResponseDecoders
    {
        /// <summary>
        /// This extracts the status code from the IActionResult
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns>an int - throws exceptions if status is null</returns>
        public static int GetStatusCode(this IActionResult actionResult)
        {
            var objResult = (actionResult as ObjectResult);
            if (objResult == null)
                throw new NullReferenceException("Could not cast the response to ObjectResult");
            return objResult.StatusCode ?? throw new NullReferenceException("Status Code as null");
        }

        /// <summary>
        /// This extracts the status code from the ActionResult<T>
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns>an int - throws exceptions if status is null</returns>
        public static int GetStatusCode<T>(this ActionResult<T> actionResult)
        {
            var objResult = (actionResult.Result as ObjectResult);
            if (objResult == null)
                throw new NullReferenceException("Could not cast the response to ObjectResult");
            return objResult.StatusCode ?? throw new NullReferenceException("Status Code as null");
        }

        /// <summary>
        /// This converts the IActionResult created by <see cref="CreateResponse"/> into a GenericServices.IStatusGeneric
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns>a status which is similar to the original status (errors might not be in the exact same form)</returns>
        public static IStatusGeneric CopyToStatus(this IActionResult actionResult)
        {
            var testStatus = new StatusGenericHandler();
            var objResult = (actionResult as ObjectResult);
            if (objResult == null)
                throw new NullReferenceException("Could not cast the response to ObjectResult");
            var errorPart = objResult as BadRequestObjectResult;
            if (errorPart != null)
            {
                //It has errors, so copy errors to status
                testStatus.AddValidationResults(ExtractErrors(errorPart));
                return testStatus;
            }
            var messagePart = objResult.Value as WebApiMessageOnly;
            if (messagePart == null)
                throw new NullReferenceException("Could not cast the response to WebApiMessageOnly");
            testStatus.Message = messagePart.Message;

            return testStatus;
        }

        /// <summary>
        /// This converts the ActionResult{T}; created by <see cref="CreateResponse"/> into a GenericServices.IStatusGeneric
        /// </summary>
        /// <param name="actionResult"></param>
        /// <returns>a status which is similar to the original status (errors might not be in the exact same form)</returns>
        public static IStatusGeneric<T> CopyToStatus<T>(this ActionResult<T> actionResult)
        {
            var testStatus = new StatusGenericHandler<T>();
            var objResult = (actionResult.Result as ObjectResult);
            if (objResult == null)
                throw new NullReferenceException("Could not cast the response to ObjectResult");
            var errorPart = objResult as BadRequestObjectResult;
            if (errorPart != null)
            {
                //It has errors, so copy errors to status
                testStatus.AddValidationResults(ExtractErrors(errorPart));
                return testStatus;
            }
            var messageAndResult = objResult.Value as WebApiMessageAndResult<T>;
            if (messageAndResult == null)
                throw new NullReferenceException("Could not cast the response to WebApiMessageAndResult<T>");
            testStatus.Message = messageAndResult.Message;
            testStatus.SetResult(messageAndResult.Results);

            return testStatus;
        }

        //----------------------------------------------------
        //private

        private static IEnumerable<ValidationResult> ExtractErrors(BadRequestObjectResult result)
        {
            var errors = new List<ValidationResult>();
            var dict = (Dictionary<string, object>)result.Value;
            foreach (var propertyName in dict.Keys)
            {
                var dictErrors = ((string[])dict[propertyName]);
                errors.AddRange(dictErrors.Select(t => new ValidationResult(t, new[] {propertyName})));
            }

            return errors;
        }


    }
}