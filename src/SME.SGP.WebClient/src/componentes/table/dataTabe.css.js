import styled from 'styled-components';
import { Base } from '../colors';

export const Container = styled.div`
  .ocultar-coluna-multi-selecao {
    .ant-table-selection-column {
      display: none !important;
    }
  }

  .ant-checkbox-indeterminate .ant-checkbox-inner::after {
    background-color: ${Base.Roxo} !important;
  }

  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${Base.Roxo};
    border-color: #ffffff;
  }

  .ant-table-thead tr th {
    text-align: center;
    border-right: solid 1px #dadada;
  }

  .ant-table-tbody tr td {
    border-right: solid 1px #dadada;
    white-space: nowrap;
    cursor: pointer;
  }

  .ant-table-column-title {
    font-weight: bold;
    letter-spacing: 0.12px;
    color: #323c47;
    text-transform: uppercase;
    font-size: 14px;
  }

  .ant-table-pagination.ant-pagination {
    text-align: center !important;
    float: none;
  }

  .ant-pagination-item {
    border-radius: 0px;

    a {
      font-family: Roboto;
      font-weight: bold;
      font-style: normal;
      font-stretch: normal;
      letter-spacing: normal;
      color: #ffffff;
    }
  }

  .ant-pagination.mini .ant-pagination-item {
    min-width: 45px;
    height: 45px;
    margin: 0;
    line-height: 45px;
  }

  .ant-pagination-prev {
    border: solid 1px #dadada !important;
  }

  .ant-pagination.mini .ant-pagination-prev {
    min-width: 45px;
    height: 45px;
    margin: 0;
    line-height: 40px;
    border-radius: 4px 0px 0px 4px;
    border-right: none !important;
  }

  .ant-pagination.mini .ant-pagination-next {
    min-width: 45px;
    height: 45px;
    margin: 0;
    line-height: 40px;
    border-radius: 0px 4px 4px 0px;
    border-left: none !important;
  }

  .ant-pagination-next {
    border: solid 1px #dadada !important;
  }

  .ant-pagination-item {
    border: solid 1px #dadada !important;
  }

  .ant-pagination-item-active {
    background: ${Base.Roxo} !important;
    border-color: ${Base.Roxo} !important;
    color: #ffffff;
    font-size: 12px;
  }

  .ant-pagination-item-active:focus,
  .ant-pagination-item-active:hover {
    border-color: ${Base.Roxo} !important;
  }

  .ant-pagination-item-active:focus a,
  .ant-pagination-item-active:hover a {
    color: #ffffff;
  }

  .ant-table-tbody tr:hover td {
    background: ${Base.Roxo} !important;
    color: #ffffff !important;

    a.texto-vermelho-negrito {
      color: #ffffff !important;
    }

    a.texto-negrito {
      color: #ffffff !important;
    }
  }

  .ant-table-tbody tr:hover {
    background: ${Base.Roxo} !important;
    color: #ffffff !important;
  }

  .ant-table-tbody tr.ant-table-row-selected > td {
    background: ${Base.Roxo};
    color: #ffffff;

    a.texto-vermelho-negrito {
      color: #ffffff !important;
    }

    a.texto-negrito {
      color: #ffffff !important;
    }
  }

  .ant-table-tbody > tr {
    -webkit-transition: none;
    transition: none;

    td {
      -webkit-transition: none;
      transition: none;

      a {
        -webkit-transition: none;
        transition: none;
      }
    }
  }
`;
