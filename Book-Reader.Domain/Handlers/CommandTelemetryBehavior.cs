using Book_Reader.Domain.Commands;
using Book_Reader.Domain.Extensions;
using Book_Reader.Domain.Handlers.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Book_Reader.Domain.Handlers
{
    public class CommandTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public CommandTelemetryBehavior(ILogger<CommandTelemetryBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                var response = await next();
                sw.Stop();
                using (CreateLogContext(request, sw, response))
                {
                    if (IsQuery(request))
                    {
                        _logger.Log(LogLevel.Information, $"QUERY Executed in {sw.ElapsedMilliseconds} milliseconds: {request.GetType().Name}");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, $"COMMAND Executed in {sw.ElapsedMilliseconds} milliseconds: {request.GetType().Name}");
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();

                using (CreateLogContextForException(request, sw))
                {
                    if (IsQuery(request))
                    {
                        _logger.Log(LogLevel.Information, $"QUERY {request.GetType().Name} Error: {ex.Message}");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, $" {request.GetType().Name} Error: {ex.Message}");
                    }
                }

                throw;
            }
        }

        private bool IsQuery(TRequest request)
        {
            return request.GetType().HasImplementedRawGeneric(typeof(IQuery));
        }


        private static IDisposable CreateLogContext(TRequest request, Stopwatch sw, TResponse response)
        {
            var outcome = "Unknown";
            var commandResponse = response as CommandResponse;
            if (commandResponse != null)
                outcome = commandResponse.IsSuccess ? "Success" : "Failure";

            return LogContext.Push(new PropertyEnricher("ExecutionTimeMs", sw.ElapsedMilliseconds),
                                   new PropertyEnricher("Command", request),
                                   new PropertyEnricher("CommandShortName", request.GetType().Name),
                                   new PropertyEnricher("Success", (response as CommandResponse)?.IsSuccess),
                                   new PropertyEnricher("ValidationErrors", (response as CommandResponse)?.ValidationErrors),
                                   new PropertyEnricher("Outcome", outcome)
                                  );
        }

        private static IDisposable CreateLogContextForException(TRequest request, Stopwatch sw)
        {
            return LogContext.Push(new PropertyEnricher("ExecutionTimeMs", sw.ElapsedMilliseconds),
                                   new PropertyEnricher("Command", request),
                                   new PropertyEnricher("Outcome", "Exception")
                                  );
        }
    }
}
