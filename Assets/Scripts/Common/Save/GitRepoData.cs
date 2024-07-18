using System;
using System.Collections.Generic;

namespace Sabanishi.SdiAssignment
{
    [Serializable]
    public class GitRepoData
    {
        public List<string> repoPaths;
        
        public GitRepoData()
        {
            repoPaths = new List<string>();
        }
    }
}