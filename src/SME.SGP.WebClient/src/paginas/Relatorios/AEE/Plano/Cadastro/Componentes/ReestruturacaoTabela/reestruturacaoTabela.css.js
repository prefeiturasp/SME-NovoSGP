import styled from 'styled-components';
import { Button } from '~/componentes';

export const BotaoEstilizado = styled(Button)`
  position: absolute !important;
  right: 16px;
  &.btn {
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

export const TextoEstilizado = styled.div`
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  max-width: 640px;
`;
