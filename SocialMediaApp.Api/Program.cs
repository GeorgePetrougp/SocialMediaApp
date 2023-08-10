using SocialMediaApp.Api.Extensions;

namespace SocialMediaApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.RegisterServices(typeof(Program));


            var app = builder.Build();
            app.RegisterPipelineComponents(typeof(Program));


            app.Run();
        }
    }
}