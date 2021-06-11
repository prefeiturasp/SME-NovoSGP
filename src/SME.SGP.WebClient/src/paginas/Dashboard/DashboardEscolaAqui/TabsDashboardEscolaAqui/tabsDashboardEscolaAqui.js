import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { ContainerTabsDashboardEscolaAqui } from '../dashboardEscolaAqui.css';
import DadosAdesao from './DadosAdesao/dadosAdesao';
import DadosComunicadosLeitura from './DadosComunicadosLeitura/dadosComunicadosLeitura';
import DadosComunicadosTotais from './DadosComunicadosTotais/dadosComunicadosTotais';

const { TabPane } = Tabs;

const TabsDashboardEscolaAqui = props => {
  const { codigoDre, codigoUe } = props;

  const [tabSelecionada, setTabSelecionada] = useState();

  useEffect(() => {
    if (!codigoDre || !codigoUe) {
      setTabSelecionada();
    }
  }, [codigoDre, codigoUe]);

  const montarDados = () => {
    return (
      <>
        <div className="col-md-12 mb-2">
          {tabSelecionada === '1' ? (
            <DadosAdesao codigoDre={codigoDre} codigoUe={codigoUe} />
          ) : (
            ''
          )}
          {tabSelecionada === '2' ? (
            <DadosComunicadosTotais codigoDre={codigoDre} codigoUe={codigoUe} />
          ) : (
            ''
          )}
          {tabSelecionada === '3' ? (
            <DadosComunicadosLeitura
              codigoDre={codigoDre}
              codigoUe={codigoUe}
            />
          ) : (
            ''
          )}
        </div>
      </>
    );
  };

  const onChangeTab = tabAtiva => {
    setTabSelecionada(tabAtiva);
  };

  return (
    <>
      {codigoDre && codigoUe ? (
        <ContainerTabsDashboardEscolaAqui>
          <ContainerTabsCard
            type="card"
            onChange={onChangeTab}
            activeKey={tabSelecionada}
          >
            <TabPane tab="Adesão" key="1">
              {montarDados()}
            </TabPane>
            <TabPane tab="Comunicados - Totais" key="2">
              {montarDados()}
            </TabPane>
            <TabPane tab="Comunicados - Leitura" key="3">
              {montarDados()}
            </TabPane>
          </ContainerTabsCard>
        </ContainerTabsDashboardEscolaAqui>
      ) : (
        ''
      )}
    </>
  );
};

TabsDashboardEscolaAqui.propTypes = {
  codigoDre: PropTypes.oneOfType([PropTypes.string]),
  codigoUe: PropTypes.oneOfType([PropTypes.string]),
};

TabsDashboardEscolaAqui.defaultProps = {
  codigoDre: '',
  codigoUe: '',
};

export default TabsDashboardEscolaAqui;
