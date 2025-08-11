using Prometheus;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with console JSON logging + Grafana Loki
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Information()
      .Enrich.FromLogContext()
      .WriteTo.Console(new JsonFormatter())
      .WriteTo.GrafanaLoki(
          "http://loki:3100",
          new[]
          {
              new LokiLabel { Key = "app", Value = "MonitoringDemo" },
              new LokiLabel { Key = "env", Value = "dev" }
          }
      );
});

// Add services
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Middleware for logging & metrics
app.UseSerilogRequestLogging();
app.UseHttpMetrics();

// Prometheus metrics endpoint
app.MapMetrics();

// Health check endpoint
app.MapHealthChecks("/health");

// Root endpoint
app.MapGet("/", () => Results.Ok(new { message = "MonitoringDemo running" }));

// Custom metrics
var requestsCounter = Metrics.CreateCounter(
    "monitoringdemo_custom_requests_total",
    "Total custom requests"
);

var processingHistogram = Metrics.CreateHistogram(
    "monitoringdemo_processing_seconds",
    "Processing time in seconds"
);

// Example API endpoint
app.MapGet("/api/do-work", async () =>
{
    requestsCounter.Inc();
    var sw = System.Diagnostics.Stopwatch.StartNew();

    // Simulated work
    await Task.Delay(Random.Shared.Next(50, 400));

    sw.Stop();
    processingHistogram.Observe(sw.Elapsed.TotalSeconds);

    Log.Information("Work completed {ElapsedMs}ms", sw.ElapsedMilliseconds);

    return Results.Ok(new { status = "done", elapsed = sw.ElapsedMilliseconds });
});

app.Run();
