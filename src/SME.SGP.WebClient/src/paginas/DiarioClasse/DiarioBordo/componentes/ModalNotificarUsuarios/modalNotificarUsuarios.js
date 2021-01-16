import React, { useState } from 'react';
import PropTypes from 'prop-types';

import { Colors, Localizador, ModalConteudoHtml } from '~/componentes';

import { confirmar } from '~/servicos';

import { BotaoEstilizado, TextoEstilizado } from './modalNotificarUsuarios.css';

const ModalNotificarUsuarios = ({
  modalVisivel,
  setModalVisivel,
  listaUsuarios,
  setListaUsuarios,
}) => {
  const [usuariosSelecionados, setUsuariosSelecionados] = useState(
    listaUsuarios
  );
  const anoAtual = window.moment().format('YYYY');

  const mudarLocalizador = valores => {
    if (valores?.professorRf) {
      setUsuariosSelecionados(estadoAntigo => {
        const usuario = estadoAntigo.find(
          item => item.usuarioId === valores.usuarioId
        );
        if (usuario) {
          return estadoAntigo;
        }
        return [
          ...estadoAntigo,
          {
            usuarioId: valores.usuarioId,
            nome: `${valores.professorNome} (${valores?.professorRf})`,
            podeRemover: true,
          },
        ];
      });
    }
  };

  const removerUsuario = usuarioId => {
    setUsuariosSelecionados(estadoAntigo =>
      estadoAntigo.filter(item => item.usuarioId !== usuarioId)
    );
  };

  const esconderModal = () => setModalVisivel(false);

  const perguntarSalvarListaUsuario = async () => {
    const resposta = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    return resposta;
  };

  const onConfirmarModal = () => {
    setListaUsuarios(usuariosSelecionados);
    esconderModal();
  };

  const fecharModal = async () => {
    esconderModal();
    const ehPraSalvar = await perguntarSalvarListaUsuario();
    if (ehPraSalvar) {
      onConfirmarModal();
    }
  };

  return (
    <ModalConteudoHtml
      titulo="Selecionar usuários para notificar"
      visivel={modalVisivel}
      esconderBotaoSecundario
      onClose={fecharModal}
      onConfirmacaoPrincipal={onConfirmarModal}
      labelBotaoPrincipal="Confirmar"
      closable
      width="50%"
      fecharAoClicarFora
      fecharAoClicarEsc
    >
      <div className="col-md-12 d-flex mb-4">
        <Localizador
          labelRF="RF"
          placeholderRF="Procure pelo RF do usuário"
          placeholderNome="Procure pelo nome do usuário"
          labelNome="Nome"
          showLabel
          onChange={mudarLocalizador}
          buscarOutrosCargos
          classesRF="p-0"
          anoLetivo={anoAtual}
          limparCamposAposPesquisa
          desabilitado={false}
        />
      </div>
      {usuariosSelecionados?.map(({ usuarioId, nome, podeRemover }) => (
        <div
          className="col-md-12 d-flex justify-content-between mb-4"
          key={`${usuarioId}`}
        >
          <span>{nome}</span>
          {podeRemover ? (
            <BotaoEstilizado
              id="btn-excluir"
              icon="trash-alt"
              iconType="far"
              color={Colors.CinzaBotao}
              onClick={() => removerUsuario(usuarioId)}
              height="13px"
              width="13px"
            />
          ) : (
            <TextoEstilizado>Professor</TextoEstilizado>
          )}
        </div>
      ))}
    </ModalConteudoHtml>
  );
};

ModalNotificarUsuarios.defaultProps = {
  listaUsuarios: [],
  modalVisivel: false,
  setListaUsuarios: () => {},
  setModalVisivel: () => {},
};

ModalNotificarUsuarios.propTypes = {
  listaUsuarios: PropTypes.oneOfType([PropTypes.any]),
  modalVisivel: PropTypes.bool,
  setListaUsuarios: PropTypes.func,
  setModalVisivel: PropTypes.func,
};

export default ModalNotificarUsuarios;
