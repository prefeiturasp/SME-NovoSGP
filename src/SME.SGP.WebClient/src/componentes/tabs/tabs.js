import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

import { Base } from '../colors';

const { TabPane } = Tabs;

const TabsComponent = props => {
  const { onChangeTab, listaTabs } = props;

  const [tamanhoTabs, setTamanhoTabs] = useState();
  const [exibirTabs, setExibirTabs] = useState(false);

  useEffect(() => {
    setTamanhoTabs(`${100 / listaTabs.length}%`);
    setExibirTabs(listaTabs.length > 0);
  }, [listaTabs]);

  const Container = styled(Tabs)`
    width: 100% !important;

    .ant-tabs-tab-next {
      display: none;
    }

    .ant-tabs-tab-prev {
      display: none;
    }

    .ant-tabs-nav {
      width: ${tamanhoTabs};
    }

    .ant-tabs-tab {
      width: 100% !important;
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

  const montarTabs = () => {
    return listaTabs.map((tab, index) => {
      return (
        <TabPane tab={tab.nome} key={index}>
          {tab.conteudo}
        </TabPane>
      );
    });
  };

  return (
    <Container onChange={onChangeTab} type="card">
      {exibirTabs ? montarTabs() : ''}
    </Container>
  );
};

TabsComponent.propTypes = {
  onChangeTab: PropTypes.func,
  listaTabs: PropTypes.array,
};

TabsComponent.defaultProps = {
  onChangeTab: () => {},
  listaTabs: [],
};

export default TabsComponent;
