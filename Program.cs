using System;
using System.ServiceModel;
using Simple.Amazon.ECS;
using System.Configuration;

namespace Simple {
	class Program {
		// your Amazon ID's
		private readonly static string accessKeyId = ConfigurationManager.AppSettings["AmazonAccessKeyId"];
		private readonly static string secretKey = ConfigurationManager.AppSettings["AmazonSecretKey"];

		// the program starts here
		static void Main(string[] args) {

			// create a WCF Amazon ECS client
			BasicHttpBinding binding		= new BasicHttpBinding(BasicHttpSecurityMode.Transport);
			binding.MaxReceivedMessageSize	= int.MaxValue;
			AWSECommerceServicePortTypeClient client = new AWSECommerceServicePortTypeClient(
				binding,
				new EndpointAddress("https://webservices.amazon.com/onca/soap?Service=AWSECommerceService"));

			// add authentication to the ECS client
			client.ChannelFactory.Endpoint.Behaviors.Add(new AmazonSigningEndpointBehavior(accessKeyId, secretKey));

			ItemLookupRequest request2 = new ItemLookupRequest();
			request2.ItemId = new String[] { "2817801997" };
			request2.IdType = ItemLookupRequestIdType.ISBN;
			request2.ResponseGroup = new String[] { "Small" };
			request2.IdTypeSpecified = true;
			request2.SearchIndex = "Books";

			ItemLookup itemLookup = new ItemLookup();
			itemLookup.Request = new ItemLookupRequest[] { request2 };
			itemLookup.AWSAccessKeyId = accessKeyId;
			itemLookup.AssociateTag = "213";

			ItemLookupResponse response2 = client.ItemLookup(itemLookup);

			foreach (var item in response2.Items[0].Item) {
				Console.WriteLine(item.ItemAttributes.Title);
			}

			// prepare an ItemSearch request
			ItemSearchRequest request	= new ItemSearchRequest();
			request.SearchIndex			= "Books";
			request.Title				= "Node.js";
			request.ResponseGroup 		= new string[] { "Small" };

			ItemSearch itemSearch		= new ItemSearch();
			itemSearch.Request			= new ItemSearchRequest[] { request };
			//itemSearch.AssociateTag 	= "testsite09f-21";
			itemSearch.AssociateTag 	= "213";
			itemSearch.AWSAccessKeyId	= accessKeyId;

			// issue the ItemSearch request
			ItemSearchResponse response	= client.ItemSearch(itemSearch);

			// write out the results
			foreach (var item in response.Items[0].Item) {
				Console.WriteLine(item.ItemAttributes.Title);
			}
			Console.ReadLine();
		}
	}
}
