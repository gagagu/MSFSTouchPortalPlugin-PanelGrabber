using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using MSFSTouchPortalPlugin_PanelGrabber;
using TouchPortalSDK;
using TouchPortalSDK.Interfaces;
using TouchPortalSDK.Messages.Events;
using TouchPortalSDK.Messages.Models;
using TouchPortalSDK.Messages.Models.Enums;

namespace MSFSTouchPortalPlugin.Services
{
  public class PanelGrabber : ITouchPortalEventHandler
  {
    public string PluginId => "MSFSPanelGrabberPlugin";

    private readonly ILogger<PanelGrabber> _logger;
    private readonly ITouchPortalClient _client;

    private IReadOnlyCollection<Setting> _settings;
    private ScreenGrabber _screenGrabber = null;

    public PanelGrabber(ITouchPortalClientFactory clientFactory,
                        ILogger<PanelGrabber> logger)
    {
      _logger = logger;
      _client = clientFactory.Create(this);
      _screenGrabber = new ScreenGrabber(logger, _client);
    }

    public void Run()
    {
      //Connect to Touch Portal:
      _client.Connect();

      //Start sending messages:
      SendMessages();
    }

    public void OnClosedEvent(string message)
    {
      _logger?.LogInformation("TouchPortal Disconnected.");

      //Optional force exits this plugin.
      Environment.Exit(0);
    }

    private void SendMessages()
    {
    }

    /// <summary>
    /// Information received when plugin is connected to Touch Portal.
    /// </summary>
    /// <param name="message"></param>
    public void OnInfoEvent(InfoEvent message)
    {
      _logger?.LogInformation($"[Info] VersionCode: '{message.TpVersionCode}', VersionString: '{message.TpVersionString}', SDK: '{message.SdkVersion}', PluginVersion: '{message.PluginVersion}', Status: '{message.Status}'");

      _settings = message.Settings;
      _logger?.LogInformation($"[Info] Settings: {JsonSerializer.Serialize(_settings)}");
    }

    /// <summary>
    /// User selected an item in a dropdown menu in the Touch Portal UI.
    /// </summary>
    /// <param name="message"></param>
    public void OnListChangedEvent(ListChangeEvent message)
    {
      _logger?.LogInformation($"[OnListChanged] {message.ListId}/{message.ActionId}/{message.InstanceId} '{message.Value}'");

    }

    /// <summary>
    /// A broadcast event was received.
    /// </summary>
    /// <param name="message"></param>
    public void OnBroadcastEvent(BroadcastEvent message)
    {
      //Use this to reapply all state... Some times if you update the state, and the page is not visible, it will not be reflected in the app.
      _logger?.LogInformation($"[Broadcast] Event: '{message.Event}', PageName: '{message.PageName}'");
    }

    /// <summary>
    /// Updated settings was received.
    /// </summary>
    /// <param name="message"></param>
    public void OnSettingsEvent(SettingsEvent message)
    {
      _settings = message.Values;
      _logger?.LogInformation($"[OnSettings] Settings: {JsonSerializer.Serialize(_settings)}");
    }

