import styled from 'styled-components';

import CampoTexto from '~/componentes/campoTexto';

import { Base } from '../../../../componentes/colors';

export const MensagemInputError = styled.b`
  color: ${Base.Vermelho};
`;

export const InputBuscaCPF = styled(CampoTexto)`
  .ant-input {
    border: 1px solid ${Base.Vermelho};
  }
`;
