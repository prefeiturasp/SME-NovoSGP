import React from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/RelatorioSemestral/ServicoRelatorioSemestral';
import { sucesso, erros } from '~/servicos/alertas';

const ObjectCardRelatorioSemestral = props => {
  const { semestre } = props;
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const dadosAlunoObjectCard = useSelector(
    store => store.relatorioSemestralPAP.dadosAlunoObjectCard
  );

  const relatorioSemestralAlunoId = useSelector(
    store =>
      store.relatorioSemestralPAP.dadosRelatorioSemestral
        .relatorioSemestralAlunoId
  );

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestralPAP.desabilitarCampos
  );

  const gerar = async () => {
    const params = {
      turmaCodigo: turmaSelecionada.turma,
      alunoCodigo: dadosAlunoObjectCard.codigoEOL,
      semestre,
    };
    await ServicoRelatorioSemestral.gerar(params)
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
      desabilitarImprimir={!relatorioSemestralAlunoId}
      onClickImprimir={gerar}
      permiteAlterarImagem={!desabilitarCampos}
    />
  );
};

export default ObjectCardRelatorioSemestral;
