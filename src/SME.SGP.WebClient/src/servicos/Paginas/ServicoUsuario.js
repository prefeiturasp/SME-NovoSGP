import {store} from '~/redux';
import {meusDados} from '~/redux/modulos/usuario/actions';
import api from '~/servicos/api';

const obterMeusDados = () =>{
    api.get('v1/usuarios/meus-dados').then(resp => {
        if (resp && resp.data) {
          const dados = resp.data
          console.log(dados);
          store.dispatch(meusDados(
            {
              nome: dados.nome,
              rf: dados.codigoRf,
              cpf: dados.cpf,
              empresa: dados.empresa,
              email: dados.email
            }
          ))
        }
      })
}

export {obterMeusDados};
