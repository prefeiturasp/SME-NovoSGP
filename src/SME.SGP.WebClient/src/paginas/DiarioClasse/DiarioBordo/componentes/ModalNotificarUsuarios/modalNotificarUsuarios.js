import React from 'react';

import { Colors, Localizador, ModalConteudoHtml } from '~/componentes';

import { BotaoEstilizado, TextoEstilizado } from './modalNotificarUsuarios.css';

// eslint-disable-next-line react/prop-types
const ModalNotificarUsuarios = ({ modalVisivel, setModalVisivel }) => {
  return (
    <ModalConteudoHtml
      titulo="Selecionar usuários para notificar"
      visivel={modalVisivel}
      esconderBotaoSecundario
      onClose={() => {
        setModalVisivel(false);
      }}
      onConfirmacaoSecundaria={() => {
        setModalVisivel(false);
      }}
      // onConfirmacaoPrincipal={() => {
      //   onConfirmarModal();
      // }}
      labelBotaoPrincipal="Confirmar"
      closable
      width="50%"
      fecharAoClicarFora
      fecharAoClicarEsc
      // desabilitarBotaoPrincipal={
      //   ehTurmaAnoAnterior() || somenteConsulta || naoPodeIncluirOuAlterar()
      // }
    >
      <div className="col-md-12 d-flex mb-4">
        <Localizador
          labelRF="RF"
          placeholderRF="Procure pelo RF do usuário"
          placeholderNome="Procure pelo nome do usuário"
          labelNome="Nome"
          // rfEdicao={usuarioRf}
          // buscandoDados={setCarregandoGeral}
          // dreId={dreCodigo}
          // anoLetivo={anoAtual}
          showLabel
          onChange={valores => {
            // if (valores && valores.professorRf) {
            //   setUsuarioRf(valores.professorRf);
            // } else {
            //   setUsuarioRf(undefined);
            // }
          }}
          buscarOutrosCargos
          classesRF="p-0"
        />
      </div>
      <div className="col-md-12 d-flex justify-content-between">
        <span>Asi Peláez (1254698)</span>
        <TextoEstilizado>Professor</TextoEstilizado>
      </div>
      <div className="col-md-12 d-flex justify-content-between">
        <span>Cândido Castaño (1254698)</span>
        <BotaoEstilizado
          id="btn-excluir"
          icon="trash-alt"
          iconType="far"
          color={Colors.CinzaBotao}
          onClick={() => {}}
          height="13px"
          width="13px"
        />
      </div>
    </ModalConteudoHtml>
  );
};

export default ModalNotificarUsuarios;
