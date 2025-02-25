using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;
using Forzza.controller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Quobject.EngineIoClientDotNet.Modules;

namespace Forzza
{
    public class ForzzaBet_CDP
    {
        
        Object lockerObj = new object();
        private string domain = Setting.Instance.domain;
        public static CookieContainer cookieContainer = null;
        private string html = string.Empty;
        protected WriteLogDelegate m_handlerWriteStatus;
        protected onWriteLogEvent m_handlerWriteLog;
        private Mainform m_mainForm;
        public List<HistoryList> Historylist = new List<HistoryList>();
        public string placeresult = string.Empty;




        public ForzzaBet_CDP(WriteLogDelegate WrittingLog, onWriteLogEvent onWriteLog, Mainform mainForm)
        {
            m_handlerWriteStatus = WrittingLog;
            m_handlerWriteLog = onWriteLog;
            m_mainForm = mainForm;
        }       


        public string getProxyLocation()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage resp = httpClient.GetAsync("http://lumtest.com/myip.json").Result;
                var strContent = resp.Content.ReadAsStringAsync().Result;
                dynamic json = JObject.Parse(strContent);
                return json["geo"]["region_name"].ToString() + " - " + json["country"].ToString();

            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog($"getProxyLocation exception {ex.StackTrace} {ex.Message}");
            }
            return "UNKNOWN";
        }

        public bool login()
        {
            if (CDPController.Instance._browserObj == null)
                CDPController.Instance.InitializeBrowser($"https://{domain}/");
            Thread.Sleep(10000);
            AutoItX.MouseClick("LEFT", 536, 365, 1, 0);
            WriteLog.WrittingLog("Login started.");
            bool isloggedin = false;
            try
            {
                
                Thread.Sleep(5000);
                CDPController.Instance.NavigateInvoke($"https://{domain}/en/live-sports-betting");
                Thread.Sleep(5000);
                int retryCount = 2;
                while (retryCount-- > 0)
                {

                    long documentId = CDPController.Instance.GetDocumentId().Result;
                    bool isFound = CDPController.Instance.FindAndClickElement(documentId, "div[id='unameDef']").Result;
                    Thread.Sleep(1000);
                    CDPMouseController.Instance.InputText(Setting.Instance.username);
                    isFound = CDPController.Instance.FindAndClickElement(documentId, "div[id='pwdDef']").Result;
                    Thread.Sleep(1000);
                    CDPMouseController.Instance.InputText(Setting.Instance.password);
                    Thread.Sleep(1000);
                    isFound = CDPController.Instance.FindAndClickElement(documentId, "input[id='btnLogIn']").Result;
                    Thread.Sleep(3000);

                    isFound = CDPController.Instance.FindElement(documentId, "input[id='btnLogOff']").Result;
                    if (isFound)
                    {
                        isloggedin = true;
                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog("In login side error:" + ex.Message);
                isloggedin = false;
            }


            return isloggedin;
        }

        public string getBalance()
        {
            string balance = "";
            try
            {
                string responseData_balance = "";
                string getbalanceURL = $"https://{domain}/client-check";
                JObject getBalanceJOB = new JObject
                {
                    ["headers"] = new JObject
                    {
                        ["accept"] = "application/json, text/javascript, */*; q=0.01",
                        ["accept-language"] = "en-US,en;q=0.9",
                        ["content-type"] = "application/json; charset=UTF-8",
                        ["priority"] = "u=0, i",
                        ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                        ["sec-ch-ua-arch"] = "\"x86\"",
                        ["sec-ch-ua-bitness"] = "\"64\"",
                        ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                        ["sec-ch-ua-mobile"] = "?0",
                        ["sec-ch-ua-model"] = "\"\"",
                        ["sec-ch-ua-platform"] = "\"Windows\"",
                        ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                        ["sec-fetch-dest"] = "empty",
                        ["sec-fetch-mode"] = "cors",
                        ["sec-fetch-site"] = "same-origin",
                        ["x-requested-with"] = "XMLHttpRequest"
                    },
                    ["referrer"] = $"https://{domain}/en/submitted-bets",
                    ["referrerPolicy"] = "strict-origin-when-cross-origin", 
                    ["body"] = "{\"args\":{\"action\":1,\"valParam\":\"\"}}",
                    ["method"] = "POST",
                    ["mode"] = "cors",
                    ["credentials"] = "include"
                };

                string functionString_GetBalance = $"var link = ''; fetch(\"{getbalanceURL}\", {getBalanceJOB}).then(res=>res.json()).then(json=>{{link = json}});";
                CDPController.Instance.ExecuteScript(functionString_GetBalance);
                int count2 = 0;
                while (count2 < 60)
                {
                    responseData_balance = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                    if (responseData_balance.Contains("payload"))
                        break;
                    Thread.Sleep(500);
                    count2++;
                }

                if (string.IsNullOrEmpty(responseData_balance))
                {
                    WriteLog.WrittingLog("getBalance no response");

                }
                dynamic jsonBalanceResp = JsonConvert.DeserializeObject<dynamic>(responseData_balance);
                balance = jsonBalanceResp["d"]?["payload"]?.ToString();


                return balance;
            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog($"getbalance error : {ex.Message}");
                return balance;
            }


        }
        

        public bool placeBet()
        {
            string responseData_placeBet = "";
            string placeBetURL = "";
            JObject PlacebetJOB = new JObject();
            string functionString_placebet = "";
            try
            {
                WriteLog.WrittingLog("Starting placebet..");
                GetToken();                                
                placeBetURL = $"https://{domain}/SportsbettingService.asmx/BetSlipHandler";
                PlacebetJOB = new JObject
                {
                    ["headers"] = new JObject
                    {
                        ["accept"] = "application/json, text/javascript, */*; q=0.01",
                        ["accept-language"] = "en-US,en;q=0.9",
                        ["content-type"] = "application/json; charset=UTF-8",
                        ["priority"] = "u=1, i",
                        ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                        ["sec-ch-ua-arch"] = "\"x86\"",
                        ["sec-ch-ua-bitness"] = "\"64\"",
                        ["sec-ch-ua-full-version"] = "\"133.0.6943.98\"",
                        ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                        ["sec-ch-ua-mobile"] = "?0",
                        ["sec-ch-ua-model"] = "\"\"",                        
                        ["sec-ch-ua-platform"] = "\"Windows\"",
                        ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                        ["sec-fetch-dest"] = "empty",
                        ["sec-fetch-mode"] = "cors",
                        ["sec-fetch-site"] = "same-origin",
                        ["token"] = $"{Setting.Instance.token}",
                        ["x-requested-with"] = "XMLHttpRequest"
                    },
                    ["referrer"] = $"https://{domain}/en/live-sports-betting",
                    ["referrerPolicy"] = "strict-origin-when-cross-origin",
                    ["body"] = $"{{\"args\":{{\"bid\":\"{Setting.Instance.marketID}\",\"mid\":\"{Setting.Instance.eventID}\",\"bmode\":0,\"sysID\":\"1\",\"action\":7,\"exclude\":false,\"calc\":{{\"amount\":1,\"type\":1}},\"sdat\":\"\",\"edat\":\"\"}}}}",
                    ["method"] = "POST",
                    ["mode"] = "cors",
                    ["credentials"] = "include"
                };

                functionString_placebet = $"var link = ''; fetch(\"{placeBetURL}\", {PlacebetJOB}).then(res=>res.json()).then(json=>{{link = json}});";
                CDPController.Instance.ExecuteScript(functionString_placebet);
                int count2 = 0;
                while (count2 < 60)
                {
                    responseData_placeBet = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                    if (responseData_placeBet.Contains("__type"))
                        break;
                    Thread.Sleep(100);
                    count2++;
                }

                if (string.IsNullOrEmpty(responseData_placeBet))
                {
                    WriteLog.WrittingLog("placebetStep1 no response");
                    return false;

                }
                dynamic jsonStep1Resp = JsonConvert.DeserializeObject<dynamic>(responseData_placeBet);
                string codestatus1 = jsonStep1Resp["d"]?["status"]?.ToString();
                if (codestatus1 == "200")
                {
                    placeBetURL = $"https://{domain}/SportsbettingService.asmx/BetSlipHandler";
                    PlacebetJOB = new JObject
                    {
                        ["headers"] = new JObject
                        {
                            ["accept"] = "application/json, text/javascript, */*; q=0.01",
                            ["accept-language"] = "en-US,en;q=0.9",
                            ["content-type"] = "application/json; charset=UTF-8",
                            ["priority"] = "u=1, i",
                            ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                            ["sec-ch-ua-arch"] = "\"x86\"",
                            ["sec-ch-ua-bitness"] = "\"64\"",
                            ["sec-ch-ua-full-version"] = "\"133.0.6943.98\"",
                            ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                            ["sec-ch-ua-mobile"] = "?0",
                            ["sec-ch-ua-model"] = "\"\"",
                            ["sec-ch-ua-platform"] = "\"Windows\"",
                            ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                            ["sec-fetch-dest"] = "empty",
                            ["sec-fetch-mode"] = "cors",
                            ["sec-fetch-site"] = "same-origin",
                            ["token"] = $"{Setting.Instance.token}",
                            ["x-requested-with"] = "XMLHttpRequest"
                        },
                        ["referrer"] = $"https://{domain}/en/live-sports-betting",
                        ["referrerPolicy"] = "strict-origin-when-cross-origin",
                        ["body"] = $"{{\"args\":{{\"bid\":\"\",\"mid\":\"\",\"bmode\":0,\"sysID\":\"1\",\"action\":6,\"exclude\":false,\"calc\":{{\"amount\":{Math.Round(Setting.Instance.stake, 2).ToString("F2")},\"type\":3}},\"sdat\":\"\",\"edat\":\"\"}}}}",
                        ["method"] = "POST",
                        ["mode"] = "cors",
                        ["credentials"] = "include"
                    };

                    functionString_placebet = $"var link = ''; fetch(\"{placeBetURL}\", {PlacebetJOB}).then(res=>res.json()).then(json=>{{link = json}});";
                    CDPController.Instance.ExecuteScript(functionString_placebet);
                    int count1 = 0;
                    while (count1 < 60)
                    {
                        responseData_placeBet = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                        if (responseData_placeBet.Contains("__type"))
                            break;
                        Thread.Sleep(100);
                        count1++;
                    }

                    if (string.IsNullOrEmpty(responseData_placeBet))
                    {
                        WriteLog.WrittingLog("placebetStep1 no response");
                        return false;

                    }
                    dynamic jsonStep2Resp = JsonConvert.DeserializeObject<dynamic>(responseData_placeBet);
                    string codestatus2 = jsonStep2Resp["d"]?["status"]?.ToString();

                    if (codestatus2 == "200")
                    {
                        placeBetURL = $"https://{domain}/SportsbettingService.asmx/BetSlipHandler";
                        PlacebetJOB = new JObject
                        {
                            ["headers"] = new JObject
                            {
                                ["accept"] = "application/json, text/javascript, */*; q=0.01",
                                ["accept-language"] = "en-US,en;q=0.9",
                                ["content-type"] = "application/json; charset=UTF-8",
                                ["priority"] = "u=1, i",
                                ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                                ["sec-ch-ua-arch"] = "\"x86\"",
                                ["sec-ch-ua-bitness"] = "\"64\"",
                                ["sec-ch-ua-full-version"] = "\"133.0.6943.98\"",
                                ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                                ["sec-ch-ua-mobile"] = "?0",
                                ["sec-ch-ua-model"] = "\"\"",
                                ["sec-ch-ua-platform"] = "\"Windows\"",
                                ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                                ["sec-fetch-dest"] = "empty",
                                ["sec-fetch-mode"] = "cors",
                                ["sec-fetch-site"] = "same-origin",
                                ["token"] = $"{Setting.Instance.token}",
                                ["x-requested-with"] = "XMLHttpRequest"
                            },
                            ["referrer"] = $"https://{domain}/en/live-sports-betting",
                            ["referrerPolicy"] = "strict-origin-when-cross-origin",
                            ["body"] = $"{{\"args\":{{\"bid\":\"{Setting.Instance.marketID}\",\"mid\":\"{Setting.Instance.eventID}\",\"bmode\":0,\"sysID\":\"1\",\"action\":9,\"exclude\":false,\"calc\":{{\"amount\":1,\"type\":1}},\"sdat\":\"\",\"edat\":\"\"}}}}",
                            ["method"] = "POST",
                            ["mode"] = "cors",
                            ["credentials"] = "include"
                        };

                        functionString_placebet = $"var link = ''; fetch(\"{placeBetURL}\", {PlacebetJOB}).then(res=>res.json()).then(json=>{{link = json}});";
                        CDPController.Instance.ExecuteScript(functionString_placebet);
                        int count = 0;
                        while (count < 100)
                        {
                            responseData_placeBet = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                            if (responseData_placeBet.Contains("__type"))
                                break;
                            Thread.Sleep(100);
                            count++;
                        }

                        if (string.IsNullOrEmpty(responseData_placeBet))
                        {
                            WriteLog.WrittingLog("placebetStep2 no response");
                            return false;
                        }
                        dynamic jsonStep3Resp = JsonConvert.DeserializeObject<dynamic>(responseData_placeBet);
                        placeresult = jsonStep3Resp["d"]?["infoMsg"]?["msg"]?.ToString();
                        if (placeresult == "Your bet slip has been successfully submitted")
                            return true;
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog($"placeBet error : {ex.Message}");
            }


            return false;
        }

        public bool getIDs(NaldotipList betItem)
        {
            try
            {
                WriteLog.WrittingLog("starting getIDs");
                string responseData_inplay = "";
                string getInplayURL = $"https://{domain}/live-sportsbook/in-play";
                JObject getinplayJOB = new JObject
                {
                    ["headers"] = new JObject
                    {
                        ["accept"] = "application/json, text/javascript, */*; q=0.01",
                        ["accept-language"] = "en-US,en;q=0.9",
                        ["content-type"] = "application/json; charset=UTF-8",
                        ["priority"] = "u=1, i",
                        ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                        ["sec-ch-ua-arch"] = "\"x86\"",
                        ["sec-ch-ua-bitness"] = "\"64\"",
                        ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                        ["sec-ch-ua-mobile"] = "?0",
                        ["sec-ch-ua-model"] = "\"\"",
                        ["sec-ch-ua-platform"] = "\"Windows\"",
                        ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                        ["sec-fetch-dest"] = "empty",
                        ["sec-fetch-mode"] = "cors",
                        ["sec-fetch-site"] = "same-origin",
                        ["token"] = $"{Setting.Instance.token}",
                        ["x-requested-with"] = "XMLHttpRequest"
                    },
                    ["referrer"] = $"https://{domain}/en/live-sports-betting",
                    ["referrerPolicy"] = "strict-origin-when-cross-origin",
                    ["body"] = "{}",
                    ["method"] = "POST",
                    ["mode"] = "cors",
                    ["credentials"] = "include"
                };

                string functionString_Getinplay = $"var link = ''; fetch(\"{getInplayURL}\", {getinplayJOB}).then(res=>res.json()).then(json=>{{link = json}});";
                CDPController.Instance.ExecuteScript(functionString_Getinplay);
                int count2 = 0;
                while (count2 < 60)
                {
                    responseData_inplay = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                    if (responseData_inplay.Contains("alm"))
                        break;
                    Thread.Sleep(100);
                    count2++;
                }

                if (string.IsNullOrEmpty(responseData_inplay))
                {
                    WriteLog.WrittingLog("GetIDS no response");
                    return false;

                }
                
                JObject parsedJson = JObject.Parse(responseData_inplay);

                // Get all "alm" elements
                var matches = parsedJson["d"]["alm"];

                // Create a list to store filtered results
                List<JObject> filteredBets = new List<JObject>();

                foreach (var match in matches)
                {
                    string matchId = match["id"].ToString();
                    string homeTeam = match["ht"].ToString();
                    string awayTeam = match["at"].ToString();

                    // Ensure "bt" is not null
                    if (match["bt"] != null)
                    {
                        // Filter "bt" elements with bt = 197 or 198
                        var filtered = match["bt"]
                            .Where(bet => bet["bt"] != null && (bet["bt"].ToString() == "197") || bet["bt"].ToString() == "198")
                            .Select(bet => new JObject
                            {
                                ["id"] = matchId,
                                ["ht"] = homeTeam,
                                ["at"] = awayTeam,
                                ["mt"] = bet["mt"],
                                ["bt"] = bet["bt"],
                                ["hc"] = bet["hc"],
                                ["bc"] = bet["bc"],
                                ["pm"] = bet["pm"] ?? null // Include "pm" only if it exists
                            });

                        // Add to result list
                        filteredBets.AddRange(filtered);
                    }
                }
                string filteredJson = JsonConvert.SerializeObject(filteredBets, Formatting.Indented);
                JArray filteredjarray = JArray.Parse(filteredJson);

                string naldoHome = betItem.Naldo_home;
                string naldoAway = betItem.Naldo_away;
                string eventid = "";
                int cntevents = filteredjarray.Count;
                for (int i = 0; i < cntevents; i++)
                {
                    var forzzaobj = (JObject)filteredjarray[i];
                    eventid = forzzaobj["id"].ToString();
                    double homeProximity = JaroWinklerDistance.proximity(forzzaobj["ht"].ToString(), naldoHome);
                    double awayProximity = JaroWinklerDistance.proximity(forzzaobj["at"].ToString(), naldoAway);
                    if ((homeProximity + awayProximity) / 2 >= 0.8)
                    {
                        if (betItem.Naldo_outcome.Contains("(") && betItem.Naldo_outcome.Contains("Menos de"))
                        {
                            string input = betItem.Naldo_outcome;
                            // Regex pattern to capture the decimal number
                            Match match = Regex.Match(input, @"\d+\.\d+");
                            string temgoal = match.Success ? match.Value : "Not Found";
                            string tempgoal = "";
                            if (temgoal.Contains(".5"))
                            {
                                tempgoal = temgoal;
                            } else
                            {
                                tempgoal = Convert.ToDouble(temgoal.Replace(".0", ".5")).ToString();
                            }
                            if (forzzaobj["bt"].ToString() == "197" && forzzaobj["hc"].ToString() == tempgoal)
                            {                                 
                                Setting.Instance.eventID = eventid;
                                //Setting.Instance.stake = Convert.ToDouble(betItem.Naldo_stake) * 0.554;
                                Setting.Instance.marketID = forzzaobj["bc"]
                                .FirstOrDefault(b => b["t"] != null && b["t"].ToString() == "-")?["id"]?.ToString();
                                HistoryList historyItem = new HistoryList();
                                historyItem.homeTeam = forzzaobj["ht"].ToString();
                                historyItem.awayTeam = forzzaobj["at"].ToString();
                                historyItem.stake = Setting.Instance.stake.ToString();
                                historyItem.outcome = betItem.Naldo_outcome;
                                historyItem.oddValue = forzzaobj["bc"]
                                .FirstOrDefault(b => b["t"] != null && b["t"].ToString() == "-")?["q"]?.ToString();
                                Historylist.Add(historyItem);                                
                                return true;
                            }
                        }
                        else if (betItem.Naldo_outcome.Contains(naldoHome) || betItem.Naldo_outcome.Contains(naldoAway))
                        {
                            string input = betItem.Naldo_outcome;
                            MatchCollection outcomematches = Regex.Matches(input, @"[+-]?\d+(?:\.\d+)?");
                            string tempAsian = outcomematches[outcomematches.Count - 1].Value;
                            string tempEuropen = "";
                            string tempteam = "";

                            if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "-2.0")
                            {
                                tempEuropen = "0:2";
                                tempteam = "1";
                            }
                            //if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "-1.5")
                            //{
                            //    tempEuropen = "0:2";
                            //    tempteam = "X";
                            //}
                            if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "-1.0")
                            {
                                tempEuropen = "0:1";
                                tempteam = "1";
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "-0.5")
                            //{
                            //    tempEuropen = "0:1";
                            //    tempteam = "X";
                            //}
                            if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "0.0")
                            {
                                WriteLog.WrittingLog("Cannot Bet for 0.0");
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "+0.5")
                            //{
                            //    tempEuropen = "1:0";
                            //    tempteam = "X";
                            //}
                            
                            if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "+1.0")
                            {
                                tempEuropen = "1:0";
                                tempteam = "1";
                            }
                            //if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "+1.5")
                            //{
                            //    tempEuropen = "2:0";
                            //    tempteam = "X";
                            //}
                            if (betItem.Naldo_outcome.ToString().Contains(naldoHome) && tempAsian == "+2")
                            {
                                tempEuropen = "2:0";
                                tempteam = "1";
                            }


                            //For Away Process

                            if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "-2.0")
                            {
                                tempEuropen = "2:0";
                                tempteam = "2";
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "-1.5")
                            //{
                            //    tempEuropen = "1:0";
                            //    tempteam = "X";
                            //}

                            if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "-1.0")
                            {
                                tempEuropen = "1:0";
                                tempteam = "2";
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "-0.5")
                            //{
                            //    tempEuropen = "1:0";
                            //    tempteam = "X";
                            //}
                            
                            if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "0.0")
                            {
                                WriteLog.WrittingLog("Cannot Bet for 0.0");
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "+0.5")
                            //{
                            //    tempEuropen = "0:1";
                            //    tempteam = "X";
                            //}
                            

                            if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "+1.0")
                            {
                                tempEuropen = "0:1";
                                tempteam = "2";
                            }

                            //if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "+1.5")
                            //{
                            //    tempEuropen = "0:2";
                            //    tempteam = "X";
                            //}

                            if (betItem.Naldo_outcome.ToString().Contains(naldoAway) && tempAsian == "+2")
                            {
                                tempEuropen = "0:2";
                                tempteam = "2";
                            }

                            if (forzzaobj["bt"].ToString() == "198" && forzzaobj["hc"].ToString() == tempEuropen)
                            {
                                Setting.Instance.eventID = eventid;
                                Setting.Instance.marketID = forzzaobj["bc"]
                                .FirstOrDefault(b => b["t"] != null && b["t"].ToString() == tempteam)?["id"]?.ToString();
                                HistoryList historyItem = new HistoryList();
                                historyItem.homeTeam = forzzaobj["ht"].ToString();
                                historyItem.awayTeam = forzzaobj["at"].ToString();
                                historyItem.stake = Setting.Instance.stake.ToString();
                                historyItem.outcome = betItem.Naldo_outcome;
                                historyItem.oddValue = forzzaobj["bc"]
                                .FirstOrDefault(b => b["t"] != null && b["t"].ToString() == tempteam)?["q"]?.ToString();
                                Historylist.Add(historyItem);
                                return true;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog($"getIDs error : {ex.Message}");
            }
            return false;
        }

        



        public void GetToken()
        {
            try
            {
                string responseData_inplay = "";
                string getinplayURL = $"https://{domain}/en/live-sports-betting";
                JObject getinplayJOB = new JObject
                {
                    ["headers"] = new JObject
                    {
                        ["accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7",
                        ["accept-language"] = "en-US,en;q=0.9",
                        ["cache-control"] = "max-age=0",
                        ["priority"] = "u=0, i",
                        ["sec-ch-ua"] = "\"Not(A:Brand\";v=\"99\", \"Google Chrome\";v=\"133\", \"Chromium\";v=\"133\"",
                        ["sec-ch-ua-arch"] = "\"x86\"",
                        ["sec-ch-ua-bitness"] = "\"64\"",
                        ["sec-ch-ua-full-version-list"] = "\"Not(A:Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"133.0.6943.98\", \"Chromium\";v=\"133.0.6943.98\"",
                        ["sec-ch-ua-mobile"] = "?0",
                        ["sec-ch-ua-model"] = "\"\"",
                        ["sec-ch-ua-platform"] = "\"Windows\"",
                        ["sec-ch-ua-platform-version"] = "\"10.0.0\"",
                        ["sec-fetch-dest"] = "document",
                        ["sec-fetch-mode"] = "navigate",
                        ["sec-fetch-site"] = "same-origin",
                        ["sec-fetch-user"] = "?1",
                        ["upgrade-insecure-requests"] = "1"
                    },
                    ["referrer"] = $"https://{domain}/en/submitted-bets",
                    ["referrerPolicy"] = "strict-origin-when-cross-origin",
                    ["body"] = null,
                    ["method"] = "GET",
                    ["mode"] = "cors",
                    ["credentials"] = "include"
                };

                string functionString_GetBalance = $"var link = ''; fetch(\"{getinplayURL}\", {getinplayJOB}).then(res=>res.text()).then(html=>{{link = html}});";
                CDPController.Instance.ExecuteScript(functionString_GetBalance);
                int count2 = 0;
                while (count2 < 60)
                {
                    responseData_inplay = CDPController.Instance.ExecuteScript("JSON.stringify(link)", true, true);
                    if (responseData_inplay.Contains("html"))
                        break;
                    Thread.Sleep(100);
                    count2++;
                }

                if (string.IsNullOrEmpty(responseData_inplay))
                {
                    WriteLog.WrittingLog("getinplayhtml no response");
                }

                dynamic jsonBalanceResp = JsonConvert.DeserializeObject<dynamic>(responseData_inplay);
                string html = jsonBalanceResp.ToString();
                string pattern = @"<input[^>]+id=['""]token['""][^>]+value=['""]([^'""]+)['""]";

                // Perform regex match
                Match match = Regex.Match(html, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    Setting.Instance.token = match.Groups[1].Value;
                    WriteLog.WrittingLog($"Token: {Setting.Instance.token}");

                }
            }
            catch (Exception ex)
            {
                WriteLog.WrittingLog($"GetToken function error: {ex.Message}");
            }
        }
    }
}
