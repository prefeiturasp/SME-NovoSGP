import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { ContainerTabsDashboard } from '../../style';

const { TabPane } = Tabs;

const TabsDashboardAEE = props => {
  const { anoLetivo, codigoDre, codigoUe } = props;

  const [tabSelecionada, setTabSelecionada] = useState();

  useEffect(() => {
    if (!anoLetivo || !codigoDre || !codigoUe) {
      setTabSelecionada();
    }
  }, [anoLetivo, codigoDre, codigoUe]);

  const onChangeTab = tabAtiva => {
    setTabSelecionada(tabAtiva);
  };

  return (
    <>
      {anoLetivo && codigoDre && codigoUe ? (
        <ContainerTabsDashboard>
          <ContainerTabsCard
            type="card"
            onChange={onChangeTab}
            activeKey={tabSelecionada}
          >
            <TabPane tab="Encaminhamentos" key="1">
              Encaminhamentos
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
  codigoUe: PropTypes.string,
  codigoDre: PropTypes.string,
};

TabsDashboardAEE.defaultProps = {
  anoLetivo: null,
  codigoDre: '',
  codigoUe: '',
};

export default TabsDashboardAEE;
