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
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import servicoSalvarConselhoClasse from '../servicoSalvarConselhoClasse';
import AlertaDentroPeriodo from './AlertaDentroPeriodo/alertaDentroPeriodo';
import AnotacoesRecomendacoes from './AnotacoesRecomendacoes/anotacoesRecomendacoes';
import ListasNotasConceitos from './ListasNotasConceito/listasNotasConceitos';
import MarcadorPeriodoInicioFim from './MarcadorPeriodoInicioFim/marcadorPeriodoInicioFim';
import Sintese from './Sintese/Sintese';

const { TabPane } = Tabs;

const DadosConselhoClasse = props => {
  const { turmaCodigo, modalidade } = props;

  const dispatch = useDispatch();

  const bimestreAtual = useSelector(
    store => store.conselhoClasse.bimestreAtual
  );

  const dadosAlunoObjectCard = useSelector(
    store => store.conselhoClasse.dadosAlunoObjectCard
  );

  const { codigoEOL, desabilitado } = dadosAlunoObjectCard;

  const [semDados, setSemDados] = useState(true);
  const [carregando, setCarregando] = useState(false);

  const validaAbaFinal = async (
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo
  ) => {
    const resposta = await ServicoConselhoClasse.acessarAbaFinalParecerConclusivo(
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo
    ).catch(e => erros(e));
    if (resposta && resposta.data) {
      return true;
    }
    return false;
  };

  // Quando passa bimestre 0 o retorno vai trazer dados do bimestre corrente!
  const caregarInformacoes = useCallback(
    async (bimestreConsulta = 0, ehFinal = false) => {
      setCarregando(true);
      setSemDados(true);
      const retorno = await ServicoConselhoClasse.obterInformacoesPrincipais(
        turmaCodigo,
        ehFinal ? '0' : bimestreConsulta,
        codigoEOL,
        ehFinal
      ).catch(e => {
        erros(e);
        if (e && e.response && e.response.status === 601) {
          dispatch(setBimestreAtual(bimestreConsulta || '1'));
        }
        setSemDados(true);
      });
      if (retorno && retorno.data) {
        const {
          conselhoClasseId,
          fechamentoTurmaId,
          bimestre,
          periodoFechamentoInicio,
          periodoFechamentoFim,
          tipoNota,
          media,
        } = retorno.data;

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
      }
      setCarregando(false);
    },
    [codigoEOL, desabilitado, turmaCodigo, dispatch]
  );

  useEffect(() => {
    if (codigoEOL && !bimestreAtual.valor) {
      caregarInformacoes();
    }
  }, [codigoEOL, bimestreAtual, caregarInformacoes]);

  const onChangeTab = async numeroBimestre => {
    const continuar = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
    if (continuar) {
      const ehFinal = numeroBimestre === 'final';
      caregarInformacoes(numeroBimestre, ehFinal);
    }
  };

  const montarDados = () => {
    return (
      <Loader loading={carregando} className={carregando ? 'text-center' : ''}>
        {!semDados ? (
          <>
            <AlertaDentroPeriodo />
            <MarcadorPeriodoInicioFim />
            <ListasNotasConceitos bimestreSelecionado={bimestreAtual} />
            <Sintese
              ehFinal={bimestreAtual.valor === 'final'}
              bimestreSelecionado={bimestreAtual}
            />
            <AnotacoesRecomendacoes bimestreSelecionado={bimestreAtual} />
          </>
        ) : semDados && !carregando ? (
          'Sem dados'
        ) : (
          ''
        )}
      </Loader>
    );
  };

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
          {modalidade !== modalidadeDto.EJA ? (
            <TabPane tab="3º Bimestre" key="3">
              {bimestreAtual.valor === '3' ? montarDados() : ''}
            </TabPane>
          ) : (
            ''
          )}
          {modalidade !== modalidadeDto.EJA ? (
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
