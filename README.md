# wows
![Python 2](https://img.shields.io/badge/Python-2.7-blue.svg)
![C#](https://img.shields.io/badge/C%23-.NET-purple.svg)
![](https://github.com/rapsealk/wows/workflows/Python%20application/badge.svg)

## 시작에 앞서..
* 필요한 소프트웨어를 설치한다.
    - World of Warships (NA) ([LINK](https://na.wargaming.net/en/games/wows))
* 게임 클라이언트
    - 해상도: 1920x1080 or 1280x720
    - 전체화면
    - 함선: II Chester (`U.S.A`, `Cruiser`)

## 설정
* Windows Forms 클라이언트를 [다운로드](https://github.com/rapsealk/WfGaming.NET/releases/tag/v0.1.0-alpha) 받습니다.
* 설치한 `Release.zip`을 압축 해제 후 `WfGaming.NET.exe`를 실행합니다.
* `모드 선택`에서 같은 폴더 안의 `Main.py`을 선택한 후 `설치` 버튼을 클릭합니다.
* `설치 완료` 메시지가 나타나면 게임을 실행한 후 `실행` 버튼을 클릭합니다.
* `Training Mode`에서 게임을 반복 진행합니다.
* 게임을 진행하면 적이 발견된 시점부터 게임이 종료될 때까지 데이터가 저장됩니다.
* 로그인한 계정의 [Google Drive](https://drive.google.com/)에 데이터를 업로드합니다.

## 게임 내 설정
* 인게임 환경에서 `ESC` 버튼을 눌러 메뉴에 들어간 후, `Settings` 항목을 클릭한다.
![Settings](https://github.com/0x0184/wows/blob/feat/log/resources/settings.png)
* 상단의 `Controls` 메뉴로 이동한 후, 두 가지 설정을 변경한다.
    - `Track the locked target` 옵션을 해제한다. [ ]
    - `Weapons` - `Fire` 버튼을 `LMB`에서 `Space`로 변경한다. `LMB` 버튼을 클릭한 후 스페이스바를 입력하면 된다.
    - 하단의 `Apply` 버튼을 클릭하여 변경사항을 적용한다.
![Settings_Controls](https://github.com/0x0184/wows/blob/feat/log/resources/settings_controls.png)

## 게임 진행 방법
* 상단의 모드 선택 버튼을 클릭한다. (기본 모드는 Co-op Battle이다.)
![Game mode](https://github.com/0x0184/wows/blob/feat/log/resources/01.png)
* 연습 게임을 생성하기 위하여 **Training**을 선택한다.
![Battle Type Training](https://github.com/0x0184/wows/blob/feat/log/resources/02.png)
* 하단의 `CREATE BATTLE` 버튼을 클릭한다.
![Create Battle](https://github.com/0x0184/wows/blob/feat/log/resources/03.png)
* `CREATE` 버튼을 클릭해서 기본 옵션대로 방을 생성한다.
![Create](https://github.com/0x0184/wows/blob/feat/log/resources/04.png)
* Bravo 팀의 `Add Player` 버튼을 클릭한 후, 순서대로 `Bot`->`U.S.A`->`Cruiser`->`II CHESTER`를 선택한다.
    - [x] Bot is moving
    - [x] Bot is armed
* 두 옵션을 체크한 후 `ADD` 버튼을 클릭하여 봇을 추가한다.
![Add Bot Player](https://github.com/0x0184/wows/blob/feat/log/resources/05.png)
* 봇이 추가되었음을 확인한 후, 자신의 전함도 `II CHESTER`로 설정되었음을 확인한다.
    - 원하는 전함은 더블클릭하여 선택할 수 있다.
* `READY` 버튼을 클릭한 후 상단의 `BATTLE!` 버튼을 클릭하여 게임에 입장한다.
![Ready and battle](https://github.com/0x0184/wows/blob/feat/log/resources/06.png)
* `Alpha`팀의 경우 맵의 11시 지역에서 시작하는데, 맵의 중앙지역으로 이동해서 적과 교전하면 된다.
* 데이터는 적이 발견된 시점부터 게임이 종료될 때까지 수집된다.
* 지원되는 입력은 다음과 같다.

|   종류   |   입 력   |
| -------- |:--------:|
| 속도 조절 | `w`, `s` |
|   회전   | `q`, `e` |
|   조준   | `Mouse`  |
|   발포   | `Space`  |

![In-Game](https://github.com/0x0184/wows/blob/feat/log/resources/08.png)
