using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using MSFSTouchPortalPlugin.Services;
using TouchPortalSDK.Interfaces;
using System.Threading;
using System.Timers;
using System.Diagnostics.Tracing;
using System.Reflection;

namespace MSFSTouchPortalPlugin_PanelGrabber
{

    public class ScreenGrabber
    {
        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window. 
        /// The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern Boolean GetWindowRect(IntPtr hWnd, ref Rectangle bounds);

        /// <summary>
        /// Retrieves a handle to the top-level window whose class name and window name match the specified strings. 
        /// This function does not search child windows. This function does not perform a case-sensitive search.
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up, or top-level window. 
        /// These windows are ordered according to their appearance on the screen. 
        /// The topmost window receives the highest rank and is the first window in the Z order.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        /// <summary>
        /// Retrieves the specified system metric or system configuration setting.
        /// </summary>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// The CreateRectRgn function creates a rectangular region.
        /// </summary>
        /// <param name="nLeftRect"></param>
        /// <param name="nTopRect"></param>
        /// <param name="nRightRect"></param>
        /// <param name="nBottomRect"></param> //
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        /// <summary>
        /// The PrintWindow function copies a visual window into the specified device context (DC), typically a printer DC
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hDC"></param>
        /// <param name="nFlags"></param> // Only the client area of the window is copied to hdcBlt. By default, the entire window is copied.
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        ///  The window region determines the area within the window where the system permits drawing. The system does not display any portion of a window that lies outside of the window region
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hRgn"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);


        /// <summary>
        /// SetWindowPos Parameters
        /// </summary>
        private const int SWP_ASYNCWINDOWPOS = 0x4000; //If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. 
        private const int SWP_DEFERERASE = 0x2000;    //Prevents generation of the WM_SYNCPAINT message.
        private const int SWP_DRAWFRAME = 0x0020;     //Draws a frame (defined in the window's class description) around the window.
        private const int SWP_FRAMECHANGED = 0x0020;    //Applies new frame styles set using the SetWindowLong function. 
        private const int SWP_HIDEWINDOW = 0x0080;      //Hides the window.
        private const int SWP_NOACTIVATE = 0x0010;    //Does not activate the window.
        private const int SWP_NOCOPYBITS = 0x0100;      //Discards the entire contents of the client area. 
        private const int SWP_NOMOVE = 0x0002;          //Retains the current position (ignores X and Y parameters).
        private const int SWP_NOOWNERZORDER = 0x0200;   //Does not change the owner window's position in the Z order.
        private const int SWP_NOREDRAW = 0x0008;      //Does not redraw changes. 
        private const int SWP_NOREPOSITION = 0x0200;    //Same as the SWP_NOOWNERZORDER flag.
        private const int SWP_NOSENDCHANGING = 0x0400;  //Prevents the window from receiving the WM_WINDOWPOSCHANGING message.
        private const int SWP_NOSIZE = 0x0001;        //Retains the current size (ignores the cx and cy parameters).
        private const int SWP_NOZORDER = 0x0004;      //Retains the current Z order (ignores the hWndInsertAfter parameter).
        private const int SWP_SHOWWINDOW = 0x0040;      //Displays the window.


        // Touch portal Logger
        private readonly ILogger<PanelGrabber> _logger;
        private readonly ITouchPortalClient _client;
 
        private const int _iTileSize = 128; // pixel
        private Graphics _gfxScreenshot = null;
      //  private Rectangle _bounds = new Rectangle();
        private Size _imageSize = new Size();
        private Bitmap _screenshot = null;
        private int _iSplitX = 0;
        private int _iSplitY = 0;
        private int _iPosX = 0;
        private int _iPosY = 0;

        /// <summary>
        /// Konstructor
        /// </summary>
        /// <param name="logger"></param>
        public ScreenGrabber(ILogger<PanelGrabber> logger, ITouchPortalClient client)
        {
            _logger = logger;
            _client = client;
        }


