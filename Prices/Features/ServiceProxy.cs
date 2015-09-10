using Common.Models;
using Prices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prices.Features
{
	public static class ServiceProxy
	{
		public static List<ProducModel> ConsumeService()
		{
			var products = new List<ProducModel>();

			var client = new ProductsServiceReference.Service1Client();
			var response = client.GetProducts();
			foreach (var product in response)
			{
				products.Add(new ProducModel { ProductId = product.ProductId, ProductName = product.ProductName, ProductPrice = product.ProductPrice });
			}

			return products;
		}
	}
}