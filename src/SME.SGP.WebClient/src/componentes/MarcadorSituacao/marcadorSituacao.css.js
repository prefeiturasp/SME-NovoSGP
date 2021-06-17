import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Container = styled.div`
  background-color: ${({ corFundo }) => Base[corFundo]};
  color: ${({ corTexto }) => Base[corTexto]};
  padding: 4px 8px;
  font-weight: bold;
  font-size: 12px;
  line-height: 16px;
  border-radius: 4px;

  span {
    margin: 10px;
  }
`;
