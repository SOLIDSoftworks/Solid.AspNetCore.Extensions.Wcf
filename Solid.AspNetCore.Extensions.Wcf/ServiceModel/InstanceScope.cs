using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.ServiceModel
{
    internal class InstanceScope : IServiceScope, IExtension<InstanceContext>
    {
        private IServiceScope _scope;

        public InstanceScope(IServiceProvider root)
        {
            _scope = root.CreateScope();            
        }

        public IServiceProvider ServiceProvider => _scope.ServiceProvider;

        public void Attach(InstanceContext owner)
        {
            
        }

        public void Detach(InstanceContext owner)
        {
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
