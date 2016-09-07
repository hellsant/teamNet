using Orient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.Model;
using TeamNetDB.Repository;
using TeamNetDB.Repository.Interfaces;
using TeamNetDB.Repository.OrientImplementation;
using TeamNetDB.Model.OrientImplementation;
using TeamNetDB.TransactionToDatabase;
using TeamNetDB.TransactionToDatabase.TransactionOrientDBBinary;
using DataLoader;
using System.Collections;

namespace TeamNetDB
{
    public class Data
    {

        private ODatabase dataBase;
        private TransactionBinary transaction;
        private List<string> list;

        public Data()
        {
            this.dataBase = UnitOfWork.GetInstance().GetDatabase();
            this.transaction = new TransactionBinary(this.dataBase);
            CleanDB();
            GeneralData();
            PersonalData();
            this.dataBase.Dispose();
        }

        private void GeneralData()
        {
            CreateEvaluatorsVertex();
            CreateCategoriesAndCompetenciesVertexes();
            CreateLevelsVertex();
            CreateTime();
        }

        private void PersonalData()
        {
            CreateUsers();
            CreateExpecteds();
            CreateResults();
        }

        private void CreateTime()
        {
            this.list = new List<string>();
            list.Add("Hours");
            list.Add("Days");
            list.Add("Weeks");
            list.Add("Months");

            CreateVertex(VertexName.Time.ToString(), list);
        }
        private void CreateResults()
        {
            DirectoryInspector inspector = new DirectoryInspector();
            ExcelFileReader reader = new ExcelFileReader();
            IList<string> files = inspector.GetNameFiles();
            IList<IList<string>> categories = reader.ReadExcelFileFormat();
            IList<IDictionary<string, IList<float>>> results = new List<IDictionary<string, IList<float>>>();
            foreach (string file in files)
            {
                string path = "Results/" + file;
                results.Add(reader.ReadExcelFileResults(path));
            }
            saveResults(categories, results);
        }

        private void saveResults(IList<IList<string>> categories, IList<IDictionary<string, IList<float>>> results)
        {
            DataProvider provider = new DataProvider();
            string management = "2014";
            foreach (IDictionary<string, IList<float>> result in results)
            {
                ORID userORID;
                ORID competencyORID;
                ORID evaluatorORID;
                float userScore = 0;
                int count = 0;
                foreach (string user in result.Keys)
                {
                    userORID = provider.GetUserORID(user);
                    foreach (float score in result[user])
                    {
                        userScore = score;
                        competencyORID = provider.GetCompetencyORID(categories.ElementAt(count).ElementAt(1));
                        evaluatorORID = provider.GetEvaluatorORID(user);
                        CreateResult360(userScore, userORID, competencyORID, evaluatorORID, management);
                        count++;
                    }
                    count = 0;
                }
            }
        }

        private void CreateUsers()
        {
            CreateVertex(VertexName.User.ToString(), "andrea", "soria", "andrea@hotmail.com", new Rinjdael().EncryptData("Andrea!"));
            CreateVertex(VertexName.User.ToString(), "vania", "catorceno", "vania@hotmail.com", new Rinjdael().EncryptData("Vania!"));
            CreateVertex(VertexName.User.ToString(), "erika", "vargas", "erika@hotmail.com", new Rinjdael().EncryptData("Erika!"));
            CreateVertex(VertexName.User.ToString(), "pedro", "barron", "pedro@hotmail.com", new Rinjdael().EncryptData("Pedro!"));
            CreateVertex(VertexName.User.ToString(), "brandon", "santander", "brandon@hotmail.com", new Rinjdael().EncryptData("Brandon!"));
            CreateVertex(VertexName.User.ToString(), "denis", "vasquez", "denis@hotmail.com", new Rinjdael().EncryptData("Denis!"));
            CreateVertex(VertexName.User.ToString(), "kevin", "titichoca", "kevin@hotmail.com", new Rinjdael().EncryptData("Kevin!"));
            CreateVertex(VertexName.User.ToString(), "juan", "chavez", "juan@hotmail.com", new Rinjdael().EncryptData("Juan!"));
            CreateVertex(VertexName.User.ToString(), "douglas", "suarez", "douglas@hotmail.com", new Rinjdael().EncryptData("Douglas!"));
            CreateVertex(VertexName.User.ToString(), "fernando", "hurtado", "fernando@hotmail.com", new Rinjdael().EncryptData("Fernando!"));
            CreateVertex(VertexName.User.ToString(), "sergio", "uriona", "sergio@hotmail.com", new Rinjdael().EncryptData("Sergio!"));
            CreateVertex(VertexName.User.ToString(), "vicente", "rodriguez", "vicente@hotmail.com", new Rinjdael().EncryptData("Vicente!"));
            CreateVertex(VertexName.User.ToString(), "dan", "canqui", "dan@hotmail.com", new Rinjdael().EncryptData("Dan!"));
        }

