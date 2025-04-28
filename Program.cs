
using NetDapperWebApi_local;
using NetDapperWebApi_local.Extensions;


namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddWebServices(builder.Configuration).AddAuthentications(builder.Configuration);
            builder.Services.AddSwaggerExplorers(builder.Configuration);
            builder.Services.AddCustomModelStateValidation();
            //--------------------------------------------
            var app = builder.Build();
            app.UseCustomExtensions();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthentications();
            app.MapControllers();
            app.UseCustomMiddlewares();
            app.Run();
        }
    }
}
