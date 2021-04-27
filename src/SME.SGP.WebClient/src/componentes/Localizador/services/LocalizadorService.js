/* eslint-disable lines-between-class-members */
import api from '~/servicos/api';

// Mocks
import { dados } from '../__mocks__/autoComplete';

class LocalizadorService {
  urlProfessores = '/v1/professores';
  dados = dados || [];

  constructor() {
    api.interceptors.request.use(config => {
      return {
        ...config,
        headers: {
          ...config.headers,
        },
      };
    });
  }

  buscarAutocomplete({ anoLetivo, dreId, nome }) {
    return api.get(
      `${this.urlProfessores}/${anoLetivo}/autocomplete/${dreId}?nomeProfessor=${nome}`
    );
  }

  buscarPorRf({ anoLetivo, rf, buscarOutrosCargos }) {
    return api.get(
      `${this.urlProfessores}/${rf}/resumo/${anoLetivo}?buscarOutrosCargos=${buscarOutrosCargos}`
    );
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
        resolve({
          data: this.dados.filter(x => x.professorRf === parseInt(rf, 10)),
        });
      }
      resolve({ data: this.dados.filter(x => x.professorNome.includes(nome)) });
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
