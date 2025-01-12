namespace ReadingIsGood.API.Middlewares
{
    public class Logger : ILogger
    {
        IWebHostEnvironment _hostingEnvironment;
        public Logger(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IDisposable BeginScope<TState>(TState state) => null;
        public bool IsEnabled(LogLevel logLevel) => true;

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string directory = $"{_hostingEnvironment.ContentRootPath}/log/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter writer = new StreamWriter(directory + DateTime.Now.ToString("ddMMyyyy") + "log.txt", true))
            {
                await writer.WriteLineAsync($"Log Level : {logLevel.ToString()} | Event ID : {eventId.Id} | Event Name : {eventId.Name} | Formatter : {formatter(state, exception)}");

                writer.Close();
                await writer.DisposeAsync();
            }
        }
    }
}
