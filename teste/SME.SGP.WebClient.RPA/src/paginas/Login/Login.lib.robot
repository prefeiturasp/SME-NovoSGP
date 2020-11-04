*** Settings ***
Documentation       Lib de comandos para o fluxo de login

Library             SeleniumLibrary
Library             ScreenCapLibrary
Library             OperatingSystem

Resource            ../../configuracao/variaveis.robot

*** Variables ***
${LOGIN URL}                http://${SERVER}/login
${TEACHER_CALENDAR_URL}     http://${SERVER}/calendario-escolar/calendario-professor

${LOGIN BUTTON SELECTOR}    div.ant-spin-container>button
${MAIN SEARCH SELECTOR}     div#containerFiltro>form>div.form-group>input
${TEATCHER_CALENDAR_BREADCRUMB_SELECTOR}    span.ant-breadcrumb-link>a[href="/calendario-escolar/calendario-professor"]

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
