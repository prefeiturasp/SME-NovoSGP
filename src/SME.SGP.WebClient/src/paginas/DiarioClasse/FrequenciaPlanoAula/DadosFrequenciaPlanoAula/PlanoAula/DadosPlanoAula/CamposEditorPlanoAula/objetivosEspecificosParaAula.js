import React, { useEffect, useRef } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Base } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import {
  setModoEdicaoPlanoAula,
  setobjetivosEspecificosParaAulaValidarObrigatoriedade,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';

const ObjetivosEspecificosParaAula = () => {
  const dispatch = useDispatch();

  const desabilitarCamposPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.desabilitarCamposPlanoAula
  );

  const dadosPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.dadosPlanoAula
  );

  const objetivosAprendizagemComponente = useSelector(
    state =>
      state.frequenciaPlanoAula.dadosPlanoAula?.objetivosAprendizagemComponente
  );

  const temPeriodoAberto = useSelector(
    state => state.frequenciaPlanoAula.temPeriodoAberto
  );

  const componenteCurricular = useSelector(
    state => state.frequenciaPlanoAula.componenteCurricular
  );

  const checkedExibirEscolhaObjetivos = useSelector(
    store => store.frequenciaPlanoAula.checkedExibirEscolhaObjetivos
  );

  const exibirSwitchEscolhaObjetivos = useSelector(
    store => store.frequenciaPlanoAula.exibirSwitchEscolhaObjetivos
  );

  const refForm = useRef();

  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6',
  };

  const onChangeObjetivosEspecificosParaAula = valor => {
    ServicoPlanoAula.atualizarDadosPlanoAula('descricao', valor);
    dispatch(setModoEdicaoPlanoAula(true));
  };

  const validarSeEhObrigatorio = () => {
    return exibirSwitchEscolhaObjetivos
      ? checkedExibirEscolhaObjetivos &&
          componenteCurricular.possuiObjetivos &&
          !ServicoPlanoAula.temPeloMenosUmObjetivoSelecionado(
            objetivosAprendizagemComponente
          )
      : componenteCurricular.possuiObjetivos &&
          !ServicoPlanoAula.temPeloMenosUmObjetivoSelecionado(
            objetivosAprendizagemComponente
          );
  };

  const objetivosEspecificosParaAulaValidaObrigatoriedade = () => {
    refForm.current.events.fire('validarSeTemErro');
  };

  useEffect(() => {
    dispatch(
      setobjetivosEspecificosParaAulaValidarObrigatoriedade(() =>
        objetivosEspecificosParaAulaValidaObrigatoriedade()
      )
    );
  }, [refForm, dispatch]);

  return (
    <>
      <CardCollapse
        key="objetivos-especificos-para-aula"
        titulo="Objetivos específicos para a aula"
        indice="objetivos-especificos-para-aula"
        configCabecalho={configCabecalho}
        show
      >
        <fieldset className="mt-3">
          {validarSeEhObrigatorio() ? (
            <p style={{ color: `${Base.VermelhoAlerta}` }}>
              Você precisa selecionar pelo menos um objetivo para poder inserir
              a descrição do plano.
            </p>
          ) : (
            ''
          )}
          <JoditEditor
            ref={refForm}
            validarSeTemErro={valor =>
              !valor && !desabilitarCamposPlanoAula && temPeriodoAberto
            }
            mensagemErro="Campo obrigatório"
            desabilitar={
              desabilitarCamposPlanoAula ||
              !temPeriodoAberto ||
              validarSeEhObrigatorio()
            }
            onChange={onChangeObjetivosEspecificosParaAula}
            value={dadosPlanoAula?.descricao}
          />
        </fieldset>
      </CardCollapse>
    </>
  );
};

export default ObjetivosEspecificosParaAula;
