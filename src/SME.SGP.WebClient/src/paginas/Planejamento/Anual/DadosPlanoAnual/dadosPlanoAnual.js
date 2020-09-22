import React, { useCallback, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosBimestresPlanoAnual,
  setListaComponentesCurricularesPlanejamento,
} from '~/redux/modulos/anual/actions';
import { erros } from '~/servicos/alertas';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import BimestresPlanoAnual from './BimestresPlanoAnual/bimestresPlanoAnual';
import ServicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';

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
    const turmaPrograma = !!(turmaSelecionada.ano === '0');
    // TODO LOADER!
    ServicoDisciplinas.obterDisciplinasPlanejamento(
      componenteCurricular.codigoComponenteCurricular,
      turmaSelecionada.turma,
      turmaPrograma,
      componenteCurricular.regencia
    )
      .then(resposta => {
        const componestes = resposta.data.map(c => {
          return {
            ...c,
            selecionada: false,
          };
        });
        dispatch(setListaComponentesCurricularesPlanejamento(componestes));
      })
      .catch(e => {
        dispatch(setDadosBimestresPlanoAnual([]));
        erros(e);
      })
      .finally(() => {
        // TODO Loader!
      });
  }, [dispatch, componenteCurricular, turmaSelecionada]);

  // Carrega a lista de bimestres para montar os card collapse com 2 ou 4 bimestres!
  const obterBimestresDadosPlanosAnual = useCallback(() => {
    // TODO Loader!
    ServicoPlanoAnual.obter(
      turmaSelecionada.anoLetivo,
      componenteCurricular.codigoComponenteCurricular,
      turmaSelecionada.unidadeEscolar,
      turmaSelecionada.turma
    )
      .then(resposta => {
        dispatch(setDadosBimestresPlanoAnual(resposta.data));
        obterListaComponentesCurricularesPlanejamento();
      })
      .catch(e => {
        dispatch(setDadosBimestresPlanoAnual([]));
        erros(e);
      })
      .finally(() => {
        // TODO Loader!
      });
  }, [
    dispatch,
    turmaSelecionada,
    componenteCurricular,
    obterListaComponentesCurricularesPlanejamento,
  ]);

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
      obterBimestresDadosPlanosAnual();
      // Quando for regencia vai ter somente uma tab que é o componente curricular selecionado do SelectComponent!
      if (componenteCurricular.regencia) {
        obterListaComponentesCurricularesPlanejamento();
      } else {
        montarListaComponenteCurricularesPlanejamento();
      }
    }
  }, [
    obterListaComponentesCurricularesPlanejamento,
    montarListaComponenteCurricularesPlanejamento,
    obterBimestresDadosPlanosAnual,
    componenteCurricular,
    dispatch,
    modalidadesFiltroPrincipal,
    turmaSelecionada,
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
