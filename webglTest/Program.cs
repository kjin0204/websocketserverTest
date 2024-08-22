using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using webglTest.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //string aa = "DFEB";
        //ushort number = Convert.ToUInt16(aa, 16); // 16진수 문자열을 uint16으로 
        //short number1 = Convert.ToInt16(aa, 16); // 16진수 문자열을 uint16으로 
        ////UInt16 aas = UInt16.Parse(aa);
        ////Int16 aas1 = Int16.Parse(aa);

        //var dddd = Convert.ToInt16("8000", 16);

        //var dddd1 = Convert.ToUInt16("8001", 16);

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        //MIME 설정==============================================================================
        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings[".unityweb"] = "application/octet-stream"; //8비트 바이너리 배열
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
            ContentTypeProvider = provider
        });
        //=======================================================================================

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}