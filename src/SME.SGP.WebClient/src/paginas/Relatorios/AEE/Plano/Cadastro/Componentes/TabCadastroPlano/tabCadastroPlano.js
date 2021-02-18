import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import { Tabs } from 'antd';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import SecaoPlanoCollapse from '../SecaoPlanoCollapse/secaoPlanoCollapse';
import SecaoReestruturacaoPlano from '../SecaoReestruturacaoPlano/secaoReestruturacaoPlano';
import { confirmar, sucesso } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';

const { TabPane } = Tabs;

const TabCadastroPasso = props => {
  const { match } = props;
  const temId = match?.params?.id;

  const dadosCollapseLocalizarEstudante = useSelector(
    store => store.collapseLocalizarEstudante.dadosCollapseLocalizarEstudante
  );
  const questionarioDinamicoEmEdicao = useSelector(
    store => store.questionarioDinamico.questionarioDinamicoEmEdicao
  );

  const dispatch = useDispatch();

  const cliqueTab = async key => {
    if (questionarioDinamicoEmEdicao && key !== '1') {
      const confirmou = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmou) {
        const salvou = await ServicoPlanoAEE.salvarPlano();
        if (salvou) {
          dispatch(setQuestionarioDinamicoEmEdicao(false));
          const mensagem = temId
            ? 'Registro alterado com sucesso'
            : 'Registro salvo com sucesso';
          sucesso(mensagem);
        }
      }
    }
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
        <TabPane tab="Reestruturação" key="2">
          <SecaoReestruturacaoPlano match={match} />
        </TabPane>
      )}
      {temId && (
        <TabPane tab="Devolutivas" disabled key="3">
          <></>
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
