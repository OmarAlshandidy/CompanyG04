using Company.G04.DAL.Moudel;
using Company.G04.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Company.G04.PL.Dtos.Auth;


namespace Company.G04.PL.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            var roles = Enumerable.Empty<RoleViewDto>();

            if (string.IsNullOrEmpty(searchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewDto()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(U => U.Name
                    .ToLower()
                    .Contains(searchInput.ToLower()))
                    .Select(R => new RoleViewDto()
                    {
                        Id = R.Id,
                        RoleName = R.Name
                    }).ToListAsync();
            }

            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewDto model)
        {
            var role = new IdentityRole()
            {
                Name = model.RoleName
            };
            if(ModelState.IsValid)
            {
                await _roleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//error 400

            var roleFromDb = await _roleManager.FindByIdAsync(id);

            if (roleFromDb == null)
                return NotFound(); // 404

            var role = new RoleViewDto()
            {
                Id = roleFromDb.Id,
                RoleName = roleFromDb.Name
            };

            return View(ViewName, role);
        }
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]//btmn3 ay request mn app khargy zy el postman msln
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewDto model)
        {

            if (id != model.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                var roleFromDb = await _roleManager.FindByIdAsync(id);

                if (roleFromDb == null)
                    return NotFound(); // 404

                roleFromDb.Name = model.RoleName;

                await _roleManager.UpdateAsync(roleFromDb);

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
        public async Task<IActionResult> Delete([FromRoute] string id, RoleViewDto model)
        {
            if (id != model.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                var roleFromDb = await _roleManager.FindByIdAsync(id);

                if (roleFromDb == null)
                    return NotFound(); // 404

                await _roleManager.DeleteAsync(roleFromDb);

                return RedirectToAction("Index");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null)
                return NotFound();

            ViewData["RoleId"] = roleId;

            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,

                };

                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId,List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser,role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser,role.Name);
                        }
                    }                   
                }
                return RedirectToAction(nameof(Edit), new {id = roleId});
            }

            return View(users);
        }

    }
}//
