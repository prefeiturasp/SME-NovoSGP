import shortid from 'shortid';
import { store } from '../redux';

import { exibir, removerAlerta } from '../redux/modulos/alertas/actions';

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
export { exibirAlerta, sucesso, erro };
