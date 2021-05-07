import { Base } from '~/componentes/colors';

const statusAcompanhamentoFechamento = {
  NAO_INICIADO: {
    descricao: 'Não Iniciado',
    cor: Base.CinzaMako,
  },
  EM_ANDAMENTO: {
    descricao: 'Em Andamento',
    cor: Base.LaranjaStatus,
  },
  PROCESSADO_PENDENCIAS: {
    descricao: 'Processado com pendências',
    cor: Base.LaranjaStatus,
  },
  PROCESSADO_SUCESSO: {
    descricao: 'Processado com sucesso',
    cor: Base.Verde,
  },
  PROCESSADO: {
    descricao: 'Processado',
    cor: Base.Verde,
  },
  CONCLUIDO: {
    descricao: 'Concluído',
    cor: Base.Verde,
  },
};

export default statusAcompanhamentoFechamento;
