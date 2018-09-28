using System;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using RockLib.Configuration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Example.Secrets.Aws.Lambda
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(string input, ILambdaContext context)
        {
            // NOTE: The 'ValueToBeReplaced1 and ValueToBeReplace2 should automatically
            // get replaced with the secrets in Amazons SecretManager
            var replacedSetting1 = Config.AppSettings["TestSecret1"];
            var replacedSetting2 = Config.AppSettings["TestSecret2"];
            Console.WriteLine($"New TestSecret1: {replacedSetting1}");
            Console.WriteLine($"New TestSecret2: {replacedSetting2}");
        }
    }
}
