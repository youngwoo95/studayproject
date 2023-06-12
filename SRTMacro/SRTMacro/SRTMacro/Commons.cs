using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SRTMacro
{
    internal class Commons
    {
        public string PythonCodes(string date,string startaddress, string stopaddress, string starttime, string endtime, string id, string pw)
        {
            string codes = $"def install_and_import(package,import_name=\"NULL\"):\r\n" +
                $"\tif import_name == \"NULL\":\r\n" +
                $"\t\timport_name = package\r\n" +
                $"\timport importlib\r\n" +
                $"\ttry:\r\n" +
                $"\t\timportlib.import_module(import_name)\r\n" +
                $"\texcept ImportError:\r\n" +
                $"\t\timport pip\r\n" +
                $"\t\tpip.main(['install', package])\r\n" +
                $"\tfinally:\r\n" +
                $"\t\tglobals()[package] = importlib.import_module(import_name)\r\n" +
                $"\treturn 0\r\n" +
                $"\r\n" +
                $"#<--import library-->#\r\n" +
                $"install_and_import(\"subprocess\")\r\n" +
                $"install_and_import(\"sys\")\r\n" +
                $"install_and_import(\"os\")\r\n" +
                $"install_and_import(\"time\")\r\n" +
                $"install_and_import(\"unicodedata\")\r\n" +
                $"install_and_import(\"requests\")\r\n" +
                $"install_and_import(\"rich\")\r\n" +
                $"install_and_import(\"selenium\")\r\n" +
                $"install_and_import(\"python-dateutil\",\"dateutil.parser\")\r\n" +
                $"install_and_import(\"datetime\")\r\n" +
                $"install_and_import(\"signal\")\r\n" +
                $"install_and_import(\"webdriver-manager\",\"webdriver_manager.chrome\")\r\n" +
                $"#<!--import library-->#\r\n" +
                $"\r\n" +
                $"from dateutil.parser import parse\r\n" +
                $"from selenium import webdriver\r\n" +
                $"from selenium.webdriver.common.by import By\r\n" +
                $"from selenium.webdriver.common.action_chains import ActionChains\r\n" +
                $"from selenium.webdriver.support.ui import WebDriverWait\r\n" +
                $"from selenium.webdriver.support.select import Select\r\n" +
                $"from selenium.webdriver.support import expected_conditions as EC\r\n" +
                $"from webdriver_manager.chrome import ChromeDriverManager\r\n" +
                $"from rich.console import Console\r\n" +
                $"console = Console()\r\n" +
                $"driver = None\r\n" +
                $"\r\n" +
                $"\r\n" +
                $"##################################################################################################################\r\n" +
                $"RESERV_DATE = '{date}'\r\n" +
                $"\r\n" +
                $"#<--USER DATA-->#\r\n" +
                $"\r\n" +
                $"DPT_NAME = '{startaddress}'\r\n" +
                $"ARV_NAME = '{stopaddress}'\r\n" +
                $"DPT_TIME_FROM = parse('{starttime}')\r\n" +
                $"ARV_TIME_END = parse('{endtime}')\r\n" +
                $"\r\n" +
                $"REFRESH_TICK = 3\r\n" +
                $"\r\n" +
                $"USER_ID = '{id}'\r\n" +
                $"USER_PW = '{pw}'\r\n" +
                $"##################################################################################################################\r\n" +
                $"\r\n" +
                $"def install_check():\r\n" +
                $"\twith console.status(\"Chrome Driver 설치 확인중..\", spinner=\"dots3\"):\r\n" +
                $"\t\tpath = ChromeDriverManager().install()\r\n" +
                $"\treturn path\r\n" +
                $"\r\n" +
                $"def main():\r\n" +
                $"\tglobal FORDAY\r\n" +
                $"\tglobal driver\r\n" +
                $"\toptions = webdriver.ChromeOptions()\r\n" +
                $"\r\n" +
                $"\t# 브라우저 윈도우 사이즈\r\n" +
                $"\toptions.add_argument('--start-maximized')\r\n" +
                $"\t# 사람처럼 보이게 하는 옵션들\r\n" +
                $"\toptions.add_argument(\"disable-gpu\")   # 가속 사용 안함\r\n" +
                $"\toptions.add_argument(\"lang=ko_KR\")    # 가짜 플러그인 탑재\r\n" +
                $"\toptions.add_argument('user-agent=Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36')  # user-agent 이름 설정\r\n" +
                $"\r\n" +
                $"\t#개발자모드 활성화\r\n" +
                $"\t#options.add_argument(\"--auto-open-devtools-for-tabs\")\r\n" +
                $"\r\n" +
                $"\t#웹드라이버 로드\r\n" +
                $"\tdriver = webdriver.Chrome(install_check(), options=options)\r\n" +
                $"\r\n" +
                $"\t#예약프로그램 시작\r\n" +
                $"\tconsole.log(f\"[bold red][SRT 예약프로그램][/bold red] '{{RESERV_DATE}} {{DPT_NAME}} - {{ARV_NAME}}' 예약시작!\")\r\n" +
                $"\r\n" +
                $"\tdriver.get(url=\"https://etk.srail.kr/main.do\") \r\n" +
                $"\r\n" +
                $"\t#로그인\r\n" +
                $"\twith console.status(\"'로그인' 버튼 로딩중...\", spinner=\"dots3\"):\r\n" +
                $"\t\tWebDriverWait(driver, 60*60*5).until(EC.element_to_be_clickable((By.XPATH, '//*[@id=\"wrap\"]/div[3]/div[1]/div/a[2]'))).click()\r\n" +
                $"\tconsole.log(\"'로그인' 버튼 선택 완료!\")\r\n" +
                $"\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"srchDvNm01\"]'))).send_keys(USER_ID)\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"hmpgPwdCphd01\"]'))).send_keys(USER_PW)\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.element_to_be_clickable((By.XPATH, '//*[@id=\"login-form\"]/fieldset/div[1]/div[1]/div[2]/div/div[2]/input'))).click()\r\n" +
                $"\r\n" +
                $"\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"dptRsStnCd\"]')))\r\n" +
                $"\tselect = Select(driver.find_element(By.NAME, 'dptRsStnCd'))\r\n" +
                $"\tselect.select_by_visible_text(DPT_NAME)\r\n" +
                $"\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"arvRsStnCd\"]')))\r\n" +
                $"\tselect = Select(driver.find_element(By.NAME, 'arvRsStnCd'))\r\n" +
                $"\tselect.select_by_visible_text(ARV_NAME)\r\n" +
                $"\r\n" +
                $"\tdriver.execute_script(\"this.selectCalendarInfo()\")\r\n" +
                $"\r\n" +
                $"\tiframe = WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"_LAYER_BODY_\"]')))\r\n" +
                $"\tdriver.switch_to.frame(iframe)\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//body')))\r\n" +
                $"\r\n" +
                $"\ttry:\r\n" +
                $"\t\tdriver.execute_script(f\"selectDateInfo('{{RESERV_DATE}}')\")\r\n" +
                $"\texcept:\r\n" +
                $"\t\tdriver.switch_to.parent_frame()\r\n" +
                $"\t\telement = WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"search-form\"]/fieldset/div[3]/div/input[1]')))\r\n" +
                $"\t\tconsole.log(element.get_attribute('value'))\r\n" +
                $"\r\n" +
                $"\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"dptTm\"]')))\r\n" +
                $"\tselect = Select(driver.find_element(By.NAME, 'dptTm'))\r\n" +
                $"\tselect.select_by_visible_text(f\"{{DPT_TIME_FROM.strftime('%H')}}시 이후\")\r\n" +
                $"\r\n" +
                $"\tdriver.execute_script(\"selectScheduleList(); return false;\")\r\n" +
                $"\ttime.sleep(1)\r\n" +
                $"\r\n" +
                $"\tresult = False\r\n" +
                $"\twhile True:\r\n" +
                $"\t\telements = driver.find_elements(By.XPATH, f\"//td[contains(text(), '직통')]\")\r\n" +
                $"\t\tnEl = len(elements)\r\n" +
                $"\r\n" +
                $"\t\tif(nEl == 0):\r\n" +
                $"\t\t\tWebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id=\"search_top_tag\"]/input'))).click()\r\n" +
                $"\t\telse:\r\n" +
                $"\t\t\tfor index in range(nEl):\r\n" +
                $"\t\t\t\telement = driver.find_element(By.XPATH, f'//*[@id=\"result-form\"]/fieldset/div[6]/table/tbody/tr[{{index+1}}]')\r\n" +
                $"\t\t\t\tdpt_time = parse(element.find_element(By.XPATH, './td[4]/em').text)\r\n" +
                $"\t\t\t\tarv_time = parse(element.find_element(By.XPATH, './td[5]/em').text)\r\n" +
                $"\t\t\t\tstr = f\"{{DPT_NAME}}출발:{{dpt_time.strftime('%H:%M')}} {{ARV_NAME}}도착:{{arv_time.strftime('%H:%M')}}\"\r\n" +
                $"\t\t\t\tif(arv_time > ARV_TIME_END):\r\n" +
                $"\t\t\t\t\tconsole.log(f\"{{str}}...도착시간초과\")\r\n" +
                $"\t\t\t\t\tcontinue\r\n" +
                $"\r\n" +
                $"\t\t\t\tel_link = element.find_element(By.XPATH, './td[7]/a')\r\n" +
                $"\t\t\t\tif(el_link.get_attribute('class') == \"btn_small btn_burgundy_dark val_m wx90\"):\r\n" +
                $"\t\t\t\t\tel_link.click()\r\n" +
                $"\t\t\t\t\tresult = True\r\n" +
                $"\t\t\t\tconsole.log(f\"{{str}}...{{'[bold red]예약[/bold red]' if result else '매진'}}\")\r\n" +
                $"\t\t\t\tif(result == True): break\r\n" +
                $"\t\tif(result == True): break\r\n" +
                $"\r\n" +
                $"\t\twith console.status(\"새로고침 중...\", spinner=\"dots3\"):\r\n" +
                $"\t\t\tdriver.refresh()\r\n" +
                $"\t\t\ttime.sleep(REFRESH_TICK)\r\n" +
                $"\t\tconsole.log(\"[bold red]새로고침완료![/bold red]\")\r\n" +
                $"\r\n" +
                $"\r\n" +
                $"if __name__ == \"__main__\":\r\n" +
                $"\ttry:\r\n" +
                $"\t\tmain()\r\n" +
                $"\texcept Exception as ex:\r\n" +
                $"\t\tprint(\"오류가 발생 했습니다.\", ex)";

            Console.WriteLine(codes);
            return codes;
        }

    }
}
