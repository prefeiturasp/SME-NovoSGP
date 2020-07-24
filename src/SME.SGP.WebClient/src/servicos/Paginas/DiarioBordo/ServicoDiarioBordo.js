import * as moment from 'moment';
import { store } from '~/redux';
import { setDadosObservacoesChat } from '~/redux/modulos/observacoesChat/actions';
import { dados } from './mockObservacoes';

const mockObservacoes = dados;

class ServicoDiarioBordo {
  obterDadosObservacoes = () => {
    return new Promise(resolve => {
      resolve(mockObservacoes);
    });
  };

  editarEditarObservacao = observacao => {
    // TODO Remover mock e chamar enpoint novo!
    const { dispatch } = store;
    const state = store.getState();

    const { observacoesChat } = state;
    const { dadosObservacoes } = observacoesChat;

    if (observacao.id) {
      console.log(`Alterado observação ID ${observacao.id}`);
      const item = dadosObservacoes.find(e => e.id === observacao.id);
      const index = dadosObservacoes.indexOf(item);
      dadosObservacoes[index] = { ...observacao };
      dispatch(setDadosObservacoesChat([...dadosObservacoes]));
    } else {
      const params = {
        id: dadosObservacoes.length + 1,
        texto: observacao,
        rf: 9999,
        proprietario: true,
        auditoria: {
          criadoEm: moment(),
          criadoPor: 'HELOISA MARIA DE MORAIS GIANNICHI',
          criadoRF: '7944560',
          alteradoEm: moment(),
          alteradoPor: 'HELOISA MARIA DE MORAIS GIANNICHI',
          alteradoRF: '7944560',
        },
      };
      console.log(`Salvando observação ID ${observacao.id}`);
      const dadosObs = dadosObservacoes;
      dadosObs.unshift(params);
      dispatch(setDadosObservacoesChat([...dadosObs]));
    }
    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ status: 200 });
      }, 2000);
    });
  };

  excluirObservacao = observacao => {
    // TODO Remover mock e chamar enpoint novo!
    console.log(`Excluido observação ID: ${observacao.id}`);
    const { dispatch } = store;
    const state = store.getState();

    const { observacoesChat } = state;
    const { dadosObservacoes } = observacoesChat;

    const item = dadosObservacoes.find(e => e.id === observacao.id);
    const index = dadosObservacoes.indexOf(item);
    dadosObservacoes.splice(index, 1);

    dispatch(setDadosObservacoesChat([...dadosObservacoes]));

    return new Promise(resolve => {
      setTimeout(() => {
        resolve({ status: 200 });
      }, 2000);
    });
  };
}

export default new ServicoDiarioBordo();
