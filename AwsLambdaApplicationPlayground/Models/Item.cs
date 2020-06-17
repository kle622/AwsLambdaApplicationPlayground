using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AwsLambdaApplicationPlayground.Models
{
    [DynamoDBTable("Thing")]
    public class Thing
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
