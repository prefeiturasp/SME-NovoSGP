import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Div = styled.div``;

export const TipoEventosLista = styled(Div)`
  bottom: 5px;
  right: 10px;
`;

export const TipoEvento = styled(Div).attrs(props => ({
  className: 'd-block badge badge-pill text-white ml-auto mr-0',
  cor: props.cor ? props.cor : Base.Roxo,
}))`
  @media screen and (min-width: 0px) and (max-width: 900px) {
    min-width: auto !important;
    font-size: 9px;
  }

  background: ${props => props.cor};
  font-size: 10px;
  margin-bottom: 2px;
  min-width: 60px;
  &:last-child {
    margin-bottom: 0;
  }
`;
