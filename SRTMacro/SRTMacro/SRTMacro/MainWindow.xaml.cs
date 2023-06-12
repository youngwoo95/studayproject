using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace SRTMacro
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 파일열기
        /// </summary>
        OpenFileDialog openfile;

        /// <summary>
        /// Python 매크로 코드 실행 세팅
        /// </summary>
        ProcessStartInfo psi;

        /// <summary>
        /// Python 매크로 코드 불러오는 객체
        /// </summary>
        Process process;

        string pythonidle;
        string date;
        string startaddress;
        string endaddress;
        string starttime;
        string endtime;


        public MainWindow()
        {
            InitializeComponent();

            comboStartAddress.ItemsSource = Values.AddressList;
            comboStartTime.ItemsSource = Values.TimeList.Keys;

            comboStopAddress.ItemsSource = Values.AddressList;
            comboStopTime.ItemsSource = Values.TimeList.Keys;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists(Values.Path))
                {
                    using (StreamReader file = File.OpenText(Values.Path))
                    {
                        string str = file.ReadToEnd();

                        JObject json = JObject.Parse(str);

                        txtPythonPath.Text = (string)json["PYTHONPATH"].ToString();
                        txtId.Text = (string)json["ID"].ToString();
                        txtPassword.Password = (string)json["PW"].ToString();
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 경로검색 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                openfile = new OpenFileDialog();
                if (openfile.ShowDialog() == true)
                {
                    txtPythonPath.Text = openfile.FileName;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 세팅파일 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JObject json = new JObject();
                json.Add("PYTHONPATH", txtPythonPath.Text);
                json.Add("ID", txtId.Text);
                json.Add("PW", txtPassword.Password);

                if (!File.Exists(Values.Path))
                {
                    using (File.Create(Values.Path)) { }
                }
                File.WriteAllText(Values.Path, json.ToString());
                MessageBox.Show("저장완료", "알림");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 매크로 시작 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                date = dpTime.SelectedDate.Value.ToString("yyyyMMdd"); // 날짜
                startaddress = comboStartAddress.Text; // 출발지
                endaddress = comboStopAddress.Text; // 도착지
                starttime = Values.TimeList[comboStartTime.Text];
                endtime = Values.TimeList[comboStopTime.Text];
                pythonidle = txtPythonPath.Text;

                Console.WriteLine(date);
                Console.WriteLine(startaddress);
                Console.WriteLine(endaddress);
                Console.WriteLine(starttime);
                Console.WriteLine(endtime);

                if (!File.Exists(Values.MacroFile))
                {
                    using (File.Create(Values.MacroFile)) { }
                }
                Commons comm = new Commons();
                string pythoncontents = comm.PythonCodes(date, startaddress, endaddress, starttime, endtime, txtId.Text, txtPassword.Password);
                File.WriteAllText(Values.MacroFile, pythoncontents);

                Task.Run(() =>
                {
                    Work();
                });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 메인 로직
        /// </summary>
        /// <returns></returns>
        private async Task Work()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = pythonidle;
                psi.Arguments = Values.MacroFile;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                psi.Verb = "runas";
                psi.RedirectStandardError = true;

                var errors = String.Empty;
                var result = String.Empty;


                using (process = Process.Start(psi))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        while (!process.HasExited)
                        {
                            await Task.Run(() =>
                            {
                                string message = reader.ReadLine();
                                Console.WriteLine(message);

                                Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate ()
                                {
                                    string message = reader.ReadLine();
                                    Console.WriteLine(message);
                                }));
                            });
                        }
                        errors = process.StandardError.ReadToEnd();
                        result = process.StandardOutput.ReadToEnd();


                        Console.WriteLine(errors);
                        Console.WriteLine(result);
                    }
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message+"-"+ex.StackTrace);
            }
        }

        /// <summary>
        /// 매크로 정지 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("매크로를 정지하시겠습니까?", "알림", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (process is not null)
                    {
                        process.Kill();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
