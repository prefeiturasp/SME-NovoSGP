import styled from 'styled-components';

// Styles
import { Base } from '~/componentes';

export const InputNomeEstilo = styled.div`
  .ant-input {
    height: 38px !important;
    padding-left: 40px !important;
  }
  .ant-input-prefix {
    i {
      color: ${Base.CinzaMenu};
    }
  }
`;
