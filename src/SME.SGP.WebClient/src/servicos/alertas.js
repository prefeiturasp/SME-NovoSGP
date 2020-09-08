import { Modal, notification } from 'antd';
import { store } from '../redux';
import {
  alertaConfirmar,
  alertaFechar,
} from '../redux/modulos/alertas/actions';

const { confirm } = Modal;

const exibirAlerta = (tipo, mensagem) => {
  let titulo;
  let classeTipo;
  switch (tipo) {
    case 'success':
      titulo = 'Sucesso';
      classeTipo = 'alerta-sucesso';
      break;
    case 'error':
      titulo = 'Erro';
      classeTipo = 'alerta-erro';
      break;
    case 'warning':
      titulo = 'Aviso';
      classeTipo = 'alerta-aviso';
      break;

    default:
      titulo = '';
      classeTipo = '';
      break;
  }
  notification[tipo]({
    message: titulo,
    description: mensagem,
    duration: 6,
    className: classeTipo,
  });
};

const sucesso = mensagem => {
  exibirAlerta('success', mensagem);
};

const erro = mensagem => {
  exibirAlerta('error', mensagem);
};

const aviso = mensagem => {
  exibirAlerta('warning', mensagem);
};

const erros = listaErros => {
  if (
    listaErros &&
    listaErros.response &&
    listaErros.response.data &&
    listaErros.response.data.mensagens
  ) {
    listaErros.response.data.mensagens.forEach(mensagem => erro(mensagem));
  } else erro('Ocorreu um erro interno.');
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

const confirmar = (
  titulo,
  texto,
  textoNegrito,
  textoOk,
  textoCancelar,
  primeiroExibirTextoNegrito
) => {
  return new Promise((resolve, _) => {
    store.dispatch(
      alertaConfirmar(
        titulo,
        texto,
        textoNegrito,
        resolve,
        textoOk,
        textoCancelar,
        primeiroExibirTextoNegrito
      )
    );
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
  confirmar,
  fecharModalConfirmacao,
  erros,
  aviso,
};
