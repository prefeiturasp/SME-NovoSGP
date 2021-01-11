*** Settings ***
Documentation       Robo de teste para validar a seguinte jornada do usuário:
...                 1. Logar
...                 2. Selecionar no filtro principal a turma 7A

Resource            Login.lib.robot

*** Keywords ***
Teardown
    Close Browser

*** Test Cases ***
User Jorney
    Open Browser To Login Page
    Login Into System
    Select Group 7A
    [Teardown]  Teardown
