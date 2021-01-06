import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import { Loader } from '~/componentes';
import {
  setAnotacoesAluno,
  setAnotacoesPedagogicas,
  setAuditoriaAnotacaoRecomendacao,
  setConselhoClasseEmEdicao,
  setDentroPeriodo,
  setRecomendacaoAluno,
  setRecomendacaoFamilia,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import AnotacoesAluno from './AnotacoesAluno/anotacoesAluno';
import AnotacoesPedagogicas from './AnotacoesPedagogicas/anotacoesPedagogicas';
import AuditoriaAnotacaoRecomendacao from './AuditoriaAnotacaoRecomendacao/auditoriaAnotacaoRecomendacao';
import RecomendacaoAlunoFamilia from './RecomendacaoAlunoFamilia/recomendacaoAlunoFamilia';

const AnotacoesRecomendacoes = props => {
  const { codigoTurma, bimestre } = props;
  const dispatch = useDispatch();

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  const {
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
    alunoDesabilitado,
  } = dadosPrincipaisConselhoClasse;

  const [dadosIniciais, setDadosIniciais] = useState({
    anotacoesPedagogicas: '',
    recomendacaoAluno: '',
    recomendacaoFamilia: '',
    anotacoesAluno: '',
  });

  const [exibir, setExibir] = useState(false);
  const [carregando, setCarregando] = useState(false);

  // TODO Validar a necessidade de chamar quando esta alterando um registro ou usar somente quando for carergar dados na tela!
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

  const setarDentroDoPeriodo = useCallback(
    valor => {
      dispatch(setDentroPeriodo(valor));
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

  const obterAnotacoesRecomendacoes = useCallback(async () => {
    setCarregando(true);

    const resposta = await ServicoConselhoClasse.obterAnotacoesRecomendacoes(
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo,
      codigoTurma,
      bimestre.valor
    ).catch(e => erros(e));

    if (resposta && resposta.data) {
      if (!alunoDesabilitado) {
        setarDentroDoPeriodo(!resposta.data.somenteLeitura);
      }
      onChangeAnotacoesPedagogicas(resposta.data.anotacoesPedagogicas);
      onChangeRecomendacaoAluno(resposta.data.recomendacaoAluno);
      onChangeRecomendacaoFamilia(resposta.data.recomendacaoFamilia);
      setarAnotacaoAluno(resposta.data.anotacoesAluno);
      setarAuditoria(resposta.data);
      setExibir(true);
    } else {
      setExibir(false);
    }
    setCarregando(false);
  }, [
    alunoCodigo,
    conselhoClasseId,
    fechamentoTurmaId,
    onChangeAnotacoesPedagogicas,
    onChangeRecomendacaoAluno,
    onChangeRecomendacaoFamilia,
    setarAnotacaoAluno,
    setarAuditoria,
    setarDentroDoPeriodo,
    alunoDesabilitado,
    bimestre,
  ]);

  useEffect(() => {
    if (alunoCodigo) {
      obterAnotacoesRecomendacoes();
    }
  }, [fechamentoTurmaId, alunoCodigo, obterAnotacoesRecomendacoes]);

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
  codigoTurma: PropTypes.oneOfType([PropTypes.string]),
  bimestre: PropTypes.oneOfType([PropTypes.number]),
};

AnotacoesRecomendacoes.defaultProps = {
  codigoTurma: '',
  bimestre: 0,
};

export default AnotacoesRecomendacoes;
