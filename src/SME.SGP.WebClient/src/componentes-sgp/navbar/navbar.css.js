import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Nav = styled.nav`
  z-index: 3000;
  height: 70px !important;
  padding-left: 15px !important;
  padding-right: 15px !important;
  @media (max-width: 767.98px) {
    height: 140px !important;
  }
`;

export const Logo = styled.img`
  height: 65px !important;
  width: 75px !important;
  @media screen and (max-width: 993px) {
    width: 4rem !important;
  }
`;

export const Botoes = styled.div`
  height: 45px !important;
  z-index: 101;
`;

export const Botao = styled.a`
  display: block !important;
  text-align: center !important;
  cursor: pointer;
`;

export const Icone = styled.i`
  align-items: center !important;
  background: ${Base.Roxo} !important;
  border-radius: 50% !important;
  color: ${Base.Branco} !important;
  display: flex !important;
  justify-content: center !important;
  font-size: 15px !important;
  height: 28px !important;
  width: 28px !important;
`;

export const Texto = styled.span`
  font-size: 10px !important;
`;

export const Div = styled.div`
  margin-left: ${props =>
    props.retraido ? '120px !important' : '260px !important'};
  @media (max-width: 767.98px) {
    left: 50%;
    margin-left: 0 !important;
    transform: translateX(-50%) translateY(-0.5rem);
  }
`;
