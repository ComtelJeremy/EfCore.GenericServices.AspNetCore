﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using AspNetCore;
using GenericServices.AspNetCore;
using StatusGeneric;
using Test.Helpers;
using Xunit;

namespace Test.UnitTests
{
    public class TestGenericServicesResponse
    {
        [Fact]
        public void TestStatusIsValidOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.Response();

            //VERIFY
            actionResult.CheckResponse(status);
        }

        [Fact]
        public void TestStatusIsValidWithStatusCodeOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.ResponseWithValidCode(201);

            //VERIFY
            actionResult.CheckResponseWithValidCode(status, 201);
        }

        [Fact]
        public void TestStatusHasErrorOk()
        {
            //SETUP
            var status = new StatusGenericHandler();
            status.AddError("An Error", "MyProp");

            //ATTEMPT
            var actionResult = status.Response();

            //VERIFY
            actionResult.CheckResponse(status);
        }

        [Fact]
        public void TestStatusHasTwoErrorOnSamePropOk()
        {
            //SETUP
            var status = new StatusGenericHandler();
            status.AddError("An Error", "MyProp");
            status.AddError("Another Error", "MyProp");

            //ATTEMPT
            var actionResult = status.Response();

            //VERIFY
            actionResult.CheckResponse(status);
        }

        [Fact]
        public void TestStatusHasTwoErrorOnSameOneDifferentPropOk()
        {
            //SETUP
            var status = new StatusGenericHandler();
            status.AddError("An Error", "MyProp");
            status.AddError("Another Error", "MyProp");
            status.AddError("Global Error");

            //ATTEMPT
            var actionResult = status.Response();

            //VERIFY
            actionResult.CheckResponse(status);
        }

        //------------------------------------------------------
        //with return result

        [Fact]
        public void TestStatusIsValidWithResultOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.Response(1);

            //VERIFY
            actionResult.CheckResponse(status, 1);
        }

        [Fact]
        public void TestStatusIsValidWithResultAndStatusCodeOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.ResponseWithValidCode(1, 201);

            //VERIFY
            actionResult.CheckResponseWithValidCode(status, 1, 201);
        }

        [Fact]
        public void TestStatusIsValidWithClassResultOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.Response<string>("Hello");

            //VERIFY
            actionResult.CheckResponse(status, "Hello");
        }

        [Fact]
        public void TestStatusIsValidWithClassResultNullOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.Response<string>(null);

            //VERIFY
            actionResult.CheckResponse(status, null);
        }

        [Fact]
        public void TestStatusIsValidWithClassResultNullAndStatusCodeOk()
        {
            //SETUP
            var status = new StatusGenericHandler();

            //ATTEMPT
            var actionResult = status.ResponseWithValidCode<string>(null, CreateResponse.OkStatusCode);

            //VERIFY
            actionResult.CheckResponse(status, null);
        }

        [Fact]
        public void TestStatusErrorWithResultOk()
        {
            //SETUP
            var status = new StatusGenericHandler();
            status.AddError("Bad");

            //ATTEMPT
            var actionResult = status.Response(1);

            //VERIFY
            actionResult.CheckResponse(status, 1);
        }

        [Fact]
        public void TestStatusErrorWithClassResultNullOk()
        {
            //SETUP
            var status = new StatusGenericHandler();
            status.AddError("Bad");

            //ATTEMPT
            var actionResult = status.Response<string>(null);

            //VERIFY
            actionResult.CheckResponse(status, null);
        }
    }
}