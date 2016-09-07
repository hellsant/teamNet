using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;

using TeamNetDB.Model.OrientImplementation;
namespace TeamNetDB.Cleaner
{
    public class OrientDataBaseCleaner : IDataBaseCleaner
    {
        public List<User> DefaultUsers { get; set; }
        public List<TeamNetDB.Model.OrientImplementation.Task> DefaultTasks { get; set; }
        public List<Competency> DefaultCompetencies { get; set; }
        public List<CompetencyLevel> DefaultCompetencyLevel { get; set; }
        public List<SuggestionData> DefaultSuggestion { get; set; }
        public List<Time> DefaultTime { get; set; }

        private ODatabase dataBase;
        private TransactionBinary executor;

        public OrientDataBaseCleaner()
        {
            this.dataBase = UnitOfWorkTest.GetInstance().getDatabase();
            this.executor = new TransactionBinary(this.dataBase);
            this.DefaultUsers = new List<User>()
            {
                new User(){Name = "Farid",Email="farid@jalasoft.com",Password = "F20142011",
                           LastName="Sanches"},
                new User(){Name = "Aida",Email="aida@jalasoft.com",Password = "A20142011",
                           LastName = "Medrano"},
                new User(){Name = "Ivet",Email="ivet@jalasoft.com",Password = "I20142011",
                           LastName = "Rojas"},
            };
            this.DefaultCompetencies = new List<Competency>(){
                new Competency(){Name="Testing"},
                new Competency(){Name="Programming"}
            };
            this.DefaultTasks = new List<TeamNetDB.Model.OrientImplementation.Task>()
            {
                new TeamNetDB.Model.OrientImplementation.Task(){Name = "Improve TDD",
                                                                Description="Improve TDD",Progress=0,
                                                                FinalProduct="document",
                                                                IsSuggestion=true,Score=2,
                                                                InitialDate="2018-06-18",EndDate="2018-07-18"},
                new TeamNetDB.Model.OrientImplementation.Task(){Name = "Research about design patterns",
                                                                Description="Research about design patterns",
                                                                Progress=0,
                                                                FinalProduct="document",IsSuggestion=true,Score=2,
                                                                InitialDate="2017-01-16",EndDate="2017-03-25"},
                new TeamNetDB.Model.OrientImplementation.Task(){Name = "Create UML diagrams",
                                                                Description="Create UML diagrams",Progress=0,
                                                                FinalProduct="document",
                                                                IsSuggestion=true,Score=2,InitialDate="2018-05-14",
                                                                EndDate="2018-06-15"},
            };

            this.DefaultTime = new List<Time>(){
                new Time() { Name = "Hours" },
                new Time() { Name = "Days" },
                new Time() { Name = "Weeks" },
                new Time() { Name = "Months" }
            };

            this.DefaultCompetencyLevel = new List<CompetencyLevel>(){
                new CompetencyLevel() { Description = "first", Value = 0},
                new CompetencyLevel() { Description = "secund", Value = 1},
                new CompetencyLevel() { Description = "third", Value = 2},
            };

            this.DefaultSuggestion = new List<SuggestionData>();
            this.DefaultSuggestion.Add(this.GetSuggestion1());
            this.DefaultSuggestion.Add(this.GetSuggestion2());
            this.DefaultSuggestion.Add(this.GetSuggestion3());
        }


        public void CleanDataBase()
        {
            List<VertexName> vertexToDelete = new List<VertexName>()
            {
                VertexName.Task,VertexName.User,VertexName.Competency,VertexName.Time,VertexName.Suggestion,VertexName.CompetencyLevel
            };
            vertexToDelete.ForEach(vertex => this.executor.Delete(vertex));
        }

        public void SetUpInitialData()
        {
            var userRepository = UnitOfWorkTest.GetInstance().UserRepository;
            this.DefaultUsers.ForEach(user=>userRepository.Insert(user));
            var pointRepository = UnitOfWorkTest.GetInstance().CompetencyRepository;
            this.DefaultCompetencies.ForEach(point => pointRepository.Insert(point));
            var pointid = pointRepository.GetSingle(this.DefaultCompetencies[0].Name);
            var userid = userRepository.GetSingle(this.DefaultUsers[0].Email);
            this.DefaultUsers[0].Id = userid.Id;
            this.DefaultCompetencies[0].Id= pointid.Id;
            this.DefaultTasks.ForEach(task => task.UserId = userid.Id);
            this.DefaultTasks.ForEach(task => task.CompetencyId = pointid.Id);
            var taskRepository = UnitOfWorkTest.GetInstance().TaskRepository;
            this.DefaultTasks.ForEach(task => taskRepository.Insert(task));

            var timeRepository = UnitOfWorkTest.GetInstance().TimeRepository;
            this.DefaultTime.ForEach(time => timeRepository.Insert(time));

            var competencyLevelRepository = UnitOfWorkTest.GetInstance().CompetencyLevelRepository;
            this.DefaultCompetencyLevel.ForEach(competencyLevel => competencyLevelRepository.Insert(competencyLevel));

            var suggestionRepository = UnitOfWorkTest.GetInstance().SugestionRepository;
            this.DefaultSuggestion.ForEach(suggestion => suggestionRepository.Insert(suggestion));
        }

        private SuggestionData GetSuggestion1()
        {
            string name = "research unit test";
            string description = "research for unit test in csharp";
            int estimantedTime = 20;
            int score = 2;
            int valuation = 3;

            string creatorOfSuggestion = "Farid";
            string competency = "Testing";
            string createdTo = "Aida";
            string time = "Months";
            int valueCompetencyLevel = 1;

            SuggestionData sugestion = new SuggestionData()
            {
                Name = name,
                Description = description,
                EstimatedTime = estimantedTime,
                Score = score,
                Valuation = valuation,
                CreatorOfSuggestion = creatorOfSuggestion,
                Competency = competency,
                CreatedTo = createdTo,
                Time = time,
                ValueCompetecyLevel = valueCompetencyLevel,
            };
            return sugestion;
        }
        private SuggestionData GetSuggestion2()
        {
            string name = "aplication calculate";
            string description = "create aplication calculate";
            int estimantedTime = 30;
            int score = 2;
            int valuation = 3;

            string creatorOfSuggestion = "Aida";
            string competency = "Programming";
            string createdTo = "Farid";
            string time = "Days";
            int valueCompetencyLevel = 2;

            SuggestionData sugestion = new SuggestionData()
            {
                Name = name,
                Description = description,
                EstimatedTime = estimantedTime,
                Score = score,
                Valuation = valuation,
                CreatorOfSuggestion = creatorOfSuggestion,
                Competency = competency,
                CreatedTo = createdTo,
                Time = time,
                ValueCompetecyLevel = valueCompetencyLevel,
            };
            return sugestion;
        }
        private SuggestionData GetSuggestion3()
        {
            string name = "test java";
            string description = "test fro java";
            int estimantedTime = 30;
            int score = 2;
            int valuation = 3;

            string creatorOfSuggestion = "Ivet";
            string competency = "Testing";
            string createdTo = "Farid";
            string time = "Hours";
            int valueCompetencyLevel = 0;
            SuggestionData sugestion = new SuggestionData()
            {
                Name = name,
                Description = description,
                EstimatedTime = estimantedTime,
                Score = score,
                Valuation = valuation,
                CreatorOfSuggestion = creatorOfSuggestion,
                Competency = competency,
                CreatedTo = createdTo,
                Time = time,
                ValueCompetecyLevel = valueCompetencyLevel,
            };
            return sugestion;
        }

    }
}
