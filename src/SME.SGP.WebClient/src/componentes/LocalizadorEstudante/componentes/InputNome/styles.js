import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const InputNomeEstilo = styled.div`
  .ant-input {
    height: 38px !important;
    padding-left: 40px !important;
  }
  .ant-select-selection__rendered {
    line-height: 38px !important;
  }
  .ant-input-prefix {
    i {
      color: ${Base.CinzaMenu};
    }
  }

  div {
    width: 100% !important;
  }
`;
