using Company.G04.DAL.Moudel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Company.G04.PL.Dtos.Auth;


namespace Company.G04.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public UserController(UserManager<AppUser> userManager) 
		{
			_userManager = userManager;
		}
		public async Task<IActionResult> Index(string searchInput)
		{
			var users = Enumerable.Empty<UserViewDto>();

			if (string.IsNullOrEmpty(searchInput))
			{
				users = await _userManager.Users.Select(U => new UserViewDto()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
				}).ToListAsync();

                foreach (var user in users)
                {
                    user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
                }
            }
			else
			{
				users = await _userManager.Users.Where(U => U.Email
                    .ToLower()
                    .Contains(searchInput.ToLower()))
					.Select(U => new UserViewDto()
					{
						Id = U.Id,
						FirstName = U.FirstName,
						LastName = U.LastName,
						Email = U.Email,
					}).ToListAsync();

                foreach (var user in users)
                {
                    user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
                }
            }

			return View(users);
		}
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//error 400

			var UserFromDb = await _userManager.FindByIdAsync(id);

			if (UserFromDb == null)
                return NotFound(); // 404

			var user = new UserViewDto()
			{
				Id = UserFromDb.Id,
				FirstName = UserFromDb.FirstName,
				LastName = UserFromDb.LastName,
				Email = UserFromDb.Email,
			};

            user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));

            return View(ViewName, user);
        }
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//btmn3 ay request mn app khargy zy el postman msln
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewDto model)
        {
           
            if (id != model.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb == null)
                    return NotFound(); // 404

                userFromDb.FirstName = model.FirstName;
                userFromDb.LastName = model.LastName;
                userFromDb.Email = model.Email;

                await _userManager.UpdateAsync(userFromDb);
                
                return RedirectToAction("Index");
                
            }

            return View(model);
        }

        public Task<IActionResult> Delete(string? id)
        {
            return Details(id, "Delete");
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]//btmn3 ay request mn app khargy zy el postman msln
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewDto model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                if (userFromDb == null)
                    return NotFound(); // 404

                await _userManager.DeleteAsync(userFromDb);

                return RedirectToAction("Index");

            }

            return View(model);
        }
    }
}
