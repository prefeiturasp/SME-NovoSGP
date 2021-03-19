import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { setDadosAcompanhamentoAprendizagem } from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { erros } from '~/servicos';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';
import DadosGerais from './Tabs/DadosGerais/dadosGerais';
import RegistrosFotos from './Tabs/RegistrosFotos/registrosFotos';

const { TabPane } = Tabs;

const DadosAcompanhamentoAprendizagem = props => {
  const dispatch = useDispatch();

  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const [tabAtual, setTabAtual] = useState('1');

  const TAB_DADOS_GERAIS = '1';
  const TAB_REGISTROS_FOTOS = '2';
  const TAB_OBSERVACOES = '3';
  const TAB_DIETA_ESPECIAL = '4';

  const onChangeTab = numeroTab => {
    setTabAtual(numeroTab);
  };

  const obterDadosAcompanhamentoAprendizagemPorEstudante = async () => {
    const retorno = await ServicoAcompanhamentoAprendizagem.obterAcompanhamentoEstudante(
      turmaSelecionada?.id,
      codigoEOL,
      semestreSelecionado
    ).catch(e => erros(e));

    if (retorno?.data) {
      dispatch(setDadosAcompanhamentoAprendizagem(retorno?.data));
    } else {
      dispatch(setDadosAcompanhamentoAprendizagem({}));
    }
  };

  useEffect(() => {
    if (turmaSelecionada && codigoEOL && semestreSelecionado) {
      obterDadosAcompanhamentoAprendizagemPorEstudante();
    }
  }, [turmaSelecionada, codigoEOL, semestreSelecionado]);

  return (
    <>
      {codigoEOL && semestreSelecionado ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={tabAtual}
        >
          <TabPane tab="Dados gerais" key={TAB_DADOS_GERAIS}>
            {tabAtual === TAB_DADOS_GERAIS ? (
              <DadosGerais semestreSelecionado={semestreSelecionado} />
            ) : (
              ''
            )}
          </TabPane>
          <TabPane tab="Registros e fotos" key={TAB_REGISTROS_FOTOS}>
            {tabAtual === TAB_REGISTROS_FOTOS ? (
              <RegistrosFotos semestreSelecionado={semestreSelecionado} />
            ) : (
              ''
            )}
          </TabPane>
          <TabPane tab="Observações" key={TAB_OBSERVACOES} disabled>
            {tabAtual === TAB_OBSERVACOES ? 'Observações' : ''}
          </TabPane>
          <TabPane tab="Dieta especial" key={TAB_DIETA_ESPECIAL} disabled>
            {tabAtual === TAB_DIETA_ESPECIAL ? 'Dieta especial' : ''}
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
