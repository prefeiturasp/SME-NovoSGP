import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import { Colors, Localizador, ModalConteudoHtml } from '~/componentes';
import LocalizadorFuncionario from '~/componentes-sgp/LocalizadorFuncionario';
import { setListaUsuariosNotificacao } from '~/redux/modulos/observacoesUsuario/actions';
import { confirmar, erro } from '~/servicos';
import { BotaoEstilizado, TextoEstilizado } from './modalNotificarUsuarios.css';

const ModalNotificarUsuarios = ({
  modalVisivel,
  setModalVisivel,
  listaUsuarios,
  usarLocalizadorFuncionario,
  parametrosLocalizadorFuncionario,
}) => {
  const [usuariosSelecionados, setUsuariosSelecionados] = useState(
    listaUsuarios
  );
  const anoAtual = window.moment().format('YYYY');
  const [modoEdicao, setModoEdicao] = useState(false);

  const dispatch = useDispatch();

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
      setModoEdicao(true);
    }
  };

  const onChangeLocalizadorFuncionario = valores => {
    if (valores?.codigoRF) {
      if (!valores?.usuarioId) {
        erro(
          'Este usuário não consta na base do SGP e não será possível notificá-lo'
        );
        return;
      }
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
            nome: `${valores.nomeServidor} (${valores?.codigoRF})`,
            podeRemover: true,
          },
        ];
      });
      setModoEdicao(true);
    }
  };

  const removerUsuario = usuarioId => {
    setUsuariosSelecionados(estadoAntigo =>
      estadoAntigo.filter(item => item.usuarioId !== usuarioId)
    );
    setModoEdicao(true);
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
    dispatch(setListaUsuariosNotificacao(usuariosSelecionados));
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
      <div className="col-md-12 mb-4">
        {usarLocalizadorFuncionario ? (
          <div className="row">
            <LocalizadorFuncionario
              id="funcionario"
              onChange={onChangeLocalizadorFuncionario}
              codigoUe={parametrosLocalizadorFuncionario?.codigoUe}
              limparCamposAposPesquisa
              mensagemErroConsultaRF="Este usuário não consta na base do SGP e não será possível notificá-lo"
            />
          </div>
        ) : (
          <div className="col-md-12">
            <div className="row">
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
                validaPerfilProfessor={false}
                mensagemErroConsultaRF="Este usuário não consta na base do SGP e não será possível notificá-lo"
              />
            </div>
          </div>
        )}
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
  setModalVisivel: () => {},
  usarLocalizadorFuncionario: false,
  parametrosLocalizadorFuncionario: {},
};

ModalNotificarUsuarios.propTypes = {
  listaUsuarios: PropTypes.oneOfType([PropTypes.any]),
  modalVisivel: PropTypes.bool,
  setModalVisivel: PropTypes.func,
  usarLocalizadorFuncionario: PropTypes.bool,
  parametrosLocalizadorFuncionario: PropTypes.oneOfType([PropTypes.object]),
};

export default ModalNotificarUsuarios;
