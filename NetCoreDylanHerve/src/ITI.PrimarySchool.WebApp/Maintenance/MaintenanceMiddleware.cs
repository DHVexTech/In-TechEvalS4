using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.PrimarySchool.WebApp.Maintenance
{
    public class MaintenanceMiddleware
    {
        readonly RequestDelegate _next;

        public MaintenanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("<h1>Site en Maintenance</h1>");
        }
    }
}
