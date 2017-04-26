using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.PrimarySchool.WebApp.Maintenance
{
    public static class MaintenanceMiddlewareExtension
    {

        public static IApplicationBuilder UseMaintenanceMiddleware(this IApplicationBuilder app) => app.UseMiddleware<MaintenanceMiddleware>();

    }
}