        private void CreateExpecteds()
        {
            //Expected Testing
            CreateExpectedVertex(1, 3, "Coding", "junior");
            CreateExpectedVertex(2, 4, "Coding", "staff");
            CreateExpectedVertex(3, 5, "Coding", "senior");

            //Expected Programing
            CreateExpectedVertex(3, 2, "Revised Code", "junior");
            CreateExpectedVertex(2, 3, "Revised Code", "staff");
            CreateExpectedVertex(1, 5, "Revised Code", "senior");

            //Expected Researching
            CreateExpectedVertex(1, 3, "Code Standards", "junior");
            CreateExpectedVertex(3, 4, "Code Standards", "staff");
            CreateExpectedVertex(2, 5, "Code Standards", "senior");

            //Expected Analytical thinking
            CreateExpectedVertex(2, 2, "Unit Tests", "junior");
            CreateExpectedVertex(1, 4, "Unit Tests", "staff");
            CreateExpectedVertex(3, 5, "Unit Tests", "senior");

            //Expected Motivation
            CreateExpectedVertex(1, 2, "OO Design", "junior");
            CreateExpectedVertex(2, 3, "OO Design", "staff");
            CreateExpectedVertex(3, 4, "OO Design", "senior");

            //Expected Compromise
            CreateExpectedVertex(2, 1, "Classes and Objects", "junior");
            CreateExpectedVertex(1, 2, "Classes and Objects", "staff");
            CreateExpectedVertex(3, 4, "Classes and Objects", "senior");

            //Expected Self Confidence
            CreateExpectedVertex(2, 2, "Collections", "junior");
            CreateExpectedVertex(1, 3, "Collections", "staff");
            CreateExpectedVertex(3, 5, "Collections", "senior");

            //Expected Planning and organization
            CreateExpectedVertex(1, 3, "Teamwork", "junior");
            CreateExpectedVertex(3, 4, "Teamwork", "staff");
            CreateExpectedVertex(2, 5, "Teamwork", "senior");

            //Expected Learning approach
            CreateExpectedVertex(3, 2, "Autonomy", "junior");
            CreateExpectedVertex(1, 3, "Autonomy", "staff");
            CreateExpectedVertex(2, 5, "Autonomy", "senior");
        }

        private void CreateLevelsVertex()
        {
            this.list = new List<string>();
            list.Add("junior");
            list.Add("staff");
            list.Add("senior");

            CreateVertex(VertexName.Level.ToString(), list);
        }

        private void CreateCategoriesAndCompetenciesVertexes()
        {
            ExcelFileReader reader = new ExcelFileReader();
            IList<IList<string>> categoriesAndCompetences = reader.ReadExcelFileFormat();
            IList<string> categories = new List<string>();
            IList<string> competencies = new List<string>();
            string category = string.Empty;
            foreach (var categ in categoriesAndCompetences)
            {
                if (!category.Equals(categ.ElementAt(0)))
                {
                    category = categ.ElementAt(0);
                    categories.Add(category);
                }
                competencies.Add(categ.ElementAt(1));
            }
            CreateVertex(VertexName.Category.ToString(), categories);
            CreateVertex(VertexName.Competency.ToString(), competencies);
        }

        private void CreateEvaluatorsVertex()
        {
            this.list = new List<string>();
            list.Add("manager");
            list.Add("team");
            list.Add("self");

            CreateVertex(VertexName.Evaluator.ToString(), list);
        }

        private void CreateExpectedVertex(int priority, int valueExpected, string point, string level)
        {
            OVertex expectedVertex = this.dataBase
                           .Create.Vertex(VertexName.Expected.ToString())
                           .Set("priority", priority)
                           .Set("valueExpected", valueExpected)
                           .Run();

            ORID expectedORID = expectedVertex.ORID;


            List<ConditionValue> condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = point });
            ORID pointORID = new ORID(transaction.GetRid(VertexName.Competency, condition));

