using Orient.Client;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.ConnectionDatabase;

namespace TeamNetDB.Repository
{
    public class UnitOfWork
    {

        #region Constructor
        public UnitOfWork()
        {
            this.connection = new ConnectionBinaryOrientDB(nameDatabase);
            this.transaction = new Transaction(this.connection);
        }
        #endregion

        #region Methods
        public static UnitOfWork GetInstance()
        {
            if (unitOfWork == null)
            {
                unitOfWork = new UnitOfWork();
            }
            return unitOfWork;
        }

        public ConnectionBinaryOrientDB GetConnection()
        {
            return this.connection;
        }

        public ODatabase GetDatabase()
        {
            return this.connection.Connection() as ODatabase;
        }
        #endregion

        #region Properties
        public IRepository<Task> TaskRepository
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

        public IRepository<Result360> Result360Repository
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

        public IRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    userRepository = new UserRepository(this.transaction);
                }
                return this.userRepository;
            }
        }

        public IRepository<Area> AreaRepository
        {
            get
            {
                if (this.areaRepository == null)
                {
                    areaRepository = new CategoyRepository(this.transaction);
                }
                return this.areaRepository;
            }
        }

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

        public IRepository<Time> TimeRepository
        {
            get
            {
                if (this.timeRepository == null)
                {
                    this.timeRepository = new TimeRepository(this.transaction);
                }
                return this.timeRepository;
            }
        }

        public IRepository<Competency> CompetencyRepository
        {
            get
            {
                if (this.competencyRepository == null)
                {
                    this.competencyRepository = new CompetencyRepository(this.transaction);
                }
                return this.competencyRepository;
            }
        }

        public IRepository<Level> LevelRepository
        {
            get
            {
                if (this.levelRepository == null)
                {
                    this.levelRepository = new LevelRepository(this.transaction);
                }
                return this.levelRepository;
            }
        }

        public IRepository<Expected> ExpectedRepository
        {
            get
            {
                if (this.expectedRepository == null)
                {
                    this.expectedRepository = new ExpectedRepository(this.transaction);
                }
                return this.expectedRepository;
            }
        }

        public IRepository<Evaluator> EvaluatorRepository
        {
            get
            {
                if (this.evaluatorRepository == null)
                {
                    this.evaluatorRepository = new EvaluatorRepository(this.transaction);
                }
                return this.evaluatorRepository;
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
                    this.groupRepository = new GroupRepository(this.transaction);
                }
                return this.groupRepository;
            }
        }

        public IRepository<RoleInGroupData> RoleInGroupRepository
        {
            get
            {
                if (this.roleInGroupRepository == null)
                {
                    this.roleInGroupRepository = new RoleInGroupRepository(this.transaction);
                }
                return this.roleInGroupRepository;
            }
        }

        public IRepository<RequestForReview> RequestForReviewRepository
        {
            get
            {
                if (this.requestForReviewRepository == null)
                {
                    this.requestForReviewRepository = new RequestForReviewRepository(this.transaction);
                }
                return this.requestForReviewRepository;
            }
        }

        public IRepository<Reference> ReferenceRepository
        {
            get
            {
                if (this.referenceRepository == null)
                {
                    this.referenceRepository = new ReferenceRepository(this.transaction);
                }
                return this.referenceRepository;
            }
        }

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
        public IRepository<TeamNetDB.Model.OrientImplementation.Role> RoleRepository
        {
            get
            {
                if (this.roleRepository == null)
                {
                    this.roleRepository = new TeamNetDB.Repository.OrientImplementation.RoleRepository(this.transaction);
                }
                return this.roleRepository;
            }
        }

        public IRepository<TeamNetDB.Model.OrientImplementation.GroupManagement> GroupManagementRepository
        {
            get
            {
                if (this.groupManagementRepository == null)
                {
                    this.groupManagementRepository = new GroupManagementRepository(this.transaction);
                }
                return this.groupManagementRepository;
            }
        }
        #endregion

        #region Fields
        private static UnitOfWork unitOfWork;

        private ConnectionBinaryOrientDB connection;
        private Transaction transaction;

        private IRepository<RequestForReview> requestForReviewRepository;
        private IRepository<Reference> referenceRepository;
        private IRepository<Publication> publicationRepository;
        private IRepository<User> userRepository;
        private IRepository<Area> areaRepository;
        private IRepository<Evaluator> evaluatorRepository;
        private IRepository<Task> taskRepository;
        private IRepository<Result360> result360Repository;
        private IRepository<SuggestionData> sugestionRepository;
        private IRepository<CompetencyLevel> competencyLevel;
        private IRepository<TeamNetDB.Model.OrientImplementation.Time> timeRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Competency> competencyRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Level> levelRepository;
        private IRepository<TeamNetDB.Model.OrientImplementation.Expected> expectedRepository;
        private IRepository<GroupData> groupRepository;
        private IRepository<RoleInGroupData> roleInGroupRepository;
        private IRepository<Role> roleRepository;
        private IRepository<GroupManagement> groupManagementRepository;
        /// <summary>
        /// Name of the primary database
        /// </summary>
        private string nameDatabase = "TeamNetDataBase";
        #endregion
    }
}
