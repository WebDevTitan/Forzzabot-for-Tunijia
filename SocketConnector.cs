using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;


namespace Forzza
{
    public class SocketConnector
    {
        private Socket _socket = null;
        private onWriteLogEvent m_handlerWriteLog;
        private WriteLogDelegate m_handlerWriteStatus;
        private onSocketEvent m_socketEventHandler;
        private Thread m_pingThread = null;
        private string m_currentIP = "";
        public List<NaldotipList> tipList;
        private Mainform _mainForm;
        private ForzzaBet_CDP _forzzaBetCdp;
        Thread placebetThread = null;
        private Label labelBalance; // Add this line

        public SocketConnector(WriteLogDelegate onWriteStatus, onSocketEvent onSocket, Mainform mainForm)
        {
            m_handlerWriteStatus = onWriteStatus;
            m_socketEventHandler = onSocket;
            _mainForm = mainForm;
            _forzzaBetCdp = new ForzzaBet_CDP(onWriteStatus, m_handlerWriteLog, mainForm); // Fix this line
            labelBalance = new Label(); // Add this line
        }

        public void startListening()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Off(Socket.EVENT_CONNECT);
                    _socket.Off(Socket.EVENT_DISCONNECT);
                    _socket.Off(Socket.EVENT_ERROR);
                    m_pingThread.Abort();
                    _socket.Close();
                    _socket = null;
                }

                m_handlerWriteStatus("socket server : " + Setting.Instance.SeverUrl);
                _socket = IO.Socket(Setting.Instance.SeverUrl);

                #region Socket Connection
                _socket.On(Socket.EVENT_RECONNECT, () =>
                {
                    try
                    {

                    }
                    catch (Exception ex) { }

                    m_handlerWriteStatus("[Log] Socket Reconnected");
                });

                _socket.On(Socket.EVENT_DISCONNECT, (data) =>
                {
                    m_handlerWriteStatus("[Log] Socket Disconnected : " + data.ToString());
                });

                _socket.On(Socket.EVENT_RECONNECTING, () =>
                {
                    m_handlerWriteStatus("[Log] Socket Reconnectings");
                });

                _socket.On(Socket.EVENT_ERROR, (data) =>
                {
                    m_handlerWriteStatus("[Log] Socket Connect Error");
                });

                #endregion

                _socket.On(Socket.EVENT_CONNECT, () =>
                {
                    try
                    {
                        m_handlerWriteStatus("[Log] Socket Connected");                        
                    }
                    catch (Exception ex) { }

                });

                _socket.On("willow_bet", (data) =>
                {
                    try
                    {
                        tipList = new List<NaldotipList>();
                        string receivedContent = data.ToString();
                        dynamic payload = JsonConvert.DeserializeObject<dynamic>(receivedContent);
                        NaldotipList betItem = new NaldotipList();
                        string eventtile = payload.match;
                        string[] teams = eventtile.Split(new string[] { " v " }, StringSplitOptions.None);
                        betItem.Naldo_home = teams[0];
                        betItem.Naldo_away = teams[1];
                        betItem.Naldo_stake = payload.stake;
                        betItem.Naldo_odd = Convert.ToString(payload.odds);
                        betItem.Naldo_outcome = payload.pick;
                        tipList.Add(betItem);
                        _mainForm.displayScan(tipList);

                        if (!(betItem.Naldo_outcome.Contains("Menos de") && !betItem.Naldo_outcome.Contains("(")))
                        {
                            //placebetThread = new Thread(() => threadfunc(betItem));
                            //placebetThread.Start();
                            bool getid = _forzzaBetCdp.getIDs(betItem);
                            if (getid)
                            {
                                WriteLog.WrittingLog("getting id is succeeded.");
                                bool placebetresult = _forzzaBetCdp.placeBet();
                                if (placebetresult)
                                {
                                    WriteLog.WrittingLog("Placing bet is successed.");
                                    _mainForm.displayHistory(_forzzaBetCdp.Historylist);
                                    _mainForm.changeLabel();
                                } else
                                {
                                    WriteLog.WrittingLog($"{_forzzaBetCdp.placeresult}");

                                }                               

                                    
                                
                            }

                        }
                        else
                        {
                            WriteLog.WrittingLog("this is not totalgoal market.");
                        }


                    }
                    catch (Exception ex)
                    {
                        m_handlerWriteStatus("[Socket] Socket Connecter Error, Reason -> " + ex.Message);
                    }
                });



            }
            catch
            {

            }
        }

        //private void threadfunc(NaldotipList betItem)
        //{
        //    try
        //    {                
        //        bool getid = _forzzaBetCdp.getIDs(betItem);
        //        if (getid)
        //        {
        //            WriteLog.WrittingLog("getting id is succeeded.");
        //            bool placebetresult = _forzzaBetCdp.placeBet();
        //            if (placebetresult)

        //                _mainForm.displayHistory(_forzzaBetCdp.Historylist);
        //            string balance = _forzzaBetCdp.getBalance();
        //            _mainForm.Invoke((MethodInvoker)delegate
        //            {
        //                labelBalance.Text = "Balance : " + balance;
        //                WriteLog.WrittingLog($"Current Balance:" + balance);
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WrittingLog("in placebetthread error:" + ex.Message);
        //    }
        //}
    }
}
