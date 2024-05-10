import styled from 'styled-components';
import { Base } from '~/componentes';

export const BotaoLista = styled.div`
  width: 46px;
  height: 46px;
  border-radius: 4px;
  border: solid 1px #a4a4a4;
  background-color: rgba(0, 0, 0, 0.04);
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;

  :hover {
    background-color: ${Base.Roxo} !important;
    color: white;
  }

  i {
    font-size: 20px;
  }
`;

export const ColunaBotaoLista = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
`;

export const CardLista = styled.div`
  padding: 10px;
  border-radius: 4px;
  box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.15);
  min-height: 488px;
  td {
    white-space: pre-wrap !important;
  }
`;

export const ListaCopiarCompensacoes = styled.div`
  display: inline-block;
  vertical-align: middle;
  margin-top: 20px;
`;
