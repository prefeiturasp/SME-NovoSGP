import styled from 'styled-components';
import { Base } from '~/componentes';

export const Titulo = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 24px;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: normal;
  color: #42474a;
  margin-bottom: 10px;
  padding-left: 10px;
`;

export const TituloAno = styled.span`
  font-weight: bold;
  font-size: 16px;
  color: ${Base.Roxo};
`;
