using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Logging;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace SamplePlugin
{

    public class Send
    {
        #region Imports
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, uint lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        #endregion

        public static void KeyPress(Keys key, int sleep = 100)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;

            IntPtr ffxiv = FindWindow("ffxiv", null);
            IntPtr editx = FindWindowEx(ffxiv, IntPtr.Zero, "FFXIVGAME", null);

            PostMessage(editx, WM_KEYDOWN, (int)key, 0x001F0001);
            Thread.Sleep(sleep);
            PostMessage(editx, WM_KEYUP, (int)key, 0xC01F0001);
        }
    }
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Paste Plugin";

        private const string commandName = "/paste";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        private Configuration Configuration { get; init; }
        private PluginUI PluginUi { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);
            this.PluginUi = new PluginUI(this.Configuration, goatImage);

            this.CommandManager.AddHandler(commandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            this.PluginUi.Dispose();
            this.CommandManager.RemoveHandler(commandName);
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            //this.PluginUi.Visible = true;
            String clipboardText = Clipboard.GetText();
            for (int i = 0; i < clipboardText.Length; i++)
            {
                Console.WriteLine(clipboardText[i]);
                switch (clipboardText[i])
                {
                    case '1':
                        Send.KeyPress(Keys.D1, 0);
                        break;

                    case '2':
                        Send.KeyPress(Keys.D2, 0);
                        break;

                    case '3':
                        Send.KeyPress(Keys.D3, 0);
                        break;

                    case '4':
                        Send.KeyPress(Keys.D4, 0);
                        break;

                    case '5':
                        Send.KeyPress(Keys.D5, 0);
                        break;

                    case '6':
                        Send.KeyPress(Keys.D6, 0);
                        break;

                    case '7':
                        Send.KeyPress(Keys.D7, 0);
                        break;

                    case '8':
                        Send.KeyPress(Keys.D8, 0);
                        break;

                    case '9':
                        Send.KeyPress(Keys.D9, 0);
                        break;

                    case '0':
                        Send.KeyPress(Keys.D0, 0);
                        break;

                    default:
                        PluginLog.LogDebug($"Found non-digit {clipboardText[i]}.");
                        break;
                }
            }
        }

        private void DrawUI()
        {
            this.PluginUi.Draw();
        }

        private void DrawConfigUI()
        {
            this.PluginUi.SettingsVisible = true;
        }
    }
}
