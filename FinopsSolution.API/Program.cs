using Azure.Identity;
using FinopsSolution.API;
using FinopsSolution.API.Clients;
using FinopsSolution.API.Repository;
using FinopsSolution.API.Services;
using FinopsSolution.API.Settings;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net;
using Microsoft.Extensions.Azure;
using FinopsSolution.API.HttpHandlers;
using Azure.Core.Pipeline;

const string azureManagementClient = "azureManagement";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<CostManagementWorker>();

builder.Services.AddOptions<AzureCredentialSettings>()
    .BindConfiguration(AzureCredentialSettings.SettingName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<SubscriptionListSettings>()
    .BindConfiguration(SubscriptionListSettings.SettingName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<TableStorageSettings>()
    .BindConfiguration(TableStorageSettings.SettingName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<CostManagementWorkerSettings>()
    .BindConfiguration(CostManagementWorkerSettings.SettingName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
{
    AzureCredentialSettings creds = sp.GetRequiredService<IOptions<AzureCredentialSettings>>().Value;

    return new ClientSecretCredential(creds.TenantId,
                                      creds.ClientId,
                                      creds.ClientSecret);
});

builder.Services.AddSingleton<BearerTokenHandler>();

AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .OrResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
    .WaitAndRetryAsync(retryCount: 3,
                       sleepDurationProvider: (int _, DelegateResult<HttpResponseMessage> response, Context _) => response.Result.Headers?.RetryAfter?.Delta ?? TimeSpan.FromSeconds(0),
                       onRetryAsync: (DelegateResult<HttpResponseMessage> _, TimeSpan _, int _, Context _) => Task.CompletedTask);

builder.Services.AddHttpClient(azureManagementClient)
    .AddHttpMessageHandler<BearerTokenHandler>()
    .AddPolicyHandler(retryPolicy);

builder.Services.AddHttpClient<AlertManagementClient>(azureManagementClient);
builder.Services.AddHttpClient<RecommendationManagementClient>(azureManagementClient);
builder.Services.AddHttpClient<ResourceGroupManagementClient>(azureManagementClient);
builder.Services.AddHttpClient<SubscriptionManagementClient>(azureManagementClient);

builder.Services.AddSingleton<AdvisorRecommendationRepository>();
builder.Services.AddSingleton<ResourceGroupCostRepository>();
builder.Services.AddSingleton<SubscriptionManagementRepository>();

builder.Services.AddSingleton<CostManagementService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
