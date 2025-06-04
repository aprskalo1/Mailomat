using Mailomat.Integrations.SudReg.Extensions;
using Mailomat.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSudRegIntegrations(builder.Configuration);

var host = builder.Build();
host.Run();