*** Settings ***
Documentation       Robo de teste para validar a seguinte jornada do usuário:
...                 1. Logar
...                 2. Selecionar no filtro principal a turma 7A
...                 3. Acessar o Menu Calendário > Calendário Professor

Resource            Login.lib.robot

*** Keywords ***
Teardown
    Close Browser
    Stop Video Recording

*** Test Cases ***
User Jorney
    Start Video Recording   monitor=1
    Open Browser To Login Page
    Login Into System
    Select Group 7A
    Go To Teatcher Calendar
    [Teardown]  Teardown
