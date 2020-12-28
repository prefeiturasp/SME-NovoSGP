import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Card, Loader, SelectComponent } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';

import {
  BotoesAcoes,
  BotaoOrdenarListaAlunos,
  DadosRegistroIndividual,
  Mensagens,
  ObjectCardRegistroIndividual,
  TabelaRetratilRegistroIndividual,
} from './componentes';

import {
  setAlunosConselhoClasse,
  setExibirLoaderGeralConselhoClasse,
  limparDadosConselhoClasse,
  setDadosAlunoObjectCard,
} from '~/redux/modulos/conselhoClasse/actions';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import servicoSalvarConselhoClasse from '~/paginas/Fechamento/ConselhoClasse/servicoSalvarConselhoClasse';

import { erros } from '~/servicos/alertas';

const RegistroIndividual = () => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);

  const { turmaSelecionada } = useSelector(state => state.usuario);
  const { turma, anoLetivo, periodo } = turmaSelecionada;

  const dispatch = useDispatch();

  const obterListaAlunos = useCallback(async () => {
    dispatch(setExibirLoaderGeralConselhoClasse(true));
    const retorno = await ServicoConselhoClasse.obterListaAlunos(
      turma,
      anoLetivo,
      periodo
    );
    if (retorno && retorno.data) {
      dispatch(setAlunosConselhoClasse(retorno.data));
      setExibirListas(true);
    }
    dispatch(setExibirLoaderGeralConselhoClasse(false));
  }, [dispatch, anoLetivo, turma, periodo]);

  useEffect(() => {
    obterListaAlunos();
  }, [obterListaAlunos]);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosConselhoClasse());
  }, [dispatch]);

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    dispatch(setDadosAlunoObjectCard(aluno));
  };

  const permiteOnChangeAluno = async () => {
    const validouNotaConceitoPosConselho = await servicoSalvarConselhoClasse.validarNotaPosConselho();
    if (validouNotaConceitoPosConselho) {
      const validouAnotacaoRecomendacao = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
      if (validouNotaConceitoPosConselho && validouAnotacaoRecomendacao) {
        return true;
      }
    }
    return false;
  };

  return (
    <Loader loading={carregandoGeral} className="w-100 my-2">
      <div className="mb-3">
        <Mensagens />
      </div>
      <Cabecalho pagina="Registro individual" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-4">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                // lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                // valueSelect={disciplinaIdSelecionada}
                // onChange={onChangeDisciplinas}
                placeholder="Selecione um componente curricular"
                disabled={!turmaSelecionada.turma}
              />
            </div>
            <div className="col-sm-12 col-lg-8 col-md-8 d-flex justify-content-end pb-4">
              <BotoesAcoes />
            </div>
          </div>
          <div className="row">
            {exibirListas && (
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
