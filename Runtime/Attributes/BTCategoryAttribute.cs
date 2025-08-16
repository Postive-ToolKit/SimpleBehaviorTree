using System;

namespace Postive.BehaviourTrees.Runtime.Attributes
{
    public class BTCategoryAttribute : Attribute
    {
        public readonly string Category;
        public readonly string Description;
        
        public BTCategoryAttribute(string category = null, string description = null)
        {
            Category = string.IsNullOrEmpty(category) ? category : "Base";
            Description = string.IsNullOrEmpty(description) ? description : "No description provided.";
        }
    }
}