import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const InputRFEstilo = styled.div`
  .ant-input {
    height: 38px;
  }

  .ant-input-suffix {
    right: 0;
    i {
      color: ${Base.Roxo};
    }

    button:disabled i {
      color: ${Base.CinzaMenu};
    }
  }
`;
