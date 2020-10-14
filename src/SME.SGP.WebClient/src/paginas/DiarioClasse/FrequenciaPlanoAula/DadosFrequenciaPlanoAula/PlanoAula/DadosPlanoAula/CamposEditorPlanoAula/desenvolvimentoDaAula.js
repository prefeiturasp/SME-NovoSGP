import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';
import { setModoEdicaoPlanoAula } from '~/redux/modulos/frequenciaPlanoAula/actions';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';

const DesenvolvimentoDaAula = () => {
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

  const onChangeDesenvolvimentoAula = valor => {
    ServicoPlanoAula.atualizarDadosParaSalvarPlanoAula(
      'desenvolvimentoAula',
      valor
    );
    dispatch(setModoEdicaoPlanoAula(true));
  };

  return (
    <>
      <CardCollapse
        key="desenvolvimento-aula"
        titulo="Desenvolvimento da aula"
        indice="desenvolvimento-aula"
        configCabecalho={configCabecalho}
        show
      >
        <fieldset className="mt-3">
          <Editor
            desabilitar={desabilitarCamposPlanoAula}
            onChange={onChangeDesenvolvimentoAula}
            inicial={dadosPlanoAula.desenvolvimentoAula}
          />
        </fieldset>
      </CardCollapse>
    </>
  );
};

export default DesenvolvimentoDaAula;
