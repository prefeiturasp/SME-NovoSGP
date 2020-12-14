import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import {
  limparDadosPlanoAnual,
  setComponenteCurricularPlanoAnual,
} from '~/redux/modulos/anual/actions';
import { confirmar, erros, aviso } from '~/servicos/alertas';
import ServicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import servicoSalvarPlanoAnual from '../../servicoSalvarPlanoAnual';

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

  const planoAnualEmEdicao = useSelector(
    store => store.planoAnual.planoAnualEmEdicao
  );

  const [carregandoComponentes, setCarregandoComponentes] = useState(undefined);
  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [codigoComponenteCurricular, setCodigoComponenteCurricular] = useState(
    undefined
  );

  const avisoPlanoTerritorio = 'O plano anual do território do saber deve ser registrado na tela "Planejamento > Território do saber';

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
        if(componente.territorioSaber){
          dispatch(limparDadosPlanoAnual());
          dispatch(setComponenteCurricularPlanoAnual(undefined));
          aviso(avisoPlanoTerritorio);
        }else
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
    dispatch(setComponenteCurricularPlanoAnual(undefined));
    setCodigoComponenteCurricular(undefined)
    resetarInfomacoes();
    if (
      turmaSelecionada &&
      turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterListaComponenteCurricular();
    } else {
      setListaComponenteCurricular([]);
    }
  }, [
    resetarInfomacoes,
    obterListaComponenteCurricular,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
    dispatch,
  ]);

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onChangeComponenteCurricular = async valor => {
    const aposValidarSalvar = () => {      
      if (valor) {
        const componente = listaComponenteCurricular.find(
          item => String(item.codigoComponenteCurricular) === valor
        );
        if(componente.territorioSaber)
          aviso(avisoPlanoTerritorio);
        else{
          dispatch(limparDadosPlanoAnual());
          dispatch(setComponenteCurricularPlanoAnual(componente));
        }          
      } else {
        dispatch(limparDadosPlanoAnual());
        dispatch(setComponenteCurricularPlanoAnual(undefined));
      }
    };

    let salvou = false;
    if (planoAnualEmEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        salvou = await servicoSalvarPlanoAnual.validarSalvarPlanoAnual();
        if (salvou) {
          aposValidarSalvar();
        }
      } else {
        aposValidarSalvar();
      }
    } else {
      aposValidarSalvar();
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
