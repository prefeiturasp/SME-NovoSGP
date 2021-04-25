import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import { erros, sucesso, ServicoAcompanhamentoAprendizagem } from '~/servicos';

const ObjectCardAcompanhamentoAprendizagem = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.acompanhamentoAprendizagem.dadosAlunoObjectCard
  );

  const desabilitarCamposAcompanhamentoAprendizagem = useSelector(
    store =>
      store.acompanhamentoAprendizagem
        .desabilitarCamposAcompanhamentoAprendizagem
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const alunoCodigo = dadosAlunoObjectCard?.codigoEOL;

  const gerar = async () => {
    await ServicoAcompanhamentoAprendizagem.gerar({
      turmaCodigo: turmaSelecionada.turma,
      alunoCodigo,
    })
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado'
        );
      })
      .catch(e => erros(e));
  };

  return (
    <DetalhesAluno
      dados={dadosAlunoObjectCard}
      desabilitarImprimir={!alunoCodigo}
      onClickImprimir={gerar}
      permiteAlterarImagem={!desabilitarCamposAcompanhamentoAprendizagem}
    />
  );
};

export default ObjectCardAcompanhamentoAprendizagem;
