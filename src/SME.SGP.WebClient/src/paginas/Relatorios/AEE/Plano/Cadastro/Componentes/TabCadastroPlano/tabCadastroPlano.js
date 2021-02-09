import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import { Tabs } from 'antd';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import SecaoPlanoCollapse from '../SecaoPlanoCollapse/secaoPlanoCollapse';

const { TabPane } = Tabs;

const TabCadastroPasso = props => {
  const { match } = props;
  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );

  return (
    <ContainerTabsCard type="card" width="20%">
      <TabPane tab="Cadastro do Plano" key="1">
        {dadosCollapseLocalizarEstudante?.codigoAluno ? (
          <SecaoPlanoCollapse match={match} />
        ) : (
          ''
        )}
      </TabPane>
      <TabPane tab="Devolutivas" disabled key="2">
        <></>
      </TabPane>
    </ContainerTabsCard>
  );
};

TabCadastroPasso.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

TabCadastroPasso.defaultProps = {
  match: {},
};

export default TabCadastroPasso;
