import { Tabs } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import modalidadeDto from '~/dtos/modalidade';
import {
  setBimestreAtual,
  setDadosPrincipaisConselhoClasse,
  setFechamentoPeriodoInicioFim,
  setExpandirLinha,
  setNotaConceitoPosConselhoAtual,
  setIdCamposNotasPosConselho,
  setDesabilitarCampos,
  setConselhoClasseEmEdicao,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import servicoSalvarConselhoClasse from '../servicoSalvarConselhoClasse';
import AlertaDentroPeriodo from './AlertaDentroPeriodo/alertaDentroPeriodo';
import AnotacoesRecomendacoes from './AnotacoesRecomendacoes/anotacoesRecomendacoes';
import ListasNotasConceitos from './ListasNotasConceito/listasNotasConceitos';
import MarcadorPeriodoInicioFim from './MarcadorPeriodoInicioFim/marcadorPeriodoInicioFim';
import Sintese from './Sintese/Sintese';
import MarcadorParecerConclusivo from './MarcadorParecerConclusivo/marcadorParecerConclusivo';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import RotasDto from '~/dtos/rotasDto';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const { TabPane } = Tabs;

const DadosConselhoClasse = props => {
  const { turmaCodigo, modalidade } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const permissoesTela = usuario.permissoes[RotasDto.CONSELHO_CLASSE];

  const dispatch = useDispatch();

  const bimestreAtual = useSelector(
    store => store.conselhoClasse.bimestreAtual
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.conselhoClasse.dadosAlunoObjectCard
  );

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const { codigoEOL, desabilitado } = dadosAlunoObjectCard;

  const [semDados, setSemDados] = useState(true);
  const [carregando, setCarregando] = useState(false);

  const validaAbaFinal = useCallback(
    async (conselhoClasseId, fechamentoTurmaId, alunoCodigo) => {
      const resposta = await ServicoConselhoClasse.acessarAbaFinalParecerConclusivo(
        conselhoClasseId,
        fechamentoTurmaId,
        alunoCodigo
      ).catch(e => erros(e));
      if (resposta && resposta.data) {
        ServicoConselhoClasse.setarParecerConclusivo(resposta.data);
        return true;
      }
      return false;
    },
    []
  );

  const limparDadosNotaPosConselhoJustificativa = useCallback(() => {
    dispatch(setExpandirLinha([]));
    dispatch(setNotaConceitoPosConselhoAtual({}));
    dispatch(setIdCamposNotasPosConselho({}));
  }, [dispatch]);

  const validaPermissoes = useCallback(
    novoRegistro => {
      const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
        modalidadesFiltroPrincipal,
        turmaSelecionada
      );
      const somenteConsulta = verificaSomenteConsulta(
        permissoesTela,
        naoSetarSomenteConsultaNoStore
      );

      const desabilitar = novoRegistro
        ? somenteConsulta || !permissoesTela.podeIncluir
        : somenteConsulta || !permissoesTela.podeAlterar;

      dispatch(setDesabilitarCampos(desabilitar));
    },
    [dispatch, permissoesTela, turmaSelecionada, modalidadesFiltroPrincipal]
  );

  // Quando passa bimestre 0 o retorno vai trazer dados do bimestre corrente!
  const caregarInformacoes = useCallback(
    async (bimestreConsulta = 0, ehFinal = false) => {
      limparDadosNotaPosConselhoJustificativa();
      ServicoConselhoClasse.setarParecerConclusivo('');
      setCarregando(true);
      setSemDados(true);
      const retorno = await ServicoConselhoClasse.obterInformacoesPrincipais(
        turmaCodigo,
        usuario.turmaSelecionada.consideraHistorico && bimestreConsulta === 0
          ? '1'
          : ehFinal
          ? '0'
          : bimestreConsulta,
        codigoEOL,
        ehFinal,
        usuario.turmaSelecionada.consideraHistorico
      ).catch(e => {
        erros(e);
        if (e && e.response && e.response.status === 601) {
          dispatch(setBimestreAtual(bimestreConsulta || '1'));
        }
        dispatch(setDadosPrincipaisConselhoClasse({}));
        setSemDados(true);
      });
      if (retorno && retorno.data) {
        const {
          conselhoClasseId,
          fechamentoTurmaId,
          conselhoClasseAlunoId,
          bimestre,
          periodoFechamentoInicio,
          periodoFechamentoFim,
          tipoNota,
          media,
        } = retorno.data;

        const novoRegistro = !conselhoClasseId;
        validaPermissoes(novoRegistro);

        let podeAcessarAbaFinal = true;
        if (ehFinal) {
          const podeAcessar = await validaAbaFinal(
            conselhoClasseId,
            fechamentoTurmaId,
            codigoEOL
          ).catch(e => erros(e));
          podeAcessarAbaFinal = podeAcessar;
        }
        if (!podeAcessarAbaFinal) {
          dispatch(setBimestreAtual(bimestreConsulta));
          setCarregando(false);
          return;
        }

        const valores = {
          fechamentoTurmaId,
          conselhoClasseId: conselhoClasseId || 0,
          conselhoClasseAlunoId,
          alunoCodigo: codigoEOL,
          turmaCodigo,
          alunoDesabilitado: desabilitado,
          tipoNota,
          media,
        };

        dispatch(setDadosPrincipaisConselhoClasse(valores));

        const datas = {
          periodoFechamentoInicio,
          periodoFechamentoFim,
        };
        dispatch(setFechamentoPeriodoInicioFim(datas));

        if (periodoFechamentoFim) {
          ServicoConselhoClasse.carregarListaTiposConceito(
            periodoFechamentoFim
          );
        } else {
          ServicoConselhoClasse.carregarListaTiposConceito();
        }

        if (ehFinal) {
          dispatch(setBimestreAtual(bimestreConsulta));
        } else if (bimestre) {
          dispatch(setBimestreAtual(String(bimestre)));
        } else {
          dispatch(setBimestreAtual('1'));
        }
        setSemDados(false);
        setCarregando(false);
      } else {
        validaPermissoes(true);
      }
      setCarregando(false);
    },
    [
      codigoEOL,
      desabilitado,
      dispatch,
      limparDadosNotaPosConselhoJustificativa,
      turmaCodigo,
      validaAbaFinal,
      validaPermissoes,
    ]
  );

  useEffect(() => {
    if (codigoEOL && !bimestreAtual.valor) {
      caregarInformacoes();
    }
  }, [codigoEOL, bimestreAtual, caregarInformacoes]);

  const onChangeTab = async numeroBimestre => {
    let continuar = false;

    const validouNotaConceitoPosConselho = await servicoSalvarConselhoClasse.validarNotaPosConselho();

    if (validouNotaConceitoPosConselho) {
      const validouAnotacaoRecomendacao = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
      if (validouNotaConceitoPosConselho && validouAnotacaoRecomendacao) {
        continuar = true;
      }
    }

    if (continuar) {
      const ehFinal = numeroBimestre === 'final';
      caregarInformacoes(numeroBimestre, ehFinal);
    }
  };

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  useEffect(() => {
    dispatch(
      setConselhoClasseEmEdicao(
        !carregando &&
          !semDados &&
          !Object.entries(dadosPrincipaisConselhoClasse).length
      )
    );
  }, [dispatch, carregando, semDados, dadosPrincipaisConselhoClasse]);

  const montarDados = () => {
    return (
      <Loader loading={carregando} className={carregando ? 'text-center' : ''}>
        {!semDados ? (
          <>
            <AlertaDentroPeriodo />
            {bimestreAtual.valor === 'final' ? (
              <MarcadorParecerConclusivo />
            ) : null}
            <MarcadorPeriodoInicioFim />
            <ListasNotasConceitos bimestreSelecionado={bimestreAtual} />
            <Sintese
              ehFinal={bimestreAtual.valor === 'final'}
              bimestreSelecionado={bimestreAtual}
            />
            <AnotacoesRecomendacoes bimestreSelecionado={bimestreAtual} />
          </>
        ) : semDados && !carregando ? (
          <div className="text-center">Sem dados</div>
        ) : null}
      </Loader>
    );
  };

  console.log(modalidade);

  return (
    <>
      {codigoEOL && bimestreAtual.valor ? (
        <ContainerTabsCard
          type="card"
          onChange={onChangeTab}
          activeKey={bimestreAtual.valor}
          className={
            modalidade === modalidadeDto.EJA
              ? 'ant-tab-nav-33'
              : 'ant-tab-nav-20'
          }
        >
          <TabPane tab="1º Bimestre" key="1">
            {bimestreAtual.valor === '1' ? montarDados() : ''}
          </TabPane>
          <TabPane tab="2º Bimestre" key="2">
            {bimestreAtual.valor === '2' ? montarDados() : ''}
          </TabPane>
          {modalidade.toString() !== modalidadeDto.EJA.toString() ? (
            <TabPane tab="3º Bimestre" key="3">
              {bimestreAtual.valor === '3' ? montarDados() : ''}
            </TabPane>
          ) : (
            ''
          )}
          {modalidade.toString() !== modalidadeDto.EJA.toString() ? (
            <TabPane tab="4º Bimestre" key="4">
              {bimestreAtual.valor === '4' ? montarDados() : ''}
            </TabPane>
          ) : (
            ''
          )}
          <TabPane tab="Final" key="final">
            {bimestreAtual.valor === 'final' ? montarDados() : ''}
          </TabPane>
        </ContainerTabsCard>
      ) : (
        ''
      )}
    </>
  );
};

DadosConselhoClasse.propTypes = {
  turmaCodigo: PropTypes.oneOfType([PropTypes.any]),
  modalidade: PropTypes.oneOfType([PropTypes.any]),
};

DadosConselhoClasse.defaultProps = {
  turmaCodigo: '',
  modalidade: '',
};

export default DadosConselhoClasse;
