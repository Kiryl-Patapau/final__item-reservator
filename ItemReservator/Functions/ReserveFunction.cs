using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ItemReservator.Models;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;

namespace ItemReservator.Functions;

public static class ReserveFunction
{
    [FunctionName("ReserveFunction")]
    public static async Task Run(
        [ServiceBusTrigger("sbq-reserving-items", Connection = "ServiceBusConnectionString")] string message,
        IBinder binder)
    {
        var order = JsonConvert.DeserializeObject<Order>(message);
        var serializedOrder = JsonConvert.SerializeObject(order);

        // Dynamic binding is used to avoid creating empty blobs in case of invalid order
        var blobAttribute = new BlobAttribute("reserved-items/{rand-guid}.json")
        {
            Access = FileAccess.Write,
            Connection = "ReservedItemsStorage"
        };
        var blob = binder.Bind<BlobClient>(blobAttribute);
        await blob.UploadAsync(new BinaryData(serializedOrder));
    }
}
