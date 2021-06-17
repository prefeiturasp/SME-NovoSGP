import styled from 'styled-components';

import { Button, Base } from '~/componentes';

export const BotaoEstilizado = styled(Button)`
  &.btn {
    background: transparent !important;
    padding: 0 !important;
    color: ${Base.CinzaBotao};

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

export const TextoEstilizado = styled.span`
  font-size: 9px;
  font-weight: bold;
  color: ${Base.CinzaMako};
  position: absolute;
  bottom: -24px;
`;
