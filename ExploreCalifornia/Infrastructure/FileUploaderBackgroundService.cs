using ExploreCalifornia.DAL;
using ExploreCalifornia.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ExploreCalifornia.Infrastructure
{
    public class FileUploaderBackgroundService : BackgroundService
    {
        private readonly ChannelReader<FileUploadMessage> _channelReader;
        private readonly ILogger<FileUploaderBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public FileUploaderBackgroundService(Channel<FileUploadMessage> channel,
            ILogger<FileUploaderBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumerTasks = Enumerable.Range(0, 20).Select(c =>
            {
                return FileReaderConsumer(_channelReader, stoppingToken, c.ToString());
            });

            await Task.WhenAll(consumerTasks);
        }

        private async Task FileReaderConsumer(ChannelReader<FileUploadMessage> channelReader,
            CancellationToken stoppingToken, string consumerId)
        {

            while (await channelReader.WaitToReadAsync(stoppingToken))
            {
                try
                {
                    var message = await channelReader.ReadAsync(stoppingToken);
                    {
                        _logger.Log(LogLevel.Information, $"reading file {message.FileName} from consumer {consumerId}");

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetService<BlogDbContext>();
                            db.Posts.Add(new Post
                            {
                                Title = message.FileName,
                                Author = message.FileName,
                                Body = $"{ message.FileName } from consumer { consumerId }",
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

    }
}
