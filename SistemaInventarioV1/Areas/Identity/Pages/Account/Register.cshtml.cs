// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SistemaInventarioV1.Modelos;
using SistemaInventarioV1.Utilidades;

namespace SistemaInventarioV1.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager; //propiedad hace referencia a la creación de los usuarios
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager; //propiedad hace referencia a la creación de los roles

        public RegisterModel(
            //inyección de las dependencias
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            //inicialización
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "El email es requerido")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "La contraseña es requerida")]
            [StringLength(18, ErrorMessage = "La contraseña debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 12)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coincide")]
            public string ConfirmPassword { get; set; }
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage = "Los nombres son requeridos")]
            [MaxLength(100)]
            public string Nombres { get; set; }
            [Required(ErrorMessage = "Los apellidos son requeridos")]
            [MaxLength(100)]
            public string Apellidos { get; set; }
            [Required(ErrorMessage = "La dirección es requerida")]
            [MaxLength(200)]
            public string Direccion { get; set; }

            [Required(ErrorMessage = "La ciudad es requerido")]
            [MaxLength(60)]
            public string Ciudad { get; set; }
            [Required(ErrorMessage = "El país es requerido")]
            [MaxLength(60)]
            public string Pais { get; set; }
            
            public string Rol { get; set; }
            //listado de roles
            public IEnumerable<SelectListItem> ListaRol { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            //llamado a la lista de roles
            Input = new InputModel()
            {
                ListaRol = _roleManager.Roles.Where(r => r.Name!=DS.RolCliente).Select(n => n.Name).Select(l => new SelectListItem
                {
                    Text = l,
                    Value = l
                })
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
        //guarda el nuevo usuario
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = CreateUser();
                //variable de mi modelo creado para usuario
                var user = new UsuarioApp{
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    Nombres = Input.Nombres,
                    Apellidos = Input.Apellidos,
                    Direccion = Input.Direccion,
                    Ciudad = Input.Ciudad,
                    Pais = Input.Pais,
                    Rol = Input.Rol,
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //verificación y creación del rol
                    if(!await _roleManager.RoleExistsAsync(DS.RolAdmin))
                    {
                        //creación del rol
                        await _roleManager.CreateAsync(new IdentityRole(DS.RolAdmin));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.RolCliente))
                    {
                        //creación del rol
                        await _roleManager.CreateAsync(new IdentityRole(DS.RolCliente));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.RolInventario))
                    {
                        //creación del rol
                        await _roleManager.CreateAsync(new IdentityRole(DS.RolInventario));
                    }

                    //asignación por defecto para asignar el rol al usuario si aún no se crea la lista de roles
                    //await _userManager.AddToRoleAsync(user, DS.RolAdmin);

                    //valdación para crear los usuarios con los roles correspondientes
                    if(user.Rol == null) //el valor lo resive desde page
                    {
                        await _userManager.AddToRoleAsync(user, DS.RolCliente);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Rol);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);

                    //Comentra desde aquí 
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirmar tu email",
                        $"Por favor confirma tu cuenta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Haz clic en el enlace</a>.");
                    //hasta aquí si no se ha configurado aún el emailSender y crear una clase EmailSender en Utilidades

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //condición para que el usuario quede logeado dependiendo al rol que pertence
                        if (user.Rol == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            //Adminitrador registra un nuevo Usuario (modulo administración usuario)
                            //"vista","controlador",area
                            return RedirectToAction("Index", "Usuario", new { Area = "Admin" });
                        }
                        
                    }
                }
                //recargar la lista de roles al actualizarse o surgir un error de register
                Input = new InputModel()
                {
                    ListaRol = _roleManager.Roles.Where(r => r.Name != DS.RolCliente).Select(n => n.Name).Select(l => new SelectListItem
                    {
                        Text = l,
                        Value = l
                    })
                };
                //Detalla lso errores que suceden al grabar al nuevo usuario
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
