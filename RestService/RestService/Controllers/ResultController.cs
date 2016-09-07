using RestService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.Repository;
using TeamNetDB.Repository.OrientImplementation;

namespace RestService.Controllers
{
    public class ResultController : ApiController
    {
        private const string management = "2014";
        private static UnitOfWork data;
        private UserResults userResults;

        /// <summary>
        /// Constructor of the controller
        /// </summary>
        public ResultController()
        {
            if (data == null)
            {
                data = UnitOfWork.GetInstance();
            }
        }

        /// <summary>
        /// Return the list of results
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [Route("api/result/{userEmail}")]
        [Authorize]
        public HttpResponseMessage GetUserResult(string userEmail)
        {
            string id = "#13:" + userEmail;
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, GetUserResults(id, management));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.NotFound, "{}");
            }
        }

        /// <summary>
        /// Return the user results by user and management.
        /// Obtain the user information.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="management"></param>
        /// <returns></returns>
        public UserResults GetUserResults(string userId, string management)
        {
            userResults = new UserResults();
            userResults.Competences = new List<CompetencyModel>();
            //userResults.Email = userEmail;
            GetUserInfo(userId);
            GetResult360(management);
            return userResults;
        }

        /// <summary>
        /// Get the results of user by management.
        /// Get the name of the competency.
        /// Get the results of the user in expecific competency.
        /// Get the expeteds of the competency.
        /// Get the descriptions of the competency.
        /// </summary>
        /// <param name="management"></param>
        private void GetResult360(string management)
        {
            List<CompetencyModel> competencesList = new List<CompetencyModel>();

            var competences = data.CompetencyRepository.GetAll();
            var evaluators = data.EvaluatorRepository.GetAll();
            var results360 = data.Result360Repository.GetAll();
            var levels = data.LevelRepository.GetAll();
            var expecteds = data.ExpectedRepository.GetAll();
            var levelRepository = data.CompetencyLevelRepository.GetAll();

            foreach (var currentCompetency in competences)
            {
                CompetencyModel newCompetency = new CompetencyModel();
                newCompetency.Name = currentCompetency.Name;
                newCompetency.Results = GetResultList(evaluators, results360, currentCompetency.Id, management);
                newCompetency.Expecteds = GetExpectedList(levels, expecteds, currentCompetency.Id);
                newCompetency.Descriptions = GetDescriptions(levelRepository, currentCompetency.Id);
                newCompetency.Average = GetAVG(newCompetency.Results);

                if (newCompetency.Results.Count > 0)
                {
                    competencesList.Add(newCompetency);
                }
            }
            userResults.Competences = competencesList;
        }

        private float GetAVG(List<ResultModel> list)
        {
            float content = 0;
            int members = 0;
            foreach (var result in list)
            {
                content += (result.Value * result.Members);
                members += result.Members;
            }

            float average = (content / members);

            return average;
        }


        /// <summary>
        /// Get the description and values of the competencies.
        /// </summary>
        /// <param name="competencyId"></param>
        /// <returns></returns>
        private List<ValueDescriptionModel> GetDescriptions(IEnumerable<CompetencyLevel> competencyLevels, string competencyId)
        {
            List<ValueDescriptionModel> descriptionList = new List<ValueDescriptionModel>();

            var competencyRepository = data.CompetencyLevelRepository as CompetencyLevelRepository;
            var list = competencyRepository.FindBy(e => e.CompetencyId.Equals(competencyId), competencyLevels).ToList();

            if (list != null)
            {
                foreach (var currentDescription in list)
                {
                    ValueDescriptionModel newDescription = new ValueDescriptionModel();
                    newDescription.Description = currentDescription.Description;
                    newDescription.Value = currentDescription.Value;

                    descriptionList.Add(newDescription);
                }
            }
            return descriptionList;
        }

        /// <summary>
        /// Get the results of the user of expecific competency.
        /// </summary>
        /// <param name="competencyId"></param>
        /// <param name="management"></param>
        /// <returns></returns>
        private List<ResultModel> GetResultList(IEnumerable<Evaluator> evaluators, IEnumerable<Result360> results360, string competencyId, string management)
        {
            List<ResultModel> resultList = new List<ResultModel>();
            var resultRepository = data.Result360Repository as ResultRepository;

            foreach (var currentEvaluator in evaluators)
            {
                ResultModel newResult = new ResultModel();
                List<Result360> evaluatorResults = resultRepository.FindBy(result =>
                    result.UserId.Equals(userResults.UserId) &&
                    result.CompetencyId.Equals(competencyId) &&
                    result.EvaluatorId.Equals(currentEvaluator.Id),
                    results360
                    ).ToList();

                if (evaluatorResults.Count > 0)
                {
                    newResult.Members = evaluatorResults.Count();
                    newResult.Evaluator = currentEvaluator.Name;
                    newResult.Value = GetAverage(evaluatorResults);
                    newResult.Management = management;

                    resultList.Add(newResult);
                }
            }
            return resultList;
        }

        /// <summary>
        /// Calculate the average of the results.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private float GetAverage(List<Result360> results)
        {
            float average = 0;
            if (results == null || results.Count == 0)
            {
                return average;
            }

            float container = 0;
            foreach (var currentResult in results)
            {
                container += currentResult.Result;
            }
            average = (container / results.Count);
            return average;
        }

        /// <summary>
        /// Get the expecteds results of the competencies.
        /// </summary>
        /// <param name="competencyId"></param>
        /// <returns></returns>
        private List<ExpectedModel> GetExpectedList(IEnumerable<Level> levels, IEnumerable<Expected> expecteds, string competencyId)
        {
            List<ExpectedModel> res = new List<ExpectedModel>();
            var expectedRepository = data.ExpectedRepository as ExpectedRepository;

            foreach (var currentLevel in levels)
            {
                ExpectedModel newExpected = new ExpectedModel();
                newExpected.Level = currentLevel.Name;
                Expected ex = expectedRepository.FindBy(e => e.CompetencyId.Equals(competencyId) && e.LevelId.Equals(currentLevel.Id)).First();
                newExpected.Priority = ex.Priority;
                newExpected.ValueExpected = ex.ValueExpected;

                res.Add(newExpected);
            }
            return res;
        }

        /// <summary>
        /// Get the user information.
        /// </summary>
        private void GetUserInfo(string userId)
        {
            User currentUser = data.UserRepository.GetSingle(userId);

            userResults.Name = currentUser.Name;
            userResults.UserId = currentUser.Id;
            userResults.Email = currentUser.Email;
        }
    }
}