using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Explordamweb.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {
        private IGamesRepository repository;
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private RoleManager<IdentityRole> roleManager;
        public int PageSize = 100;

        public AdminController(IGamesRepository repo,
            RoleManager<IdentityRole> roleMgr,
            UserManager<AppUser> usrMgr,
            IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash)
        {
            repository = repo;
            roleManager = roleMgr;
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        

        public IActionResult Index() => View();
        public IActionResult Charts() => View();
        public IActionResult Roles() => View(roleManager.Roles);
        public IActionResult CreateRole() => View();
        public IActionResult Users() => View(userManager.Users);
        public IActionResult List(string sorton, int GamesPage = 1)
        {


            IEnumerable<Games> ItemList;
            switch (sorton)
            {
                case "Genre":
                    ItemList = repository.Games.OrderBy(g => g.Genre).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                    break;
                case "Id":
                    ItemList = repository.Games.OrderBy(g => g.ID).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                    break;
                case "QueryName":
                    ItemList = repository.Games.OrderBy(g => g.QueryName).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                    break;
                case "Price":
                    ItemList = repository.Games.OrderBy(g => g.PriceFinal).Skip((GamesPage - 1) * PageSize).Take(PageSize);
                    break;
                default:
                    ItemList = repository.Games.Skip((GamesPage - 1) * PageSize).Take(PageSize);
                    break;
            }

            return View(new GamesListViewModel
            {
                Games = ItemList,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = GamesPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Games.Count()
                },
                CurrentCategory = sorton

            });
        }
        public IActionResult Edit(string name) =>
                    View(repository.Games
                    .FirstOrDefault(p => p.QueryName == name));

        [HttpPost]
        public IActionResult Edit(Games game)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(game);
                TempData["message"] = $"{game.QueryName} has been saved";
                return RedirectToAction("List");
            }
            else
            {
                // there is something wrong with the data values
                return View(game);
            }

        }

        public IActionResult Create() => View("Edit", new Games());

        [HttpPost]
        public IActionResult Delete(Games game)
        {
            Games deletedGame = repository.DeleteGame(game);
            if (deletedGame != null)
            {
                TempData["message"] = $"{deletedGame.QueryName} was deleted";
            }
            return RedirectToAction("List");
        }

        public ViewResult CreateUser() => View();
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result
                = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Users");
                    return RedirectToAction("Users");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string name)
        {
            AppUser user = await userManager.FindByNameAsync(name);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Users", userManager.Users);
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string name)
        {
            AppUser user = await userManager.FindByNameAsync(name);
            if (user != null)
            {
                return View(new EditAccountForm
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = "",
                });
            }
            else
            {
                return RedirectToAction("Users");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditAccountForm form)
        {
            AppUser user = await userManager.FindByNameAsync(form.UserName);
            if (user != null)
            {
                user.PhoneNumber = form.PhoneNumber;
                user.Email = form.Email;
                IdentityResult validEmail
                = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(form.Password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                    user, form.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                        form.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                || (validEmail.Succeeded
                && form.Password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return View("Roles", roleManager.Roles);
        }


        public async Task<IActionResult> EditRole(string name)
        {
            IdentityRole role = await roleManager.FindByNameAsync(name);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name)
                ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user,
                        model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user,
                        model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Roles));
            }
            else
            {
                return await EditRole(model.RoleId);
            }
        }
        [Authorize]
        public IActionResult UserPage() => View(GetData(nameof(UserPage)));
        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("UserPage",
        GetData(nameof(OtherAction)));
        private Dictionary<string, object> GetData(string actionName) =>
        new Dictionary<string, object>
        {
            ["Action"] = actionName,
            ["User"] = HttpContext.User.Identity.Name,
            ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
            ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
            ["In Users Role"] = HttpContext.User.IsInRole("Users"),
            ["Country"] = CurrentUser.Result.Country,
            ["AccountType"] = CurrentUser.Result.AccountType
        };


        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(
        [Required]Countries country,
        [Required]AccountTypes accountType)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;
                user.Country = country;
                user.AccountType = accountType;
                await userManager.UpdateAsync(user);
                return RedirectToAction("UserPage");
            }
            return View(await CurrentUser);
        }
        public Task<AppUser> CurrentUser =>
        userManager.FindByNameAsync(HttpContext.User.Identity.Name);


        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }




    }

}