import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes/colors';

export const DiaWrapper = styled.div`
  display: flex;
  flex-direction: row;
  min-width: 14.28% !important;
  max-width: 14.28% !important;
  height: 62px;
  cursor: pointer;
  font-size: 16px;
  color: ${props => (props.mesAtual ? Base.Preto : Base.CinzaDesabilitado)};
  border: 1px solid ${Base.CinzaBordaCalendario};
  border-top: 0 !important;
  border-left: 0 !important;
  background-color: white;

  ${props =>
    props.numeroDia === 0 &&
    `background-color: ${Base.RosaCalendario} !important;`}

  ${props =>
    props.numeroDia === 6 &&
    `background-color: ${Base.CinzaCalendario} !important;
     border-right: 0 !important;`}

  ${props =>
    props.selecionado &&
    `border-bottom: 0 !important;
    background-color: white !important;
    `}

  .numeroDia {
    display: flex;
    flex-direction: column;
    align-items: flex-end;
    justify-content: flex-end;
    padding-bottom: 0.3rem;
    padding-top: 0.3rem;

    &:nth-child(1) {
      display: flex;
      align-items: flex-start;
      justify-content: space-between;
    }
  }
`;

export const TipoEventosLista = styled.div`
  bottom: 5px;
  right: 10px;
`;

export const TipoEvento = styled.div.attrs(props => ({
  className: 'd-block badge badge-pill text-white ml-auto mr-0',
  cor: props.cor ? props.cor : Base.Roxo,
}))`
  background: ${props => props.cor};
  font-size: 10px;
  margin-bottom: 2px;
  min-width: 60px;
  &:last-child {
    margin-bottom: 0;
  }
`;

export const IconeAtividadeAvaliativa = styled.i`
  font-size: 15px;
  color: ${Base.AzulBreadcrumb};
`;
