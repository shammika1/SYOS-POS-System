using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SYOS.Server.Services
{
    public class RequestHandler<TRequest, TResponse>
    {
        private readonly BlockingCollection<(TRequest Request, TaskCompletionSource<TResponse> ResponseTask)> requestQueue;
        private readonly Func<TRequest, Task<TResponse>> processRequest;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Task processingTask;

        public RequestHandler(Func<TRequest, Task<TResponse>> processRequest)
        {
            this.processRequest = processRequest;
            requestQueue = new BlockingCollection<(TRequest, TaskCompletionSource<TResponse>)>();
            cancellationTokenSource = new CancellationTokenSource();
            processingTask = Task.Run(ProcessRequestsAsync);
        }

        public async Task<TResponse> HandleRequestAsync(TRequest request)
        {
            var tcs = new TaskCompletionSource<TResponse>();
            requestQueue.Add((request, tcs));
            return await tcs.Task;
        }

        private async Task ProcessRequestsAsync()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var (request, responseTask) = requestQueue.Take(cancellationTokenSource.Token);
                    try
                    {
                        var response = await processRequest(request);
                        responseTask.SetResult(response);
                    }
                    catch (Exception ex)
                    {
                        responseTask.SetException(ex);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        public async Task StopAsync()
        {
            cancellationTokenSource.Cancel();
            requestQueue.CompleteAdding();
            await processingTask;
        }
    }
}