using Microsoft.Extensions.Configuration;

namespace ExploreCalifornia
{
    public class FeatureToggles
    {
        private readonly IConfiguration _configuration;

        public FeatureToggles()
        {

        }

        public FeatureToggles(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString
        {
            get
            {
                return _configuration.GetValue<string>("ConnectionStrings:BlogDbContext");
            }
        }

        public bool EnabledDeveloperExceptions
        {
            get
            {
                return _configuration.GetValue<bool>("FeatureToggles:EnableDeveloperExceptions");
            }
        }

        public bool EnableDeveloperExceptions { get; set; }
    }
}