    /// <summary>
    /// User clicked an action.
    /// </summary>
    /// <param name="message"></param>
    public void OnActionEvent(ActionEvent message)
    {
      switch (message.ActionId)
      {
        case "MSFSPanelGrabberPlugin.Plugin.Action.Init":
          var initPosX = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosX"] ?? "<null>";
          var initPosY = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.PosY"] ?? "<null>";
          var initWidth = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Width"] ?? "<null>";
          var initHeight = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.Height"] ?? "<null>";
          var initGridX = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridX"] ?? "<null>";
          var initGridY = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.GridY"] ?? "<null>";
          var initTileSize = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.TileSize"] ?? "<null>";
          var initCompressionLevel = message["MSFSPanelGrabberPlugin.Plugin.Action.Init.Data.CompressionLevel"] ?? "<null>";

          _logger?.LogInformation($"[OnAction] Init Grabber with " +
                        $"PosX: '{initPosX}', " +
                        $"Posy: '{initPosY}', " +
                        $"Width: '{initWidth}', " +
                        $"Height: '{initHeight}'" +
                        $"GridX: '{initGridX}'" +
                        $"GridY: '{initGridY}'" +
                        $"TileSize: '{initTileSize}'" +
                        $"CompressionLevel: '{initCompressionLevel}'");
          _screenGrabber?.InitGrabber(initPosX, initPosY, initWidth, initHeight, initGridX, initGridY, initTileSize, initCompressionLevel);


          break;
        case "MSFSPanelGrabberPlugin.Plugin.Action.Grab":
          _logger?.LogInformation($"[OnAction] Grab Frame");
          _screenGrabber?.GrabFrame();
          break;
        case "MSFSPanelGrabberPlugin.Plugin.Action.Release":
          _logger?.LogInformation($"[OnAction] Release Memory");
          _screenGrabber?.Release();
          break;

        case "MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow":
          var moveWindowName = message["MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.WindowName"] ?? "<null>";
          var movePosX = message["MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosX"] ?? "<null>";
          var movePosY = message["MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.PosY"] ?? "<null>";
          var moveWidth = message["MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.Width"] ?? "<null>";
          var moveHeight = message["MSFSPanelGrabberPlugin.Plugin.Action.MoveWindow.Data.Height"] ?? "<null>";

          _logger?.LogInformation($"[OnAction] MoveWindow with WindowName: '{moveWindowName}', PosX: '{movePosX}', PosY: '{movePosY}',Width: '{moveWidth}',Height:'{moveHeight}'");
          _screenGrabber?.MoveWindow(moveWindowName, movePosX, movePosY, moveWidth, moveHeight);
          break;

        default:
          var dataArray = message.Data
              .Select(dataItem => $"\"{dataItem.Key}\":\"{dataItem.Value}\"")
              .ToArray();

          var dataString = string.Join(", ", dataArray);
          _logger?.LogInformation($"[OnAction] PressState: {message.GetPressState()}, ActionId: {message.ActionId}, Data: '{dataString}'");
          break;
      }
    }

    /// <summary>
    /// Here you can react on what the person clicked in the option.
    /// </summary>
    /// <param name="message"></param>
    public void OnNotificationOptionClickedEvent(NotificationOptionClickedEvent message)
    {
      _logger?.LogInformation($"[OnNotificationOptionClickedEvent] NotificationId: '{message.NotificationId}', OptionId: '{message.OptionId}'");


    }

    /// <summary>
    /// Event fired when the TP user moves a slider which uses one of this plugin's Connectors.
    /// This event is very similar to the OnActionEvent but with a `Value` attribute reflecting the slider's
    /// current position. Like actions, they may contain extra user-selected data.
    /// </summary>
    /// <param name="message"><see cref="ConnectorChangeEvent"/></param>
    public void OnConnecterChangeEvent(ConnectorChangeEvent message)
    {
      var dataArray = message.Data
          .Select(dataItem => $"\"{dataItem.Key}\":\"{dataItem.Value}\"")
          .ToArray();

      var dataString = string.Join(", ", dataArray);
      _logger?.LogInformation($"[OnConnecterChangeEvent] ConnectorId: '{message.ConnectorId}', Value: '{message.Value}', Data: '{dataString}'");
    }

    /// <summary>
    /// This event is generated when a TP user creates or modifies a slider which uses one of this plugin's Connectors.
    /// See the TP API documentation for usage details.
    /// </summary>
    /// <param name="message"><see cref="ShortConnectorIdNotificationEvent"/></param>
    public void OnShortConnectorIdNotificationEvent(ShortConnectorIdNotificationEvent message)
    {
      _logger?.LogInformation($"[OnShortConnectorIdNotificationEvent] ConnectorId: '{message.ConnectorId}', ShortID: '{message.ShortId}'");
    }

    /// <summary>
    /// The event was unknown and not handled, ex. a new version of TP is out with new features.
    /// </summary>
    /// <param name="jsonMessage"></param>
    public void OnUnhandledEvent(string jsonMessage)
    {
      _logger?.LogWarning($"Unhandled message: {jsonMessage}");
    }


  }
}
