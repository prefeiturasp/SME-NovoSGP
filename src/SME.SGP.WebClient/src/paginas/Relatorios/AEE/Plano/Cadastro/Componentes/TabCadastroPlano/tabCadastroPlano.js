import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import { useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import SecaoPlanoCollapse from '../SecaoPlanoCollapse/secaoPlanoCollapse';
import SecaoReestruturacaoPlano from '../SecaoReestruturacaoPlano/secaoReestruturacaoPlano';
import SecaoDevolutivasPlanoCollapse from '../SecaoDevolutivasPlano/secaoDevolutivasPlanoCollapse';
import { situacaoPlanoAEE } from '~/dtos';

const { TabPane } = Tabs;

const TabCadastroPasso = props => {
  const { match } = props;
  const temId = match?.params?.id;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);

  const cliqueTab = async key => {
    ServicoPlanoAEE.cliqueTabPlanoAEE(key, temId);
  };

  return dadosCollapseLocalizarEstudante?.codigoAluno ? (
    <ContainerTabsCard type="card" width="20%" onTabClick={cliqueTab}>
      <TabPane tab="Cadastro do Plano" key="1">
        {dadosCollapseLocalizarEstudante?.codigoAluno ? (
          <SecaoPlanoCollapse match={match} />
        ) : (
          ''
        )}
      </TabPane>
      {temId && (
        <TabPane
          tab="Reestruturação"
          key="2"
          disabled={
            planoAEEDados?.situacao !== situacaoPlanoAEE.EmAndamento &&
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
          disabled={planoAEEDados?.situacao === situacaoPlanoAEE.EmAndamento}
        >
          <SecaoDevolutivasPlanoCollapse match={match} />
        </TabPane>
      )}
    </ContainerTabsCard>
  ) : (
    ''
  );
};

TabCadastroPasso.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

TabCadastroPasso.defaultProps = {
  match: {},
};

export default TabCadastroPasso;
