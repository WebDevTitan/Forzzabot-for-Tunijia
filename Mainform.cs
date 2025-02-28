using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Forzza.controller;
using MySqlConnector;
using Quobject.EngineIoClientDotNet.Modules;

namespace Forzza
{
    public delegate void onWriteStatusEvent(string status);
    public delegate void onWriteLogEvent(string filename, string strLog);
    public delegate void onSocketEvent(string type);
    public partial class Mainform : Form
    {
        Thread threadForzza = null;
        private string prevMessage = string.Empty;
        private string location = string.Empty;        
        public event onWriteLogEvent onWriteLog;
        public event onWriteStatusEvent onWriteStatus;
        public event onSocketEvent onSocket;
        private SocketConnector _socketConnector = null;
        private bool isRunning = false;
        private List<NaldotipList> Feedlist = new List<NaldotipList>();
        private string connectionString = "server=localhost;port=3306;database=yourdb;user=root;password=;";
        MySqlConnection connection = null;
        public Mainform()
        {
            InitializeComponent();
            //this.onWriteStatus += WriteStatus;
            this.onWriteLog += LogToFile;
            WriteLog.WrittingLog += WriteStatus;
            LoadsettingInfo();
            initSet();            
        }

        private void WriteStatus(string status)
        {
            try
            {
                string curPath = Directory.GetCurrentDirectory();
                if (txtLogs.InvokeRequired)
                    txtLogs.Invoke(WriteLog.WrittingLog, status);
                else
                {
                    string logText = ((string.IsNullOrEmpty(txtLogs.Text) ? "" : "\r") + string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status));
                    if (txtLogs.Lines.Length > 3000)
                        txtLogs.Clear();

                    txtLogs.AppendText(logText);
                    txtLogs.ScrollToCaret();
                    prevMessage = status;
                }

            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog(ex.Message);
            }
        }

        private void LogToFile(string filename, string result)
        {
            try
            {
                string curPath = Directory.GetCurrentDirectory();
                if (string.IsNullOrEmpty(filename))
                    return;
                StreamWriter streamWriter = new StreamWriter((Stream)System.IO.File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
                if (!string.IsNullOrEmpty(result))
                    streamWriter.WriteLine(result);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canSave())
                    return;
                setValues();
                saveSettingInfo();               

                MessageBox.Show("Saved");
            }
            catch(Exception ex)
            {
                WriteLog.WrittingLog("Error in Save info process:" + ex.Message);
            }
            
        }

        private bool canSave()
        {
            try
            {
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    txtUsername.Focus();
                    MessageBox.Show("Please input username");
                    return false;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Focus();
                    MessageBox.Show("Please input password");
                    return false;
                }
                if (string.IsNullOrEmpty(txtDomain.Text))
                {
                    txtDomain.Focus();
                    MessageBox.Show("Please input domain");
                    return false;
                }
                if (string.IsNullOrEmpty(txtLicense.Text))
                {
                    txtLicense.Focus();
                    MessageBox.Show("Please input License");
                    return false;

                }
            }
            catch
            {
                return false;
            }
            
