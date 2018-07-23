using System;

namespace serlog_SqlServer.Exceptions
{

    [Serializable]
    public class ApplicationNameNotProvidedException : Exception
    {
        public ApplicationNameNotProvidedException() : base("Can not find Application Name in config file, consider checking pre installation guides.")
        { }
    }
}
