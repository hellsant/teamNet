using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.TransactionToDatabase
{
    public enum EdgesName
    {
        UserSuggestion,
        SuggestionCompetency,
        SuggestionLevel,
        SuggestionUser,
        SuggestionTime,
        CategoryCompetency,
        CompetencyExpected,
        SuggestionCompetencyLevel,
        CompetencyTask,
        CompetencyCompetencyLevel,

        PointExpected,
        PointResult360,
        UserResult360,
        EvaluatorResult360,
        LevelExpected,
        CompetencyResult360,
        UserLevel,
        PointTask,
        UserTask,
        ReviewerTask,
        Reviewer,
        TaskForReview,
        TaskReference,
        PublicationUser,
        ContentPublication,
        PublicationGroup,
        
        HasRoleInGroup,
        HasGroup,
        HasRole,
        CanHave
    }
}
