using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
//using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json.Linq;

namespace AzureDevOpsConnector
{
    class AzDOManager
    {
        static VssConnection connection;

        public static int CreateWorkItem(string[] workItemDetails)
        {
            //Fill Iteration path and assigned to values from azure dev ops
            var workItem = new JsonPatchDocument
            {
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/System.Title",
                    Value     = workItemDetails[5]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/System.Description",
                    Value     = workItemDetails[2]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/Microsoft.VSTS.Common.AcceptanceCriteria",
                    Value     = workItemDetails[3]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/System.Tags",
                    Value     = workItemDetails[4]
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/Microsoft.VSTS.Common.BacklogPriority",
                    Value     = "3"
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/Microsoft.VSTS.Common.Priority",
                    Value     = "3"
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/Microsoft.VSTS.Scheduling.StoryPoints",
                    Value     = Convert.ToInt64(workItemDetails[0])
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/System.IterationPath",
                    Value     = workItemDetails[8]
                }
                ,
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path      = "/fields/System.AssignedTo",

                    Value     = workItemDetails[1]
                }
            };


            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            WorkItem createdWorkItem = witClient.CreateWorkItemAsync(workItem, workItemDetails[6], workItemDetails[7]).Result;

            return createdWorkItem.Id.Value;
        }


        public static WorkItem ReadWorkItem(string project, int workItemId)
        {
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            WorkItem createdWorkItem = witClient.GetWorkItemAsync(project,workItemId).Result;
            return createdWorkItem;
        }

        public static bool initializeConnection(string personalAccessToken,Uri orgUrl)
        {
            connection = new VssConnection(orgUrl, new VssBasicCredential("", personalAccessToken));

            return true;
        }



    }

    
    //Try console application
   /* class Program
    { 
        const String personalAccessToken = "";//Fill in access token from Azure devops personal tokens

        const string title                 = "Test AzDO item";
        const string description           = "Test Description";
        const string acceptanceCriteria    = "NA";
        const string tags                  = "Test Tags";


        static void Main()
        {
            Uri orgUrl = new Uri("");//Fill azure devops site URL
            AzDOManager.initializeConnection(personalAccessToken, orgUrl);
            string assignedTo = "";//name of person from azure devops users
            string projectName = "";// project under the devops url
            string itemType = "";// ex: "Task","User Story"
            string[] workItemDetails; //update order and index as required
            string iterationPath = ""; //iteration path from azure devops ,put // for / in path
            //create the work item
            workItemDetails = new string[] { "1", assignedTo,description, acceptanceCriteria, tags,title,projectName, itemType,iterationPath };
            int workId = AzDOManager.CreateWorkItem(workItemDetails);
            WorkItem workItem = AzDOManager.ReadWorkItem(workItemDetails[6],workId);
            Console.WriteLine(workItem.Id);
        }
    }*/
    
}

