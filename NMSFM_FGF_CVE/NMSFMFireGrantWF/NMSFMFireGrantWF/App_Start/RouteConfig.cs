using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace NMSFMFireGrantWF
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.Ignore("{*allaxd}", new { allaxd = @".*\.axd(/.*)?" });
            //routes.Add(new System.ServiceModel.Activation.ServiceRoute("", newSystem.ServiceModel.Activation.WebServiceHostFactory(), typeof(CacheService)));

            routes.MapPageRoute("EditUser", "EditUser/{UserId}", "~/Account/EditUser.aspx");
            routes.MapPageRoute("Category", "Category/{CategoryId}", "~/Admin/Category.aspx");
            routes.MapPageRoute("PriorAppsAdmin", "Admin/Prior/{PriorApps}", "~/Admin/Home.aspx");
            routes.MapPageRoute("PriorAppsUser", "User/Prior/{PriorApps}", "~/User/Home.aspx");
            routes.MapPageRoute("SignatorLogin", "LoginSignator/{ApplicationId}/{LoginToken}", "~/Account/Login.aspx");
            routes.MapPageRoute("ViewApplication", "Application/Reporting/View", "~/Application/Reporting/ApplicationPrint.aspx");
            routes.MapPageRoute("ViewAwardLetter", "Application/Reporting/ViewAward", "~/Application/Reporting/GrantAwarded.aspx");
            routes.MapPageRoute("ViewDenialLetter", "Application/Reporting/ViewDenial", "~/Application/Reporting/DenialLetter.aspx");
            routes.MapPageRoute("ViewNotFundedLetter", "Application/Reporting/ViewNotFunded", "~/Application/Reporting/NotFunded.aspx");
            routes.MapPageRoute("ApproveRegistration", "ApproveRegistration/{Approve}/{UserId}/{RegistrationToken}", "~/Account/Register.aspx");
            routes.MapPageRoute("ResetUserPassword", "ResetUserPassword/{UserId}/{ForgotPasswordToken}", "~/Account/ResetPassword.aspx");
        }
    }
}
