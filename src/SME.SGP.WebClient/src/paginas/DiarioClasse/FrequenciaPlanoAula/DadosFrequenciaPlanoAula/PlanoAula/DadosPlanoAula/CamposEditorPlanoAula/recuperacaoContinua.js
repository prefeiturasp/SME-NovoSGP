import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
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

  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6',
  };

  const onChangeRecuperacaoContinua = valor => {
    ServicoPlanoAula.atualizarDadosParaSalvarPlanoAula(
      'recuperacaoAula',
      valor
    );
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
          <Editor
            desabilitar={desabilitarCamposPlanoAula}
            onChange={onChangeRecuperacaoContinua}
            inicial={dadosPlanoAula.recuperacaoAula}
          />
        </fieldset>
      </CardCollapse>
    </>
  );
};

export default RecuperacaoContinua;
