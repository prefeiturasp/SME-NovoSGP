import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Card, Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';

import {
  BotaoOrdenarListaAlunos,
  BotoesAcoes,
  DadosRegistroIndividual,
  Mensagens,
  ObjectCardRegistroIndividual,
  TabelaRetratilRegistroIndividual,
} from './componentes';

import {
  limparDadosRegistroIndividual,
  setAuditoriaNovoRegistro,
  setAlunosRegistroIndividual,
  setComponenteCurricularSelecionado,
  setDadosAlunoObjectCard,
  setDesabilitarCampos,
  setRegistroIndividualEmEdicao,
  atualizarMarcadorDiasSemRegistroExibir,
} from '~/redux/modulos/registroIndividual/actions';

import {
  confirmar,
  ehTurmaInfantil,
  erros,
  history,
  ServicoDisciplina,
  ServicoRegistroIndividual,
  sucesso,
} from '~/servicos';

import { URL_HOME } from '~/constantes';

const RegistroIndividual = () => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [turmaInfantil, setTurmaInfantil] = useState(false);

  const {
    componenteCurricularSelecionado,
    dadosParaSalvarNovoRegistro,
    desabilitarCampos,
    registroIndividualEmEdicao,
  } = useSelector(state => state.registroIndividual);
  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.id || 0;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const dispatch = useDispatch();

  const obterListaAlunos = useCallback(async () => {
    const retorno = await ServicoRegistroIndividual.obterListaAlunos({
      componenteCurricularId: componenteCurricularSelecionado,
      turmaId,
    }).catch(e => erros(e));
    if (retorno?.data) {
      dispatch(setAlunosRegistroIndividual(retorno.data));
      setExibirListas(true);
    }
  }, [dispatch, componenteCurricularSelecionado, turmaId]);

  useEffect(() => {
    if (componenteCurricularSelecionado) {
      obterListaAlunos();
    }
  }, [obterListaAlunos, componenteCurricularSelecionado]);

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);
  }, [modalidadesFiltroPrincipal, turmaSelecionada]);

  const resetarInfomacoes = useCallback(
    ehDataAnterior => {
      if (ehDataAnterior) {
        dispatch(limparDadosRegistroIndividual());
        return;
      }
      dispatch(setRegistroIndividualEmEdicao(false));
      dispatch(setDesabilitarCampos(false));
    },
    [dispatch]
  );

  const cadastrarRegistroIndividual = async () => {
    setCarregandoGeral(true);
    const { alunoCodigo, data, registro } = dadosParaSalvarNovoRegistro;
    const retorno = await ServicoRegistroIndividual.salvarRegistroIndividual({
      turmaId,
      componenteCurricularId: componenteCurricularSelecionado,
      alunoCodigo,
      registro,
      data,
    })
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (retorno?.status === 200) {
      sucesso('Ocorrência cadastrada com sucesso.');
      dispatch(setAuditoriaNovoRegistro(retorno.data));
      dispatch(atualizarMarcadorDiasSemRegistroExibir(alunoCodigo));

      const dataAtual = window.moment(window.moment().format('YYYY-MM-DD'));
      const ehDataAnterior = window.moment(dataAtual).isAfter(data);
      resetarInfomacoes(ehDataAnterior);
    }
  };

  const editarRegistroIndividual = async () => {
    setCarregandoGeral(true);
    const { id, alunoCodigo, data, registro } = dadosParaSalvarNovoRegistro;
    const retorno = await ServicoRegistroIndividual.editarRegistroIndividual({
      id,
      turmaId,
      componenteCurricularId: componenteCurricularSelecionado,
      alunoCodigo,
      registro,
      data,
    })
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (retorno?.status === 200) {
      sucesso('Ocorrência editada com sucesso.');
      dispatch(setAuditoriaNovoRegistro(retorno.data));
      dispatch(atualizarMarcadorDiasSemRegistroExibir(alunoCodigo));

      const dataAtual = window.moment(window.moment().format('YYYY-MM-DD'));
      const ehDataAnterior = window.moment(dataAtual).isAfter(data);
      resetarInfomacoes(ehDataAnterior);
    }
  };

  const onChangeAlunoSelecionado = async aluno => {
    if (registroIndividualEmEdicao) {
      salvarRegistroIndividual();
    }
    resetarInfomacoes();
    if (!aluno.desabilitado) {
      dispatch(setDadosAlunoObjectCard(aluno));
    }
  };

  const permiteOnChangeAluno = async () => {
    return true;
  };

  const obterComponentesCurriculares = useCallback(async () => {
    const turma = turmaSelecionada?.turma || 0;
    setCarregandoGeral(true);
    const resposta = await ServicoDisciplina.obterDisciplinasPorTurma(turma)
      .catch(e => erros(e))
      .finally(() => setCarregandoGeral(false));

    if (resposta?.data?.length) {
      setListaComponenteCurricular(resposta?.data);

      if (resposta?.data.length === 1) {
        dispatch(
          setComponenteCurricularSelecionado(
            String(resposta?.data[0].codigoComponenteCurricular)
          )
        );
      }
    }
  }, [dispatch, turmaSelecionada]);

  const resetarTela = useCallback(() => {
    dispatch(setRegistroIndividualEmEdicao(false));
    dispatch(setAuditoriaNovoRegistro());
  }, [dispatch]);

  useEffect(() => {
    if (turmaSelecionada?.turma && turmaInfantil) {
      obterComponentesCurriculares();
      return;
    }

    setListaComponenteCurricular([]);
    setComponenteCurricularSelecionado(undefined);
    resetarTela();
  }, [
    turmaSelecionada,
    obterComponentesCurriculares,
    resetarTela,
    turmaInfantil,
  ]);

  const onChangeComponenteCurricular = valor => {
    dispatch(setComponenteCurricularSelecionado(valor));
  };

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const escolheCadastrar = () => {
    const { id } = dadosParaSalvarNovoRegistro;
    if (id) {
      editarRegistroIndividual();
      return;
    }
    cadastrarRegistroIndividual();
  };

  const salvarRegistroIndividual = async () => {
    const confirmado = await pergutarParaSalvar();
    if (confirmado) {
      escolheCadastrar();
    }
    return true;
  };

  const onClickVoltar = async () => {
    let validouSalvarRegistro = true;
    if (registroIndividualEmEdicao && turmaInfantil && desabilitarCampos) {
      validouSalvarRegistro = await salvarRegistroIndividual();
    }

    if (validouSalvarRegistro) {
      history.push(URL_HOME);
      resetarInfomacoes();
      dispatch(setDadosAlunoObjectCard({}));
    }
  };

  const onClickCancelar = () => {
    dispatch(limparDadosRegistroIndividual());
  };

  const onClickCadastrar = () => {
    escolheCadastrar();
  };

  return (
    <Loader loading={carregandoGeral} className="w-100">
      <Mensagens />
      <Cabecalho pagina="Registro individual" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-4">
              <SelectComponent
                id="componenteCurricular"
                name="ComponenteCurricularId"
                lista={listaComponenteCurricular}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={componenteCurricularSelecionado}
                onChange={onChangeComponenteCurricular}
                placeholder="Selecione um componente curricular"
                disabled={
                  !turmaInfantil || listaComponenteCurricular?.length === 1
                }
              />
            </div>
            <div className="col-sm-12 col-lg-8 col-md-8 d-flex justify-content-end pb-4">
              <BotoesAcoes
                onClickVoltar={onClickVoltar}
                onClickCancelar={onClickCancelar}
                onClickCadastrar={onClickCadastrar}
                modoEdicao={registroIndividualEmEdicao}
                desabilitarCampos={desabilitarCampos}
                turmaInfantil={turmaInfantil}
              />
            </div>
          </div>
          <div className="row">
            {exibirListas && turmaInfantil && (
              <>
                <div className="col-md-12 mb-3 d-flex">
                  <BotaoOrdenarListaAlunos />
                </div>
                <div className="col-md-12 mb-2">
                  <TabelaRetratilRegistroIndividual
                    onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                    permiteOnChangeAluno={permiteOnChangeAluno}
                  >
                    <>
                      <div className="mb-2">
                        <ObjectCardRegistroIndividual />
                      </div>
                      <DadosRegistroIndividual
                        turmaCodigo={turmaSelecionada.turma}
                        modalidade={turmaSelecionada.modalidade}
                      />
                    </>
                  </TabelaRetratilRegistroIndividual>
                </div>
              </>
            )}
          </div>
        </div>
      </Card>
    </Loader>
  );
};

export default RegistroIndividual;
