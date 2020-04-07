import styled from 'styled-components';
import { Base } from '~/componentes';

export const ListaAnotacao = styled.div`
  table {
    margin-bottom: 0;
  }

  th {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
    color: ${Base.CinzaMako} !important;
    font-weight: normal !important;
  }

  td {
    padding: 0;
  }

  .ck-editor__editable_inline {
    max-height: 180px !important;
    color: ${Base.CinzaBotao} !important;

    ::-webkit-scrollbar-track {
      background-color: #f4f4f4 !important;
    }

    ::-webkit-scrollbar {
      width: 9px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
      border-radius: 2.5px !important;
    }

    ::-webkit-scrollbar-thumb {
      background: #a8a8a8 !important;
      border-radius: 3px !important;
    }
  }
`;

export const Tabela = styled.div`
  border-radius: 2px;
  border-left: 6px solid;
`;
