using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Data;

namespace Todo.Api.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            var builder = new ConfigurationBuilder()
                //.SetBasePath(env.ContentRootPath)
                // .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                // .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();


            services.AddDbContext<TodoContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            var mediatoRAssembly = Assembly.Load("Todo.Api");
            services.AddMediatR(mediatoRAssembly, Assembly.GetCallingAssembly());

            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddScoped<DbInitializer>();
        }
    }
}
