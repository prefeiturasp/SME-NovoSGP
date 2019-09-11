import shortid from 'shortid';
import React from 'react';
import { Modal } from 'antd';
import { store } from '../redux';
import {
  exibir,
  removerAlerta,
  alertaConfirmar,
  alertaFechar,
} from '../redux/modulos/alertas/actions';

const { confirm, error } = Modal;

const exibirAlerta = (tipo, mensagem, fixo = false) => {
  const id = shortid.generate();
  const alerta = {
    tipo,
    id,
    mensagem,
  };
  store.dispatch(exibir(alerta));
  if (!fixo) {
    setTimeout(() => {
      store.dispatch(removerAlerta(id));
    }, 3000);
  }
};

const sucesso = mensagem => {
  exibirAlerta('success', mensagem);
};

const erro = mensagem => {
  exibirAlerta('danger', mensagem);
};

const confirmacao = (
  titulo,
  texto,
  confirmar,
  cancelar,
  okText,
  okType,
  cancelText
) => {
  confirm({
    title: titulo,
    content: texto,
    okText: okText || 'Confirmar',
    okType: okType || 'primary',
    cancelText: cancelText || 'Cancelar',
    onOk() {
      confirmar();
    },
    onCancel() {
      cancelar();
    },
  });
};

const erroMensagem = (titulo, texto) => {
  error({
    title: titulo,
    content: <div>{texto ? texto.map(t => <p>{t}</p>) : null}</div>,
    type: 'error',
  });
};

const confirmar = (titulo, texto, textoNegrito) => {
  return new Promise((resolve, reject) => {
    store.dispatch(alertaConfirmar(titulo, texto, textoNegrito, resolve));
  });
};

const fecharModalConfirmacao = () => {
  store.dispatch(alertaFechar());
};

export {
  exibirAlerta,
  sucesso,
  erro,
  confirmacao,
  erroMensagem,
  confirmar,
  fecharModalConfirmacao,
};
