import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const CaixaDiasLetivos = styled.div`
  width: 69.6px;
  height: 38px;
  border-radius: 4px;
  border: solid 1px #ced4da;
  background-color: #635c5c;
  font-weight: bold;
  color: #ffffff;
  text-align: center;
  padding-top: 9px;
  margin-right: 10px;
`;

export const TextoDiasLetivos = styled.div`
  width: 150px;
  height: 33px;
  font-family: Roboto;
  font-size: 14px;
  color: #686868;
`;

export const ListaCopiarEventos = styled.div`
  display: inline-block;
  vertical-align: middle;
  margin-top: 20px;
`;

export const StatusAguardandoAprovacao = styled.div`
  background-color: ${Base.Roxo};
  border: solid 0.5px ${Base.Roxo};
  border-radius: 3px;
  color: ${Base.Branco};
  font-size: 11px;
  font-weight: bold;
  max-height: 24px;
  object-fit: contain;
  padding: 5px 10px 5px 10px;
  text-align: center;
  margin-right: 9px;
`;
