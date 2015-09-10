using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Prices.Startup1))]

namespace Prices
{
	public class Startup1
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}