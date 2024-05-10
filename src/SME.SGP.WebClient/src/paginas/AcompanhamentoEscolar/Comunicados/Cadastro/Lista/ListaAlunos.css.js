import { Base } from '~/componentes';
import styled from 'styled-components';

const Container = styled.div`
  .ant-table-pagination.ant-pagination {
    float: none;
    text-align: center !important;
  }

  .ant-checkbox-indeterminate .ant-checkbox-inner::after {
    background-color: ${Base.Roxo} !important;
  }

  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${Base.Roxo};
    border-color: ${Base.Branco} !important;
  }

  .ant-pagination-item {
    border-radius: 0;

    a {
      font-family: Roboto;
      font-style: normal;
      font-stretch: normal;
      font-weight: bold;
      letter-spacing: normal;
    }
  }

  .ant-pagination.mini .ant-pagination-item {
    height: 45px;
    line-height: 45px;
    margin: 0;
    min-width: 45px;
  }

  .ant-pagination-prev {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
  }

  .ant-pagination.mini .ant-pagination-prev {
    border-radius: 4px 0px 0px 4px;
    border-right: none !important;
    height: 45px;
    line-height: 40px;
    margin: 0;
    min-width: 45px;
  }

  .ant-pagination.mini .ant-pagination-next {
    border-radius: 0px 4px 4px 0px;
    border-left: none !important;
    height: 45px;
    line-height: 40px;
    margin: 0;
    min-width: 45px;
  }

  .ant-pagination-next {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
  }

  .ant-pagination-item {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
  }

  .ant-pagination-item-active {
    background: ${Base.Roxo} !important;
    border-color: ${Base.Roxo} !important;
    color: ${Base.Branco} !important;
    font-size: 12px;
  }

  .ant-pagination-item-active:focus,
  .ant-pagination-item-active:hover {
    border-color: ${Base.Roxo} !important;
  }

  .ant-pagination-item-active:focus a,
  .ant-pagination-item-active:hover a {
    color: ${Base.Branco} !important;
  }
`;

export default Container;
