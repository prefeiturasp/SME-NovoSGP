import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { AlertaPermiteSomenteTurmaInfantil } from '~/componentes-sgp';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import SelectComponent from '~/componentes/select';
import {
  limparDadosAcompanhamentoAprendizagem,
  setAlunosAcompanhamentoAprendizagem,
  setCodigoAlunoSelecionado,
  setDadosAlunoObjectCard,
  setExibirLoaderGeralAcompanhamentoAprendizagem,
} from '~/redux/modulos/acompanhamentoAprendizagem/actions';
import { ehTurmaInfantil, ServicoCalendarios } from '~/servicos';
import { erros } from '~/servicos/alertas';
import ServicoAcompanhamentoAprendizagem from '~/servicos/Paginas/Relatorios/AcompanhamentoAprendizagem/ServicoAcompanhamentoAprendizagem';
import { Container } from './acompanhamentoAprendizagem.css';
import BotaoOrdenarListaAlunos from './DadosAcompanhamentoAprendizagem/BotaoOrdenarListaAlunos/botaoOrdenarListaAlunos';
import BotoesAcoesAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/BotoesAcoes/botoesAcoesAcompanhamentoAprendizagem';
import DadosAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/dadosAcompanhamentoAprendizagem';
import ObjectCardAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/ObjectCardAcompanhamentoAprendizagem/objectCardRelatorioSemestral';
import TabelaRetratilAcompanhamentoAprendizagem from './DadosAcompanhamentoAprendizagem/TabelaRetratilAcompanhamentoAprendizagem/tabelaRetratilAcompanhamentoAprendizagem';
import LoaderAcompanhamentoAprendizagem from './laderAcompanhamentoAprendizagem';

const AcompanhamentoAprendizagem = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma, anoLetivo } = turmaSelecionada;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [listaSemestres, setListaSemestres] = useState([]);
  const [semestreSelecionado, setSemestreSelecionado] = useState(undefined);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosAcompanhamentoAprendizagem());
  }, [dispatch]);

  const obterListaAlunos = useCallback(
    async semestreConsulta => {
      if (turma) {
        dispatch(setExibirLoaderGeralAcompanhamentoAprendizagem(true));
        // TODO Trocar endpoint!
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
        } else {
          resetarInfomacoes();
          dispatch(setAlunosAcompanhamentoAprendizagem([]));
        }
      }
    },
    [anoLetivo, dispatch, turma, resetarInfomacoes]
  );

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
    if (turma) {
      obterListaSemestres();
    } else {
      setSemestreSelecionado(undefined);
      setListaSemestres([]);
    }
  }, [
    obterListaAlunos,
    turma,
    resetarInfomacoes,
    dispatch,
    obterListaSemestres,
  ]);

  useEffect(() => {
    if (semestreSelecionado) {
      obterListaAlunos(semestreSelecionado);
    } else {
      dispatch(setAlunosAcompanhamentoAprendizagem([]));
    }
  }, [semestreSelecionado, obterListaAlunos, dispatch]);

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

    dispatch(setCodigoAlunoSelecionado(aluno.codigoEOL));
  };

  const onChangeSemestre = valor => {
    resetarInfomacoes();
    dispatch(setAlunosAcompanhamentoAprendizagem([]));
    dispatch(setCodigoAlunoSelecionado());
    setSemestreSelecionado(valor);
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
                <BotoesAcoesAcompanhamentoAprendizagem />
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
                      id="semestre"
                      lista={listaSemestres}
                      valueOption="semestre"
                      valueText="descricao"
                      valueSelect={semestreSelecionado}
                      onChange={onChangeSemestre}
                      placeholder="Selecione o semestre"
                    />
                  </div>
                </div>
              </div>
              {semestreSelecionado ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <BotaoOrdenarListaAlunos />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratilAcompanhamentoAprendizagem
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                    >
                      <ObjectCardAcompanhamentoAprendizagem />
                      <DadosAcompanhamentoAprendizagem
                        codigoTurma={turmaSelecionada.turma}
                        modalidade={turmaSelecionada.modalidade}
                        semestreSelecionado={semestreSelecionado}
                      />
                    </TabelaRetratilAcompanhamentoAprendizagem>
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
