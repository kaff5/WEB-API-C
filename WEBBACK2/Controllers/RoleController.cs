using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBBACK2.Services;
using WEBBACK2.Models.RoleDir;

namespace WEBBACK2.Controllers
{
    [Route("roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {


        public IRoleService service;

        public RoleController(IRoleService service)
        {
            this.service = service;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<List<RoleDto>> Get()
        {
            return Ok(service.GetRoles());
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<RoleDto> Get(int id)
        {
            return Ok(service.GetRole(id));
        }

    }
}
