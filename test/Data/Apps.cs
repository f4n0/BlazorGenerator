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

namespace test.Data
{
  [AddToMenu("Apps", "/apps")]
  [Route("/apps")]
  [DataContract]
  public class ServicesApps : ListPage
  {
    /// <summary>
    /// The app ID.
    /// </summary>
    /// <value>The app ID.</value>
    [DataMember(Name = "Id", EmitDefaultValue = false)]
    public Guid Id { get; set; }

    /// <summary>
    /// The app publisher.
    /// </summary>
    /// <value>The app publisher.</value>
    [DataMember(Name = "Publisher", EmitDefaultValue = false)]
    [Visible]
    public string Publisher { get; set; }

    /// <summary>
    /// The name of the app.
    /// </summary>
    /// <value>The name of the app.</value>
    [DataMember(Name = "Name", EmitDefaultValue = false)]
    [Visible]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets SyncState
    /// </summary>
    [DataMember(Name = "SyncState", EmitDefaultValue = false)]
    [Visible]
    public AppSyncState SyncState { get; set; }

    /// <summary>
    /// The app version.
    /// </summary>
    /// <value>The app version.</value>
    [DataMember(Name = "Version", EmitDefaultValue = false)]
    [Visible]
    public Version Version { get; set; }

    /// <summary>
    /// The data version of the app.
    /// </summary>
    /// <value>The data version of the app.</value>
    [DataMember(Name = "DataVersion", EmitDefaultValue = false)]
    [Visible]
    public Version DataVersion { get; set; }

    /// <summary>
    /// Indicates whether this app is installed (true) or published (false) or unknown (null).
    /// </summary>
    /// <value>Indicates whether this app is installed (true) or published (false) or unknown (null).</value>
    [DataMember(Name = "IsInstalled", EmitDefaultValue = false)]
    [Visible]
    public bool? IsInstalled { get; set; }

    public IAppPackage[] GetDependencies()
    {
      return Dependencies?.Cast<IAppPackage>().ToArray();
    }

    /// <summary>
    /// Gets or Sets Scope
    /// </summary>
    [DataMember(Name = "Scope", EmitDefaultValue = false)]
    public AppScope Scope { get; set; }

    /// <summary>
    /// Specifies whether this apps dependencies have been resolved.
    /// </summary>
    /// <value>Specifies whether this apps dependencies have been resolved.</value>
    [DataMember(Name = "DependenciesResolved", EmitDefaultValue = false)]
    public bool DependenciesResolved { get; private set; }

    /// <summary>
    /// The list of dependencies of this app.
    /// </summary>
    /// <value>The list of dependencies of this app.</value>
    [DataMember(Name = "Dependencies", EmitDefaultValue = false)]
    public List<object> Dependencies { get; private set; }

    protected override Task OnInitializedAsync()
    {
      var client = new RestClient("http://br-labsdev2:9462/api/v2/services/Integration/apps/all");
      var res = client.Execute(new RestRequest());
      Data = JsonConvert.DeserializeObject<List<ServicesApps>>(res.Content).Cast<dynamic>().ToList();
      return base.OnInitializedAsync();
    }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum AppSyncState
  {
    /// <summary>
    /// Enum Cleaned for value: Cleaned
    /// </summary>
    [EnumMember(Value = "Cleaned")]
    Cleaned = 1,
    /// <summary>
    /// Enum Removed for value: Removed
    /// </summary>
    [EnumMember(Value = "Removed")]
    Removed = 2,
    /// <summary>
    /// Enum Added for value: Added
    /// </summary>
    [EnumMember(Value = "Added")]
    Added = 3,
    /// <summary>
    /// Enum NotSynced for value: NotSynced
    /// </summary>
    [EnumMember(Value = "NotSynced")]
    NotSynced = 4,
    /// <summary>
    /// Enum Synced for value: Synced
    /// </summary>
    [EnumMember(Value = "Synced")]
    Synced = 5
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum AppScope
  {
    /// <summary>
    /// Enum Global for value: Global
    /// </summary>
    [EnumMember(Value = "Global")]
    Global = 1,
    /// <summary>
    /// Enum Tenant for value: Tenant
    /// </summary>
    [EnumMember(Value = "Tenant")]
    Tenant = 2
  }
}
