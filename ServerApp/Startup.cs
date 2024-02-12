using System.Diagnostics;
public class Startup
{
    // ...

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
    Nginx nginx = new Nginx();
    }
}