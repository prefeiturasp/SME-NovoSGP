import styled from 'styled-components';
import { Base } from '~/componentes';

export const ListaPlanejamentos = styled.div`
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
    background: white !important;
  }

  .titulo {
    color: #42474a;
    font-weight: bold;
  }

  .data {
    background: #f5f6f8;
    color: #086397 !important;
    font-weight: bold !important;
  }

  .cj {
    color: ${Base.Laranja} !important;
    font-weight: bold !important;
  }
`;

export const Tabela = styled.div`
  border-radius: 2px;
`;

export const EditorPlanejamento = styled.fieldset`
  .ck-editor__editable_inline {
    max-height: 180px !important;
  }
  .jodit-container {
    border: 0;
  }
`;