        /// <summary>
        /// Move Window to location
        /// </summary>
        /// <param name="windowName"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void MoveWindow(string windowName, string posX, string posY, string width, string height)
        {
            try
            {
                //check values
                if (string.IsNullOrEmpty(windowName))
                    return;

                if (string.IsNullOrEmpty(posX))
                    return;

                if (string.IsNullOrEmpty(posY))
                    return;

                if (string.IsNullOrEmpty(width))
                    return;

                if (string.IsNullOrEmpty(height))
                    return;


                //convert values
                int iPosX = 0;
                int iPosY = 0;
                int iWidth = 0;
                int iHeight = 0;

                try
                {
                    int.TryParse(posX, out iPosX);
                    int.TryParse(posY, out iPosY);
                    int.TryParse(width, out iWidth);
                    int.TryParse(height, out iHeight);
                }
                catch
                {
                    _logger?.LogError($"[MoveWindow] TryParse throws error");
                    return;
                }

                // Find window
                IntPtr myWindow = FindWindow(null, windowName);
                // found?
                if (myWindow == IntPtr.Zero)
                {
                    _logger?.LogError($"[MoveWindow] FindWindow returns null");
                    return;
                }
                //move
                SetWindowPos(myWindow, 0, iPosX, iPosY, iWidth, iHeight, SWP_NOZORDER | SWP_NOACTIVATE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on MoveWindow");
            }
        } // moveWindow



        /// <summary>
        /// Initialize an start the grabber timer
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="splitX"></param>
        /// <param name="splitY"></param>
        public void InitGrabber(string posX, string posY, string splitX, string splitY)
        {
            try
            {


                int iSplitX = 0;
                int iSplitY = 0;
                int iPosX = 0;
                int iPosY = 0;
     
                if (_gfxScreenshot != null)
                {
                    _gfxScreenshot = null;
                }


                //check values
                if (string.IsNullOrEmpty(splitX))
                {
                    _logger?.LogError($"[InitGrabber] SplitX is null");
                    return;
                }

                if (string.IsNullOrEmpty(splitY))
                {
                    _logger?.LogError($"[InitGrabber] SplitY is null");
                    return;
                }

                if (string.IsNullOrEmpty(posX))
                {
                    _logger?.LogError($"[InitGrabber] pos X is null");
                    return;
                }

                if (string.IsNullOrEmpty(posY))
                {
                    _logger?.LogError($"[InitGrabber] PosY is null");
                    return;
                }

             
                //convert values


                try
                {
                    int.TryParse(splitX, out iSplitX);
                    int.TryParse(splitY, out iSplitY);
                    int.TryParse(posX, out iPosX);
                    int.TryParse(posY, out iPosY);
                }
                catch
                {
                    _logger?.LogError($"[InitGrabber] TryParse throws error");
                    return;
                }

                if (iSplitX <= 0 || iSplitY <= 0)
                {
                    _logger?.LogError($"[InitGrabber] SplitX or SplitY is 0");
                    return;
                }

                _iSplitX = iSplitX;
                _iSplitY = iSplitY;
                _iPosX = iPosX;
                _iPosY = iPosY;

                _screenshot = new Bitmap(_iTileSize, _iTileSize, PixelFormat.Format32bppArgb);
                _gfxScreenshot = Graphics.FromImage(_screenshot);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on InitGrabber");
            }
        } // Init Grabber

        public void GrabFrame()
        {
            try
            {
                // reset event
                _client.StateUpdate("MSFSPanelGrabberPlugin.State.NewFrame", "no");

                if (_gfxScreenshot == null)
                {
                    return;
                }

                // tile image
                for (int y = 0; y < _iSplitY; y++)
                {
                    for (int x = 0; x < _iSplitX; x++)
                    {

                        _gfxScreenshot.CopyFromScreen(_iPosX + (x * _iTileSize), _iPosY + (y * _iTileSize), 0, 0, new Size(_iTileSize, _iTileSize), CopyPixelOperation.SourceCopy);
                        using (var ms = new MemoryStream())
                        {
                            _screenshot.Save(ms, ImageFormat.Jpeg);
                            var SigBase64 = Convert.ToBase64String(ms.GetBuffer()); // Get Base64
                            _client.StateUpdate("MSFSPanelGrabberPlugin.State.Cell_" + x.ToString() + "_" + y.ToString(), SigBase64);
                        }

                    } // x
                } // y


                _client.StateUpdate("MSFSPanelGrabberPlugin.State.NewFrame", "yes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Grab");
            }
        }

        public void Release()
        {
            try
            {
                if (_gfxScreenshot != null)
                {
                    _gfxScreenshot = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on Grab");
            }
        }

    } // class
} // namespace
