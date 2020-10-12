*** Settings ***
Documentation       Robo de teste para validar a seguinte jornada do usuário:
...                 1. Logar
...                 2. Selecionar no filtro principal a turma 7A
...                 3. Acessar o Menu Calendário > Calendário Professor
...                 4. Selecionar o mês setembro > dia 18/09
...                 5. Clicar em Cadastrar Aula > Selecionar aula normal e cadastrar
...                 6. Selecionar o mês setembro > dia 18/09 > clicar em frequência
...                 7. Na tela nova, expandir a opção frequencia e clicar em salvar

Library             SeleniumLibrary
Library             ScreenCapLibrary
Library             OperatingSystem

*** Variables ***
${SERVER}                   dev-novosgp.sme.prefeitura.sp.gov.br
${BROWSER}                  Chrome
${DELAY}                    0
${VALID USER}               7944560
${VALID PASSWORD}           Sgp@1234

${LOGIN URL}                http://${SERVER}/login
${TEACHER_CALENDAR_URL}     http://${SERVER}/calendario-escolar/calendario-professor

${LOGIN BUTTON SELECTOR}    div.ant-spin-container>button
${MAIN SEARCH SELECTOR}     div#containerFiltro>form>div.form-group>input
${TEATCHER_CALENDAR_BREADCRUMB_SELECTOR}    span.ant-breadcrumb-link>a[href="/calendario-escolar/calendario-professor"]
${SEPTEMBER_MONTH_IN_CALENDAR}  //div[@role='button']/div[@class='w-100']/div[@class='w-100' and text()='Setembro']

*** Keywords ***
Open Browser To Login Page
    Open Browser    ${LOGIN URL}    ${BROWSER}
    Maximize Browser Window
    Set Selenium Speed    ${DELAY}

Login Into System
    Input Username  ${VALID USER}
    Input Password  ${VALID PASSWORD}
    Submit Credentials
    Wait Until Page Contains Element    css=${MAIN SEARCH SELECTOR}

Select Group 7A
    Input Main Search  7A

Go To Teatcher Calendar
    Go To           ${TEACHER_CALENDAR_URL}
    Wait Until Page Contains Element    css=${TEATCHER_CALENDAR_BREADCRUMB_SELECTOR}

Select September Month Into Teatcher Calendar
    Click Element   xpath=${SEPTEMBER_MONTH_IN_CALENDAR}
    Capture Page Screenshot

Input Username
    [Arguments]     ${username}
    Click Element   id=usuario
    Input Text      id=usuario    ${username}

Input Password
    [Arguments]     ${password}
    Click Element   id=senha
    Input Text      id=senha    ${password}

Input Main Search
    [Arguments]     ${searchText}
    Click Element   css=${MAIN SEARCH SELECTOR}
    Input Text      css=${MAIN SEARCH SELECTOR}   ${searchText}
    Press Keys      css=${MAIN SEARCH SELECTOR}   ENTER

Submit Credentials
    Click Button    css=${LOGIN BUTTON SELECTOR}

*** Test Cases ***
User Jorney
    Start Video Recording   monitor=2
    Open Browser To Login Page
    Login Into System
    Select Group 7A
    Go To Teatcher Calendar
    Select September Month Into Teatcher Calendar
    [Teardown]  Close Browser
    [Teardown]  Stop Video Recording
