using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamNetDB.TransactionToDatabase;

namespace TeamNetDB.Model
{
    public class EdgesSuggestion
    {
        public EdgesSuggestion()
        {
            this.edges = new List<EdgesName>();
            this.AddEdgesName();
        }

        private void AddEdgesName()
        {
            edges.Add(EdgesName.UserSuggestion);
            edges.Add(EdgesName.SuggestionCompetency);
            edges.Add(EdgesName.SuggestionLevel);
            edges.Add(EdgesName.SuggestionUser);
            edges.Add(EdgesName.SuggestionCompetencyLevel);
            edges.Add(EdgesName.SuggestionTime);
        }

        public List<EdgesName> GetEdgesName()
        {
            return this.edges;
        }
        
        private List<EdgesName> edges;
    }
}
