import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import DadosGerais from './Tabs/DadosGerais/dadosGerais';
import RegistrosFotos from './Tabs/RegistrosFotos/registrosFotos';

const { TabPane } = Tabs;

const DadosAcompanhamentoAprendizagem = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [tabAtual, setTabAtual] = useState('1');

  const onChangeTab = numeroTab => {
    setTabAtual(numeroTab);
  };

  return (
    <>
      {codigoEOL && semestreSelecionado ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={tabAtual}
        >
          <TabPane tab="Dados gerais" key="1">
            <DadosGerais semestreSelecionado={semestreSelecionado} />
          </TabPane>
          <TabPane tab="Registros e fotos" key="2">
            <RegistrosFotos semestreSelecionado={semestreSelecionado} />
          </TabPane>
          <TabPane tab="Observações" key="3" disabled>
            Observações
          </TabPane>
          <TabPane tab="Dieta especial" key="4" disabled>
            Dieta especial
          </TabPane>
        </ContainerTabsCard>
      ) : (
        ''
      )}
    </>
  );
};

DadosAcompanhamentoAprendizagem.propTypes = {
  semestreSelecionado: PropTypes.string,
};

DadosAcompanhamentoAprendizagem.defaultProps = {
  semestreSelecionado: '',
};

export default DadosAcompanhamentoAprendizagem;
