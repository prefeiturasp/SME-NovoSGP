import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerTabsCard } from './tabs.css';

const { TabPane } = Tabs;

const TabsComponent = props => {
  const { onChangeTab, listaTabs } = props;
  const [exibirTabs, setExibirTabs] = useState(false);

  useEffect(() => {
    setExibirTabs(listaTabs.length > 0);
  }, [listaTabs]);

  const montarTabs = () => {
    return listaTabs.map((tab, index) => {
      const { nome, conteudo: Conteudo } = tab;
      return (
        <TabPane tab={nome} key={`tab-${index}`}>
          {typeof Conteudo === 'object' || typeof Conteudo === 'function' ? (
            <Conteudo />
          ) : (
            Conteudo
          )}
        </TabPane>
      );
    });
  };

  return (
    <ContainerTabsCard activeKey="tab-0" onChange={onChangeTab} type="card">
      {exibirTabs ? montarTabs() : ''}
    </ContainerTabsCard>
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
