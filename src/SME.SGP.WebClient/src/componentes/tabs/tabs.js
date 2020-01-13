import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { Container } from './tabs.css';

const { TabPane } = Tabs;

const TabsComponent = props => {
  const { onChangeTab, listaTabs } = props;
  const [exibirTabs, setExibirTabs] = useState(false);

  useEffect(() => {
    setExibirTabs(listaTabs.length > 0);
  }, [listaTabs]);

  const montarTabs = () => {
    return listaTabs.map((tab, index) => {
      return (
        <TabPane tab={tab.nome} key={`tab-${index}`}>
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
