using System.Collections.Generic;
using Newtonsoft.Json;

namespace ItemReservator.Models;

public class Order
{
    [JsonProperty(Required = Required.Always)]
    public string CustomerEmail { get; set; }

    [JsonProperty(Required = Required.Always)]
    public IEnumerable<Item> Items { get; set; }
}
