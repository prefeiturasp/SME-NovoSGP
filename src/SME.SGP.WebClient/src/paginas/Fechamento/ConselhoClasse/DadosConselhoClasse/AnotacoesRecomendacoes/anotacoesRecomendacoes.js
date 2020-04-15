import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import {
  setAnotacoesAluno,
  setAnotacoesPedagogicas,
  setRecomendacaoAluno,
  setRecomendacaoFamilia,
  setConselhoClasseEmEdicao,
  setDadosAnotacoesRecomendacoes,
  setAuditoriaAnotacaoRecomendacao,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import AnotacoesAluno from './AnotacoesAluno/anotacoesAluno';
import AnotacoesPedagogicas from './AnotacoesPedagogicas/anotacoesPedagogicas';
import RecomendacaoAlunoFamilia from './RecomendacaoAlunoFamilia/recomendacaoAlunoFamilia';
import { Loader } from '~/componentes';
import AuditoriaAnotacaoRecomendacao from './AuditoriaAnotacaoRecomendacao/auditoriaAnotacaoRecomendacao';

const AnotacoesRecomendacoes = props => {
  const {
    bimestreSelecionado,
    codigoTurma,
    modalidade,
    codigoEOL,
    alunoDesabilitado,
  } = props;

  const dispatch = useDispatch();

  const [dadosIniciais, setDadosIniciais] = useState({
    anotacoesPedagogicas: '',
    recomendacaoAluno: '',
    recomendacaoFamilia: '',
    anotacoesAluno: '',
  });

  const [exibir, setExibir] = useState(false);
  const [carregando, setCarregando] = useState(false);

  const onChangeAnotacoesRecomendacoes = useCallback(
    (valor, campo) => {
      const dadosDto = dadosIniciais;
      dadosDto[campo] = valor;
      setDadosIniciais(dadosDto);
    },
    [dadosIniciais]
  );

  const onChangeAnotacoesPedagogicas = useCallback(
    valor => {
      dispatch(setAnotacoesPedagogicas(valor));
      onChangeAnotacoesRecomendacoes(valor, 'anotacoesPedagogicas');
    },
    [dispatch, onChangeAnotacoesRecomendacoes]
  );

  const onChangeRecomendacaoAluno = useCallback(
    valor => {
      dispatch(setRecomendacaoAluno(valor));
      onChangeAnotacoesRecomendacoes(valor, 'recomendacaoAluno');
    },
    [dispatch, onChangeAnotacoesRecomendacoes]
  );

  const onChangeRecomendacaoFamilia = useCallback(
    valor => {
      dispatch(setRecomendacaoFamilia(valor));
      onChangeAnotacoesRecomendacoes(valor, 'recomendacaoFamilia');
    },
    [dispatch, onChangeAnotacoesRecomendacoes]
  );

  const setarAnotacaoAluno = useCallback(
    valor => {
      dispatch(setAnotacoesAluno(valor));
    },
    [dispatch]
  );

  const setarDados = useCallback(
    dados => {
      const valores = {
        fechamentoTurmaId: dados.fechamentoTurmaId,
        conselhoClasseId: dados.conselhoClasseId,
        bimestre: dados.bimestre,
        periodoFechamentoInicio: dados.periodoFechamentoInicio,
        periodoFechamentoFim: dados.periodoFechamentoFim,
      };
      dispatch(setDadosAnotacoesRecomendacoes(valores));
    },
    [dispatch]
  );

  const setarAuditoria = useCallback(
    dados => {
      const { auditoria } = dados;
      if (auditoria) {
        const auditoriaDto = {
          criadoEm: auditoria.criadoEm,
          criadoPor: auditoria.criadoPor,
          criadoRF: auditoria.criadoRF,
          alteradoEm: auditoria.alteradoEm,
          alteradoPor: auditoria.alteradoPor,
          alteradoRF: auditoria.alteradoRF,
        };
        dispatch(setAuditoriaAnotacaoRecomendacao(auditoriaDto));
      }
    },
    [dispatch]
  );

  const obterAnotacoesRecomendacoes = useCallback(
    async (bimestre, codigoAluno, turma, ehFinal) => {
      setCarregando(true);

      const resposta = await ServicoConselhoClasse.obterAnotacoesRecomendacoes(
        turma,
        codigoAluno,
        bimestre,
        modalidade,
        ehFinal
      ).catch(e => erros(e));

      if (resposta && resposta.data) {
        onChangeAnotacoesPedagogicas(resposta.data.anotacoesPedagogicas);
        onChangeRecomendacaoAluno(resposta.data.recomendacaoAluno);
        onChangeRecomendacaoFamilia(resposta.data.recomendacaoFamilia);
        setarAnotacaoAluno(resposta.data.anotacoesAluno);
        setarDados(resposta.data);
        setarAuditoria(resposta.data);
        setExibir(true);
      } else {
        setExibir(false);
      }
      setCarregando(false);
    },
    [
      modalidade,
      onChangeAnotacoesPedagogicas,
      onChangeRecomendacaoAluno,
      onChangeRecomendacaoFamilia,
      setarAnotacaoAluno,
      setarDados,
      setarAuditoria,
    ]
  );

  useEffect(() => {
    if (codigoTurma && codigoEOL) {
      const ehFinal = bimestreSelecionado.valor === 'final';
      obterAnotacoesRecomendacoes(
        ehFinal ? '0' : bimestreSelecionado.valor,
        codigoEOL,
        codigoTurma,
        ehFinal
      );
    }
  }, [
    bimestreSelecionado,
    codigoEOL,
    codigoTurma,
    obterAnotacoesRecomendacoes,
  ]);

  const setarConselhoClasseEmEdicao = emEdicao => {
    dispatch(setConselhoClasseEmEdicao(emEdicao));
  };

  return (
    <Loader
      className={carregando ? 'text-center' : ''}
      loading={carregando}
      tip="Carregando recomendações e anotações"
    >
      {exibir ? (
        <>
          <RecomendacaoAlunoFamilia
            alunoDesabilitado={alunoDesabilitado}
            onChangeRecomendacaoAluno={valor => {
              onChangeRecomendacaoAluno(valor);
              setarConselhoClasseEmEdicao(true);
            }}
            onChangeRecomendacaoFamilia={valor => {
              onChangeRecomendacaoFamilia(valor);
              setarConselhoClasseEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <AnotacoesPedagogicas
            alunoDesabilitado={alunoDesabilitado}
            onChange={valor => {
              onChangeAnotacoesPedagogicas(valor);
              setarConselhoClasseEmEdicao(true);
            }}
            dadosIniciais={dadosIniciais}
          />
          <AnotacoesAluno />
          <AuditoriaAnotacaoRecomendacao />
        </>
      ) : (
        ''
      )}
    </Loader>
  );
};

AnotacoesRecomendacoes.propTypes = {
  bimestreSelecionado: PropTypes.string,
  codigoTurma: PropTypes.string,
  modalidade: PropTypes.oneOfType([PropTypes.any]),
  codigoEOL: PropTypes.string,
  alunoDesabilitado: PropTypes.bool,
};

AnotacoesRecomendacoes.defaultProps = {
  bimestreSelecionado: '',
  codigoTurma: '',
  modalidade: '',
  codigoEOL: '',
  alunoDesabilitado: false,
};

export default AnotacoesRecomendacoes;
