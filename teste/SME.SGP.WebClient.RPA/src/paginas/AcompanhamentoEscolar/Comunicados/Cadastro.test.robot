*** Settings ***
Documentation       Robo de teste 

Library             SeleniumLibrary

Resource            ../../../configuracao/variaveis.robot
Resource            ../../Login/Login.lib.robot
Resource            Cadastro.lib.robot

*** Variables ***

${NOVO COMUNICADO URL}                http://${SERVER}/gestao/acompanhamento-escolar/comunicados/novo

*** Keywords ***
Teardown Story 20085
    Close Browser


*** Test Cases ***
Story 20085
    Open Browser To Login Page
    Login Into System

    [Teardown]  Teardown Story 20085
