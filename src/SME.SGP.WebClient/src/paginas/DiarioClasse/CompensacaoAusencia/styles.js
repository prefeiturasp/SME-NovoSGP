import styled from 'styled-components';
import { Base } from '~/componentes';

export const AlunosCompensacao = styled.div`
  span {
    padding: 3px 9px 3px 9px;
    margin: 6px;
    border-radius: 10px;
    box-shadow: 0 0 2px 0 rgba(12, 35, 48, 0.3);
    font-size: 11px;
    color: rgba(0, 0, 0, 0.65);
    background-color: #ffffff;
  }
`;

export const Badge = styled.button`
  &:last-child {
    margin-right: 10 !important;
  }

  &[aria-pressed='true'] {
    background: ${Base.Roxo} !important;
    color: white !important;
  }
`;

export const BotaoListaAlunos = styled.div`
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

export const ColunaBotaoListaAlunos = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  flex-direction: column;
`;

export const CardTabelaAlunos = styled.div`
  padding: 10px;
  height: 488px;
  border-radius: 4px;
  box-shadow: 0 6px 18px 0 rgba(0, 0, 0, 0.15);
  background-color: #ffffff;
`;

export const ListaCopiarCompensacoes = styled.div`
  display: inline-block;
  vertical-align: middle;
  margin-top: 20px;
`;

