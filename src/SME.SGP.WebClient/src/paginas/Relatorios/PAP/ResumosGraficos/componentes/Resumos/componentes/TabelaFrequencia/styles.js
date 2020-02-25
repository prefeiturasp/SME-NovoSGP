import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

export const ContainerTabela = styled.div`
  .ant-table-thead > tr.ant-table-row-hover:not(.ant-table-expanded-row) > td,
  .ant-table-tbody > tr.ant-table-row-hover:not(.ant-table-expanded-row) > td,
  .ant-table-thead > tr:hover:not(.ant-table-expanded-row) > td,
  .ant-table-tbody > tr:hover:not(.ant-table-expanded-row) > td {
    background: unset;
  }

  .ant-table-small
    > .ant-table-content
    > .ant-table-header
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-body
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-scroll
    > .ant-table-header
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-scroll
    > .ant-table-body
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-fixed-left
    > .ant-table-header
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-fixed-right
    > .ant-table-header
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-fixed-left
    > .ant-table-body-outer
    > .ant-table-body-inner
    > table
    > .ant-table-thead
    > tr
    > th,
  .ant-table-small
    > .ant-table-content
    > .ant-table-fixed-right
    > .ant-table-body-outer
    > .ant-table-body-inner
    > table
    > .ant-table-thead
    > tr
    > th {
    background: #fafafa;
  }

  margin: 0 !important;
  padding: 0 !important;

  table {
    tr {
      th,
      td {
        color: ${Base.CinzaBotao} !important;
      }

      td,
      th {
        &.primeirasColunas {
          color: ${Base.CinzaMako} !important;
        }

        &:not(.primeirasColunas) {
          text-align: center !important;
        }
      }

      // td:not(.primeirasColunas),
      // th {
      //   text-align: center !important;
      // }

      th {
        font-weight: bold;
      }

      td.headerTotal {
        background-color: ${Base.CinzaBadge} !important;
        font-weight: bold;
      }
    }
  }

  th.headerTotal {
    background-color: ${Base.Roxo} !important;
    color: ${Base.Branco} !important;
  }
`;
