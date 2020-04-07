import React, { useState } from 'react';
import { Tabs } from 'antd';
import Card from '~/componentes/card';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import ListaNotasConselho from './ListaNotasConselho/ListaNotasConselho';

const DadosConselhoClasse = () => {
  const [abaAtiva, setAbaAtiva] = useState('1');
  const onChangeTab = chaveAba => {
    setAbaAtiva(chaveAba);
  };
  const { TabPane } = Tabs;
  return (
    <>
      <Card>
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={abaAtiva}
        >
          <TabPane tab="1º Bimestre" key="1">
            <ListaNotasConselho />
          </TabPane>
          <TabPane tab="2º Bimestre" key="2">
            teste
          </TabPane>
          <TabPane tab="Final" key="final">
            teste
          </TabPane>
        </ContainerTabsCard>
      </Card>
    </>
  );
};

export default DadosConselhoClasse;
