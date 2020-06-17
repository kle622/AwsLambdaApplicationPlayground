using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsLambdaApplicationPlayground.Cqrs
{
    public class CreateThingCommand
    {
        public string Name { get; set; }
    }
}