            return true;
        }
        private void setValues()
        {
            Setting.Instance.username = txtUsername.Text;
            Setting.Instance.password = txtPassword.Text;
            Setting.Instance.domain = txtDomain.Text;
            Setting.Instance.license = txtLicense.Text;           
            Setting.Instance.stake = Convert.ToDouble(txtStake.Text);
            Setting.Instance.SeverUrl = "http://95.179.244.192:5002/";
        }

        private void saveSettingInfo()
        {
            try
            {
                string text = string.Format("username={0}\r\npassword={1}\r\ndomain={2}\r\nstake={3}\r\nlicense={4}", Setting.Instance.username, Setting.Instance.password, Setting.Instance.domain, Setting.Instance.stake.ToString(), Setting.Instance.license);
                File.WriteAllText("setting.txt", text);

            }
            catch
            {

            }
        }
        private void initSet()
        {
            try
            {
                txtUsername.Text = Setting.Instance.username;
                txtPassword.Text = Setting.Instance.password;                
                txtDomain.Text = Setting.Instance.domain;
                txtStake.Text = Setting.Instance.stake.ToString();
                txtLicense.Text = Setting.Instance.license;
            }
            catch
            {

            }            
        }

        private void LoadsettingInfo()
        {

            try
            {
                if (File.Exists("setting.txt"))
                {
                    string[] lines = File.ReadAllLines("setting.txt");
                    foreach (string line in lines)
                    {
                        string[] values = line.Split('=');
                        if (values.Length == 2)
                        {
                            if (values[0] == "username")
                                Setting.Instance.username = values[1];
                            else if (values[0] == "password")
                                Setting.Instance.password = values[1];
                            else if (values[0] == "domain")
                                Setting.Instance.domain = values[1];
                            else if (values[0] == "stake")
                                Setting.Instance.stake = Convert.ToDouble(values[1]);
                            else if (values[0] == "license")
                                Setting.Instance.license = values[1];
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                bool canstart = Doconnect();
                if (canstart)
                {                                       
                    _socketConnector = new SocketConnector(WriteLog.WrittingLog, onSocket, this);
                    WriteLog.WrittingLog("Bot Starting...");                
                    threadForzza = new Thread(ThreadFunc);
                    threadForzza.Start();
                }
            }
            catch
            {

            }
        }

        public bool Doconnect()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("Connection successful!");
                if (!string.IsNullOrEmpty(Setting.Instance.license))
                {
                    string query = $"SELECT * FROM licenses WHERE BotAccountInfo = '{Setting.Instance.username}'";
                    //// Create a command object                    
                    //MySqlCommand command = new MySqlCommand(query, connection);

                    //return true;
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows == false)
                        {
                            WriteLog.WrittingLog("Your Account is not exist. Please Input Correct User Info");
                            return false;
                        }
                        while (reader.Read()) // Loop through results
                        {
                            string licenseKey = reader["LicenseKey"].ToString();
                            DateTime expiryDate = Convert.ToDateTime(reader["ExpiryDate"]);
                            DateTime issueDate = Convert.ToDateTime(reader["IssueDate"]);
                            string status = reader["Status"].ToString();
                            string username = reader["BotAccountInfo"].ToString();

                            if(licenseKey != Setting.Instance.license)
                            {
                                WriteLog.WrittingLog("Your license is invalid.");
                                return false;
                            }
                            if(username != Setting.Instance.username )
                            {
                                
                            }
                            if(expiryDate < DateTime.Now)
                            {
                                WriteLog.WrittingLog("Your license is expired.");
                                return false;
                            }
                            

                            Console.WriteLine($"License: {licenseKey}, Expiry: {expiryDate}");
                            
                        }
                    }
                }

            }
            catch
            {
                return false;
            }
            return true ;
        }

        
        
        
        public void displayScan(List<NaldotipList> LiveGame)
        {

            try
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        bindingTiplist.DataSource = LiveGame;
                        bindingTiplist.ResetBindings(false);
                        if (dataGridViewScan.DataSource == null)
                        {
                            dataGridViewScan.DataSource = bindingTiplist;
                        }
                        WriteLog.WrittingLog("Display updated ScanList.");
                    }

                    catch
                    {
                        WriteLog.WrittingLog("Error in display Scanlist.");
                    }

                }));

            }

            catch (Exception ex)
            {
                WriteLog.WrittingLog("Display ScanLists error" + ex.Message);
            }
        }

        public void displayHistory(List<HistoryList> LiveGame)
        {

            try
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        bindingHistory.DataSource = LiveGame;
                        bindingHistory.ResetBindings(false);
                        if (dataGridViewHistory.DataSource == null)
                        {
                            dataGridViewHistory.DataSource = bindingHistory;
                        }
                        WriteLog.WrittingLog("Display updated HistoryList.");
                    }

                    catch
                    {
                        WriteLog.WrittingLog("Error in display History.");
                    }

                }));

            }

            catch (Exception ex)
            {
                WriteLog.WrittingLog("Display ScanLists error" + ex.Message);
            }
        }

        private void ScheduleBrowserReload()
        {
            var timer = new System.Timers.Timer(10 * 60 * 1000); // 10 minutes in milliseconds
            timer.Elapsed += (sender, e) =>
            {
                CDPController.Instance.ReloadBrowser();
                WriteLog.WrittingLog("reload page");
                Thread.Sleep(4000);
            };
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void ThreadFunc()
        {
            
            try
            {
                
                ForzzaBet_CDP forzza = new ForzzaBet_CDP(WriteLog.WrittingLog, onWriteLog, this);
                location = forzza.getProxyLocation();
                WriteLog.WrittingLog("region : " + location);
                bool loginResult = forzza.login();                        
                if (loginResult)
                {
                    _socketConnector.startListening();
                    ScheduleBrowserReload();
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelUser.Text = "User : " + Setting.Instance.username;
                    });
                    WriteLog.WrittingLog("Login Success");
                    string balance = forzza.getBalance();
                    this.Invoke((MethodInvoker)delegate
                    {
                        labelBalance.Text = "Balance : " + balance;
                        WriteLog.WrittingLog($"Current Balance:" + balance);
                    });
                    
                }
                else
                {
                    WriteLog.WrittingLog("Login Failed");
                }
                        
                

            }
                catch (Exception ex)
                {
                    WriteLog.WrittingLog($"Error in mainthread : " + ex.Message);
                }
            
        }        

        public void changeLabel()
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {

                    ForzzaBet_CDP forzza = new ForzzaBet_CDP(WriteLog.WrittingLog, onWriteLog, this);
                    string balance = forzza.getBalance();
                    labelBalance.Text = "Balance : " + balance;
                    WriteLog.WrittingLog($"Current Balance:" + balance);
                });
            }
            catch
            {

            }
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            try
            {
                WriteLog.WrittingLog("The bot has been stopped!");

                try
                {
                    if (threadForzza != null) threadForzza.Abort();
                }
                catch
                {
                }
                //FakeUserAction.Intance.stopHumanThread();
            }
            catch
            {
            }
        }
    }
}
