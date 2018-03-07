using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Solid.AspNetCore.Extensions.Wcf;

namespace Temp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWcfServiceWithMetadata<Service>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseWcfService<Service, IService>("/service");
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service : IService
    {
        public string Echo(string str)
        {
            return str;
        }

        public EchoObject ObjectEcho(EchoObject obj)
        {
            return obj;
        }
    }

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string Echo(string str);

        [OperationContract]
        EchoObject ObjectEcho(EchoObject obj);
    }

    [DataContract]
    public class EchoObject
    {
        [DataMember]
        public string Value { get; set; }
    }
}
