using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Utilities
{
    public static class Utils
    {
        public const string TenantId = "57594912-ac84-4ed8-be54-de05f5dc0433"; //Tenant ID Azure AD
        public const string ClientId = "06734822-31ac-43dc-bfb7-ba6a08683e88"; //Application ID in Azure AD
        public const string ClientSecret = "heq8Q~KY7CkKLno03I_vH2RKgwXwZVA8a3WNCc2m"; // App's secret Value (not Secret ID)
        public const string subscriptionIdList = "79b7a5fc-a883-497d-ac63-7ef6b3de8bb0"; //Ex: xyz-xpto,abc-def

        public const string DbConnectionString = "Server=(local)\\sqlexpress;Database=TransactionDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
        public const int PercentageWarning = 10;

        public const string EmailFrom = "email_from";
        public const string EmailTo = "email_to";
        public const string EmailHost = "email_host";
        public const int EmailPort = 587;
        public const string EmailCredential = "email";
        public const string EmailPassword = "password";

        public const string storageUri = "https://akshayfinopsdata.table.core.windows.net";
        public const string accountName = "akshayfinopsdata";
        public const string storageAccountKey = "ofRdvWqxqN1SyaVRSwBwHyXjUXWOKVurySMMA3ac4bnjxLKFPzcDb8WBzvxHMw4jePqY1155Ldxt+ASt/l/+zw==";
        public const string StorageAccountConnStr = "DefaultEndpointsProtocol=https;AccountName=akshayfinopsdata;AccountKey=ofRdvWqxqN1SyaVRSwBwHyXjUXWOKVurySMMA3ac4bnjxLKFPzcDb8WBzvxHMw4jePqY1155Ldxt+ASt/l/+zw==;EndpointSuffix=core.windows.net";
        public const int RunCostManagemementApiInSeconds = 300; //seconds to run the job
        public const int RunEmailRescheduleInSeconds = 300; //seconds to run the job
    }
}
