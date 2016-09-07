using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.Repository.OrientImplementation;
using Orient.Client;
using TeamNetDB.Model.OrientImplementation;
using System.Threading;
using System.Diagnostics;

namespace TeamNetTest.TestDataBase.QueryMeasurer
{
    /// <summary>
    /// Summary description for QueryMeasurer
    /// </summary>
    [TestClass]
    public class QueryMeasurerTest
    {
        [TestInitialize]
        public void SetUp()
        {
            this.connection = new TeamNetDB.ConnectionDatabase.ConnectionBinaryOrientDB(this.nameDatabase);
            this.transaction = new Transaction(this.connection);
            this.publications = new List<Publication>();
            this.query = "SELECT FROM publication";
            documents = this.transaction.QueryTraverse(query);
            this.stopwatch = new Stopwatch();
            this.cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            this.memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        [TestMethod]
        public void TestTraverseWithSeveralIfSentences()
        {
            this.stopwatch.Start();
            for (int i = 0; i < 100; i++ )
            {
                foreach (ODocument document in documents)
                {
                    List<ODocument> traverseDocuments = this.transaction.QueryTraverse("select name, link from ( traverse * from " + document.ORID.ToString() + " while $depth <= 1 ) where $depth >= 1");
                    int cont = 0;
                    string name = string.Empty;
                    string date = string.Empty;
                    string comment = string.Empty;
                    string link = string.Empty;
                    foreach (ODocument traverseDocument in traverseDocuments)
                    {
                        comment = document.GetField<string>("comment");
                        date = document.GetField<string>("date");
                        if (cont == 0)
                        {
                            link = traverseDocument.GetField<string>("link");
                            cont++;
                        }
                        else if (cont == 1)
                        {
                            name = traverseDocument.GetField<string>("name");
                            cont = 0;
                        }
                    }
                    Publication publication = new Publication() { Comment = comment, Date = date, Publicator = name, Task = link };
                    this.publications.Add(publication);
                }
            }
            this.stopwatch.Stop();
            Debug.WriteLine(this.publications.Count);
            Debug.WriteLine(this.stopwatch.Elapsed.ToString());
            Debug.WriteLine(this.cpuCounter.NextValue());
            Debug.WriteLine(this.memoryCounter.NextValue());
        }

        [TestMethod]
        public void TestTraverseMakingSeveralQueries()
        {
            this.stopwatch.Start();
            for (int i = 0; i < 100; i++ )
            {
                foreach (ODocument document in documents)
                {
                    List<ODocument> users = this.transaction.QueryTraverse("select name from ( traverse * from " + document.ORID.ToString() + " while $depth <= 1 ) where @class = \"User\"");
                    List<ODocument> tasks = this.transaction.QueryTraverse("select link from ( traverse * from " + document.ORID.ToString() + " while $depth <= 1 ) where @class = \"Task\"");
                    if(users.Count != 0 && tasks.Count != 0)
                    {
                        string name = users[0].GetField<string>("name");
                        string date = document.GetField<string>("date");
                        string comment = document.GetField<string>("comment");
                        string link = tasks[0].GetField<string>("link");
                        Publication publication = new Publication() { Comment = comment, Date = date, Publicator = name, Task = link };
                        this.publications.Add(publication);
                    }
                }
            }
            this.stopwatch.Stop();
            Debug.WriteLine(this.publications.Count);
            Debug.WriteLine(this.stopwatch.Elapsed);
            Debug.WriteLine(this.cpuCounter.NextValue());
            Debug.WriteLine(this.memoryCounter.NextValue());
        }

        private TeamNetDB.Repository.Interfaces.ITransaction transaction;
        private string nameDatabase = "TeamNetDataBaseTest";
        private TeamNetDB.ConnectionDatabase.ConnectionBinaryOrientDB connection;
        private string query;
        IList<ODocument> documents;
        private IList<Publication> publications;
        private Stopwatch stopwatch;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter memoryCounter;
    }
}
