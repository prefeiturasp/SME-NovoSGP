*** Settings ***
Documentation       Robo de teste 

Library             SeleniumLibrary
Library             ScreenCapLibrary
Library             OperatingSystem

Resource            ../../../configuracao/variaveis.robot
Resource            ../../Login/Login.lib.robot
Resource            Cadastro.lib.robot

*** Variables ***

${NOVO COMUNICADO URL}                http://${SERVER}/gestao/acompanhamento-escolar/comunicados/novo

*** Keywords ***



*** Test Cases ***
Story 20085
    Start Video Recording   monitor=1
    Open Browser To Login Page
    Login Into System

    

    [Teardown]  Teardown
