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
  setAlunosRegistroIndividual,
  setAuditoriaNovoRegistro,
  setComponenteCurricularSelecionado,
  setDadosAlunoObjectCard,
  setExibirLoaderGeralRegistroIndividual,
  setRecolherRegistrosAnteriores,
  setRegistroIndividualEmEdicao,
} from '~/redux/modulos/registroIndividual/actions';

import {
  ehTurmaInfantil,
  erros,
  ServicoDisciplina,
  ServicoRegistroIndividual,
  verificaSomenteConsulta,
} from '~/servicos';

import { RotasDto } from '~/dtos';
import MetodosRegistroIndividual from './metodosRegistroIndividual';

const RegistroIndividual = () => {
  const [exibirListas, setExibirListas] = useState(false);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [turmaInfantil, setTurmaInfantil] = useState(false);

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );

  const podeRealizarNovoRegistro = useSelector(
    state => state.registroIndividual.podeRealizarNovoRegistro
  );
  const exibirLoaderGeralRegistroIndividual = useSelector(
    state => state.registroIndividual.exibirLoaderGeralRegistroIndividual
  );

  const { turmaSelecionada, permissoes } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.id || 0;
  const permissoesTela = permissoes[RotasDto.REGISTRO_INDIVIDUAL];

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

  const permiteOnChangeAluno = async () => {
    return true;
  };

  const obterComponentesCurriculares = useCallback(async () => {
    const turma = turmaSelecionada?.turma || 0;
    dispatch(setExibirLoaderGeralRegistroIndividual(true));
    const resposta = await ServicoDisciplina.obterDisciplinasPorTurma(turma)
      .catch(e => erros(e))
      .finally(() => dispatch(setExibirLoaderGeralRegistroIndividual(false)));

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
    dispatch(setDadosAlunoObjectCard({}));
    setListaComponenteCurricular([]);
    setComponenteCurricularSelecionado(undefined);
  }, [dispatch]);

  useEffect(() => {
    if (turmaSelecionada?.turma && turmaInfantil) {
      obterComponentesCurriculares();
      return;
    }
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

  const onChangeAlunoSelecionado = async aluno => {
    MetodosRegistroIndividual.verificarSalvarRegistroIndividual();
    MetodosRegistroIndividual.resetarInfomacoes(true);
    if (!aluno.desabilitado) {
      dispatch(setDadosAlunoObjectCard(aluno));
    }

    if (podeRealizarNovoRegistro) {
      dispatch(setRecolherRegistrosAnteriores(true));
    }
  };

  const validaSomenteConsulta = useCallback(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  useEffect(() => {
    validaSomenteConsulta();
    return () => {
      resetarTela();
    };
  }, [turmaSelecionada, validaSomenteConsulta, resetarTela]);

  return (
    <Loader loading={exibirLoaderGeralRegistroIndividual} className="w-100">
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
              <BotoesAcoes turmaInfantil={turmaInfantil} />
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
                      <DadosRegistroIndividual />
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
