using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Res
{
    public class Parameter
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class StagedTarget
    {
        public string url { get; set; }
        public string resourceUrl { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class StagedUploadsCreate
    {
        public List<StagedTarget> stagedTargets { get; set; }
    }

    public class Data
    {
        public StagedUploadsCreate stagedUploadsCreate { get; set; }
    }

    public class ThrottleStatus
    {
        public double maximumAvailable { get; set; }
        public int currentlyAvailable { get; set; }
        public double restoreRate { get; set; }
    }

    public class Cost
    {
        public int requestedQueryCost { get; set; }
        public int actualQueryCost { get; set; }
        public ThrottleStatus throttleStatus { get; set; }
    }

    public class Extensions
    {
        public Cost cost { get; set; }
    }

    public class StagedUploadsRes
    {
        public Data data { get; set; }
        public Extensions extensions { get; set; }
    }

    
}
