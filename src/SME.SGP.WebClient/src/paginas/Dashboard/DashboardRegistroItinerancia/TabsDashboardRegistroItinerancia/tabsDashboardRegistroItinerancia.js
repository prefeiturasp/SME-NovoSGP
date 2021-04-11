import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { ContainerTabsDashboard } from '../../style';
import GraficosRegistroItinerancia from './GraficosRegistroItinerancia/graficosRegistroItinerancia';

const { TabPane } = Tabs;

const TabsDashboardRegistroItinerancia = props => {
  const { anoLetivo, dreId, ueId, mesSelecionado } = props;

  const [tabSelecionada, setTabSelecionada] = useState();

  const TAB_REGISTROS = '1';

  useEffect(() => {
    if (anoLetivo && dreId && ueId && mesSelecionado) {
      setTabSelecionada(TAB_REGISTROS);
    } else {
      setTabSelecionada();
    }
  }, [anoLetivo, dreId, ueId, mesSelecionado]);

  const onChangeTab = tabAtiva => {
    setTabSelecionada(tabAtiva);
  };

  return (
    <>
      {anoLetivo && dreId && ueId && mesSelecionado ? (
        <ContainerTabsDashboard>
          <ContainerTabsCard
            type="card"
            onChange={onChangeTab}
            activeKey={tabSelecionada}
          >
            <TabPane tab="Registros" key={TAB_REGISTROS}>
              <GraficosRegistroItinerancia
                anoLetivo={anoLetivo}
                dreId={dreId}
                ueId={ueId}
                mesSelecionado={mesSelecionado}
              />
            </TabPane>
          </ContainerTabsCard>
        </ContainerTabsDashboard>
      ) : (
        ''
      )}
    </>
  );
};

TabsDashboardRegistroItinerancia.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  ueId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  dreId: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  mesSelecionado: PropTypes.string,
};

TabsDashboardRegistroItinerancia.defaultProps = {
  anoLetivo: null,
  dreId: null,
  ueId: null,
  mesSelecionado: '',
};

export default TabsDashboardRegistroItinerancia;
