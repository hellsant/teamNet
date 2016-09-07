using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamNetDB.TransactionToDatabase
{
    public class BuilderString
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public BuilderString()
        {
            this.builderString = string.Empty;
        }
        
        /// <summary>
        /// Build the chain structure.
        /// </summary>
        /// <param name="newBuilderString">The string to form.</param>
        /// <param name="conditions">The list of conditions to add a wing chain</param>
        /// <returns></returns>
        public string StructureString(string newBuilderString, List<ConditionValue> conditions, bool isUpdate = false)
        {
            this.builderString = newBuilderString;
            this.Builder(conditions, isUpdate);
            return this.builderString;
        }

        /// <summary>
        /// Add conditions to the backbone.
        /// </summary>
        /// <param name="conditions">list contions</param>
        private void Builder(List<ConditionValue> conditions, bool isUpdate)
        {
            int reference = 0;
            foreach (ConditionValue condition in conditions)
            {
                if (reference > 0)
                {
                    if (isUpdate)
                    {
                        this.builderString += " , ";
                    }
                    else 
                    {
                        this.builderString += " and ";
                    }
                }
                this.builderString += " "+condition.ConditionColumn + " = '" +condition.ValueCondition +"' ";
                reference++;
            }
        }
        
        /// <summary>
        /// He is handling the chain structure.
        /// </summary>
        private string builderString;
    }
}
