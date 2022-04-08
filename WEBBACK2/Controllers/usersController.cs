using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBBACK2.Models;
using WEBBACK2.Models.UserDir;
using WEBBACK2.Services;

namespace WEBBACK2.Controllers
{
    [Route("users")]
    [ApiController]
    public class usersController : ControllerBase
    {
        public IUserService service;

        public usersController(IUserService service)
        {
            this.service = service;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<UserDto>> Get()
        {
            return Ok(service.GetUsers());
            
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserMyselfDto> Get(int id)
        {
            return Ok(service.GetUser(id, User));
        }

        [HttpPatch("{id}")]
        [Authorize]
        public ActionResult<UserMyselfDto> Patch(int id , PatchUser model)
        {
            return service.Patch(id,model,User);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MessageOk>> Delete(int id)
        {
            await service.Delete(id);
            return Ok(new MessageOk("OK"));
        }

        [HttpPost("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post(int id, RoleBody model)
        {
            await service.Role(id,model);
            return Ok(new MessageOk("OK"));
        }

    }
}
