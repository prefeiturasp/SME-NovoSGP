import { Pagination } from 'antd';
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerPaginacao = styled(Pagination)`
  text-align: center;

  .ant-pagination-item {
    border-radius: 0 !important;
    height: 45px;
    line-height: 45px;
    margin: 0;
    min-width: 45px;

    a {
      font-family: Roboto;
      font-style: normal;
      font-stretch: normal;
      font-weight: bold;
      letter-spacing: normal;
    }
  }

  .ant-pagination-prev {
    height: 45px;
    line-height: 40px;
    margin: 0;
    min-width: 45px;

    .ant-pagination-item-link {
      border-radius: 4px 0px 0px 4px;
      border-right: 0px !important;
    }
  }

  .ant-pagination-next {
    height: 45px;
    line-height: 40px;
    margin: 0;
    min-width: 45px;

    .ant-pagination-item-link {
      border-radius: 0px 4px 4px 0px;
      border-left: 0px !important;
    }
  }

  .ant-pagination-prev:focus .ant-pagination-item-link,
  .ant-pagination-next:focus .ant-pagination-item-link,
  .ant-pagination-prev:hover .ant-pagination-item-link,
  .ant-pagination-next:hover .ant-pagination-item-link {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
  }

  .ant-pagination-item-active {
    background: ${Base.Roxo} !important;
    border-color: ${Base.Roxo} !important;
    color: ${Base.Branco} !important;
    font-size: 12px;
  }

  .ant-pagination-item-active a {
    color: ${Base.Branco} !important;
  }

  .ant-pagination-item {
    border: solid 1px ${Base.CinzaDesabilitado} !important;
  }
`;
