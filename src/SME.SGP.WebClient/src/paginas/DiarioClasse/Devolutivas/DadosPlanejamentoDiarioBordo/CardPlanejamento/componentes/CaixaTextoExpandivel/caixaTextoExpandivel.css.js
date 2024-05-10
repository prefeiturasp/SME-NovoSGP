import styled, { css } from 'styled-components';
import { Base, Button } from '~/componentes';

export const Container = styled.div`
  .card {
    &-header {
      background-color: transparent;
      border: 0;
      font-weight: bold;
      font-size: 14px;
      color: ${Base.CinzaMako};
      padding: 15px 16px;
    }
    &-body {
      padding: 0;
    }
  }
`;

export const BotaoEstilizado = styled(Button)`
  position: absolute !important;
  right: 16px;
  &.btn {
    background: transparent !important;
    padding: 0 !important;
    color: ${Base.Azul};
    &:hover {
      background-color: transparent !important;
      color: inherit !important;
    }
    &-primary {
      &:focus {
        box-shadow: none;
      }
    }
    i {
      margin: 0 !important;
    }
  }
`;

export const EditorPlanejamento = styled.fieldset`
  .ck-editor__editable_inline {
    max-height: 180px !important;
  }
  .jodit {
    &-container {
      background: ${Base.CinzaFundoEditor} !important;
      border-radius: 0 !important;
      border: 0 !important;
      border-top: 1px solid ${Base.CinzaDesabilitado} !important;
      box-shadow: 0px 1px 4px rgba(8, 35, 48, 0.1);
      padding: 0 8px 20px 6px;
    }
    &-status-bar,
    &-editor__resize {
      display: none;
    }
    &-toolbar__box {
      border: 0 !important;
    }
    iframe {
      margin-top: 16px;
    }
  }
`;

export const IframeStyle = css`
  p {
    margin: 0;
  }
  ::-webkit-scrollbar {
    width: 8px;
  }
  ::-webkit-scrollbar-track {
    background: #fff;
    border-radius: 10px;
  }
  ::-webkit-scrollbar-thumb {
    background: #a4a4a4;
    border: 2px white solid;
    background-clip: padding-box;
    border-radius: 10px;
  }
`;

export const TextoSimples = styled.div`
  height: 260px;
  background: ${Base.CinzaFundoEditor};
  padding-top: 20px;
  padding-right: 8px;
  border-top: 1px solid ${Base.CinzaDesabilitado};
  box-sizing: border-box;
  box-shadow: 0px 1px 4px rgba(8, 35, 48, 0.1);

  div {
    background: ${Base.CinzaFundoEditor};
    padding: 8px 20px 14px 16px;
    height: 220px;
    overflow: auto;
    ::-webkit-scrollbar {
      width: 8px;
    }
    ::-webkit-scrollbar-track {
      background: ${Base.Branco};
      border-radius: 10px;
    }
    ::-webkit-scrollbar-thumb {
      background: ${Base.CinzaBotao};
      border: 2px ${Base.Branco} solid;
      background-clip: padding-box;
      border-radius: 10px;
    }
  }
`;

export const FundoEditor = styled.div`
  height: 560px;
  background: ${Base.CinzaFundoEditor};
`;
