import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Nav = styled.nav`
  height: 70px;
`;

export const Logo = styled.img`
  max-height: 65px;
  max-width: 75px;
`;

export const Div = styled.div`
  button {
    margin-right: 1rem;
  }
  button:last-child {
    margin-right: 0;
  }
`;

export const Container = styled(Div)`
  background: ${Base.Branco};
`;

export const Texto = styled(Div)`
  font-size: 14px;
  letter-spacing: normal;
  line-height: normal;
`;

export const Titulo = styled.h1`
  font-size: 24px;
  font-weight: bold;
`;

export const Rotulo = styled.label`
  color: ${Base.CinzaMako};
`;

export const CampoTexto = styled.input`
  color: ${Base.CinzaBotao};
  font-size: 14px;
`;

export const Validacoes = styled(Div)`
  color: ${Base.CinzaBotao};
  font-size: 12px;
  font-weight: bold;
`;

export const Itens = styled.ul`
  line-height: 18px;
`;

export const Icone = styled.i`
  font-size: 16px;
  font-style: normal;
  line-height: 18px;
  margin-left: 5px;
`;

export const MensagemErro = styled(Div)`
  border: solid 2px ${Base.VermelhoNotificacao};
  color: ${Base.VermelhoNotificacao};
  max-height: 85px;
  max-width: 295px;
`;
