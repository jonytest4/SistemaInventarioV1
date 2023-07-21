using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV1.Utilidades
{
    public class ErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresLower),
                Description = "La contraseña debe tener al menos una letra Minúscula"
            };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError() { 
                Code = nameof(PasswordRequiresUpper),
                Description = "La contrase debe tener al menos una letra Mayúscula"
            };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError() {
               Code = nameof(PasswordRequiresDigit),
               Description = "La contraseña debe tener al menos un Dígito"
            };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "La contraseña debe tener al menos un caracter Especial"
            };
        }
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordMismatch),
                Description = "Las contraseñas no coinciden"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = nameof(PasswordTooShort),
                Description = "La contraseña debe tener al menos 12 caracteres"
            };
        }
    }
}
