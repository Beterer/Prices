using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Prices.Features;
using Prices.Models;
using Common.Models;
using System.Threading.Tasks;

namespace Prices.Hubs
{
	public class PricesHub : Hub
	{
		private static List<ProducModel> Products { get; set; }
		private static System.Timers.Timer ServiceConsumerTimer { get; set; }
		private static List<UserProductsModel> UserProducts = new List<UserProductsModel>();
		private static object _lock = new Object();

		#region client methods
		private void SendPrices(string products, string connectionId)
		{
			Clients.Client(connectionId).sendPrices(products);
		}

		public void AttachUserToProducts(string productIds)
		{
			if (UserProducts.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId) == null)
				UserProducts.Add(new UserProductsModel { ConnectionId = Context.ConnectionId, ProductIds = productIds.Split(',') });
			else
				UserProducts.Where(p => p.ConnectionId == Context.ConnectionId).First().ProductIds = productIds.Split(',');

		}
		#endregion

		#region events
		public override System.Threading.Tasks.Task OnConnected()
		{
			if (ServiceConsumerTimer == null)
				InitialiseTimer();

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			UserProducts.RemoveAll(p => p.ConnectionId == Context.ConnectionId);

			return base.OnDisconnected(stopCalled);
		}		

		private void OnServiceRefresh(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (_lock)
			{
				Products = ServiceProxy.ConsumeService();
				var parsedProducts = String.Empty;
				var productsForUser = new List<ProducModel>();

				foreach (var userProduct in UserProducts)
				{
					foreach (string prodId in userProduct.ProductIds)
					{						
						productsForUser.Add(Products.Where(p => p.ProductId == Convert.ToInt32(prodId)).First());						
					}

					parsedProducts = ParseProductsToHTML(productsForUser);
					productsForUser.Clear();
					SendPrices(parsedProducts, userProduct.ConnectionId);
				}
			}
		}
		#endregion

		private void InitialiseTimer()
		{
			ServiceConsumerTimer = new System.Timers.Timer(1000);
			ServiceConsumerTimer.Elapsed += OnServiceRefresh;
			ServiceConsumerTimer.AutoReset = true;
			ServiceConsumerTimer.Enabled = true;
		}

		private string ParseProductsToHTML(List<ProducModel> products)
		{
			var html = String.Empty;

			foreach (var product in products)
			{
				html += "<p id=" + product.ProductId + "p>" + product.ProductName + ":" + product.ProductPrice + "</p>"; //TODO: do this pretty
			}

			return html;
		}
	}
}