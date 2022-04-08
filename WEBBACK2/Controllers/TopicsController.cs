using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEBBACK2.Models;
using WEBBACK2.Models.TopicDir;
using WEBBACK2.Services;


namespace WEBBACK2.Controllers
{
    [Route("topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        public ITopicService service;

        public TopicsController(ITopicService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult<List<TopicsDto>> Get([FromQuery] string? name, int? parentId)
        {
            return Ok(service.GetTopics(name, parentId));
        }
        [HttpPost]
        /*[Authorize(Roles = "Admin")]*/
        public ActionResult<List<TopicsDtoWithChild>> Post([FromBody] TopicPostModelDto model)
        {
            return Ok(service.CreateTopic(model));

        }

        [HttpGet("{id}")]
        public ActionResult<List<TopicsDtoWithChild>> Get(int id)
        {
            return Ok(service.GetTopic(id));
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<TopicsDtoWithChild>> Patch(int id, TopicPatchModel model)
        {
            return Ok(service.PatchTopic(id,model));
        }

        [HttpDelete("{id}")]
       /* [Authorize(Roles = "Admin")]*/
        public async Task<ActionResult<MessageOk>> Delete(int id)
        {
            await service.Delete(id);
            return Ok(new MessageOk("OK"));
        }
        [HttpGet("{id}/childs")]
        public ActionResult<List<ChildsTopicDto>> GetChild(int id)
        {
             return Ok(service.GetChilds(id));
        }


        [HttpPost("{id}/childs")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<TopicsDtoWithChild>> PostChilds(int id,[FromBody] List<int> model)
        {
             return Ok(service.PostChilds(id, model));
        }

        [HttpDelete("{id}/childs")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<TopicsDtoWithChild>> DeleteChilds(int id, [FromBody] List<int> model)
        {
            return Ok(service.DeleteChilds(id, model));
        }

    }
}
