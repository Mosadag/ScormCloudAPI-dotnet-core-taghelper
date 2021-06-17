using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudScormAPIExample.Models;
using Com.RusticiSoftware.Cloud.V2.Api;
using Com.RusticiSoftware.Cloud.V2.Model;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;

namespace CloudScormAPIExample.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("ScormTag")]
    public class ScormTagHelper : TagHelper
    {
        private readonly IConfiguration _configuration;
        public ScormTagHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CourseId { get; set; }
        public string Caption { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var cloudScormAPI = new CloudScormAPI();
            _configuration.GetSection("CloudScormAPI").Bind(cloudScormAPI);
            output.TagName = "a";
            //"https://cloud.scorm.com/EngineWebServices",
            // Configure HTTP basic authorization: APP_NORMAL
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.Username = cloudScormAPI.Username;
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.Password = cloudScormAPI.Password;

            // Then (optionally) further authenticate via Oauth2 token access
            ApplicationManagementApi applicationManagementApi = new ApplicationManagementApi();

            var permissions = new PermissionsSchema { Scopes = new List<string> { "read" } };
            var expiry = DateTime.Now.AddMinutes(30);

            var tokenRequest = new TokenRequestSchema(permissions, expiry);
            StringResultSchema tokenResult = applicationManagementApi.CreateToken(tokenRequest);
            Com.RusticiSoftware.Cloud.V2.Client.Configuration.Default.AccessToken = tokenResult.Result;

            // this call will now use Oauth2 with the "read:registration" scope
            // if configured.  otherwise the basic auth credentials will be used

            CourseApi course = new CourseApi();


           

            var url = course.BuildCoursePreviewLaunchLink(CourseId, new LaunchLinkRequestSchema(200, cloudScormAPI.LaunchLink));
            output.Attributes.SetAttribute("href", url.LaunchLink);
            output.Attributes.SetAttribute("target", "_blank");
            output.Content.SetContent(Caption);
        }
    }
}
