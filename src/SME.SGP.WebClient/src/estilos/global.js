import { createGlobalStyle } from 'styled-components';

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
    font-family: 'Roboto', sans-serif;
    font-stretch: normal;
    height: 100%;
    letter-spacing: normal;
    line-height: normal;
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
  .fonte-13 {
    font-size: 13px !important;
  }
  .fonte-14 {
    font-size: 14px !important;
  }
  .fonte-16 {
    font-size: 16px !important;
  }
`;
