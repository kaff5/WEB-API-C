using System.Linq;
using WEBBACK2.Exceptions;
using WEBBACK2.Models.Data;
using WEBBACK2.Models.TaskDir;
using WEBBACK2.Models.TopicDir;

namespace WEBBACK2.Services
{
    public interface ITopicService
    {
        public List<TopicsDto> GetTopics(string? name, int? parentId);
        public List<TopicsDtoWithChild> CreateTopic(TopicPostModelDto model);

        public List<TopicsDtoWithChild> GetTopic(int id);

        public Task Delete(int id);

        public List<TopicsDtoWithChild> PatchTopic(int id, TopicPatchModel model);

        public List<ChildsTopicDto> GetChilds(int id);

        public List<TopicsDtoWithChild> PostChilds(int id, List<int> model);

        public List<TopicsDtoWithChild> DeleteChilds(int id, List<int> model);
    }


    public class TopicService : ITopicService
    {

        private readonly ApplicationDbContext _context;

        public TopicService(ApplicationDbContext context)
        {
            _context = context;
        }




        public List<TopicsDto> GetTopics(string? name, int? parentId)
        {
            if (name is not null && parentId is not null)
            {

                List<Topic> topics = _context.Topics.Where(x => (x.name == name) && (x.parentId == parentId)).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicdto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicdto);
                }

                return topicsDto;
            }
            else if (name is not null)
            {
                List<Topic> topics = _context.Topics.Where(x => x.name == name).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicdto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicdto);
                }

                return topicsDto;
            }
            else if (parentId is not null)
            {
                List<Topic> topics = _context.Topics.Where(x => x.parentId == parentId).ToList();
                List<TopicsDto> topicsDto = new List<TopicsDto>();

                foreach (Topic topic in topics)
                {
                    TopicsDto topicdto = new TopicsDto()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId
                    };
                    topicsDto.Add(topicdto);
                }

                return topicsDto;
            }





            return _context.Topics.Select(x => new TopicsDto
            {
                id = x.id,
                name = x.name,
                parentId = x.parentId,
            }).ToList();
        }


        public List<TopicsDtoWithChild> CreateTopic(TopicPostModelDto model)
        {
            Topic topic = new Topic
            {
                id = 0,
                name = model.name,
                parentId = model.parentId
            };

            _context.Topics.Add(topic);
            _context.SaveChanges();

            if (topic.parentId is not null)
            {

                return GetTopic((int)topic.parentId);
            }
            else
            {
                List<ChildsTopicDto> child = new List<ChildsTopicDto>();

                return new List<TopicsDtoWithChild>()
                {
                    new TopicsDtoWithChild()
                    {
                        id = topic.id,
                        name = topic.name,
                        parentId = topic.parentId,
                        childs = child
                    }
                };
            }
        }
        public List<TopicsDtoWithChild> GetTopic(int id)
        {
            Topic topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }
            else
            {
                List<Topic> childs = new List<Topic>(_context.Topics.Where(x => x.parentId == topic.id));
                List<ChildsTopicDto> topicsDto = new List<ChildsTopicDto>();

                foreach (Topic child in childs)
                {
                    topicsDto.Add(new ChildsTopicDto()
                    {
                        id = child.id,
                        name = child.name,
                        parentId = child.parentId
                    });
                }

                TopicsDtoWithChild t = new TopicsDtoWithChild()
                {
                    id = topic.id,
                    name = topic.name,
                    parentId = topic.parentId,
                    childs = topicsDto
                };
                List<TopicsDtoWithChild> sns = new List<TopicsDtoWithChild>()
                {
                    t
                };
                return sns;
            }
        }


        public List<TopicsDtoWithChild> PatchTopic(int id, TopicPatchModel model)
        {
            var topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }
            topic.name = model.name;
            topic.parentId = model.parentId;

            _context.SaveChanges();

            return GetTopic(id);
        }


        public async Task Delete(int id)
        {
            Topic topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            List<Topic> listChilds = _context.Topics.Where(x => x.parentId == id).ToList();

            foreach (Topic child in listChilds)
            {
                FindChilds(child);
            }

            List<Task1> listTasks = _context.Tasks.Where(x => x.topicId == topic.id).ToList();
            foreach (Task1 task in listTasks)
            {
                if (task.input != null)
                {
                    if (System.IO.File.Exists($"wwwroot{task.input}"))
                    {
                        System.IO.File.Delete($"wwwroot{task.input}");
                    }
                }
                if (task.output != null)
                {
                    if (System.IO.File.Exists($"wwwroot{task.output}"))
                    {
                        System.IO.File.Delete($"wwwroot{task.output}");
                    }
                }
            }


            DeleteTopic(topic);
            _context.SaveChanges();
        }

        void FindChilds(Topic topic)
        {
            List<Topic> listChilds = _context.Topics.Where(x => x.parentId == topic.id).ToList();

            foreach (Topic child in listChilds)
            {
                FindChilds(child);
            }
            //TODO

            List<Task1> listTasks = _context.Tasks.Where(x => x.topicId == topic.id).ToList();
            foreach (Task1 task in listTasks)
            {
                if (task.input != null)
                {
                    if (System.IO.File.Exists($"wwwroot{task.input}"))
                    {
                        System.IO.File.Delete($"wwwroot{task.input}");
                    }
                }
                if (task.output != null)
                {
                    if (System.IO.File.Exists($"wwwroot{task.output}"))
                    {
                        System.IO.File.Delete($"wwwroot{task.output}");
                    }
                }
            }

            DeleteTopic(topic);
        }


        void DeleteTopic(Topic topic)
        {
            _context.Topics.Remove(topic);
        }

        public List<ChildsTopicDto> GetChilds(int id)
        {
            Topic topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }
            else
            {
                List<ChildsTopicDto> childs = new List<ChildsTopicDto>();

                var mas = _context.Topics.Where(x => x.parentId == topic.id);
                foreach (var child in mas)
                {
                    childs.Add(new ChildsTopicDto()
                    {
                        id = child.id,
                        name = child.name,
                        parentId = child.parentId,
                    });
                }
                return childs;
            }

        }

        public List<TopicsDtoWithChild> PostChilds(int id, List<int> model)
        {
            var topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }
            foreach (int child in model)
            {
                Topic childF = _context.Topics.Find(child);
                childF.parentId = topic.id;
                _context.SaveChanges();
            }

            return GetTopic(id);
        }

        public List<TopicsDtoWithChild> DeleteChilds(int id, List<int> model)
        {
            var topic = _context.Topics.Find(id);
            if (topic is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }


            foreach (int n in model)
            {
                Topic childTopic = _context.Topics.Find(n);
                if (childTopic is null)
                {
                    throw new ObjectNotFoundException("Check if the topic has child");
                }
                childTopic.parentId = null;
                _context.SaveChanges();
            }

            return GetTopic(id);
        }

    }
}
