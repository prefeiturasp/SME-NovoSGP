import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { RotasDto } from '~/dtos';
import { setDesabilitarCamposAcompanhamentoAprendizagem } from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { verificaSomenteConsulta } from '~/servicos';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';
import DadosGerais from './Tabs/DadosGerais/dadosGerais';
import RegistrosFotos from './Tabs/RegistrosFotos/registrosFotos';

const { TabPane } = Tabs;

const DadosAcompanhamentoAprendizagem = props => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { codigoEOL } = dadosAlunoObjectCard;

  const { semestreSelecionado } = props;

  const permissoesTela =
    usuario.permissoes[RotasDto.ACOMPANHAMENTO_APRENDIZAGEM];

  const [tabAtual, setTabAtual] = useState('1');

  const TAB_DADOS_GERAIS = '1';
  const TAB_REGISTROS_FOTOS = '2';
  const TAB_OBSERVACOES = '3';
  const TAB_DIETA_ESPECIAL = '4';

  const onChangeTab = numeroTab => {
    setTabAtual(numeroTab);
  };

  const validaPermissoes = useCallback(
    (novoRegistro, podeEditar) => {
      const somenteConsulta = verificaSomenteConsulta(permissoesTela);

      const desabilitar =
        dadosAlunoObjectCard.desabilitado ||
        !podeEditar ||
        (novoRegistro
          ? somenteConsulta || !permissoesTela.podeIncluir
          : somenteConsulta || !permissoesTela.podeAlterar);

      dispatch(setDesabilitarCamposAcompanhamentoAprendizagem(desabilitar));
    },
    [dispatch, dadosAlunoObjectCard, permissoesTela]
  );

  const obterDadosAcompanhamentoAprendizagemPorEstudante = async () => {
    const retorno = await ServicoAcompanhamentoAprendizagem.obterAcompanhamentoEstudante(
      turmaSelecionada?.id,
      codigoEOL,
      semestreSelecionado
    );

    const { acompanhamentoAlunoSemestreId, podeEditar } = retorno;
    validaPermissoes(acompanhamentoAlunoSemestreId, podeEditar);
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
