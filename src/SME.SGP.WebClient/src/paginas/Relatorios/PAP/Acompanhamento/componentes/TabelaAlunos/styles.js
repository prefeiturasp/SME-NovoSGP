// Componentes
import styled from 'styled-components';
import { Base } from '~/componentes';

export const ContainerTabela = styled.div`
  // background: linear-gradient(black 30%, hsla(0, 0%, 100%, 0)),
  //   linear-gradient(hsla(0, 0%, 100%, 0) 10px, black 70%) bottom,
  //   radial-gradient(at top, rgba(0, 0, 0, 0.2), transparent 70%),
  //   radial-gradient(at bottom, rgba(0, 0, 0, 0.2), transparent 70%) bottom;
  // background-repeat: no-repeat;
  // background-size: 100% 20px, 100% 70px, 100% 0px, 100% 0px;
  // background-attachment: local, local, scroll, scroll;
`;

export const Tabela = styled.table`
  width: 100%;
  display: flex;
  flex-direction: column;

  thead {
    background-color: ${Base.CinzaFundo};
    border: 1px solid ${Base.CinzaDesabilitado};

    tr {
      box-shadow: 0px 2px 4px 0px #00000024;
      display: flex;
    }

    tr td {
      padding: 0.7rem 0.5rem;
      font-weight: bold;
      display: flex;
      align-items: center;

      &:first-child {
        width: 34%;
      }

      &:nth-child(2),
      :nth-child(3) {
        display: flex;
        width: 8%;
        text-align: center;
        justify-content: center;
      }

      &:not(:last-child) {
        border-right: 1px solid ${Base.CinzaDesabilitado};
      }
    }
  }

  tbody,
  thead {
    display: flex;
    width: 100% !important;

    tr {
      display: flex;
      width: 100%;
    }
  }

  tbody {
    max-height: 400px;
    min-height: 100px;
    overflow-y: scroll;
    display: flex;
    flex-direction: column;
    position: relative;

    tr.semConteudo {
      td {
        width: 100% !important;
        color: ${Base.CinzaBotao};
        display: flex;
        justify-content: center;
        align-items: center;
      }
    }

    tr {
      flex-direction: row;
      width: 100%;

      td {
        border-left: 1px solid ${Base.CinzaDesabilitado};
        border-bottom: 1px solid ${Base.CinzaDesabilitado};
        padding: 0.7rem 0.5rem;
        display: flex;
        align-items: center;

        &:first-child {
          min-width: 35px;
        }

        &:last-child {
          width: 49.4%;
          border-right: 1px solid ${Base.CinzaDesabilitado};
        }

        &:nth-child(2) {
          flex: 1;
        }

        &:nth-child(3),
        :nth-child(4) {
          width: 8%;
          justify-content: center;
        }

        &:not(:nth-child(2)) {
          text-align: center;
        }
      }
    }
  }

  thead tr td:last-child,
  tbody tr td:last-child {
    display: flex;

    div.opcaoSelect {
      width: 50%;
      margin: 0 auto;

      @media screen and (max-width: 997px) {
        width: 70%;
      }
    }
  }
`;
