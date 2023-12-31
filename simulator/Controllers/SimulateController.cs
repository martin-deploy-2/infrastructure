﻿using System.Diagnostics;
using System.Text;
using System.Text.Json;
using MartinSimulator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MartinSimulator.Controllers;

[Serializable]
public class MartinException : Exception
{
    public MartinException() { }
    public MartinException(string message) : base(message) { }
    public MartinException(string message, Exception inner) : base(message, inner) { }
    protected MartinException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class SimulateController : Controller
{
    private readonly ILogger<SimulateController> _logger;
    private readonly IHostApplicationLifetime _app;

    public SimulateController(ILogger<SimulateController> logger, IHostApplicationLifetime app)
    {
        _logger = logger;
        _app = app;
    }

    [HttpPost]
    public IActionResult Exception()
    {
        _logger.LogInformation(nameof(Exception));
        throw new MartinException("Simulate exception.");
    }

    [HttpPost]
    public IActionResult Shutdown()
    {
        _logger.LogInformation(nameof(Shutdown));
        _app.StopApplication();
        return NoContent();
    }

	/// <summary>
	/// An enumeration containing the available robot actions. The available actions are:
	/// <list type="table">
	/// <listheader>
	/// <term>Action</term>
	/// <term>Description</term>
	/// <term>Power Consumption</term>
	/// </listheader>
	/// <item>
	/// <term>Forward</term>
	/// <term>Move forwards in a straight line.</term>
	/// <term>50W</term>
	/// </item>
	/// <item>
	/// <term>Backward</term>
	/// <term>Move backwards in a straight line.</term>
	/// <term>50W</term>
	/// </item>
	/// <item>
	/// <term>RotateLeft</term>
	/// <term>Rotate to the left.</term>
	/// <term>30W</term>
	/// </item>
	/// <item>
	/// <term>RotateRight</term>
	/// <term>Rotate to the right.</term>
	/// <term>30W</term>
	/// </item>
	/// <item>
	/// <term>Dig</term>
	/// <term>Tells the robot to dig and obtain a soil sample.</term>
	/// <term>800W</term>
	/// </item>
	/// </list>
	/// </summary>
	[HttpPost]
    public IActionResult Crash()
    {
        _logger.LogInformation(nameof(Crash));
        Environment.Exit(42);
        throw new MartinException("Simulate crash."); // should never be thrown
    }

    [HttpPost]
    public async Task<ActionResult> Echo()
    {
        _logger.LogInformation(nameof(Echo));
        object echo;

        using (var stream = new StreamReader(HttpContext.Request.Body))
        {
            var body = await stream.ReadToEndAsync();
            var headers = new Dictionary<string,string?>();
            foreach (var item in HttpContext.Request.Headers)
            {
                headers.Add(item.Key, item.Value);
            }

            echo = new
            {
                Request = $"{Request.Method} {Request.Path}{Request.QueryString} {Request.Protocol}",
                Headers = headers,
                Body = JsonSerializer.Deserialize<Dictionary<string, object?>>(body)
            };
        }

		_logger.LogInformation("{STRING}", JsonSerializer.Serialize(echo, new JsonSerializerOptions
		{
			WriteIndented = true,
		}));

        if (HttpContext.Request.Method == "OPTIONS" &&
            HttpContext.Request.Headers.ContainsKey("WebHook-Request-Origin"))
        {
            // CloudEvents spec:
            // https://github.com/cloudevents/spec/blob/v1.0/http-webhook.md#41-validation-request
            // We let all the webhooks use this endpoint freely without limits
            HttpContext.Response.Headers.Add("WebHook-Allowed-Origin", new StringValues("*"));
            HttpContext.Response.Headers.Add("WebHook-Allowed-Rate", new StringValues("*"));
        }

        return Ok(echo);
    }
}
