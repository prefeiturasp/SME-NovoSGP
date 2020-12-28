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
  const [
    componenteCurricularSelecionado,
    setComponenteCurricularSelecionado,
  ] = useState();
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [modoEdicao, setModoEdicao] = useState(false);
  const [turmaInfantil, setTurmaInfantil] = useState(false);

  const { turmaSelecionada } = useSelector(state => state.usuario);
  const { turma } = turmaSelecionada;
  const turmaId = turma || 0;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const dispatch = useDispatch();

  const obterListaAlunos = useCallback(async () => {
    const retorno = await ServicoRegistroIndividual.obterListaAlunos({
      componenteCurricularId: componenteCurricularSelecionado,
      turmaId,
    });
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

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosRegistroIndividual());
  }, [dispatch]);

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    dispatch(setDadosAlunoObjectCard(aluno));
  };

  const permiteOnChangeAluno = async () => {
    // const validouNotaConceitoPosConselho = await servicoSalvarConselhoClasse.validarNotaPosConselho();
    // const validouAnotacaoRecomendacao = validouNotaConceitoPosConselho
    //   ? await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia()
    //   : false;
    return true;
  };

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    const { data } = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
    ).catch(e => erros(e));

    if (data?.length) {
      setListaComponenteCurricular(data);

      if (data.length === 1) {
        setComponenteCurricularSelecionado(
          String(data[0].codigoComponenteCurricular)
        );
      }
    }

    setCarregandoGeral(false);
  }, [turmaId]);

  const resetarTela = useCallback(() => {
    setModoEdicao(false);
    setAuditoria();
  }, []);

  useEffect(() => {
    if (turmaId && turmaInfantil) {
      obterComponentesCurriculares();
      return;
    }

    setListaComponenteCurricular([]);
    setComponenteCurricularSelecionado(undefined);
    resetarTela();
  }, [turmaId, obterComponentesCurriculares, resetarTela, turmaInfantil]);

  const onChangeComponenteCurricular = valor => {
    setComponenteCurricularSelecionado(valor);
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
                    <div className="mb-2">
                      <ObjectCardRegistroIndividual />
                    </div>
                    <DadosRegistroIndividual
                      turmaCodigo={turmaSelecionada.turma}
                      modalidade={turmaSelecionada.modalidade}
                    />
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