            condition = new List<ConditionValue>();
            condition.Add(new ConditionValue() { ConditionColumn = "name", ValueCondition = level });
            ORID levelORID = new ORID(transaction.GetRid(VertexName.Level, condition));

            CreateEdge(EdgesName.CompetencyExpected.ToString(), EdgesName.CompetencyExpected.ToString(), pointORID, expectedORID);
            CreateEdge(EdgesName.LevelExpected.ToString(), EdgesName.LevelExpected.ToString(), levelORID, expectedORID);
        }

        private void CreateResult360(float result, ORID userORID, ORID pointORID, ORID evaluatorORID, string management)
        {
            OVertex result360Vertex = this.dataBase
                           .Create.Vertex(VertexName.Result360.ToString())
                           .Set("result", result)
                           .Set("management", management)
                           .Set("pointID", " ")
                           .Set("evaluatorID", " ")
                           .Set("userID", " ")
                           .Run();

            ORID result360ORID = result360Vertex.ORID;

            CreateEdge(EdgesName.CompetencyResult360.ToString(), EdgesName.CompetencyResult360.ToString(), pointORID, result360ORID);
            CreateEdge(EdgesName.UserResult360.ToString(), EdgesName.UserResult360.ToString(), userORID, result360ORID);
            CreateEdge(EdgesName.EvaluatorResult360.ToString(), EdgesName.EvaluatorResult360.ToString(), evaluatorORID, result360ORID);
        }

        private void CreateVertex(string vertexName, IList<string> list)
        {
            foreach (string item in list)
            {
                OVertex newVertex = this.dataBase
                           .Create.Vertex(vertexName)
                           .Set("name", item)
                           .Run();
                ORID newVertexORID = newVertex.ORID;
                if (vertexName == "Competency")
                {
                    CreateCompetencyLevel(item, newVertexORID);
                }
            }
        }

        private void CreateCompetencyLevel(string item, ORID newVertexORID)
        {
            switch (item)
            {
                case "Coding":
                    CreateDescription(newVertexORID, CODINGLEVEL0, 0);
                    CreateDescription(newVertexORID, CODINGLEVEL1, 1);
                    CreateDescription(newVertexORID, CODINGLEVEL2, 2);
                    break;
                case "Revised Code":
                    CreateDescription(newVertexORID, REVISEDLEVEL0, 0);
                    CreateDescription(newVertexORID, REVISEDLEVEL1, 1);
                    CreateDescription(newVertexORID, REVISEDLEVEL2, 2);
                    break;
                case "Code Standards":
                    CreateDescription(newVertexORID, CODELEVEL0, 0);
                    CreateDescription(newVertexORID, CODELEVEL1, 1);
                    CreateDescription(newVertexORID, CODELEVEL2, 2);
                    break;
                case "Unit Tests":
                    CreateDescription(newVertexORID, UNITLEVEL0, 0);
                    CreateDescription(newVertexORID, UNITLEVEL1, 1);
                    CreateDescription(newVertexORID, UNITLEVEL2, 2);
                    break;
                case "OO Design":
                    CreateDescription(newVertexORID, OOLEVEL0, 0);
                    CreateDescription(newVertexORID, OOLEVEL1, 1);
                    CreateDescription(newVertexORID, OOLEVEL2, 2);
                    break;
                case "Classes and Objects":
                    CreateDescription(newVertexORID, CLASSESLEVEL0, 0);
                    CreateDescription(newVertexORID, CLASSESLEVEL1, 1);
                    CreateDescription(newVertexORID, CLASSESLEVEL2, 2);
                    break;
                case "Collections":
                    CreateDescription(newVertexORID, COLLECTIONLEVEL0, 0);
                    CreateDescription(newVertexORID, COLLECTIONLEVEL1, 1);
                    CreateDescription(newVertexORID, COLLECTIONLEVEL2, 2);
                    break;
                case "Teamwork":
                    CreateDescription(newVertexORID, TEAMLEVEL0, 0);
                    CreateDescription(newVertexORID, TEAMLEVEL1, 1);
                    CreateDescription(newVertexORID, TEAMLEVEL2, 2);
                    break;
                case "Autonomy":
                    CreateDescription(newVertexORID, AUTONOMYLEVEL0, 0);
                    CreateDescription(newVertexORID, AUTONOMYLEVEL1, 1);
                    CreateDescription(newVertexORID, AUTONOMYLEVEL2, 2);
                    break;
            }
        }

        private void CreateDescription(ORID vertexORID, string description, int level)
        {
            OVertex competencyLevelVertex = this.dataBase
                           .Create.Vertex(VertexName.CompetencyLevel.ToString())
                           .Set("value", level)
                           .Set("description", description)
                           .Run();

            ORID competencyLevelORID = competencyLevelVertex.ORID;

            CreateEdge(EdgesName.CompetencyCompetencyLevel.ToString(), EdgesName.CompetencyCompetencyLevel.ToString(), vertexORID, competencyLevelORID);
        }

        private void CreateVertex(string vertexName, string userName, string lastName, string email, string pass)
        {
            OVertex taskVertex = this.dataBase
                           .Create.Vertex(vertexName)
                           .Set("name", userName)
                           .Set("lastName", lastName)
                           .Set("email", email)
                           .Set("password", pass)
                           .Run();
        }

        private void CreateEdge(string edgeName, string clusterName, ORID from, ORID to)
        {
            OEdge edge = this.dataBase
                       .Create.Edge(edgeName)
                       .Cluster(clusterName)
                       .From(from)
                       .To(to)
                       .Run();
        }



        private void CleanDB()
        {
            transaction.Delete(VertexName.Evaluator);
            transaction.Delete(VertexName.Competency);
            transaction.Delete(VertexName.Category);
            transaction.Delete(VertexName.Level);

            transaction.Delete(VertexName.User);
            transaction.Delete(VertexName.Result360);
            transaction.Delete(VertexName.Expected);
            transaction.Delete(VertexName.Time);
            transaction.Delete(VertexName.CompetencyLevel);
            transaction.Delete(VertexName.Suggestion);
        }


        static string CODINGLEVEL0 = "Is not capable of writing code in the defined programming language";
        static string CODINGLEVEL1 = "Is able to make asignments, conditionals, cicles, and recognizes the scope of the variables in a block";
        static string CODINGLEVEL2 = "Can write methods, understands and recognices the change of variables and knows the scope of class variables";
        
        static string REVISEDLEVEL0 = "Is not capable of understanding code written in the defined programming language";
        static string REVISEDLEVEL1 = "Identifies the instruccions of a program";
        static string REVISEDLEVEL2 = "Understands how the code he is workikg with is executed and is capable to identify anomalies";
        
        static string CODELEVEL0 = "Written code follows the rules";
        static string CODELEVEL1 = "Consistently gives meaningful names to variables, methods and/or classes; follows the identation rules to make the program easier to read";
        static string CODELEVEL2 = "Consistently orders his/her code taking in count sections for constants, attributes, constructors and methods";

        static string UNITLEVEL0 = "Does not test his/her code";
        static string UNITLEVEL1 = "Performs manual tests";
        static string UNITLEVEL2 = "Codifies his/her tests using libraries that can be executed whenever its needed";

        static string OOLEVEL0 = "Does not have an abstract vision of the code you write";
        static string OOLEVEL1 = "Identifies the classes and responsabilities in the program is scope";
        static string OOLEVEL2 = "Identifies the relationships between classes(aggregation, inheritance, implementation, association";

        static string CLASSESLEVEL0 = "Is not acquainted with the paradigm";
        static string CLASSESLEVEL1 = "Creates instances. Makes classes identifying properties and methods. Understands which methods to implement";
        static string CLASSESLEVEL2 = "Applies encapsulation using properly the structures(classes, abstract classes, methods, etc) allowed by the language/infrastructure and access modifiers";
        
        static string COLLECTIONLEVEL0 = "Is not acquainted with collections";
        static string COLLECTIONLEVEL1 = "Knows arrays and collections";
        static string COLLECTIONLEVEL2 = "Uses properly (and consistently) the addecuate type of collections";

        static string TEAMLEVEL0 = "Does not work well in a team";
        static string TEAMLEVEL1 = "Communicates, collaborates and provides ideas";
        static string TEAMLEVEL2 = "Assumes responsabilities in the team; helps organizing the team";
        
        static string AUTONOMYLEVEL0 = "Is dependant";
        static string AUTONOMYLEVEL1 = "Works under pressure and/or supervision";
        static string AUTONOMYLEVEL2 = "Compromises and assumes responsabilities";
    }
}
