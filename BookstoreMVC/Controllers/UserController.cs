using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }

        [HttpGet]
        public IActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserRegistration([Bind] UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = this.userBL.RegisterUser(userModel);
                    return View(result);
                }
                return View(userModel);
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin([Bind] UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = this.userBL.Login(userModel);
                    return View(result);
                }
                return View(userModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult DashBoard()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
