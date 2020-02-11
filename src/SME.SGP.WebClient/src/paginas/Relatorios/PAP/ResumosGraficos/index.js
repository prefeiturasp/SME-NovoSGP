import React, { useState, useEffect, lazy } from 'react';

// Redux
import { useSelector } from 'react-redux';

// Ant
import { Tabs } from 'antd';

// Componentes
import { Card, ButtonGroup, LazyLoad, Loader } from '~/componentes';
import Filtro from './componentes/Filtro';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Estilos
import { ContainerTabs } from './styles';

// Serviços
import { erro } from '~/servicos/alertas';
import ResumosGraficosPAPServico from '~/servicos/Paginas/Relatorios/PAP/ResumosGraficos';

import RotasDto from '~/dtos/rotasDto';

const ResumosGraficosPAP = () => {
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const [somenteConsulta] = useState(false);
  const [tabAtiva, setTabAtiva] = useState('relatorios');
  const [filtro, setFiltro] = useState({});
  const [dados, setDados] = useState({});
  const [carregandoRelatorios, setCarregandoRelatorios] = useState(false);
  const [carregandoGraficos, setCarregandoGraficos] = useState(false);

  const onClickVoltar = () => {};

  const Resumos = lazy(() => import('./componentes/Resumos'));
  const Graficos = lazy(() => import('./componentes/Graficos'));

  useEffect(() => {
    async function buscarDados() {
      try {
        setCarregandoGraficos(true);
        setCarregandoRelatorios(true);
        const {
          data,
          status,
        } = await ResumosGraficosPAPServico.ListarFrequencia(filtro);
        if (data && status === 200) {
          setCarregandoGraficos(false);
          setCarregandoRelatorios(false);
          setDados(data);
        }
      } catch (err) {
        setCarregandoGraficos(false);
        setCarregandoRelatorios(false);
        erro(`Não foi possível completar a requisição! ${err}`);
      }
    }
    buscarDados();
  }, [filtro]);

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
        <Filtro onFiltrar={filtroAtual => setFiltro(filtroAtual)} />
        <ContainerTabs
          tabPosition="top"
          type="card"
          tabBarGutter={10}
          onChange={key => setTabAtiva(key)}
          activeKey={tabAtiva}
          defaultActiveKey="relatorios"
        >
          <Tabs.TabPane tab="Relatórios" key="relatorios">
            <Loader loading={carregandoRelatorios}>
              {tabAtiva === 'relatorios' ? (
                <LazyLoad>
                  <Resumos filtro={filtro} dados={dados} />
                </LazyLoad>
              ) : (
                ''
              )}
            </Loader>
          </Tabs.TabPane>
          <Tabs.TabPane tab="Gráficos" key="graficos">
            <Loader loading={carregandoGraficos}>
              {tabAtiva === 'graficos' ? (
                <LazyLoad>
                  <Graficos dados={dados} />
                </LazyLoad>
              ) : (
                ''
              )}
            </Loader>
          </Tabs.TabPane>
        </ContainerTabs>
      </Card>
    </>
  );
};

export default ResumosGraficosPAP;
