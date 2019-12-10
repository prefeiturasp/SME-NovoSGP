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

export const Divergencia = styled.div`
  display:flex;
  justify-content: center;
  align-items: center;
  vertical-align: middle;
  background-color: ${Base.LaranjaAlerta};
  border-radius: 4px;
  width: 24px;
  height: 24px;
  padding-left: 1px;
  i{
    font-size: 8px;
    color: ${Base.Branco};
  }
  `
export const SpanDivergencia = styled.span`
  font-size: 10px;
  width: 150px;
  word-wrap: break-word;
  padding-left: 10px;
`
