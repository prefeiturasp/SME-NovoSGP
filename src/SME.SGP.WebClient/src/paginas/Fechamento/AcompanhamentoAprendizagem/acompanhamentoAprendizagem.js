import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AlertaPermiteSomenteTurmaInfantil } from '~/componentes-sgp';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import { RotasDto } from '~/dtos';
import situacaoMatriculaAluno from '~/dtos/situacaoMatriculaAluno';
import {
  limparDadosAcompanhamentoAprendizagem,
  setAlunosAcompanhamentoAprendizagem,
  setApanhadoGeralEmEdicao,
  setCodigoAlunoSelecionado,
  setDadosAlunoObjectCard,
  setDadosApanhadoGeral,
  setExibirLoaderGeralAcompanhamentoAprendizagem,
} from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import {
  resetarDadosRegistroIndividual,
  setComponenteCurricularSelecionado,
  setDadosAlunoObjectCard as setDadosAlunoObjectCardRegistroIndividual,
} from '~/redux/modulos/registroIndividual/actions';
import {
  ehTurmaInfantil,
  ServicoCalendarios,
  ServicoDisciplina,
  verificaSomenteConsulta,
} from '~/servicos';
import { erros } from '~/servicos/alertas';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';
import { Container } from './acompanhamentoAprendizagem.css';
import ApanhadoGeral from './DadosAcompanhamentoAprendizagem/ApanhadoGeral/apanhadoGeral';
import BotaoGerarRelatorioAprendizagem from './DadosAcompanhamentoAprendizagem/BotaoGerarRelatorioAprendizagem/botaoGerarRelatorioAprendizagem';
import BotaoOrdenarListaAlunos from './DadosAcompanhamentoAprendizagem/BotaoOrdenarListaAlunos/botaoOrdenarListaAlunos';
import BotoesAcoesAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/BotoesAcoes/botoesAcoesAcompanhamentoAprendizagem';
import DadosAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/dadosAcompanhamentoAprendizagem';
import ObjectCardAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/ObjectCardAcompanhamentoAprendizagem/objectCardAcompanhamentoAprendizagem';
import TabelaRetratilAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/TabelaRetratilAcompanhamentoAprendizagem/tabelaRetratilAcompanhamentoAprendizagem';
import LoaderAcompanhamentoAprendizagem from './loaderAcompanhamentoAprendizagem';

