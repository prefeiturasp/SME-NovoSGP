import React, { useState, useEffect } from 'react';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import Editor from '~/componentes/editor/editor';

const ModalJustificativa = props => {
  const { exibirModal, valor, onCancelar } = props;

  const [justificativa, setJustificativa] = useState(valor);
  const [visivel, setVisivel] = useState(false);

  const onConfirmar = () => { };


  const onClose = () => { };

  const onChangeJustificativa = novoValor => {
    setJustificativa(novoValor);
  };

  return (
    <>
      <ModalConteudoHtml
        key="inserirJutificativa"
        visivel={exibirModal}
        onConfirmacaoPrincipal={onConfirmar}
        onConfirmacaoSecundaria={onCancelar}
        onClose={onClose}
        labelBotaoPrincipal="Confirmar"
        labelBotaoSecundario="Cancelar"
        titulo="Inserir justificativa"
        closable={false}
        fecharAoClicarFora={false}
        fecharAoClicarEsc={false}
      >
        <fieldset className="mt-3">
          <Editor
            onChange={onChangeJustificativa}
            inicial={justificativa}
          />
        </fieldset>
      </ModalConteudoHtml>
    </>
  );
};

export default ModalJustificativa;
