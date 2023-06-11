
using Newtonsoft.Json;

namespace TPEntry_Generator
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    TouchPortalEntry.Root myEntry = new TouchPortalEntry.Root();
    int SplitX = 1;
    int SplitY = 1;

    private void Form1_Load(object sender, EventArgs e)
    {

      myEntry.sdk = 4;
      myEntry.version = 1;
      myEntry.name = "MSFS Touch Portal Panel Grabber";
      myEntry.id = "MSFSPanelGrabberPlugin";
      myEntry.settings = new List<TouchPortalEntry.Settings>();
      myEntry.plugin_start_cmd = @"%TP_PLUGIN_FOLDER%MSFSTouchPortalPlugin-PanelGrabber/MSFSTouchPortalPlugin-PanelGrabber.exe";

      TouchPortalEntry.Category myCategory = new TouchPortalEntry.Category();

      myCategory.id = "MSFSPanelGrabberPlugin.Plugin";
      myCategory.name = "MSFS - PanelGrabber";

      List<TouchPortalEntry.Action> myActionList = new List<TouchPortalEntry.Action>();

      // Start
      TouchPortalEntry.Action initAction = new TouchPortalEntry.Action();
      initAction.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init";
      initAction.name = "MSFS - PanelGrabber - Init PanelGrabber";
      initAction.prefix = "Plugin";
      initAction.type = "communicate";
      initAction.tryInline = true;
      initAction.format = "Pos X {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosX$} " +
                          "Pos Y {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosY$} " +
                          "Width {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Width$} " +
                          "Height {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Height$} " +
                          "Grid X {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridX$} " +
                          "Grid Y {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridY$} " +
                          "Tile Size {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.TileSize$} " +
                          "Compression Level {$MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.CompressionLevel$} ";
      initAction.hasHoldFunctionality = true;

      // data
      List<TouchPortalEntry.Data> initDataList = new List<TouchPortalEntry.Data>();

      // Pos X
      TouchPortalEntry.Data initDataPosX = new TouchPortalEntry.Data();
      initDataPosX.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosX";
      initDataPosX.type = "number";
      initDataPosX.label = "Grab Pos X";
      initDataPosX.@default = "0";
      initDataPosX.minValue = "0";
      initDataPosX.maxValue = "9999";
      initDataPosX.allowDecimals = "false";
      initDataList.Add(initDataPosX);

      // PosY
      TouchPortalEntry.Data initDataPosY = new TouchPortalEntry.Data();
      initDataPosY.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosY";
      initDataPosY.type = "number";
      initDataPosY.label = "Grab Pos Y";
      initDataPosY.@default = "0";
      initDataPosY.minValue = "0";
      initDataPosY.maxValue = "9999";
      initDataPosY.allowDecimals = "false";
      initDataList.Add(initDataPosY);

      // Width
      TouchPortalEntry.Data initDataWidth= new TouchPortalEntry.Data();
      initDataWidth.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Width";
      initDataWidth.type = "number";
      initDataWidth.label = "Grab Width";
      initDataWidth.@default = "0";
      initDataWidth.minValue = "0";
      initDataWidth.maxValue = "9999";
      initDataWidth.allowDecimals = "false";
      initDataList.Add(initDataWidth);

      // height
      TouchPortalEntry.Data initDataHeight = new TouchPortalEntry.Data();
      initDataHeight.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Height";
      initDataHeight.type = "number";
      initDataHeight.label = "Grab Height";
      initDataHeight.@default = "0";
      initDataHeight.minValue = "0";
      initDataHeight.maxValue = "9999";
      initDataHeight.allowDecimals = "false";
      initDataList.Add(initDataHeight);


      // GridX
      TouchPortalEntry.Data initDataGridX = new TouchPortalEntry.Data();
      initDataGridX.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridX";
      initDataGridX.type = "number";
      initDataGridX.label = "Grid Size X";
      initDataGridX.@default = "1";
      initDataGridX.minValue = "1";
      initDataGridX.maxValue = "12";
      initDataGridX.allowDecimals = "false";
      initDataList.Add(initDataGridX);

      // GridY
      TouchPortalEntry.Data initDataGridY = new TouchPortalEntry.Data();
      initDataGridY.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridY";
      initDataGridY.type = "number";
      initDataGridY.label = "Grid Size Y";
      initDataGridY.@default = "1";
      initDataGridY.minValue = "1";
      initDataGridY.maxValue = "12";
      initDataGridY.allowDecimals = "false";
      initDataList.Add(initDataGridY);

      //TileSize
      TouchPortalEntry.Data initDataTileSize = new TouchPortalEntry.Data();
      initDataTileSize.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.TileSize";
      initDataTileSize.type = "number";
      initDataTileSize.label = "Tile Size";
      initDataTileSize.@default = "128";
      initDataTileSize.minValue = "1";
      initDataTileSize.maxValue = "1024";
      initDataTileSize.allowDecimals = "false";
      initDataList.Add(initDataTileSize);

      // CompressionLevel
      TouchPortalEntry.Data initDataCompressionLevel = new TouchPortalEntry.Data();
      initDataCompressionLevel.id = "MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.CompressionLevel";
      initDataCompressionLevel.type = "number";
      initDataCompressionLevel.label = "Compression Level";
      initDataCompressionLevel.@default = "100";
      initDataCompressionLevel.minValue = "1";
      initDataCompressionLevel.maxValue = "100";
      initDataCompressionLevel.allowDecimals = "false";
      initDataList.Add(initDataCompressionLevel);


      initAction.data = initDataList;
      myActionList.Add(initAction);

      // Grab
      TouchPortalEntry.Action grabAction = new TouchPortalEntry.Action();
      grabAction.id = "MSFSPanelGrabberPlugin.Plugin.Action.Grab";
      grabAction.name = "MSFS - PanelGrabber - Grab Frame";
      grabAction.prefix = "Plugin";
      grabAction.type = "communicate";
      grabAction.tryInline = true;
      grabAction.hasHoldFunctionality = true;
      myActionList.Add(grabAction);

      // Release
      TouchPortalEntry.Action releaseAction = new TouchPortalEntry.Action();
      releaseAction.id = "MSFSPanelGrabberPlugin.Plugin.Action.Release";
      releaseAction.name = "MSFS - PanelGrabber - Release Memony";
      releaseAction.prefix = "Plugin";
      releaseAction.type = "communicate";
      releaseAction.tryInline = true;
      releaseAction.hasHoldFunctionality = true;
      myActionList.Add(releaseAction);


      // Move Window
      TouchPortalEntry.Action moveAction = new TouchPortalEntry.Action();
      moveAction.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow";
      moveAction.name = "MSFS - PanelGrabber - Move Window";
      moveAction.prefix = "Plugin";
      moveAction.type = "communicate";
      moveAction.tryInline = true;
      moveAction.format = "Window Name {$MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.WindowName$} Position X {$MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosX$} Position Y {$MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosY$} Width {$MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.Width$} Height {$MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.height$}";
      moveAction.hasHoldFunctionality = true;


      List<TouchPortalEntry.Data> moveWindowDataList = new List<TouchPortalEntry.Data>();

      TouchPortalEntry.Data moveDataWindowName = new TouchPortalEntry.Data();
      moveDataWindowName.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.WindowName";
      moveDataWindowName.type = "text";
      moveDataWindowName.label = "Windows Name";
      moveDataWindowName.@default = "AS1000_PFD";
      moveWindowDataList.Add(moveDataWindowName);

      TouchPortalEntry.Data moveDataPosX = new TouchPortalEntry.Data();
      moveDataPosX.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosX";
      moveDataPosX.type = "number";
      moveDataPosX.label = "Window Pos X";
      moveDataPosX.@default = "0";
      moveDataPosX.minValue = "0";
      moveDataPosX.maxValue = "9999";
      moveDataPosX.allowDecimals = "false";
      moveWindowDataList.Add(moveDataPosX);

      TouchPortalEntry.Data moveDataPosY = new TouchPortalEntry.Data();
      moveDataPosY.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosY";
      moveDataPosY.type = "number";
      moveDataPosY.label = "Window Pos Y";
      moveDataPosY.@default = "0";
      moveDataPosY.minValue = "0";
      moveDataPosY.maxValue = "9999";
      moveDataPosY.allowDecimals = "false";
      moveWindowDataList.Add(moveDataPosY);


      TouchPortalEntry.Data moveDataWidth = new TouchPortalEntry.Data();
      moveDataWidth.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.Width";
      moveDataWidth.type = "number";
      moveDataWidth.label = "Window Width";
      moveDataWidth.@default = "50";
      moveDataWidth.minValue = "50";
      moveDataWidth.maxValue = "9999";
      moveDataWidth.allowDecimals = "false";
      moveWindowDataList.Add(moveDataWidth);

      TouchPortalEntry.Data moveDataHeight = new TouchPortalEntry.Data();
      moveDataHeight.id = "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.Height";
      moveDataHeight.type = "number";
      moveDataHeight.label = "Window height";
      moveDataHeight.@default = "50";
      moveDataHeight.minValue = "50";
      moveDataHeight.maxValue = "9999";
      moveDataHeight.allowDecimals = "false";
      moveWindowDataList.Add(moveDataHeight);

      moveAction.data = moveWindowDataList;


      myActionList.Add(moveAction);

      myCategory.actions = myActionList;

      // Events
      List<TouchPortalEntry.Event> myEventsList = new List<TouchPortalEntry.Event>();

      TouchPortalEntry.Event myEvent1 = new TouchPortalEntry.Event();
      myEvent1.id = "MSFSPanelGrabberPlugin.Event.NewFrame";
      myEvent1.name = "MSFS - PanelGrabber - New Frame Available";
      myEvent1.format = "New Frame Available: $val";
      myEvent1.type = "communicate";
      myEvent1.valueType = "choice";
      myEvent1.valueChoices = new List<string>() { "yes", "no" };
      myEvent1.valueStateId = "MSFSPanelGrabberPlugin.State.NewFrame";

      myEventsList.Add(myEvent1);
      myCategory.events = myEventsList;

      // States
      List<TouchPortalEntry.State> myStateList = new List<TouchPortalEntry.State>();

      TouchPortalEntry.State myState = new TouchPortalEntry.State();
      myState.id = "MSFSPanelGrabberPlugin.State.NewFrame";
      myState.type = "choice";
      myState.desc = "MSFS - PanelGrabber - New Frame Available";
      myState.@default = "not set";
      myState.valueChoices = new List<string>() { "yes", "no" };
      myStateList.Add(myState);



      for (int x = 0; x < SplitX; x++)
      {
        for (int y = 0; y < SplitY; y++)
        {
          TouchPortalEntry.State myState2 = new TouchPortalEntry.State();
          myState2.id = "MSFSPanelGrabberPlugin.State.Cell_" + x.ToString() + "_" + y.ToString();
          myState2.type = "text";
          myState2.desc = "MSFS - PanelGrabber - Cell [x,y]=[" + x.ToString() + "," + y.ToString() + "]";
          myState2.@default = "";
          myStateList.Add(myState2);
        }
      }

      myCategory.states = myStateList;

      myEntry.categories = new List<TouchPortalEntry.Category>() { myCategory };


    }

    private void button1_Click(object sender, EventArgs e)
    {
      string json = JsonConvert.SerializeObject(myEntry, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore
      });

      using (StreamWriter writer = new StreamWriter(tbFilename.Text))
      {
        writer.Write(json);
      }

      MessageBox.Show("finish");
    }
  }
}