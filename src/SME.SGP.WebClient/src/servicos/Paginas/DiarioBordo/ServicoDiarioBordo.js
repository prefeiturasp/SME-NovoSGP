import { dados } from './mockObservacoes';

const mockObservacoes = dados;

class ServicoDiarioBordo {
  obterDadosObservacoes = () => {
    return new Promise(resolve => {
      resolve(mockObservacoes);
    });
  };
}

export default new ServicoDiarioBordo();