const AcompanhamentoAprendizagem = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma, anoLetivo } = turmaSelecionada;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );

  const permissoesTela =
    usuario.permissoes[RotasDto.ACOMPANHAMENTO_APRENDIZAGEM];

  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [listaSemestres, setListaSemestres] = useState([]);
  const [semestreSelecionado, setSemestreSelecionado] = useState(undefined);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosAcompanhamentoAprendizagem());
  }, [dispatch]);

  const obterComponentesCurriculares = useCallback(async () => {
    dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(true));
    const resposta = await ServicoDisciplina.obterDisciplinasPorTurma(turma)
      .catch(e => erros(e))
      .finally(() =>
        dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(false))
      );

    if (resposta?.data?.length) {
      setListaComponenteCurricular(resposta?.data);

      if (resposta?.data.length === 1) {
        dispatch(
          setComponenteCurricularSelecionado(
            String(resposta?.data[0].codigoComponenteCurricular)
          )
        );
      }
    } else {
      dispatch(setComponenteCurricularSelecionado());
      setListaComponenteCurricular([]);
    }
  }, [dispatch, turma]);

  const obterListaSemestres = useCallback(async () => {
    if (ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)) {
      const retorno = await ServicoAcompanhamentoAprendizagem.obterListaSemestres().catch(
        e => erros(e)
      );
      if (retorno?.data) {
        setListaSemestres(retorno.data);
      } else {
        setListaSemestres([]);
      }
    }
  }, [modalidadesFiltroPrincipal, turmaSelecionada]);

  useEffect(() => {
    resetarInfomacoes();
    dispatch(setAlunosAcompanhamentoAprendizagem([]));
    dispatch(resetarDadosRegistroIndividual());

    if (
      turmaSelecionada?.turma &&
      ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterComponentesCurriculares();
      obterListaSemestres();
    } else {
      setSemestreSelecionado(undefined);
      setListaSemestres([]);
    }

    return () => {
      dispatch(resetarDadosRegistroIndividual());
      dispatch(setComponenteCurricularSelecionado());
      dispatch(setDadosApanhadoGeral({}));
      dispatch(setApanhadoGeralEmEdicao(false));
      resetarInfomacoes();
    };
  }, [
    dispatch,
    resetarInfomacoes,
    obterListaSemestres,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
    obterComponentesCurriculares,
  ]);

  useEffect(() => {
    const ehInfantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    verificaSomenteConsulta(permissoesTela, !ehInfantil);
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const obterFrequenciaAluno = async codigoAluno => {
    const retorno = await ServicoCalendarios.obterFrequenciaAluno(
      codigoAluno,
      turma
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      return retorno.data;
    }
    return 0;
  };

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    const frequenciaGeralAluno = await obterFrequenciaAluno(aluno.codigoEOL);
    const novoAluno = aluno;
    novoAluno.frequencia = frequenciaGeralAluno;
    dispatch(setDadosAlunoObjectCard(aluno));
    dispatch(setDadosAlunoObjectCardRegistroIndividual(aluno));

    dispatch(setCodigoAlunoSelecionado(aluno.codigoEOL));
  };

  const obterListaAlunos = useCallback(
    async semestreConsulta => {
      if (turma) {
        dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(true));

        const retorno = await ServicoAcompanhamentoAprendizagem.obterListaAlunos(
          turma,
          anoLetivo,
          semestreConsulta
        )
          .catch(e => erros(e))
          .finally(() =>
            dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(false))
          );

        if (retorno?.data) {
          dispatch(setAlunosAcompanhamentoAprendizagem(retorno.data));
          const primeiroEstudanteAtivo = retorno.data.find(
            item => item.situacaoCodigo === situacaoMatriculaAluno.Ativo
          );
          if (primeiroEstudanteAtivo) {
            onChangeAlunoSelecionado(primeiroEstudanteAtivo);
          }
        } else {
          resetarInfomacoes();
          dispatch(setAlunosAcompanhamentoAprendizagem([]));
        }
      }
    },
    [anoLetivo, dispatch, turma, resetarInfomacoes]
  );

  useEffect(() => {
    if (componenteCurricularSelecionado && semestreSelecionado) {
      obterListaAlunos(semestreSelecionado);
    } else {
      dispatch(setAlunosAcompanhamentoAprendizagem([]));
    }
  }, [
    componenteCurricularSelecionado,
    semestreSelecionado,
    obterListaAlunos,
    dispatch,
  ]);

  const onChangeSemestre = valor => {
    resetarInfomacoes();
    dispatch(setAlunosAcompanhamentoAprendizagem([]));
    dispatch(setCodigoAlunoSelecionado());
    dispatch(setDadosApanhadoGeral({}));
    dispatch(setApanhadoGeralEmEdicao(false));
    setSemestreSelecionado(valor);
  };

  const permiteOnChangeAluno = async () => {
    const continuar = await ServicoAcompanhamentoAprendizagem.salvarDadosAcompanhamentoAprendizagem(
      semestreSelecionado
    );

    return continuar;
  };

  return (
    <Container>
      {!turmaSelecionada.turma ? (
        <div className="col-md-12">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-sem-turma',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </div>
      ) : (
        ''
      )}
      {turmaSelecionada.turma ? <AlertaPermiteSomenteTurmaInfantil /> : ''}
      <Cabecalho pagina="Relatório do Acompanhamento da Aprendizagem" />
      <LoaderAcompanhamentoAprendizagem>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <BotoesAcoesAcompanhamentoAprendizagem
                  semestreSelecionado={semestreSelecionado}
                />
              </div>
            </div>
          </div>
          {turmaSelecionada?.turma &&
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
            <>
              <div className="col-md-12 mb-2">
                <div className="row">
                  <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
                    <SelectComponent
                      id="componenteCurricular"
                      name="ComponenteCurricularId"
                      lista={listaComponenteCurricular || []}
                      valueOption="codigoComponenteCurricular"
                      valueText="nome"
                      valueSelect={componenteCurricularSelecionado}
                      placeholder="Selecione um componente curricular"
                      disabled={listaComponenteCurricular?.length === 1}
                    />
                  </div>
                  <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
                    <SelectComponent
                      id="semestre"
                      lista={listaSemestres}
                      valueOption="semestre"
                      valueText="descricao"
                      valueSelect={semestreSelecionado}
                      onChange={onChangeSemestre}
                      placeholder="Selecione o semestre"
                      disabled={!componenteCurricularSelecionado}
                    />
                  </div>
                </div>
              </div>
              {componenteCurricularSelecionado && semestreSelecionado ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <BotaoOrdenarListaAlunos />
                    <BotaoGerarRelatorioAprendizagem
                      semestre={semestreSelecionado}
                    />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratilAcompanhamentoAprendizagem
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                      permiteOnChangeAluno={permiteOnChangeAluno}
                    >
                      <ObjectCardAcompanhamentoAprendizagem
                        semestre={semestreSelecionado}
                      />
                      <DadosAcompanhamentoAprendizagem
                        codigoTurma={turmaSelecionada.turma}
                        modalidade={turmaSelecionada.modalidade}
                        semestreSelecionado={semestreSelecionado}
                      />
                    </TabelaRetratilAcompanhamentoAprendizagem>
                  </div>
                  <div className="col-md-12 mb-2 mt-2">
                    <ApanhadoGeral semestreSelecionado={semestreSelecionado} />
                  </div>
                </>
              ) : (
                ''
              )}
            </>
          ) : (
            ''
          )}
        </Card>
      </LoaderAcompanhamentoAprendizagem>
    </Container>
  );
};

export default AcompanhamentoAprendizagem;
