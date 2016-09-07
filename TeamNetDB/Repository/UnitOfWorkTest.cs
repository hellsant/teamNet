using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Model;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Model.OrientImplementation;

namespace TeamNetDB.Repository
{
    public class UnitOfWorkTest : IDisposable
    {
        
        /// <summary>
        /// Static method that handles the instance of the class.
        /// </summary>
        /// <returns>instace the object.</returns>
        public static UnitOfWorkTest GetInstance()
        {
            if (unitOfWorkTest == null)
            {
                unitOfWorkTest = new UnitOfWorkTest();
            }
            return unitOfWorkTest;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public UnitOfWorkTest()
        {
            this.connection = new ConnectionDatabase.ConnectionBinaryOrientDB(this.nameDatabase);
            this.transaction = new Transaction(this.connection);
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// It is responsible for returning the repository tasks.
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Task> TaskRepository
        {
            get
            {
                if (this.taskRepository == null)
                {
                    taskRepository = new TaskRepository(transaction);
                }
                return this.taskRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository Result 360.
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Result360> Result360Repository
        {
            get
            {
                if (this.result360Repository == null)
                {
                    result360Repository = new ResultRepository(transaction);
                }
                return this.result360Repository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository users.
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    userRepository = new TeamNetDB.Repository.OrientImplementation.UserRepository(this.transaction);
                }
                return this.userRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository Category
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Area> AreaRepository
        {
            get
            {
                if (this.areaRepository == null)
                {
                    areaRepository = new TeamNetDB.Repository.OrientImplementation.CategoyRepository(this.transaction);
                }
                return this.areaRepository;
            }
        }

        /// <summary>
        /// Manages information database.
        /// </summary>
        /// <returns></returns>
        public ODatabase getDatabase()
        {
            return this.dataBase;
        }

        public TeamNetDB.ConnectionDatabase.ConnectionBinaryOrientDB GetConnection()
        {
            return this.connection;
        }

        /// <summary>
        /// Initializes the repository suggestion.
        /// </summary>
        public IRepository<SuggestionData> SugestionRepository
        {
            get
            {
                if (this.sugestionRepository == null)
                {
                    this.sugestionRepository = new SuggestionRepository(this.transaction);
                }
                return this.sugestionRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository Competency Level
        /// </summary>
        public IRepository<CompetencyLevel> CompetencyLevelRepository
        {
            get
            {
                if (this.competencyLevel == null)
                {
                    this.competencyLevel = new CompetencyLevelRepository(this.transaction);
                }
                return this.competencyLevel;
            }
        }


        /// <summary>
        /// It is responsible for returning the repository time
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Time> TimeRepository
        {
            get
            {
                if (this.timeRepository == null)
                {
                    this.timeRepository = new TeamNetDB.Repository.OrientImplementation.TimeRepository(this.transaction);
                }
                return this.timeRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository Competency
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Competency> CompetencyRepository
        {
            get
            {
                if (this.competencyRepository == null)
                {
                    this.competencyRepository = new TeamNetDB.Repository.OrientImplementation.CompetencyRepository(this.transaction);
                }
                return this.competencyRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository level.
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Level> LevelRepository
        {
            get
            {
                if (this.levelRepository == null)
                {
                    this.levelRepository = new TeamNetDB.Repository.OrientImplementation.LevelRepository(this.transaction);
                }
                return this.levelRepository;
            }
        }

        /// <summary>
        /// It is responsible for returning the repository Expected
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Expected> ExpectedRepository
        {
            get
            {
                if (this.expectedRepository == null)
                {
                    this.expectedRepository = new TeamNetDB.Repository.OrientImplementation.ExpectedRepository(this.transaction);
                }
                return this.expectedRepository;
            }
        }
        
        /// <summary>
        /// It is responsible for returning the repository evaluator
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Evaluator> EvaluatorRepository
        {
            get
            {
                if (this.evaluatorRepository == null)
                {
                    this.evaluatorRepository = new TeamNetDB.Repository.OrientImplementation.EvaluatorRepository(this.transaction);
                }
                return this.evaluatorRepository;
            }
        }
        
        /// <summary>
        /// It is responsible for returning the repository publication
        /// </summary>
        public IRepository<TeamNetDB.Model.OrientImplementation.Publication> PublicationRepository
        {
            get
            {
                if (this.publicationRepository == null)
                {
                    this.publicationRepository = new TeamNetDB.Repository.OrientImplementation.PublicationRepository(this.transaction);
                }
                return this.publicationRepository;
            }
        }
        
        /// <summary>
        /// It is responsible for returning the repository GroupData.
        /// </summary>
        public IRepository<GroupData> GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository = new TeamNetDB.Repository.OrientImplementation.GroupRepository(this.transaction);
                }
                return this.groupRepository;
            }
        }
        

        private static UnitOfWorkTest unitOfWorkTest;
        private TeamNetDB.ConnectionDatabase.ConnectionBinaryOrientDB connection;
        private Orient.Client.ODatabase dataBase;
        private Transaction transaction;

        private IRepository<TeamNetDB.Model.OrientImplementation.User> userRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Area> areaRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Evaluator> evaluatorRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Task> taskRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Result360> result360Repository;
        private IRepository<SuggestionData> sugestionRepository;
        private IRepository<CompetencyLevel> competencyLevel;
        private IRepository<TeamNetDB.Model.OrientImplementation.Time> timeRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Competency> competencyRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Level> levelRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Expected> expectedRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Publication> publicationRepository;
        private IRepository<GroupData> groupRepository;
        /// <summary>
        /// Name database of test.
        /// </summary>
        private string nameDatabase = "TeamNetDataBaseTest";
    }
}
