import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Div = styled.div`
  button {
    margin-right: 1rem;
  }
  button:last-child {
    margin-right: 0;
  }
`;

export const Validacoes = styled(Div)`
  color: ${Base.CinzaBotao};
  font-size: 12px;
  font-weight: bold;

  div.validacoes {
    line-height: 18px;
  }
`;

export const Validacao = styled.li`
  ${props =>
    props.mensagem.exibir
      ? props.mensagem.status === true && `color: ${Base.Verde}`
      : ''};
  ${props =>
    props.mensagem.exibir
      ? props.mensagem.status === false && `color: ${Base.VermelhoNotificacao}`
      : ''};
  font-weight: normal;

  i {
    font-size: 16px;
    font-style: normal;
    line-height: 18px;
    margin-left: 5px;
  }
`;
