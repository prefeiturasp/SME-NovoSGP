import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { ContainerTabsDashboard } from '../../style';
import GraficosEncaminhamentos from './Encaminhamentos/graficosEncaminhamentos';

const { TabPane } = Tabs;

const TabsDashboardAEE = props => {
  const { anoLetivo, dreId, ueId } = props;

  const [tabSelecionada, setTabSelecionada] = useState();

  useEffect(() => {
    if (!anoLetivo || !dreId || !ueId) {
      setTabSelecionada();
    }
  }, [anoLetivo, dreId, ueId]);

  const onChangeTab = tabAtiva => {
    setTabSelecionada(tabAtiva);
  };

  return (
    <>
      {anoLetivo && dreId && ueId ? (
        <ContainerTabsDashboard>
          <ContainerTabsCard
            type="card"
            onChange={onChangeTab}
            activeKey={tabSelecionada}
          >
            <TabPane tab="Encaminhamentos" key="1">
              <GraficosEncaminhamentos
                anoLetivo={anoLetivo}
                dreId={dreId}
                ueId={ueId}
              />
            </TabPane>
            <TabPane tab="Planos" key="2">
              Planos
            </TabPane>
          </ContainerTabsCard>
        </ContainerTabsDashboard>
      ) : (
        ''
      )}
    </>
  );
};

TabsDashboardAEE.propTypes = {
  anoLetivo: PropTypes.oneOfType(PropTypes.any),
  ueId: PropTypes.string,
  dreId: PropTypes.string,
};

TabsDashboardAEE.defaultProps = {
  anoLetivo: null,
  dreId: '',
  ueId: '',
};

export default TabsDashboardAEE;
