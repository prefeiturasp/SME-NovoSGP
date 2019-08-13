import { createGlobalStyle } from 'styled-components';

export default createGlobalStyle`
@import url('https://fonts.googleapis.com/css?family=Roboto&display=swap');

* {
    margin: 0;
    padding: 0;
    outline: 0;
    box-sizing: border-box;
  }
  *:focus {
    outline: 0;
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
`;
