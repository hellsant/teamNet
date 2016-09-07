using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace RestService.Security.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {            
            return base.SendAsync(request, cancellationToken);
        }
    }
}