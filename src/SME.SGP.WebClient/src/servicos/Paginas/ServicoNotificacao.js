import { erros, erro, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';

class ServicoNotificacao {
  excluir = async (notificacoesId, callback) => {
    api
      .delete('v1/notificacoes/', {
        data: notificacoesId,
      })
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(resultado => {
            if (resultado.sucesso) {
              sucesso(resultado.mensagem);
            } else {
              erro(resultado.mensagem);
            }
          });
        }
        if (callback) callback();
      })
      .catch(listaErros => erros(listaErros));
  };

  marcarComoLida = (idsNotificacoes, callback) => {
    api
      .put('v1/notificacoes/status/lida', idsNotificacoes)
      .then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(resultado => {
            if (resultado.sucesso) {
              sucesso(resultado.mensagem);
            } else {
              erro(resultado.mensagem);
            }
          });
        }
        if (callback) callback();
      })
      .catch(listaErros => erros(listaErros));
  };
}

export default new ServicoNotificacao();
