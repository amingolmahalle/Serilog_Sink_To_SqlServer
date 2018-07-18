using System;

namespace serlog_SqlServer.Exception
{

    [Serializable]
    public class ApplicationNameNotProvidedException : System.Exception
    {
        public ApplicationNameNotProvidedException() : base("Can not find Application Name in config file, consider checking pre installation guides.")
        { }
    }
}
