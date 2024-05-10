import styled from 'styled-components';
import LinkRouter from '~/componentes/linkRouter';
import Row from '~/componentes/row';
import FundoLogin from '~/recursos/FundoLogin.jpg';
import Card from '~/componentes/cardBootstrap';
import { Base } from '~/componentes/colors';
import CardBody from '~/componentes/cardBody';

export const Fundo = styled(Row)`
  background: url(${FundoLogin});
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  height: -moz-available;
  height: -webkit-fill-available;
  height: fill-available;
`;

export const Logo = styled.img`
  align-self: center;
  justify-self: center;
`;

export const Formulario = styled.div`
  align-self: center;
  justify-self: center;
  padding: 0px;
`;

export const LogoSGP = styled.div`
  padding: 0px;
`;

export const CampoTexto = styled.input`
  ::-webkit-input-placeholder {
    font-size: 14px !important;
    font-family: Roboto !important;
    font-size: 14px !important;
    font-weight: normal !important;
    font-style: normal !important;
    font-stretch: normal !important;
    line-height: 1.6 !important;
    letter-spacing: normal !important;
    color: ${Base.CinzaBotao} !important;
  }
  ::-moz-placeholder {
    font-size: 14px !import;
  }
  :-ms-input-placeholder {
    font-size: 14px !import;
  }
  :-moz-placeholder {
    font-size: 14px !import;
  }
`;

export const Rotulo = styled.label`
  font-family: Roboto !important;
  font-size: 14px !important;
  font-weight: normal !important;
  font-style: normal !important;
  font-stretch: normal !important;
  line-height: normal !important;
  letter-spacing: normal !important;
  color: ${Base.CinzaMako} !important;
`;

export const LogoSP = styled(LogoSGP)`
  align-self: end;
  justify-self: center;
  justify-content: center;
  justify-items: center;
`;

export const Cartao = styled(Card)`
  height: auto;
  border-radius: 6px;
`;

export const CorpoCartao = styled(CardBody)`
  display: flex;
  height: 100%;
  justify-content: center;
`;

export const Centralizar = styled.div`
  display: flex;
  justify-content: center;
`;

export const Link = styled(LinkRouter)`
  justify-self: center;
  justify-content: center;
  justify-items: center;
`;

export const LabelLink = styled.label`
  font-family: Roboto;
  font-size: 12px;
  font-weight: normal;
  font-style: normal;
  font-stretch: normal;
  color: ${Base.Roxo} !important;
  cursor: pointer;
`;

export const ErroTexto = styled.label`
  font-family: Roboto;
  font-size: 12px;
  font-weight: normal;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  color: ${Base.Vermelho};
  padding-left: 5px;
  margin-top: 5px;
  margin-bottom: 0px;
`;

export const ErroGeral = styled.h2`
  font-family: Roboto;
  font-size: 14px;
  font-weight: bold;
  font-style: normal;
  font-stretch: normal;
  line-height: normal;
  letter-spacing: 0.28px;
  color: ${Base.Vermelho};
  border-radius: 4px;
  border: solid 2px ${Base.Vermelho};
  padding: 1rem;
  text-align: center;
`;

export const MensagemMobile = styled.div`
  font-size: 18px;
  color: ${Base.Vermelho};
  border: 2px solid ${Base.Vermelho};
  border-radius: 4px;
  padding: 10px;
  margin: 50px 0 50px 0;
`;

export const TextoAjuda =
  'Digite seu RF. Para usuários externos, insira seus dados de usuário. Caso não possua, procure a SME.';
