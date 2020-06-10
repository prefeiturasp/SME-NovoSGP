import { dados } from '../__mocks__/autoComplete';

class LocalizadorService {
  dados = dados || [];

  buscarNomeMock({ codigo, nome }) {
    return new Promise(resolve => {
      if (codigo) {
        resolve({
          data: this.dados.filter(x => x.alunoCodigo === parseInt(codigo, 10)),
        });
      }
      resolve({ data: this.dados.filter(x => x.alunoNome.includes(nome)) });
    });
  }

  buscarCodigoMock({ codigo, nome }) {
    return new Promise(resolve => {
      if (codigo) {
        resolve({
          data: this.dados.find(x => x.alunoCodigo === parseInt(codigo, 10)),
        });
      }
      resolve({ data: this.dados.find(x => x.alunoNome.includes(nome)) });
    });
  }
}

export default new LocalizadorService();
