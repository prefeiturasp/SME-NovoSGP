import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setBimestresPlanoAnual,
  setEhRegistroMigrado,
  setExibirLoaderPlanoAnual,
  setListaComponentesCurricularesPlanejamento,
  setListaTurmasParaCopiar,
} from '~/redux/modulos/anual/actions';
import { erros } from '~/servicos/alertas';
import ServicoComponentesCurriculares from '~/servicos/Paginas/ComponentesCurriculares/ServicoComponentesCurriculares';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import BimestresPlanoAnual from './BimestresPlanoAnual/bimestresPlanoAnual';

const DadosPlanoAnual = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  // Seta o componente curricular selecionado no SelectComponent quando não é REGENCIA!
  const montarListaComponenteCurricularesPlanejamento = useCallback(() => {
    dispatch(
      setListaComponentesCurricularesPlanejamento([componenteCurricular])
    );
  }, [dispatch, componenteCurricular]);

  // Carrega lista de componentes para montar as TABS!
  const obterListaComponentesCurricularesPlanejamento = useCallback(() => {
    dispatch(setExibirLoaderPlanoAnual(true));
    ServicoComponentesCurriculares.obterComponetensCuricularesRegencia(
      turmaSelecionada.id
    )
      .then(resposta => {
        dispatch(setListaComponentesCurricularesPlanejamento(resposta.data));
      })
      .catch(e => {
        dispatch(setBimestresPlanoAnual([]));
        erros(e);
      })
      .finally(() => {
        dispatch(setExibirLoaderPlanoAnual(false));
      });
  }, [dispatch, turmaSelecionada]);

  // Carrega a lista de bimestres para montar os card collapse com 2 ou 4 bimestres!
  const obterBimestresPlanoAnual = useCallback(() => {
    dispatch(setExibirLoaderPlanoAnual(true));
    return ServicoPlanoAnual.obterBimestresPlanoAnual(turmaSelecionada.id)
      .then(resposta => {
        dispatch(setBimestresPlanoAnual(resposta.data));
        return resposta.data;
      })
      .catch(e => {
        dispatch(setBimestresPlanoAnual([]));
        erros(e);
      })
      .finally(() => {
        dispatch(setExibirLoaderPlanoAnual(false));
      });
  }, [dispatch, turmaSelecionada]);

  const obterTurmasParaCopiarConteudo = useCallback(() => {
    ServicoPlanoAnual.obterTurmasParaCopia(
      turmaSelecionada.id,
      componenteCurricular.codigoComponenteCurricular
    )
      .then(resposta => {
        dispatch(setListaTurmasParaCopiar(resposta.data));
      })
      .catch(e => {
        dispatch(setListaTurmasParaCopiar([]));
        erros(e);
      });
  }, [componenteCurricular, turmaSelecionada, dispatch]);

  /**
   * carrega a lista de bimestres com os dados dos planos
   */
  useEffect(() => {
    // TODO VER PARA LIMPAR OS DADOS ANTIGOS!
    if (
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) &&
      componenteCurricular &&
      componenteCurricular.codigoComponenteCurricular &&
      turmaSelecionada &&
      turmaSelecionada.turma
    ) {
      ServicoPlanoAnual.obterPlanejamentoId();
      obterTurmasParaCopiarConteudo();
      obterBimestresPlanoAnual().then(dados => {
        if (dados && dados.length) {
          const ehMigrado = dados.find(item => item.migrado);
          dispatch(setEhRegistroMigrado(!!ehMigrado));
          // Quando for MIGRADO mostrar somente um tab com o componente curricular já selecionado!
          if (ehMigrado) {
            montarListaComponenteCurricularesPlanejamento();
          } else if (componenteCurricular.regencia) {
            // Quando for REGENCIA carregar a lista de componentes curriculares!
            obterListaComponentesCurricularesPlanejamento();
          } else {
            montarListaComponenteCurricularesPlanejamento();
          }
        }
      });
    }
  }, [
    obterListaComponentesCurricularesPlanejamento,
    montarListaComponenteCurricularesPlanejamento,
    obterBimestresPlanoAnual,
    componenteCurricular,
    dispatch,
    modalidadesFiltroPrincipal,
    turmaSelecionada,
    obterTurmasParaCopiarConteudo,
  ]);

  return (
    <>
      {componenteCurricular &&
      componenteCurricular.codigoComponenteCurricular ? (
        <BimestresPlanoAnual />
      ) : (
        ''
      )}
    </>
  );
};

export default DadosPlanoAnual;
