using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Eos.Blazor.Generator;
using Microsoft.AspNetCore.Components;
using Eos.Blazor.Generator.Attributes;
using Eos.Blazor.Generator.Components;
using System.Threading.Tasks;
using RestSharp;
using System.Runtime.Serialization;
using Eos.Nav.Common.Apps;
using Newtonsoft.Json.Converters;
using System.Linq;
using Eos.Blazor.Generator.Models;
using Eos.Bare.Client.Model;

namespace test.Data
{
  [AddToMenu("Apps", "/apps")]
  [Route("/apps")]
  public class ServicesApps : ListPage<ServiceAppPackage>
  {

    protected override Task OnInitializedAsync()
    {
      var client = new RestClient("http://br-labsdev2:9462/api/v2/services/Integration/apps/all");
      var res = client.Execute(new RestRequest());
      var test = new VisibleField<ServiceAppPackage>() { 
        Caption = "test",
        Getter = f => f.Name,
        Setter = (f, v) =>
        {
          f.Name = v as string;
        }
      };
      VisibleFields.Add(test);
      Data = JsonConvert.DeserializeObject<List<ServiceAppPackage>>(res.Content);
      return base.OnInitializedAsync();
    }
  }


}
