﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Solid.AspNetCore.Extensions.Wcf.Models
{
    public delegate ServiceHost ServiceHostFactoryDelegate<TService>(IServiceProvider provider, TService singleton, Type type, Uri[] baseAddresses);
}
