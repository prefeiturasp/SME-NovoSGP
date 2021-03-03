import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
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
import { ServicoCalendarios } from '~/servicos';
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
  const { turma, anoLetivo, periodo } = turmaSelecionada;

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
        console.log(`Semetre selecionado: ${semestreConsulta}`);
        const retorno = await ServicoAcompanhamentoAprendizagem.obterListaAlunos(
          turma,
          anoLetivo,
          periodo
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
    [anoLetivo, dispatch, turma, periodo, resetarInfomacoes]
  );

  const obterListaSemestres = useCallback(async () => {
    const retorno = await ServicoAcompanhamentoAprendizagem.obterListaSemestres().catch(
      e => erros(e)
    );
    if (retorno?.data) {
      setListaSemestres(retorno.data);
    } else {
      setListaSemestres([]);
    }
  }, []);

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
    turmaSelecionada,
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
          {turmaSelecionada?.turma ? (
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
                        semestreConsulta={semestreSelecionado}
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
