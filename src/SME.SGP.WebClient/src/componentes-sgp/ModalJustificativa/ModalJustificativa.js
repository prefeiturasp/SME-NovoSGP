import React, { useState } from 'react';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import Editor from '~/componentes/editor/editor';

const ModalJustificativa = props => {
  const { exibirModal, valor } = props;

  const [justificativa, setJustificativa] = useState(valor);
  const [visivel, setVisivel] = useState(exibirModal);

  const onConfirmar = () => { };

  const onCancelar = () => {
    setVisivel(false);
  };

  const onClose = () => { };

  const onChangeJustificativa = novoValor => {
    setJustificativa(novoValor);
  };

  return (
    <>
      <ModalConteudoHtml
        key="inserirJutificativa"
        visivel={visivel}
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
