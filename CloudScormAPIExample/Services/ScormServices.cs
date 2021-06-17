using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudScormAPIExample.Models;
using Com.RusticiSoftware.Cloud.V2.Api;
using Com.RusticiSoftware.Cloud.V2.Model;
using Microsoft.Extensions.Configuration;

namespace CloudScormAPIExample.Services
{
    public class ScormServices
    {
        private readonly IConfiguration _configuration;
        

        public ScormServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<CourseSchema> LoadAllCourse()
        {
            var cloudScormAPI = new CloudScormAPI();
            _configuration.GetSection("CloudScormAPI").Bind(cloudScormAPI);
           
            //For debug remove it 
            //"https://cloud.scorm.com/EngineWebServices",
            // Configure HTTP basic authorization: APP_NORMAL
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.Username = cloudScormAPI.Username;
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.Password = cloudScormAPI.Password;
               ;

            // Then (optionally) further authenticate via Oauth2 token access
            ApplicationManagementApi applicationManagementApi = new ApplicationManagementApi();

            var permissions = new PermissionsSchema {Scopes = new List<string> {"read"}};
            var expiry = DateTime.Now.AddMinutes(30);

            var tokenRequest = new TokenRequestSchema(permissions, expiry);
            StringResultSchema tokenResult = applicationManagementApi.CreateToken(tokenRequest);
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.AccessToken = tokenResult.Result;

            // this call will now use Oauth2 with the "read:registration" scope
            // if configured.  otherwise the basic auth credentials will be used

            CourseApi course = new CourseApi();

            var res = course.GetCourses(); //"T15Leaders010140bb81a8-fcfe-481a-8209-30ba6ac28b11");
            return res.Courses.Count > 0 ? res.Courses : new List<CourseSchema>();


        }
        //
    }
}
