import React from 'react';
import PropTypes from 'prop-types';
import { Tabs } from 'antd';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import SecaoInformacoesPlanoCollapse from '../SecaoInformacoesPlano/secaoInformacoesPlanoCollapse';
import SecaoDevolutivasPlanoCollapse from '../SecaoDevolutivasPlano/secaoDevolutivasPlanoCollapse';

const { TabPane } = Tabs;

const TabCadastroPasso = ({ match }) => {
  return (
    <ContainerTabsCard type="card" width="20%">
      <TabPane tab="Cadastro do Plano" key="1">
        <SecaoInformacoesPlanoCollapse />
      </TabPane>
      <TabPane tab="Devolutivas" key="2">
        <SecaoDevolutivasPlanoCollapse />
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
