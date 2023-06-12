using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRTMacro
{
    public class Values
    {
        /// <summary>
        /// 지역이름 리스트
        /// </summary>
        public static List<string> AddressList = new List<string>()
        {
            "수서",
            "동탄",
            "평택지제",
            "천안아산",
            "오송",
            "대전",
            "김천(구미)",
            "서대구",
            "동대구",
            "신경주",
            "울산(통도사)",
            "부산",
            "공주",
            "익산",
            "정읍",
            "광주송정",
            "나주",
            "목포",
        };

        /// <summary>
        /// 시간 리스트
        /// </summary>
        public static Dictionary<string,string> TimeList = new Dictionary<string,string>()
        {
            {"00시","00:00"},
            {"02시","02:00"},
            {"04시","04:00"},
            {"06시","06:00"},
            {"08시","08:00"},
            {"10시","10:00"},
            {"12시","12:00"},
            {"14시","14:00"},
            {"16시","16:00"},
            {"18시","18:00"},
            {"20시","20:00"},
            {"22시","22:00"}
        };

        /// <summary>
        /// APPDATA 경로
        /// </summary>
        //public static string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string AppData = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// JSON 파일 경로
        /// </summary>
        public static string Path = String.Format(@"{0}\\SRT.json", AppData);


        public static string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /// <summary>
        /// 매크로 파일 경로
        /// </summary>
        public static string MacroFile = String.Format(@"{0}\\SRTPYTHON.py", FilePath);
    }
}
