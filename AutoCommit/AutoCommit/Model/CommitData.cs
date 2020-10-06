using System;
using System.Collections.Generic;
using System.Text;

namespace AutoCommit.Model
{
    public class CommitData
    {
        public string Account
        {
            get;
            set;
        }

        public string Branch
        {
            get;
            set;
        }

        public string Repo
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public List<FileData> Files
        {
            get;
            set;
        }

        public class FileData
        {
            public string Path
            {
                get;
                set;
            }

            public string Content
            {
                get;
                set;
            }
        }
    }
}
