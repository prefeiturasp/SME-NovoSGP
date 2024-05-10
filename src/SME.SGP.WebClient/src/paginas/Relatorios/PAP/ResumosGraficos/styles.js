import styled from 'styled-components';

// Ant
import { Tabs } from 'antd';

// Componentes
import { Base } from '~/componentes';

export const ContainerTabs = styled(Tabs)`
  width: 100%;

  .btn-imprimir {
    i {
      margin-right: 0px !important;
    }
  }

  .ant-tabs-tab {
    width: 100% !important;
    margin-right: 0px !important;
    border: 1px solid ${Base.CinzaDesabilitado} !important;
  }

  .ant-tabs-tab-active {
    border-bottom: 0 !important;
    color: ${Base.Roxo} !important;
    font-weight: bold;
  }

  .ant-tabs-tab:hover {
    color: ${Base.Roxo} !important;
  }

  .ant-tabs-nav {
    width: 50%;
    &.div:first-child {
      background: red;
      display: flex;
    }
  }
`;
