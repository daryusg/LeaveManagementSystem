// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using LeaveManagementSystem.Application.Services.LeaveAllocations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace LeaveManagementSystem.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicatiionUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILeaveAllocationsService _leaveAllocationsService;
        private readonly UserManager<ApplicatiionUser> _userManager;
        private readonly IUserStore<ApplicatiionUser> _userStore;
        private readonly IUserEmailStore<ApplicatiionUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment; //189


        public RegisterModel(
            ILeaveAllocationsService leaveAllocationService,
            UserManager<ApplicatiionUser> userManager,
            IUserStore<ApplicatiionUser> userStore,
            SignInManager<ApplicatiionUser> signInManager,
            RoleManager<IdentityRole> roleManager, /* added line*/
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment/*189*/)
        {
            this._leaveAllocationsService = leaveAllocationService;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            this._roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment; //189
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        //public InputModel Input { get; set; }
        public InputModel Input { get; set; } = new();

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
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required] //link(ed) to the data model
            [DataType(DataType.Date)]
            [Display(Name = "Date Of Birth")]
            public DateOnly DateOfBirth { get; set; }

            [Display(Name = "Role")]
            public string RoleName { get; set; }
            public string[] RoleNames { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            //
            //
            var roles = await _roleManager.Roles
                .Select(q => q.Name)
                .Where(q => q != Roles.Administrator)
                .ToArrayAsync();
            Input.RoleNames = roles;
            //
            //
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //
                //to prevent: SqlException: Cannot insert the value NULL into column 'FirstName', table 'LeaveManagementSystemDb.dbo.AspNetUsers'; column does not allow nulls. INSERT fails. The statement has been terminated.
                user.DateOfBirth = Input.DateOfBirth;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                //
                //
                //the following line inserts into db
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    //
                    //
                    //Add Role(Name)
                    if (Input.RoleName == Roles.Supervisor)
                        await _userManager.AddToRolesAsync(user, [Roles.Employee, Roles.Supervisor]);
                    else
                        await _userManager.AddToRoleAsync(user, Roles.Employee);
                    //
                    //
                    var userId = await _userManager.GetUserIdAsync(user);
                    await _leaveAllocationsService.AllocateLeaveAsync(userId); //124
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    //
                    //
                    //grab the template 189
                    var emailTemplatePath = Path.Combine(_webHostEnvironment.WebRootPath, "templates", "layout_email.html");
                    var template = await System.IO.File.ReadAllTextAsync(emailTemplatePath);
                    var messageBody = template
                        .Replace("{UserName}", $"{Input.FirstName} {Input.LastName}")
                        .Replace("{MessageContent}", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", messageBody);
                    //
                    //
                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."); //189

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            // we also know that we need to reload the list of roles since it was a form-submitted value
            //
            //
            var roles = await _roleManager.Roles
                .Select(q => q.Name)
                .Where(q => q != Roles.Administrator)
                .ToArrayAsync();
            Input.RoleNames = roles;
            //
            //
            return Page();
        }

        private ApplicatiionUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicatiionUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicatiionUser)}'. " +
                    $"Ensure that '{nameof(ApplicatiionUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicatiionUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicatiionUser>)_userStore;
        }
    }
}
