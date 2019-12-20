import shortid from 'shortid';

export const estadoInicial = {
  turmas: [],
};

const adicionarAvaliacao = () => ({
  type: '@copiaAvaliacao/adicionarAvaliacao',
});

const excluirAvaliacao = payload => ({
  type: '@copiaAvaliacao/excluirAvaliacao',
  payload,
});

const carregarData = payload => ({
  type: '@copiaAvaliacao/carregarData',
  payload,
});

const selecionarTurma = payload => ({
  type: '@copiaAvaliacao/selecionarTurma',
  payload,
});

const diasParaHabilitar = payload => ({
  type: '@copiaAvaliacao/diasParaHabilitar',
  payload,
});

const selecionarData = payload => ({
  type: '@copiaAvaliacao/selecionarData',
  payload,
});

const erroData = payload => ({
  type: '@copiaAvaliacao/erroData',
  payload,
});

// Actions
export {
  adicionarAvaliacao,
  excluirAvaliacao,
  carregarData,
  selecionarTurma,
  diasParaHabilitar,
  selecionarData,
  erroData,
};

export default (state, action) => {
  const { payload, type } = action;
  switch (type) {
    case '@copiaAvaliacao/adicionarAvaliacao':
      return {
        turmas: [
          ...state.turmas,
          {
            id: shortid.generate(),
            turmaId: '',
            data: '',
            temErro: false,
            mensagemErro: '',
          },
        ],
      };
    case '@copiaAvaliacao/excluirAvaliacao':
      return {
        turmas: state.turmas.filter(x => x.id !== payload),
      };
    case '@copiaAvaliacao/carregarData':
      return {
        ...state,
        turmas: state.turmas.map(x =>
          x.id === payload.id
            ? {
                ...x,
                carregandoData: payload.valor,
              }
            : x
        ),
      };
    case '@copiaAvaliacao/selecionarTurma':
      return {
        ...state,
        turmas: state.turmas.map(x =>
          x.id === payload.id
            ? {
                ...x,
                turmaId: payload.turmaId,
              }
            : x
        ),
      };
    case '@copiaAvaliacao/diasParaHabilitar':
      return {
        ...state,
        turmas: state.turmas.map(x =>
          x.id === payload.id
            ? {
                ...x,
                diasParaHabilitar: payload.datas.map(y =>
                  window.moment(y.data).format('YYYY-MM-DD')
                ),
              }
            : x
        ),
      };
    case '@copiaAvaliacao/selecionarData':
      return {
        ...state,
        turmas: state.turmas.map(x =>
          x.id === payload.id
            ? {
                ...x,
                data: payload.data,
              }
            : x
        ),
      };
    case '@copiaAvaliacao/erroData':
      return {
        ...state,
        turmas: state.turmas.map(x =>
          x.turmaId === payload.turmaId && x.id === payload.id
            ? {
                ...x,
                temErro: true,
                mensagemErro: 'Turma já possui avaliação',
              }
            : {
                ...x,
                temErro: false,
              }
        ),
      };
    default:
      break;
  }
  return null;
};
