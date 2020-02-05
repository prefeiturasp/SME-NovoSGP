import React, { useState, Suspense, lazy } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Ant
import { Tabs } from 'antd';

// Componentes
import { Card, ButtonGroup } from '~/componentes';
import Filtro from './componentes/Filtro';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Estilos
import { ContainerTabs } from './styles';

import RotasDto from '~/dtos/rotasDto';

const ResumosGraficosPAP = () => {
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const [somenteConsulta] = useState(false);
  const [tabAtiva, setTabAtiva] = useState('relatorios');

  const onClickVoltar = () => {};

  const Resumos = lazy(() => import('./componentes/Resumos'));
  const Graficos = lazy(() => import('./componentes/Graficos'));

  return (
    <>
      <Cabecalho pagina="Resumos e gráficos PAP" />
      <Card>
        <ButtonGroup
          somenteConsulta={somenteConsulta}
          permissoesTela={permissoesTela[RotasDto.PAP]}
          onClickVoltar={onClickVoltar}
          desabilitarBotaoPrincipal
        />
        <Filtro />
        <ContainerTabs
          tabPosition="top"
          type="card"
          tabBarGutter={10}
          onChange={key => setTabAtiva(key)}
          activeKey={tabAtiva}
          defaultActiveKey="relatorios"
        >
          <Tabs.TabPane tab="Relatórios" key="relatorios">
            {tabAtiva === 'relatorios' ? (
              <Suspense fallback={<h1>Carregando...</h1>}>
                <Resumos />
              </Suspense>
            ) : (
              ''
            )}
          </Tabs.TabPane>
          <Tabs.TabPane tab="Gráficos" key="graficos">
            {tabAtiva === 'graficos' ? (
              <Suspense fallback={<h1>Carregando...</h1>}>
                <Graficos />
              </Suspense>
            ) : (
              ''
            )}
          </Tabs.TabPane>
        </ContainerTabs>
      </Card>
    </>
  );
};

export default ResumosGraficosPAP;
