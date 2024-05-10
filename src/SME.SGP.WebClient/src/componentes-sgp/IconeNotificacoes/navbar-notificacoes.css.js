import { Badge } from 'antd';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const Count = styled(Badge)`
  color: ${Base.Branco} !important;
  ${props =>
    !props.count &&
    `
    i {
      background: ${Base.CinzaDesabilitado} !important;
      cursor: default !important;
    }
  `}
  sup {
    background: ${Base.VermelhoNotificacao} !important;
    display: flex !important;
    font-size: 9px !important;
    height: 18px !important;
    justify-content: center !important;
    min-width: 18px !important;
    width: 18px !important;
  }
`;

const Lista = styled.div`
  font-size: 9px !important;
  margin-top: 5px !important;
  min-width: 360px !important;
  right: 0 !important;
  z-index: 1 !important;
`;

const Tr = styled.tr`
  cursor: pointer !important;
  &:first-child {
    th,
    td {
      border-top: 0 none !important;
    }
  }
  td:first-child {
    color: ${Base.CinzaIconeNotificacao} !important;
  }
  th,
  td {
    border-color: ${Base.CinzaDesabilitado} !important;
    padding-bottom: 0.5rem !important;
    padding-bottom: 0.5rem !important;
    ${props =>
      props.status === 1 &&
      `
      background: ${Base.RoxoNotificacao} !important;
      font-weight: bold !important;
      &.status {
          color: ${Base.VermelhoNotificacao} !important;
          text-transform: uppercase !important;
      }`}
    &.w-75 {
      width: 160px !important;
    }
    &.w-25 {
      width: 50px !important;
    }
  }
`;

export { Tr, Lista, Count };
