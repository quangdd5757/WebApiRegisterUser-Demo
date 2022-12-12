using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;
using WebApiRegisterUser_Demo.Models;

namespace WebApiRegisterUser_Demo.Services
{

    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken, IMongoCollection<UserService> _usersCollection, string _fileDataUsers, UserService _userService);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        //private IMongoCollection<ProductInfo> _productInfoCollection;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken, IMongoCollection<UserService> _usersCollection, string _fileDataUsers, UserService _userService)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation("Scoped Processing Service is working. Count: {Count}", executionCount);

                #region define data users
                // read data from file
                string _dataUsers = File.ReadAllText(_fileDataUsers);
                DataUserModel? _dataUserModel = JsonSerializer.Deserialize<DataUserModel>(_dataUsers);
                if (_dataUserModel != null)
                {
                    // import data users
                    var users = new List<User>();
                    foreach (UserModel user in _dataUserModel.users)
                    {
                        if (_userService.GetUserByEmail(user.email) == null)
                        {
                            var temp = new User(user);
                            temp.active = 1;
                            users.Add(temp);
                        }
                    }
                    if (users.Count() > 0)
                    {
                        _userService.CreateUsers(users);
                        _logger.LogInformation("Create default data user done! ({})", users.Count());
                    }
                }
                #endregion
                await Task.Delay(100000, stoppingToken);
                //stoppingToken.ThrowIfCancellationRequested();
            }
        }
    }

    public class ConsumeScopedServiceHostedService : BackgroundService
    {
        private readonly ILogger<ConsumeScopedServiceHostedService> _logger;
        private readonly IMongoCollection<UserService> _usersCollection;
        private readonly UserService _userService;
        private readonly string _fileDataUsers;

        public ConsumeScopedServiceHostedService(IServiceProvider services,
            ILogger<ConsumeScopedServiceHostedService> logger, IOptions<MongoDBSettings> mongoDBSettings, IOptions<DataUserConfig> dataUserConfig, UserService userService)
        {
            Services = services;
            _logger = logger;
            // get connection mongodb
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<UserService>(mongoDBSettings.Value.CollectionUsers);
            _userService = userService;
            // get path file data users
            _fileDataUsers = dataUserConfig.Value.PathFile;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.DoWork(stoppingToken, _usersCollection, _fileDataUsers, _userService);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
