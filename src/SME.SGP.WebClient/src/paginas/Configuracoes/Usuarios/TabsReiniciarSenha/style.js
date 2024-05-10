import styled from 'styled-components';
import { Tabs } from 'antd';

import { Base } from '../../../../componentes/colors';

export const ContainerTabs = styled(Tabs)`
  .ant-tabs-tab {
    margin-right: 0px !important;
    border: 1px solid ${Base.CinzaDesabilitado} !important;
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
