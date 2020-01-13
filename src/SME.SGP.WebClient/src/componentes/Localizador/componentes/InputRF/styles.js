import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const InputRFEstilo = styled.div`
  span {
    color: ${Base.Vermelho};
  }

  span.mensagemErro {
    padding: 3px 0 !important;
    display: block;
    font-size: 0.8rem;
  }

  span[class*='is-invalid'] {
    .ant-input {
      border-color: #dc3545 !important;
    }
  }

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
