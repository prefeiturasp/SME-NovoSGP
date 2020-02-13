import React, { useState, useEffect, useMemo, lazy } from 'react';

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
  const TabGraficos = lazy(() => import('./componentes/TabGraficos'));

  const filtroTela = useMemo(() => {
    return {
      DreId: filtro.dreId,
      UeId: filtro.ueId,
      CicloId: filtro.cicloId,
      TurmaId: filtro.turmaId,
      Periodo: filtro.periodo,
      Ano: filtro.ano,
    };
  }, [filtro]);

  useEffect(() => {
    async function buscarDados() {
      try {
        setCarregandoGraficos(true);
        setCarregandoRelatorios(true);
        const requisicoes = await Promise.all([
          ResumosGraficosPAPServico.ListarTotalEstudantes(filtroTela),
          ResumosGraficosPAPServico.ListarFrequencia(filtroTela),
          ResumosGraficosPAPServico.ListarResultados(filtroTela),
          ResumosGraficosPAPServico.ListarInformacoesEscolares(filtroTela),
        ]);

        setDados({
          totalEstudantes: { ...requisicoes[0].data },
          frequencia: [...requisicoes[1].data.frequencia],
          resultados: { ...requisicoes[2].data },
          informacoesEscolares: { ...requisicoes[3].data },
        });

        setCarregandoGraficos(false);
        setCarregandoRelatorios(false);
      } catch (err) {
        setCarregandoGraficos(false);
        setCarregandoRelatorios(false);
        erro(`Não foi possível completar a requisição! ${err}`);
      }
    }
    buscarDados();
  }, [filtroTela]);

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
          <Tabs.TabPane tab="Resumos" key="relatorios">
            <Loader loading={carregandoRelatorios}>
              {tabAtiva === 'relatorios' ? (
                <LazyLoad>
                  <Resumos dados={dados} />
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
                  <TabGraficos dados={dados} />
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
