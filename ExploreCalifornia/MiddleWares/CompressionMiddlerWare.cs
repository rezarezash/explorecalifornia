using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Builder;

namespace ExploreCalifornia.MiddleWares
{
    public class CompressionMiddlerWare
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression();
        }
    }
}
