import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import {
  limparDadosPlanoAnual,
  setComponenteCurricularPlanoAnual,
} from '~/redux/modulos/anual/actions';
import { erros } from '~/servicos/alertas';
import ServicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const ComponenteCurricularPlanoAnual = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const componenteCurricular = useSelector(
    store => store.planoAnual.componenteCurricular
  );

  const [carregandoComponentes, setCarregandoComponentes] = useState(undefined);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [codigoComponenteCurricular, setCodigoComponenteCurricular] = useState(
    undefined
  );

  const obterListaComponenteCurricular = useCallback(async () => {
    setCarregandoComponentes(true);
    const resposta = await ServicoDisciplinas.obterDisciplinasPorTurma(
      turmaSelecionada.turma
    ).catch(e => {
      erros(e);
      setCarregandoComponentes(false);
    });
    if (resposta && resposta.data) {
      setListaComponenteCurricular(resposta.data);
      if (resposta.data.length === 1) {
        const componente = resposta.data[0];
        dispatch(setComponenteCurricularPlanoAnual(componente));
      }
    } else {
      setListaComponenteCurricular([]);
      dispatch(setComponenteCurricularPlanoAnual(undefined));
    }
    setCarregandoComponentes(false);
  }, [turmaSelecionada, dispatch]);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosPlanoAnual());
  }, [dispatch]);

  useEffect(() => {
    resetarInfomacoes();
    if (
      turmaSelecionada &&
      turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterListaComponenteCurricular();
    } else {
      dispatch(setComponenteCurricularPlanoAnual(undefined));
      setListaComponenteCurricular([]);
    }
  }, [
    resetarInfomacoes,
    obterListaComponenteCurricular,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
    dispatch,
  ]);

  const onChangeComponenteCurricular = async valor => {
    // TODO - perguntaDescartarRegistros() quando tiver em edição!;
    if (valor) {
      const componente = listaComponenteCurricular.find(
        item => String(item.codigoComponenteCurricular) === valor
      );
      dispatch(setComponenteCurricularPlanoAnual(componente));
    } else {
      dispatch(setComponenteCurricularPlanoAnual(undefined));
    }
  };

  useEffect(() => {
    if (componenteCurricular) {
      setCodigoComponenteCurricular(
        String(componenteCurricular.codigoComponenteCurricular)
      );
    } else {
      setCodigoComponenteCurricular(undefined);
    }
  }, [componenteCurricular]);

  return (
    <Loader loading={carregandoComponentes} tip="">
      <SelectComponent
        id="componente-curricular"
        lista={listaComponenteCurricular || []}
        valueOption="codigoComponenteCurricular"
        valueText="nome"
        valueSelect={codigoComponenteCurricular}
        onChange={onChangeComponenteCurricular}
        placeholder="Selecione um componente curricular"
        disabled={
          !turmaSelecionada.turma ||
          ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ||
          (listaComponenteCurricular && listaComponenteCurricular.length === 1)
        }
      />
    </Loader>
  );
};

export default ComponenteCurricularPlanoAnual;
