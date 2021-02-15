import React, { useState } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';

import { Editor, ModalConteudoHtml, SelectComponent } from '~/componentes';

import { confirmar } from '~/servicos';

const ModalReestruturacaoPlano = ({ key, esconderModal, exibirModal }) => {
  const [modoEdicao, setModoEdicao] = useState(false);
  const [listaVersao, setListaVersao] = useState([]);

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    setModoEdicao(false);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    if (modoEdicao) {
      const ehPraSalvar = await perguntarSalvarListaUsuario();
      if (ehPraSalvar) {
        onConfirmarModal();
      }
    }
  };

  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key={key}
      visivel={exibirModal}
      titulo="Reestruturação do plano"
      onClose={fecharModal}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width="50%"
      closable
    >
      <div className="col-12 mb-2 p-0">
        <SelectComponent
          label="Selecione o plano que corresponde a reestruturação"
          lista={listaVersao}
          valueOption="valor"
          valueText="desc"
          name="diaSemana"
          onChange={() => setModoEdicao(true)}
        />
      </div>
      <div className="col-12 mb-2 p-0">
        <Editor />
      </div>
    </ModalConteudoHtml>
  );
};

ModalReestruturacaoPlano.defaultProps = {
  esconderModal: () => {},
  exibirModal: false,
};

ModalReestruturacaoPlano.propTypes = {
  esconderModal: PropTypes.func,
  exibirModal: PropTypes.bool,
  key: PropTypes.string.isRequired,
};

export default ModalReestruturacaoPlano;
