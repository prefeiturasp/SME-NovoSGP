import { Tabs } from 'antd';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import DadosGerais from './Tabs/DadosGerais/dadosGerais';

const { TabPane } = Tabs;

const DadosAcompanhamentoAprendizagem = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const { codigoEOL } = dadosAlunoObjectCard;

  const [tabAtual, setTabAtual] = useState('1');

  const onChangeTab = numeroTab => {
    setTabAtual(numeroTab);
  };

  return (
    <>
      {codigoEOL ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={tabAtual}
        >
          <TabPane tab="Dados gerais" key="1">
            <DadosGerais />
          </TabPane>
          <TabPane tab="Registros e fotos" key="2">
            Registros e fotos
          </TabPane>
          <TabPane tab="Observações" key="3">
            Observações
          </TabPane>
          <TabPane tab="Dieta especial" key="4">
            Dieta especial
          </TabPane>
        </ContainerTabsCard>
      ) : (
        ''
      )}
    </>
  );
};

export default DadosAcompanhamentoAprendizagem;
