import styled from 'styled-components';
import { Base } from '~/componentes';

export const DadosAlunoModal = styled.div`
  width: 100%;
  height: 100%;
  padding: 5px;
  border: solid 1px ${Base.CinzaDesabilitado};
  color: #42474a;
  display: flex;
  font-size: 13px;
  align-items: center;

  i {
    margin-right: 5px;
    font-size: 55px;
    color: ${Base.CinzaDesabilitado};
  }
`;

export const EditorAnotacao = styled.fieldset`
  .ck-editor__editable_inline {
    max-height: 180px !important;
  }
`;
