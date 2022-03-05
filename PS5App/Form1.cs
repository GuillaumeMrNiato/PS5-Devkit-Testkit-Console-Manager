using ProsperoTMLib;

namespace PS5App
{
    public partial class Form1 : Form
    {
        private static TMAPIClass PS5 = new TMAPIClass();
        private static ITarget Target = null;
        public static void Init()
        {
            PS5.CheckCompatibility(eCompileTimeVersion.COMPILE_TIME_VERSION);
        }
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        public static void AddTarget(string hostName)
        {
            PS5.AddTarget(hostName);
        }
        public static bool SelectTarget()
        {
            Target = PS5.GetTargetByHandle(1);
            if (Target == null)
                return false;
            else
                return true;
        }
        public static bool Connect()
        {
            if (Target == null)
            {
                return false;
            }
            else
            {
                if (Target.PowerState.DisplayPowerState.HasFlag(eDisplayPowerState.DISPLAY_POWER_STATE_ON))
                {
                    Target.TakeOwnership(true);
                    Target.RequestConnection(eConnectionLevel.CONNECTION_LEVEL_OWNER, true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static string GetSelectedTargetHostName()
        {
            return Target.HostName;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddTarget(textBox1.Text);
            if (SelectTarget())
            {
            }
            if (Connect())
            {
                toolStripStatusLabel2.ForeColor = Color.Green;
                toolStripStatusLabel2.Text = "Connected";
                MessageBox.Show("Console Connected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("An error as occured", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            label3.Text = Target.HostName;
            label4.Text = Target.ConnectionInfo.CurrentOwner;
            toolStripStatusLabel5.Text = Target.Attributes.DevkitType.ToString();
            toolStripStatusLabel5.ForeColor = Color.Green;
            toolStripStatusLabel8.Text = "" + Target.Attributes.SdkVersionString;
            toolStripStatusLabel8.ForeColor = Color.Green;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            textBox2.Text  = Target.GetFileServingRoot();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                Target.SetReleaseCheckMode(eReleaseCheckMode.RELEASE_CHECK_MODE_RELEASE_MODE);
            }
            if (comboBox1.SelectedIndex == 1)
            {
                Target.SetReleaseCheckMode(eReleaseCheckMode.RELEASE_CHECK_MODE_ASSIST_MODE);
            }
            if (comboBox1.SelectedIndex == 2)
            {
                Target.SetReleaseCheckMode(eReleaseCheckMode.RELEASE_CHECK_MODE_DEVELOPMENT_MODE);
            }
            Target.GetFileServingRoot();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Target.Reboot();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Target.RestMode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Target.PowerOff();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Init();
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
                Target.SetFileServingRoot(textBox2.Text);
            }
            MessageBox.Show("File Serving Root changed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}