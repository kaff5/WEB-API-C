using WEBBACK2.Models.Data;
using WEBBACK2.Models;
using WEBBACK2.Models.TaskDir;
using WEBBACK2.Services;
using WEBBACK2.Models.UserDir;
using WEBBACK2.Exceptions;

namespace WEBBACK2.Services
{
    public interface ISolutionService
    {
        public List<SolutionDto> GetSolutions(int? taskId, int? userId);

        public List<TaskDto> PostVerdict(int solutionId, PostVerdictDto model);
        public SolutionDto PostSolution(int id, PostSolutionModel model, string Name);

    }
    public class SolutionService: ISolutionService
    {

        private readonly ApplicationDbContext _context;

        public SolutionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SolutionDto> GetSolutions(int? taskId, int? userId)
        {

            Solution c = new Solution();
            if (taskId is not null && userId is not null)
            {

                List<Solution> solutions = _context.Solutions.Where(x => (x.taskId == taskId) && (x.authorId == userId)).ToList();
                List<SolutionDto> solutionsDto = new List<SolutionDto>();

                foreach (Solution solution in solutions)
                {
                    SolutionDto solutionDto = new SolutionDto()
                    {
                        id = solution.id,
                        sourceCode = solution.sourceCode,
                        programmingLanguage = c.PostLangInString(solution.programmingLanguage),
                        verdict = c.PostVerdictInString(solution.verdict),
                        authorId = solution.authorId,
                        taskId = solution.taskId,
                    };
                    solutionsDto.Add(solutionDto);
                }

                return solutionsDto;
            }
            else if (taskId is not null)
            {

                List<Solution> solutions = _context.Solutions.Where(x => x.taskId == taskId).ToList();
                List<SolutionDto> solutionsDto = new List<SolutionDto>();

                foreach (Solution solution in solutions)
                {
                    SolutionDto solutionDto = new SolutionDto()
                    {
                        id = solution.id,
                        sourceCode = solution.sourceCode,
                        programmingLanguage = c.PostLangInString(solution.programmingLanguage),
                        verdict = c.PostVerdictInString(solution.verdict),
                        authorId = solution.authorId,
                        taskId = solution.taskId,
                    };
                    solutionsDto.Add(solutionDto);
                }

                return solutionsDto;
            }
            else if (userId is not null)
            {

                List<Solution> solutions = _context.Solutions.Where(x => x.authorId == userId).ToList();
                List<SolutionDto> solutionsDto = new List<SolutionDto>();

                foreach (Solution solution in solutions)
                {
                    SolutionDto solutionDto = new SolutionDto()
                    {
                        id = solution.id,
                        sourceCode = solution.sourceCode,
                        programmingLanguage = c.PostLangInString(solution.programmingLanguage),
                        verdict = c.PostVerdictInString(solution.verdict),
                        authorId = solution.authorId,
                        taskId = solution.taskId,
                    };
                    solutionsDto.Add(solutionDto);
                }

                return solutionsDto;
            }








            return _context.Solutions.Select(x => new SolutionDto
            {
                id = x.id,
                sourceCode = x.sourceCode,
                programmingLanguage = c.PostLangInString(x.programmingLanguage),
                verdict = c.PostVerdictInString(x.verdict),
                authorId = x.authorId,
                taskId = x.taskId,
            }).ToList();
        }


        public List<TaskDto> PostVerdict(int solutionId, PostVerdictDto model)
        {
            Solution solution = _context.Solutions.Find(solutionId);

            if (solution is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }


            solution.verdict = solution.GetVerdict(model.verdict);


            _context.SaveChanges();

            TaskService c = new TaskService(_context);
            TaskDto task = c.GetTask(solution.taskId);

            return new List<TaskDto>
            {
                task
            };

        }


        public SolutionDto PostSolution(int id, PostSolutionModel model, string Name)
        {

            User user = _context.Users.Where(x => x.userName == Name).FirstOrDefault();
            if (user is null)
            {
                throw new Exception("Element not found");
            }
            Task1 task = _context.Tasks.Find(id);

            if (task is null)
            {
                throw new ObjectNotFoundException("Element not found");
            }

            Solution s = new Solution();

            Solution solution = new Solution
            {
                id = 0,
                sourceCode = model.sourceCode,
                programmingLanguage = s.GetLang(model.programmingLanguage),
                verdict = s.GetVerdict("Pending"),
                taskId = id,
                authorId = user.userId
            };

            _context.Solutions.Add(solution);
            _context.SaveChanges();

            return new SolutionDto
            {
                id = solution.id,
                sourceCode = solution.sourceCode,
                programmingLanguage = solution.PostLangInString(solution.programmingLanguage),
                verdict = solution.PostVerdictInString(solution.verdict),
                authorId = solution.authorId,
                taskId = solution.taskId
            };

        }

    }
}
