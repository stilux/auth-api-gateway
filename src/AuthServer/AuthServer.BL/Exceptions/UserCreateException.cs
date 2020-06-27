using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.BL.Exceptions
{
    public class UserCreateException : Exception
    {
        private readonly IEnumerable<IdentityError> _errors;

        public UserCreateException(IEnumerable<IdentityError> errors)
        {
            _errors = errors;
        }
        
        public IEnumerable<IdentityError> Errors => _errors;
    }
}