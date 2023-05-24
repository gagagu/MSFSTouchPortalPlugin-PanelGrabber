using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPEntry_Generator
{
  public class TouchPortalEntry
  {
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Action
    {
      public string id { get; set; }
      public string name { get; set; }
      public string prefix { get; set; }
      public string type { get; set; }
      public bool tryInline { get; set; }
      public string format { get; set; }
      public bool hasHoldFunctionality { get; set; }
      public List<Data> data { get; set; }
    }

    public class Category
    {
      public string id { get; set; }
      public string name { get; set; }
      public List<Action> actions { get; set; }
      public List<Event> events { get; set; }
      public List<State> states { get; set; }
    }

    public class Data
    {
      public string id { get; set; }
      public string type { get; set; }
      public string label { get; set; }
      public string @default { get; set; }
      public string minValue { get; set; }
      public string maxValue { get; set; }
      public string allowDecimals { get; set; }
    }

    public class Event
    {
      public string id { get; set; }
      public string name { get; set; }
      public string format { get; set; }
      public string type { get; set; }
      public string valueType { get; set; }
      public List<string> valueChoices { get; set; }
      public string valueStateId { get; set; }
    }

    public class Root
    {
      public int sdk { get; set; }
      public int version { get; set; }
      public string name { get; set; }
      public string id { get; set; }
      public List<Settings> settings { get; set; }
      public string plugin_start_cmd { get; set; }
      public List<Category> categories { get; set; }
    }

    public class State
    {
      public string id { get; set; }
      public string type { get; set; }
      public string desc { get; set; }
      public string @default { get; set; }
      public List<string> valueChoices { get; set; }
    }

    public class Settings
    {
      public string name { get; set; }
      public string defaut { get; set; }
      public string type { get; set; }
      public int maxLength { get; set; }

      public bool isPassword { get; set; }

      public int minValue { get; set; }
      public int maxValue { get; set; }
      public bool readOnly { get; set; }

    }
  }
}
