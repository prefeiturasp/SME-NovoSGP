import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Div = styled.div`
  .badge-aula {
    background: ${Base.Roxo};
  }
  .badge-cj {
    background: ${Base.Laranja};
  }
`;

export const TipoEventosLista = styled(Div)`
  bottom: 5px;
  right: 10px;
`;

export const TipoEvento = styled(Div)`
  font-size: 10px;
  margin-bottom: 2px;
  min-width: 60px;
  &:last-child {
    margin-bottom: 0;
  }
`;
