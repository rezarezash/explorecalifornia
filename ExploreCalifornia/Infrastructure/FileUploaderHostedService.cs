using ExploreCalifornia.DAL;
using ExploreCalifornia.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ExploreCalifornia.Infrastructure
{
    public class FileUploaderHostedService : IHostedService
    {
        private readonly ChannelReader<FileUploadMessage> _channelReader;
        private readonly ILogger<FileUploaderHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer? _timer;
        public FileUploaderHostedService(ILogger<FileUploaderHostedService> logger,
                Channel<FileUploadMessage> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer ??= new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            while (await _channelReader.WaitToReadAsync())
            {
                try
                {
                    var message = await _channelReader.ReadAsync();
                    {
                        _logger.Log(LogLevel.Information, $"reading file {message.FileName} from  hosted service");

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetService<BlogDbContext>();
                            db.Posts.Add(new Post
                            {
                                Title = message.FileName,
                                Author = message.FileName,
                                Body = $"{message.FileName} from consumer hosted service",
                                PostedOn = DateTime.Now
                            });

                            await db.SaveChangesAsync();
                        }

                        await Task.Delay(50);
                    }
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, e.Message);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
    }
}
