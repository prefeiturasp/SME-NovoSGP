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
      border: 0 !important;
      background: ${Base.CinzaFundoEditor} !important;
      padding: 0 8px 20px 6px;
    }
    &-status-bar,
    &-editor__resize {
      display: none;
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
    background: #ffffff;
    border-radius: 10px;
  }
  ::-webkit-scrollbar-thumb {
    background: #a4a4a4;
    border: 2px white solid;
    background-clip: padding-box;
    border-radius: 10px;
  }
`;
