
// using HotChocolate;
// using HotChocolate.AspNetCore;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;

// var builder = WebApplication.CreateBuilder(args);

// builder.Services
//     .AddEndpointsApiExplorer()
//     .AddSwaggerGen()
//     .AddGraphQLServer()
//     .AddQueryType<Query>();

// var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseRouting();

// app.MapControllers();
// app.MapGraphQL();

// app.Run();

// public class Query
// {

// }

// using System;

// class Program
// {
//     static async Task Main(string[] args)
//     {
//         Console.WriteLine("Welcome to Palworld Server Manager!");
//         Console.WriteLine("Configure ports for forwarding?");
//         string portForward = Console.ReadLine() ?? "";
//         if (portForward == "y")
//         {
//             string forwardMorePorts;
//             do{
//             int intPort;
//             int extPort;

//             do
//             {
//                 Console.WriteLine("Enter internal port:");
//             }
//             while (!int.TryParse(Console.ReadLine(), out intPort));

//             do
//             {
//                 Console.WriteLine("Enter external port:");
//             }
//             while (!int.TryParse(Console.ReadLine(), out extPort));

//             UPnPPortForwarder portForwarder = new UPnPPortForwarder();
//             await portForwarder.SetupPortForwarding(intPort, extPort);
//             Console.WriteLine("Would you like to configure more ports to forward?(y/n)");
//                         forwardMorePorts = Console.ReadLine() ?? "";
//                         }
//                         while (forwardMorePorts == "y");

//             Console.WriteLine("Press any key to exit...");
//         }
//         if (args.Length > 0)
//         {
//         }
//         Console.WriteLine("Press any key to exit...");
//         Console.ReadKey();
//     }
// }

