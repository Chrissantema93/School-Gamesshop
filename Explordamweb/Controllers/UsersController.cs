using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Explordamweb.Models;
using Explordamweb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Explordamweb.Services;

namespace Explordamweb.Controllers
{

    public class UsersController : Controller
    {

        private UserManager<AppUser> userManager;
        private IOrderRepository repository;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        private IWishListRepository Wishrepository;
        private IGamesRepository Gamerepository;
        private IUserValidator<AppUser> userValidator;


        public UsersController(UserManager<AppUser> usrMgr,
                                IOrderRepository repo,
                                IWishListRepository Wrepo,
                                IGamesRepository Grepo,
                                IUserValidator<AppUser> userValid,
                                IPasswordValidator<AppUser> passValid,
                                IPasswordHasher<AppUser> passwordHash
                                )
        {
            userManager = usrMgr;
            repository = repo;
            Wishrepository = Wrepo;
            Gamerepository = Grepo;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;

        }




        public async Task<IActionResult> Index()
        {

            if (User.Identity.Name == null)
                {
                    return new RedirectResult("/");
                }
            
            else
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);
                return View(new UserInfo
                {
                    Details = user,
                    IsAdmin = await userManager.IsInRoleAsync(user, "Admins")
                });
            }
        }

        public async Task<IActionResult> History()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/");
            }

            else
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);

                return View(
                    repository.Orders.Where(p => p.UserID == user.Id)
                );
            }
        }

        public async Task<IActionResult> WishList()
        {
            if (User.Identity.Name == null)
            {
                return new RedirectResult("/");
            }

            else
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);

                return View(
                    Wishrepository.WishList.Where(w => w.UserId == user.Id)
                );
            }
        }

        public async Task<IActionResult> RemoveFromWishlist(int Gameid, string returnUrl)
        {
            if (User.Identity.Name != null || Gameid == 0)
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);
                Wishrepository.RemoveItem(Wishrepository.WishList.Where(g => g.GameId == Gameid && g.UserId == user.Id).FirstOrDefault());
                return RedirectToAction(nameof(WishList));
            }
            return new RedirectResult("/error");
        }

        public async Task<IActionResult> AddToWishlist(int Gameid, string returnUrl)
        {

            if (User.Identity.Name != null)
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);
                Games targetgame = Gamerepository.Games.Where(g => g.ID == Gameid).FirstOrDefault();

                Wishrepository.AddItem(new Wishitem
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    GameId = Gameid,
                    GameImage = targetgame.HeaderImage,
                    GameGenre = targetgame.Genre,
                    GameName = targetgame.QueryName,
                    GamePrice = targetgame.PriceFinal,
                    DateAdded = DateTime.Now
                });
                return new RedirectResult(returnUrl);
            }
            return new RedirectResult("/Account/Register");


        }



        [HttpGet]
        public async Task<IActionResult> EditDetails()
        {
            if (User.Identity.Name != null)
            {
                var username = userManager.GetUserName(User);
                var user = await userManager.FindByNameAsync(username);
                return View(new EditAccountForm
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Password = "",
                });
            }

            return new RedirectResult("/error");
        }


        [HttpPost]
        public async Task<IActionResult> EditDetails(EditAccountForm form)
        {


            void AddErrorsFromResult(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            AppUser user = await userManager.FindByNameAsync(form.UserName);
            if (user != null)
            {
                user.PhoneNumber = form.PhoneNumber;
                user.Email = form.Email;
                IdentityResult validEmail = await userValidator.ValidateAsync(userManager, user);
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
                        return RedirectToAction("Index");
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
            return View(new EditAccountForm
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = "",
            });
        }


    }
}
