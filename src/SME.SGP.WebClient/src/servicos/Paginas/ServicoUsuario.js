import { store } from '~/redux';
import { meusDados } from '~/redux/modulos/usuario/actions';
import api from '~/servicos/api';
import { setarPerfis } from '~/redux/modulos/perfil/actions';
import { erro, sucesso } from '../alertas';

const obterMeusDados = () => {
  api.get('v1/usuarios/meus-dados').then(resp => {
    if (resp && resp.data) {
      const dados = resp.data;
      store.dispatch(
        meusDados({
          nome: dados.nome,
          rf: dados.codigoRf,
          cpf: dados.cpf,
          empresa: dados.empresa,
          email: dados.email,
        })
      );
    }
  });
};

const obterPerfis = login => {
  api
    .get(`v1/autenticacao/${login}/perfis/listar`)
    .then(resp => {
      if (resp && resp.data) {
        store.dispatch(setarPerfis(resp.data));
        sucesso('Perfis atualizados');
      }
    })
    .catch(err => {
      erro('Não foi possivel obter os perfis do ususario');
    });
};

const obterTodosPerfis = () => {
  return api.get('v1/usuarios/perfis');
};

const obterListaSituacoes = () => {
  return api.get('v1/usuarios/situacoes');
};

export { obterMeusDados, obterPerfis, obterTodosPerfis, obterListaSituacoes };
