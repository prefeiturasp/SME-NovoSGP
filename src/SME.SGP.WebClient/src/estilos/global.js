import { createGlobalStyle } from 'styled-components';
import { Base } from '../componentes/colors';

export default createGlobalStyle`
@import url('https://fonts.googleapis.com/css?family=Roboto&display=swap');

* {
    margin: 0;
    padding: 0;
    outline: 0;
    box-sizing: border-box;
  }
  *:focus {
    outline: 0 !important;
    box-shadow: none !important;
  }
  html, body, #root {
    height: 100%;
    font-family: Roboto, Helvetica, sans-serif;
    font-stretch: normal;
    line-height: normal;
    letter-spacing: normal;
  }
  body {
    -webkit-font-smoothing: antialiased;
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
  .fonte-14 {
    font-size: 14px !important;
  }

  .ant-select-dropdown-menu-item:hover {
    background-color: ${Base.Roxo}  !important;
    color: #ffffff;
  }

  .ant-select-dropdown-menu-item-selected {
    background-color:  ${Base.Roxo}  !important;
    color: #ffffff !important;
  }

  // .btn-outline-voltar {
  //   color: #086397 !important;
  //   background-color: #086397 !important;
  // }

  // .btn-outline-voltar:hover {
  //   color: #fff !important;
  //   background-color: #086397  !important;
  //   border-color: #086397  !important;
  // }

  // .btn-outline-cancelar {
  //   color: #6933ff !important;
  //   border-color: #6933ff !important;
  // }

  // .btn-outline-cancelar:hover {
  //   color: #fff !important;
  //   background-color: #6933ff  !important;
  //   border-color: #6933ff  !important;
  // }

  // .btn {
  //   min-width: 100px;
  //   max-width: 100px;
  //   display: block;
  //   width: 100%;
  // }

  // .btn-outline-salvar {
  //   color: #6933ff !important;
  //   border-color: #6933ff !important;
  // }

  // .btn-outline-salvar:hover {
  //   color: #fff !important;
  //   background-color: #6933ff !important;
  //   border-color: #6933ff !important;
  // }
`;
