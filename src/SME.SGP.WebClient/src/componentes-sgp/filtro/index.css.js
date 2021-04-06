import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const Container = styled.div`
  max-width: 568px !important;
  width: 50% !important;
  z-index: 100;
  @media (max-width: 575.98px) {
    max-width: 80%;
  }
  @media screen and (max-width: 993px) {
    width: 400px !important;
  }
`;

export const Campo = styled.input`
    background: ${Base.CinzaFundo};
    font-weight: bold;
    height: 45px;
    &::placeholder {
      font-weight: normal;
    }
    &:focus {
      background: ${Base.Branco};
      box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
      color ${Base.Preto};
      font-weight: normal;
      &:read-only {
        background: ${Base.CinzaFundo};
        font-weight: bold;
        box-shadow: none;
      }
    }
  `;

export const Icone = styled.i`
  color: ${Base.CinzaMako};
  cursor: pointer;
`;

export const Busca = styled(Icone)`
  left: 0;
  max-height: 23px;
  max-width: 14px;
  padding: 1rem;
  right: 0;
  top: 0;
`;

export const Fechar = styled(Icone)`
  right: 50px;
  top: 15px;
`;

export const SetaFunction = alternarFocoBusca => {
  return styled(Icone)`
    background: ${Base.CinzaDesabilitado};
    max-height: 36px;
    max-width: 36px;
    padding: 0.7rem 0.9rem;
    right: 5px;
    top: 5px;
    transition: transform 0.3s;
    ${alternarFocoBusca && 'transform: rotate(180deg);'}
  `;
};

export const ItemLista = styled.li`
  cursor: pointer;
  &:hover,
  &:focus,
  &.selecionado {
    background: ${Base.Roxo};
    color: ${Base.Branco};
  }
`;
