using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common.Models;

namespace ProductsService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
	public class Service1 : IService1
	{
		public List<ProducModel> GetProducts()
		{
			var products = new List<ProducModel>();
			var rnd = new Random();
			products.Add(new ProducModel { ProductId = 1, ProductName = "P1", ProductPrice = rnd.Next(1, 40) });
			products.Add(new ProducModel { ProductId = 2, ProductName = "P2", ProductPrice = rnd.Next(1, 40) });
			products.Add(new ProducModel { ProductId = 3, ProductName = "P3", ProductPrice = rnd.Next(1, 40) });
			products.Add(new ProducModel { ProductId = 4, ProductName = "P4", ProductPrice = rnd.Next(1, 40) });
			products.Add(new ProducModel { ProductId = 5, ProductName = "P5", ProductPrice = rnd.Next(1, 40) });

			return products;
		}

		public CompositeType GetDataUsingDataContract(CompositeType composite)
		{
			if (composite == null)
			{
				throw new ArgumentNullException("composite");
			}
			if (composite.BoolValue)
			{
				composite.StringValue += "Suffix";
			}
			return composite;
		}
	}
}
