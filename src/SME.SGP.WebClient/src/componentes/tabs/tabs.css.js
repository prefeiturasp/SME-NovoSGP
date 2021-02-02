import styled from 'styled-components';
import { Tabs } from 'antd';

import { Base } from '../colors';

export const ContainerTabsCard = styled(Tabs)`

  width: 100% !important;

  .ant-tabs-tab-next {
    display: none;
  }

  .ant-tabs-tab-prev {
    display: none;
  }

  .ant-tabs-nav {
    width: ${props => (props.width ? props.width : '25%')};
  }

  .ant-tabs-tab {
    width: 100% !important;
    margin-right: 0px !important;
    border: 1px solid ${Base.CinzaDesabilitado} !important;

    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  .ant-tabs-nav .ant-tabs-tab:hover {
    color: rgba(0, 0, 0, 0.65);
  }

  .ant-tabs-tab-active:hover {
    color: ${Base.Roxo} !important;
  }
  .ant-tabs-tab-active {
    color: ${Base.Roxo} !important;
    border-bottom: 1px solid #fff !important;
  }

  .ant-tabs-nav-container-scrolling {
    padding-right: 0px;
    padding-left: 0px;
  }
`;
