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
  setAlunosRegistroIndividual,
  setDadosAlunoObjectCard,
  setComponenteCurricularSelecionado,
} from '~/redux/modulos/registroIndividual/actions';

import {
  confirmar,
  ehTurmaInfantil,
  erros,
  history,
  ServicoDisciplina,
  ServicoRegistroIndividual,
} from '~/servicos';

import { URL_HOME } from '~/constantes';

const RegistroIndividual = () => {
  const [auditoria, setAuditoria] = useState();
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [modoEdicao, setModoEdicao] = useState(false);
  const [turmaInfantil, setTurmaInfantil] = useState(false);

  const componenteCurricularSelecionado = useSelector(
    state => state.registroIndividual.componenteCurricularSelecionado
  );
  const { turmaSelecionada } = useSelector(state => state.usuario);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const dispatch = useDispatch();

  const obterListaAlunos = useCallback(async () => {
    const turmaId = turmaSelecionada?.id || 0;
    const retorno = await ServicoRegistroIndividual.obterListaAlunos({
      componenteCurricularId: componenteCurricularSelecionado,
      turmaId,
    }).catch(e => erros(e));
    if (retorno?.data) {
      dispatch(setAlunosRegistroIndividual(retorno.data));
      setExibirListas(true);
    }
  }, [dispatch, componenteCurricularSelecionado, turmaSelecionada]);

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

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosRegistroIndividual());
  }, [dispatch]);

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    if (!aluno.desabilitado) {
      dispatch(setDadosAlunoObjectCard(aluno));
    }
  };

  const permiteOnChangeAluno = async () => {
    return true;
  };

  const obterComponentesCurriculares = useCallback(async () => {
    const turmaId = turmaSelecionada?.turma || 0;
    setCarregandoGeral(true);
    const resposta = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
    ).catch(e => erros(e));

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

    setCarregandoGeral(false);
  }, [dispatch, turmaSelecionada]);

  const resetarTela = useCallback(() => {
    setModoEdicao(false);
    setAuditoria();
  }, []);

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

  const cadastrarRegistroIndividual = async () => {
    const confirmado = await pergutarParaSalvar();
    if (confirmado) {
      // const salvou = await validaAntesDoSubmit(form);
      // if (salvou) {
      //   return true;
      // }
      // return false;
    }
    return true;
  };

  const onClickVoltar = async (observacaoEmEdicao, novaObservacao) => {
    let validouSalvarDiario = true;
    if (modoEdicao && turmaInfantil && !desabilitarCampos) {
      validouSalvarDiario = await cadastrarRegistroIndividual();
    }

    const validouSalvarObservacao = true;
    if (novaObservacao) {
      // validouSalvarObservacao = await salvarObservacao({
      //   observacao: novaObservacao,
      // });
    } else if (observacaoEmEdicao) {
      // validouSalvarObservacao = await salvarObservacao(observacaoEmEdicao);
    }

    if (validouSalvarDiario && validouSalvarObservacao) {
      history.push(URL_HOME);
    }
  };

  const onClickCancelar = () => {};

  const onClickCadastrar = () => {};

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
                modoEdicao={modoEdicao}
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
