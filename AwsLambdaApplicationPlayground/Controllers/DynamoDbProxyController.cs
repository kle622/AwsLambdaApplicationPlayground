using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AwsLambdaApplicationPlayground.Models;
using AwsLambdaApplicationPlayground.Cqrs;
using Amazon.Runtime.Internal;

namespace AwsLambdaApplicationPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoDbProxyController : ControllerBase
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly ILogger _logger;
        private readonly string _dynamoDbTableName;

        public DynamoDbProxyController(
            IConfiguration configuration,
            IDynamoDBContext dynamoDbContext,
            ILogger<DynamoDbProxyController> logger)
        {
            _dynamoDbTableName = nameof(Thing);
            if (String.IsNullOrEmpty(_dynamoDbTableName))
            {
                var message = "Missing configuration for DynamoDB table name";
                _logger.LogCritical(message);
                throw new Exception(message);
            }

            _dynamoDbContext = dynamoDbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<JsonResult> Get(string id, CancellationToken cancellationToken)
        {
            var response =  await _dynamoDbContext.LoadAsync<Thing>(id, cancellationToken);

            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<JsonResult> Create([FromBody] CreateThingCommand createItemCommand, CancellationToken cancellationToken)
        {
            var thing = new Thing
            {
                Id = Guid.NewGuid().ToString(),
                Name = createItemCommand.Name
            };
            await _dynamoDbContext.SaveAsync<Thing>(thing, cancellationToken);

            return new JsonResult(thing);
        }

        [HttpPost]
        public async Task<JsonResult> Update([FromBody] UpdateThingCommand updateThingCommand, CancellationToken cancellationToken)
        {
            var thing = new Thing
            {
                Id = updateThingCommand.Id,
                Name = updateThingCommand.Name
            };
            await _dynamoDbContext.SaveAsync<Thing>(thing, cancellationToken);

            return new JsonResult(updateThingCommand);
        }

        [HttpDelete] 
        public async Task<JsonResult> Delete([FromBody] DeleteThingCommand deleteThingCommand, CancellationToken cancellationToken)
        {
            await _dynamoDbContext.DeleteAsync<Thing>(deleteThingCommand.Id);

            return new JsonResult(deleteThingCommand);
        }
    }
}
