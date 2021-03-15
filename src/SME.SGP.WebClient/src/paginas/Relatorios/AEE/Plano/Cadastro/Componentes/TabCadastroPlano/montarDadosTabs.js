import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { situacaoPlanoAEE } from '~/dtos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import SecaoDevolutivasPlanoCollapse from '../SecaoDevolutivasPlano/secaoDevolutivasPlanoCollapse';
import SecaoPlanoCollapse from '../SecaoPlanoCollapse/secaoPlanoCollapse';
import SecaoReestruturacaoPlano from '../SecaoReestruturacaoPlano/secaoReestruturacaoPlano';

const { TabPane } = Tabs;

const MontarDadosTabs = props => {
  const { match } = props;
  const temId = match?.params?.id;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  // Seção pode voltar no futuro!
  const exibirTabReestruturacao = false;

  const cliqueTab = async key => {
    ServicoPlanoAEE.cliqueTabPlanoAEE(key, temId);
  };

  return dadosCollapseLocalizarEstudante?.codigoAluno ? (
    <ContainerTabsCard type="card" width="20%" onTabClick={cliqueTab}>
      <TabPane tab="Cadastro do Plano" key="1">
        <SecaoPlanoCollapse match={match} />
      </TabPane>
      {temId && exibirTabReestruturacao && (
        <TabPane
          tab="Reestruturação"
          key="2"
          disabled={
            planoAEEDados?.situacao !== situacaoPlanoAEE.EmAndamento &&
            planoAEEDados?.situacao !== situacaoPlanoAEE.Expirado &&
            planoAEEDados?.situacao !== situacaoPlanoAEE.Reestruturado &&
            planoAEEDados?.situacao !== situacaoPlanoAEE.Encerrado &&
            planoAEEDados?.situacao !==
              situacaoPlanoAEE.EncerradoAutomaticamento
          }
        >
          <SecaoReestruturacaoPlano match={match} />
        </TabPane>
      )}
      {temId && (
        <TabPane
          tab="Devolutivas"
          key="3"
          disabled={
            planoAEEDados?.situacao === situacaoPlanoAEE.EmAndamento ||
            planoAEEDados?.situacao === situacaoPlanoAEE.Expirado ||
            planoAEEDados?.situacao === situacaoPlanoAEE.Reestruturado
          }
        >
          <SecaoDevolutivasPlanoCollapse match={match} />
        </TabPane>
      )}
    </ContainerTabsCard>
  ) : (
    ''
  );
};

MontarDadosTabs.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

MontarDadosTabs.defaultProps = {
  match: {},
};

export default MontarDadosTabs;
