using System;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace ThickClientCTF
{
    public partial class MainForm : Form
    {
        private const string FakeFlag = "FLAG{this_is_not_the_real_flag}";
        private readonly int[] _revealSequence = { 38, 38, 40, 40, 37, 39, 37, 39, 66, 65 };
        private int _seqPosition;

        internal bool ButtonWasRevealed { get; private set; }
        internal bool ButtonWasClicked { get; private set; }
        internal bool DecoyModeEnabled { get; private set; }
        internal int RuntimeSeed { get; }

        public MainForm()
        {
            RuntimeSeed = new Random(Guid.NewGuid().GetHashCode()).Next(100000, 999999);
            InitializeComponent();
            statusLabel.Text = "Reverse me. Or discover the reveal sequence.";
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var code = (int)e.KeyCode;
            if (code == _revealSequence[_seqPosition])
            {
                _seqPosition++;
                if (_seqPosition == _revealSequence.Length)
                {
                    RevealButton();
                }
            }
            else
            {
                _seqPosition = 0;
            }

            if (e.Control && e.Shift && e.KeyCode == Keys.D)
            {
                DecoyModeEnabled = true;
                statusLabel.Text = "Debug mode says: " + FakeFlag;
            }
        }

        private void RevealButton()
        {
            ButtonWasRevealed = true;
            hiddenButton.Visible = true;
            statusLabel.Text = "Hidden action unlocked.";
        }

        private void HiddenButton_Click(object sender, EventArgs e)
        {
            ButtonWasClicked = true;
            var reflected = typeof(MainForm).GetMethod("InvokeGuardedRequest", BindingFlags.NonPublic | BindingFlags.Instance);
            if (reflected == null)
            {
                statusLabel.Text = "Internal method not found.";
                return;
            }

            var result = reflected.Invoke(this, null) as string;
            statusLabel.Text = result ?? "Unexpected empty response.";
        }

        private string InvokeGuardedRequest()
        {
            if (!CheckAccess())
            {
                return "Access check failed. Nice try.";
            }

            if (!ButtonWasRevealed || !ButtonWasClicked)
            {
                return "UI state invalid. Trigger full flow.";
            }

            return RequestFlagFromServer();
        }

        private bool CheckAccess()
        {
            return false;
        }

        private string RequestFlagFromServer()
        {
            try
            {
                var tokenBuilder = new TokenBuilder(this);
                var token = tokenBuilder.BuildToken();
                var baseUrl = ConfigurationManager.AppSettings["FlagServerUrl"] ?? "http://127.0.0.1:5000/flag";
                var url = $"{baseUrl}?token={Uri.EscapeDataString(token)}";

                using (var client = new WebClient())
                {
                    return client.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                return "Request failed: " + ex.Message;
            }
        }
    }
}
