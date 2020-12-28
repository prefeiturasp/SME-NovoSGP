import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import { setModoEdicaoPlanoAula } from '~/redux/modulos/frequenciaPlanoAula/actions';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';

const RecuperacaoContinua = () => {
  const dispatch = useDispatch();

  const desabilitarCamposPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.desabilitarCamposPlanoAula
  );

  const dadosPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.dadosPlanoAula
  );

  const temPeriodoAberto = useSelector(
    state => state.frequenciaPlanoAula.temPeriodoAberto
  );

  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6',
  };

  const onChangeRecuperacaoContinua = valor => {
    ServicoPlanoAula.atualizarDadosPlanoAula('recuperacaoAula', valor);
    dispatch(setModoEdicaoPlanoAula(true));
  };

  return (
    <>
      <CardCollapse
        key="recuperacao-continua"
        titulo="Recuperação contínua"
        indice="recuperacao-continua"
        configCabecalho={configCabecalho}
      >
        <fieldset className="mt-3">
          <JoditEditor
            desabilitar={desabilitarCamposPlanoAula || !temPeriodoAberto}
            onChange={onChangeRecuperacaoContinua}
            value={dadosPlanoAula.recuperacaoAula}
          />
        </fieldset>
      </CardCollapse>
    </>
  );
};

export default RecuperacaoContinua;
