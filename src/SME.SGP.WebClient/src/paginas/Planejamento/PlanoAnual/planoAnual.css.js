import styled from 'styled-components';
import { Base } from '../../../componentes/colors';

export const Label = styled.label``;

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
`;

export const TituloAno = styled.span`
  font-weight: bold;
  font-size: 16px;
  color: ${Base.Roxo};
`;

export const Planejamento = styled.div`
  object-fit: contain;
  font-family: Roboto;
  font-size: 11px;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: normal;
  color: #c8c8c8;
  padding-top: 6px;
`;

export const Select = styled.select`
  border-radius: 4px;
  border: solid 1px #ced4da;
  background-color: #ffffff;
  margin-rigth: 10px !important;
`;

export const ParagrafoAlerta = styled.p`
  color: ${Base.VermelhoAlerta};
`;
