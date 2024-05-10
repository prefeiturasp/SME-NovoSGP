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
    color: ${Base.CinzaBotao} !important;
  }
`;

export const Tabela = styled.div`
  border-radius: 2px;
  border-left: 6px solid;
`;
