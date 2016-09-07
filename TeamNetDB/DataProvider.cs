using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using Orient.Client;

namespace TeamNetDB
{
    public class DataProvider
    {
        public DataProvider()
        {
        }

        public ORID GetCompetencyORID(string competencyName)
        {
            this.competencyRepo = UnitOfWork.GetInstance().CompetencyRepository;
            ORID result;
            var competencies = this.competencyRepo.FindBy(competency => competency.Name.Equals(competencyName));
            result = new ORID(competencies.ElementAt(0).Id);
            return result;
        }

        public ORID GetUserORID(string evaluatedName)
        {
            this.userRepo = UnitOfWork.GetInstance().UserRepository;
            char[] delimiterChars = { ' '};
            ORID result;
            var users = this.userRepo.FindBy(user => user.Name.Equals(evaluatedName.Split(delimiterChars)[0]));
            result = new ORID(users.ElementAt(0).Id);
            return result;
        }

        public ORID GetEvaluatorORID(string evaluatedName)
        {
            this.evaluatorRepo = UnitOfWork.GetInstance().EvaluatorRepository;
            ORID result;
            if (evaluatedName.Contains("self"))
            {
                var evaluators = this.evaluatorRepo.FindBy(evaluator => evaluator.Name.Equals("self"));
                result = new ORID(evaluators.ElementAt(0).Id);
            }
            else
            {
                var evaluators = this.evaluatorRepo.FindBy(evaluator => evaluator.Name.Equals("team"));
                result = new ORID(evaluators.ElementAt(0).Id);
            }
            return result;
        }

        private IRepository<Competency> competencyRepo;
        private IRepository<User> userRepo;
        private IRepository<Evaluator> evaluatorRepo;
    }
}
