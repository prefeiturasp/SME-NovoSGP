import api from '~/servicos/api';

// Mocks
import { dados } from '../__mocks__/autoComplete';

class LocalizadorService {
  urlBuscarPessoa = '/buscarPessoaPorRfOuNome';

  constructor() {
    api.interceptors.request.use(config => {
      return {
        ...config,
        headers: {
          ...config.headers,
          Italo: ';D',
        },
      };
    });
  }

  buscarPessoa({ rf, nome }) {
    api
      .post(this.urlBuscarPessoa, { rf, nome })
      .then(resp => {
        return {
          sucesso: true,
          mensagem: 'Foi encontrado',
          dados: resp.data,
        };
      })
      .catch(err => ({
        sucesso: false,
        erroGeral: `Não foi encontrado! ${err}`,
      }));
  }

  metodoBuscar({ rf, nome }) {
    return new Promise(resolve => {
      if (rf) {
        resolve({ data: dados.filter(x => x.rf === parseInt(rf)) });
      }
      resolve({ data: dados.filter(x => x.nome.includes(nome)) });
    });
  }

  buscarPessoasMock({ rf, nome }) {
    return this.metodoBuscar({ rf, nome })
      .then(resp => {
        return {
          sucesso: true,
          mensagem: 'Foi encontrado',
          dados: resp.data,
        };
      })
      .catch(err => ({
        sucesso: false,
        erroGeral: `Não foi encontrado! ${err}`,
      }));
  }
}

export default new LocalizadorService();
