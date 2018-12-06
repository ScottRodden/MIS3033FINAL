using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Quiz_Me
{
    public class Function
    {


        private static HttpClient httpClient;

        public Function()
        {
            httpClient = new HttpClient();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            var requestType = input.GetRequestType();
            string outputText = "";

            if (requestType == typeof(LaunchRequest))
            {
                return BodyResponse("Welcome to the Quiz Me Skill, Say Math, Science, Spelling or History to begin.", false);

            }

            else if (requestType == typeof(IntentRequest))
            {
                //outputText += "Request type is intent" 
                var intent = input.Request as IntentRequest;

                //SubjectIntent
                if (intent.Intent.Name.Equals("subject"))
                {
                    var subjectrequested = intent.Intent.Slots["course"].Value;

                    if (subjectrequested == null)
                    {
                        return BodyResponse("I did not recognize the subject that you requested, please try again", false);
                    }
                    else if (intent.Intent.Name.Equals("AMAZON.StopIntent"))
                    {
                        return BodyResponse("You have now exited the Quiz Me Skill", true);
                    }
                    else
                    {
                        return BodyResponse("I dont know how to handle this intent. Fatal error. Will self destruct in ten seconds.", false);
                    }
                }
            }
        }

        private SkillResponse BodyResponse(string outputSpeech, bool shouldEndSession, string repromtText = "Just say, Quiz me on any of the following subjects. Math, Science, History" +
            "Spelling")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromtText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromtText } };
            };

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.0"
            };
            return skillResponse;

        }

        public string FunctionHandler(string input, ILambdaContext context)
        {
            return input?.ToUpper();
        }

        public class Course
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public string CorrectLetter { get; set; }
        }



       
    }
}
