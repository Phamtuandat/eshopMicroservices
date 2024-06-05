using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Identity.Api.Areas.Identity.Models.Manage;
using Microsoft.AspNetCore.Authorization;
using Identity.Api.Services;
using MassTransit;
using BuildingBlocks.Messaging.Events;
using Microsoft.Extensions.Caching.Distributed;
using Duende.IdentityServer.Extensions;

namespace Identity.Api.Areas.Identity.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Area("Identity")]
    [Authorize]
    [Route("Manage/[action]")]
    public class ManageController(UserManager<ApplicationUser> userMg,
        SignInManager<ApplicationUser> signMg, ILogger<ManageController> logger,
        IWebHostEnvironment env,
        IPublishEndpoint publish,
        IDistributedCache cache
        ) : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInMgr = signMg;
        private readonly UserManager<ApplicationUser> _userMgr = userMg;
        private readonly ILogger<ManageController> _logger = logger;
        private readonly IWebHostEnvironment _env = env;
        private readonly IPublishEndpoint _publish = publish;
        private readonly IDistributedCache _cache = cache;

        [TempData]
        public string? Message { get; set; }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            var profilePicture = "https://th.bing.com/th/id/OIP.XnpM4kcShhqe-aPu7rvF5wHaF3?w=232&h=184&c=7&r=0&o=5&dpr=1.3&pid=1.7";
            var user = await _userMgr.GetUserAsync(User);
            var userModel = user.Adapt<UserProfileViewModel>();
            userModel.ProfilePicture ??= profilePicture;
            return View(userModel);

        }


        [HttpGet]
        public async Task<IActionResult> EditProfilePicture()
        {

            var currentUser = await _userMgr.GetUserAsync(User);
            if (currentUser == null) RedirectToAction("Login", "Account");
            currentUser.ProfilePicture ??= "https://th.bing.com/th/id/OIP.XnpM4kcShhqe-aPu7rvF5wHaF3?w=232&h=184&c=7&r=0&o=5&dpr=1.3&pid=1.7";
            return View(new ProfilePictureViewModel() { ProfilePicture = currentUser?.ProfilePicture });
        }

        [HttpPost]
        public async Task<IActionResult> EditProfilePicture(ProfilePictureViewModel model)
        {
            var file = model.FormFile;
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            if (userId == null) RedirectToAction("login", "account");
            var currentUser = await _userMgr.FindByIdAsync(userId);
            if (currentUser == null) RedirectToAction("Login", "Account");
            currentUser.ProfilePicture ??= "https://th.bing.com/th/id/OIP.XnpM4kcShhqe-aPu7rvF5wHaF3?w=232&h=184&c=7&r=0&o=5&dpr=1.3&pid=1.7";
            if (file != null)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var path = Path.Combine(_env.WebRootPath, "Images/ProfileImage", newFileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    string filePath = Path.Combine("Images", "ProfileImage", newFileName);
                    string url = $"{Request.Scheme}://{Request.Host}/{filePath}";
                    await file.CopyToAsync(stream);
                    currentUser.ProfilePicture = url;
                    await _userMgr.UpdateAsync(currentUser);
                    Message = "Change profile picture is successfully!";
                    _logger.LogInformation(path);
                }
                return RedirectToAction(nameof(Profile));
            }
            return RedirectToAction(nameof(EditProfilePicture));
        }

        [HttpGet]
        public async Task<IActionResult> ChangeName()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeName(string? firstName, string? lastName)
        {
            if (firstName == null || lastName == null) return View();

            var user = await _userMgr.GetUserAsync(User);
            if (user != null) RedirectToAction("login", "account");
            user.FirstName = firstName;
            user.LastName = lastName;
            await _userMgr.UpdateAsync(user);
            Message = "Update name is successfully!";
            return Redirect(nameof(Profile));
        }


        [HttpGet]
        public IActionResult ChangePasswordOption()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SentOtp()
        {
            var user = await _userMgr.GetUserAsync(User);
            if (user != null)
            {
                var otp = GenerateOtp(6);
                if (otp.IsNullOrEmpty())
                {
                    throw new Exception("Can not create OTP code");
                }

                try
                {
                    var token = await _userMgr.GeneratePasswordResetTokenAsync(user);
                    await _cache.SetStringAsync(otp, token);
                    var message = new SentOTPEvent()
                    {
                        Code = otp,
                        Email = user.Email

                    };
                    await _publish.Publish(message);
                    return Ok();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            await _signInMgr.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userMgr.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var isValidPassword = await _userMgr.CheckPasswordAsync(user, model.CurrentPassword);
            if (isValidPassword)
            {
                var token = await _cache.GetStringAsync(model.OtpCode);
                var result = await _userMgr.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded)
                {
                    Message = "Password is changed successfully!";
                    return RedirectToAction(nameof(Profile));
                }
                else
                {
                    Message = "Otp is invalid, please try again!";
                    return View(model);
                }

            }
            else
            {
                Message = "Current password is invalid";
                return View();
            }
        }

        private string GenerateOtp(int length)
        {
            Random random = new Random();
            char[] otp = new char[length];

            for (int i = 0; i < length; i++)
            {
                otp[i] = (char)('0' + random.Next(0, 10));
            }

            return new string(otp);
        }

    }



}
