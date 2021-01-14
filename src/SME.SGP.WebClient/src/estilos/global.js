import { createGlobalStyle } from 'styled-components';
import { Base } from '../componentes/colors';
import ExclamacaoCampoErro from '~/recursos/ExclamacaoCampoErro.svg';
import '../componentes/campoData/campoData.css';

export default createGlobalStyle`
  @import url('https://fonts.googleapis.com/css?family=Roboto:400,700&display=swap');

  *, *:before, *:after {
    box-sizing: border-box;
    margin: 0;
    outline: 0;
    padding: 0;
  }
  *:focus {
    box-shadow: none;
  }
  html, body, #root {
    font-family: 'FontAwesome', 'Roboto', sans-serif !important;
    font-stretch: normal;
    height: 100%;
    letter-spacing: normal;
    line-height: normal;
    @media (max-width: 767.98px) {
      height: auto;
    }
  }
  body {
    -webkit-font-smoothing: antialiased;
    background: ${Base.CinzaFundo} !important;
    overflow-x: hidden;
  }
  button {
    cursor: pointer;
  }
  .fonte-10 {
    font-size: 10px !important;
  }
  .fonte-12 {
    font-size: 12px !important;
  }
  .fonte-13 {
    font-size: 13px !important;
  }
  .fonte-14 {
    font-size: 14px !important;
  }
  .fonte-16 {
    font-size: 16px !important;
  }

  .ant-calendar-picker-container {
    z-index: 9999 !important;
  }

  .ant-select-dropdown {
    z-index: 9999 !important;
  }

  .ant-modal, .ant-modal-wrap {
    z-index: 9999 !important;
  }

  .ant-dropdown {
    z-index: 10000 !important;
  }

  .ant-select-dropdown-menu-item:hover {
    background-color: ${Base.Roxo}  !important;
    color: #ffffff;
  }

  .ant-select-dropdown-menu-item-selected {
    background-color:  ${Base.Roxo}  !important;
    color: #ffffff !important;
  }

  .ant-select-dropdown-menu-item  {
    -webkit-transition: none !important;
    transition: none !important;
  }

  .ant-select-selected-icon {
    color: white !important;
  }

  .desabilitar-elemento {
    pointer-events: none !important;
    opacity: 0.6 !important;
  }

  @media (max-width: 544px) {

    .hidden-xs-down{
      display: none !important;
    }

   }

  .p-l-5{
    padding-left: 5px !important;
  }

  .p-r-5{
    padding-right: 5px !important;
  }

  .m-r-10{
    margin-right: 10px !important;
  }

  .p-r-20{
    padding-right: 20px !important;
  }

  .p-r-11{
    padding-right: 11px !important;
  }

  .p-l-20{
    padding-left: 20px !important;
  }

  .p-b-20{
    padding-bottom: 20px !important;
  }

  .p-t-24{
    padding-top: 24px !important;
  }

  .p-t-20{
    padding-top: 20px !important;
  }

  .p-r-8{
    padding-right: 8px !important;
  }

  .p-l-8{
    padding-left: 8px !important;
  }

  .m-t-10{
    margin-top: 10px !important;
  }

  .m-b-10{
    margin-bottom: 10px !important;
  }

  .m-r-0{
    margin-rigth: 0px !important;
  }

  .m-l-0{
    margin-left: 0px !important;
  }

  .m-t-0{
    margin-top: 0px !important;
  }

  .m-b-0{
    margin-bottom: 0px !important;
  }

  .p-r-0{
    padding-right: 0px !important;
  }

  .p-l-0{
    padding-left: 0px !important;
  }

  .p-t-0{
    padding-top: 0px !important;
  }

  .p-b-0{
    padding-bottom: 0px !important;
  }

  .p-r-10{
    padding-right: 10px !important;
  }

  .p-l-10{
    padding-left: 10px !important;
  }

  .m-b-20{
    margin-bottom: 20px !important;
  }

  .border-vermelhoAlerta{
    border-color: ${Base.VermelhoAlerta} !important;
  }

  .border-2{
    border-width: 2px !important;
  }

  .border-radius-4{
    border-radius: 4px !important;
  }

  .mb-6 {
    margin-bottom: 6rem !important;
  }

/*MENU*/

.menuItem{
    color: ${Base.CinzaMenuItem} !important;
  }

  .ant-menu-inline .ant-menu-item:not(:first-child), .ant-menu-vertical.ant-menu-sub > .ant-menu-item:not(:first-child){
    margin-bottom: 0px !important;
    border-top: 1px solid ${Base.RoxoClaro} !important;
  }

  .ant-menu-vertical.ant-menu-sub > .ant-menu-dark{
    background: white !important;
  }

  .ant-menu-vertical.ant-menu-sub >.ant-menu-item-selected, .ant-menu-vertical.ant-menu-sub > .ant-menu-item-selected{
    background: ${Base.CinzaMenu} !important;
    border-bottom-width: 8px;
    padding-left: 32px !important;
    border-left: solid ${Base.RoxoClaro} 8px !important;
  }

  .ant-menu-item {
    padding-left: 34px !important;
    font-size: 12px !important;
    padding-left: 40px !important;
  }

  .ant-menu-inline.ant-menu-sub{
    background: ${Base.Branco} !important;
  }

  .ant-menu-item, .ant-menu-submenu-open {
    background: ${Base.Branco} !important;
  }

  .ant-menu-sub, .ant-menu-submenu-popup{
    box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
    -webkit-box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
    -moz-box-shadow: 2px 5px 6px rgba(50,50,50,0.77) !important;
  }

  .ant-menu-inline, .ant-menu-submenu-title, .ant-menu-item{
    margin-bottom: 0px !important;
    margin-top: 0px !important;
    top: 0;
  }

  :root{
    --posicao-item-menu-top: 0;
    --posicao-item-menu-left: 110px;
    --espacamento-conteudo: 250px;
  }

  div > div > .ant-menu-submenu-popup{
    top: var(--posicao-item-menu-top) !important;
    left: var(--posicao-item-menu-left) !important;
  }

  .ant-menu-submenu .ant-menu-submenu-vertical{
    background: ${Base.Branco} !important;
    color: ${Base.CinzaMenuItem} !important;
  }

  .ant-menu-submenu-popup{
    position: fixed !important;
    overflow-y: auto;
    max-height: calc(100vh - 140px);
    margin-bottom:800px !important;

    ::-webkit-scrollbar {
      width: 10px;
      border-radius: 4px;
    }

    ::-webkit-scrollbar-thumb {
      background: #dad7d7;
      border-radius: 4px;
      padding-right:5px;
    }
  }

  .texto-vermelho {
    color: #b40c02 !important;
  }

  .texto-vermelho-negrito {
    color: #b40c02 !important;
    font-weight: bold !important;
  }

  .cor-novo-registro-lista {
    font-weight: bold !important;
    color: #42474a !important;
  }

  .ant-modal-footer {
    border-top: 0px !important;
    display: flex !important;
    justify-content: flex-end !important;
  }

  .ant-modal-title{
    font-size: 24px !important;
  }

  form{
    width:100%;
  }

  .desabilitado{
    background: transparent !important;
    border-color: ${Base.CinzaDesabilitado} !important;
    color: ${Base.CinzaDesabilitado} !important;
    cursor: unset !important;
  }

  .form-control.is-invalid, .was-validated .form-control:invalid{
    background-image : url(${ExclamacaoCampoErro}) !important;
    background-size: auto !important;
  }

  .ck-editor__editable_inline {
    min-height: 180px !important;
    list-style-position: inside;
    color:black;
  }

  .erro{
    color: ${Base.Vermelho}
  }

  .secao-conteudo{
    margin-left: var(--espacamento-conteudo);

    @media screen and (max-width: 993px) {
      margin-left: 115px !important;
    }
  }

  .ant-pagination-item-active a{
    color:white
  }
  .ant-pagination-item-active a:hover{
    color:#1890ff
  }

  .ant-notification {
    z-index: 99999 !important;
    top: 85px !important;
    width: 30% !important;
  }

  .alerta-sucesso {
    color: #155724 !important;
    background-color: #d4edda !important;
    border: 1px solid #155724 !important;

    .ant-notification-notice-message {
      color: #155724 !important;
    }
  }

  .alerta-aviso {
    color: #856404 !important;
    background-color: #fff3cd !important;
    border: 1px solid #856404 !important;

    .ant-notification-notice-message {
      color: #856404 !important;
    }
  }

  .alerta-erro {
    color: #721c24 !important;
    background-color: #f8d7da !important;
    border:1px solid #721c24 !important;

    .ant-notification-notice-message {
      color: #721c24 !important;
    }
  }

  .ant-tooltip {    
    z-index: 999999 !important;    
  }

  .ant-time-picker-panel {
    z-index: 999999 !important;  
  }
`;
