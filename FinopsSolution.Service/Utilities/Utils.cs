using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinopsSolution.Service.Utilities
{
    public static class Utils
    {
        public const string TenantId = "fd4a983a-7713-4aae-8c12-629983f1fe91"; //Tenant ID Azure AD
        public const string ClientId = "ebac428b-9843-4326-88d9-50f59048dd88"; //Application ID in Azure AD
        public const string ClientSecret = "qmZ8Q~y4Q6Ai~zq3eFqP45T_9OB_~Dhizyfj6a0o"; // App's secret Value (not Secret ID)
        public const string subscriptionIdList = "353c281f-ad57-43e1-8c7d-53f387cbec07"; //Ex: xyz-xpto,abc-def

        public const string DbConnectionString = "Server=(local)\\sqlexpress;Database=TransactionDB;Trusted_Connection=True;MultipleActiveResultSets=True;";
        public const int PercentageWarning = 10;

        public const string EmailFrom = "email_from";
        public const string EmailTo = "email_to";
        public const string EmailHost = "email_host";
        public const int EmailPort = 587;
        public const string EmailCredential = "email";
        public const string EmailPassword = "password";

        public const string storageUri = "https://storusefinopsdata.table.core.windows.net";
        public const string accountName = "storusefinopsdata";
        public const string storageAccountKey = "yaNYbwWY0KOPjP5yX0ssshiNBOT/2jPH0lNSYzEmH59lYAjpsgl7XbWYsQdeRuzI2UzRXu6AGQjU+AStXHslAg==";
        public const string StorageAccountConnStr = "DefaultEndpointsProtocol=https;AccountName=storusefinopsdata;AccountKey=yaNYbwWY0KOPjP5yX0ssshiNBOT/2jPH0lNSYzEmH59lYAjpsgl7XbWYsQdeRuzI2UzRXu6AGQjU+AStXHslAg==;EndpointSuffix=core.windows.net";
        public const int RunCostManagemementApiInSeconds = 3600; //seconds to run the job
        public const int RunEmailRescheduleInSeconds = 300; //seconds to run the job
    }
}
