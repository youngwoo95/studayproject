def install_and_import(package,import_name="NULL"):
	if import_name == "NULL":
		import_name = package
	import importlib
	try:
		importlib.import_module(import_name)
	except ImportError:
		import pip
		pip.main(['install', package])
	finally:
		globals()[package] = importlib.import_module(import_name)
	return 0

#<--import library-->#
install_and_import("subprocess")
install_and_import("sys")
install_and_import("os")
install_and_import("time")
install_and_import("unicodedata")
install_and_import("requests")
install_and_import("rich")
install_and_import("selenium")
install_and_import("python-dateutil","dateutil.parser")
install_and_import("datetime")
install_and_import("signal")
install_and_import("webdriver-manager","webdriver_manager.chrome")
#<!--import library-->#

from dateutil.parser import parse
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.action_chains import ActionChains
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support.select import Select
from selenium.webdriver.support import expected_conditions as EC
from webdriver_manager.chrome import ChromeDriverManager
from rich.console import Console
console = Console()
driver = None


##################################################################################################################
RESERV_DATE = '20230618'

#<--USER DATA-->#

DPT_NAME = '부산'
ARV_NAME = '수서'
DPT_TIME_FROM = parse('16:00')
ARV_TIME_END = parse('20:00')

REFRESH_TICK = 3

USER_ID = '2398453169'
USER_PW = 'rladyddn!!95'
##################################################################################################################

def install_check():
	with console.status("Chrome Driver 설치 확인중..", spinner="dots3"):
		path = ChromeDriverManager().install()
	return path

def main():
	global FORDAY
	global driver
	options = webdriver.ChromeOptions()

	# 브라우저 윈도우 사이즈
	options.add_argument('--start-maximized')
	# 사람처럼 보이게 하는 옵션들
	options.add_argument("disable-gpu")   # 가속 사용 안함
	options.add_argument("lang=ko_KR")    # 가짜 플러그인 탑재
	options.add_argument('user-agent=Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36')  # user-agent 이름 설정

	#개발자모드 활성화
	#options.add_argument("--auto-open-devtools-for-tabs")

	#웹드라이버 로드
	driver = webdriver.Chrome(install_check(), options=options)

	#예약프로그램 시작
	console.log(f"[bold red][SRT 예약프로그램][/bold red] '{RESERV_DATE} {DPT_NAME} - {ARV_NAME}' 예약시작!")

	driver.get(url="https://etk.srail.kr/main.do") 

	#로그인
	with console.status("'로그인' 버튼 로딩중...", spinner="dots3"):
		WebDriverWait(driver, 60*60*5).until(EC.element_to_be_clickable((By.XPATH, '//*[@id="wrap"]/div[3]/div[1]/div/a[2]'))).click()
	console.log("'로그인' 버튼 선택 완료!")

	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="srchDvNm01"]'))).send_keys(USER_ID)
	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="hmpgPwdCphd01"]'))).send_keys(USER_PW)
	WebDriverWait(driver, 60*60*5).until(EC.element_to_be_clickable((By.XPATH, '//*[@id="login-form"]/fieldset/div[1]/div[1]/div[2]/div/div[2]/input'))).click()


	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="dptRsStnCd"]')))
	select = Select(driver.find_element(By.NAME, 'dptRsStnCd'))
	select.select_by_visible_text(DPT_NAME)

	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="arvRsStnCd"]')))
	select = Select(driver.find_element(By.NAME, 'arvRsStnCd'))
	select.select_by_visible_text(ARV_NAME)

	driver.execute_script("this.selectCalendarInfo()")

	iframe = WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="_LAYER_BODY_"]')))
	driver.switch_to.frame(iframe)
	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//body')))

	try:
		driver.execute_script(f"selectDateInfo('{RESERV_DATE}')")
	except:
		driver.switch_to.parent_frame()
		element = WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="search-form"]/fieldset/div[3]/div/input[1]')))
		console.log(element.get_attribute('value'))

	WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="dptTm"]')))
	select = Select(driver.find_element(By.NAME, 'dptTm'))
	select.select_by_visible_text(f"{DPT_TIME_FROM.strftime('%H')}시 이후")

	driver.execute_script("selectScheduleList(); return false;")
	time.sleep(1)

	result = False
	while True:
		elements = driver.find_elements(By.XPATH, f"//td[contains(text(), '직통')]")
		nEl = len(elements)

		if(nEl == 0):
			WebDriverWait(driver, 60*60*5).until(EC.presence_of_element_located((By.XPATH, '//*[@id="search_top_tag"]/input'))).click()
		else:
			for index in range(nEl):
				element = driver.find_element(By.XPATH, f'//*[@id="result-form"]/fieldset/div[6]/table/tbody/tr[{index+1}]')
				dpt_time = parse(element.find_element(By.XPATH, './td[4]/em').text)
				arv_time = parse(element.find_element(By.XPATH, './td[5]/em').text)
				str = f"{DPT_NAME}출발:{dpt_time.strftime('%H:%M')} {ARV_NAME}도착:{arv_time.strftime('%H:%M')}"
				if(arv_time > ARV_TIME_END):
					console.log(f"{str}...도착시간초과")
					continue

				el_link = element.find_element(By.XPATH, './td[7]/a')
				if(el_link.get_attribute('class') == "btn_small btn_burgundy_dark val_m wx90"):
					el_link.click()
					result = True
				console.log(f"{str}...{'[bold red]예약[/bold red]' if result else '매진'}")
				if(result == True): break
		if(result == True): break

		with console.status("새로고침 중...", spinner="dots3"):
			driver.refresh()
			time.sleep(REFRESH_TICK)
		console.log("[bold red]새로고침완료![/bold red]")


if __name__ == "__main__":
	try:
		main()
	except Exception as ex:
		print("오류가 발생 했습니다.", ex)