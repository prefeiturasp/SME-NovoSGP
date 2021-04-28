import React from 'react';
import { useSelector } from 'react-redux';

import { DetalhesAluno } from '~/componentes';

import { erros, ServicoRegistroIndividual, sucesso } from '~/servicos';

const ObjectCardRegistroIndividual = () => {
  const dadosAlunoObjectCard = useSelector(
    store => store.registroIndividual.dadosAlunoObjectCard
  );

  const desabilitarCampos = useSelector(
    state => state.registroIndividual.desabilitarCampos
  );

  const dataInicioImpressaoRegistrosAnteriores = useSelector(
    state => state.registroIndividual.dataInicioImpressaoRegistrosAnteriores
  );

  const dataFimImpressaoRegistrosAnteriores = useSelector(
    state => state.registroIndividual.dataFimImpressaoRegistrosAnteriores
  );

  const turmaSelecionada = useSelector(state => state.usuario.turmaSelecionada);

  const { codigoEOL } = dadosAlunoObjectCard;

  const gerar = async () => {
    await ServicoRegistroIndividual.gerar({
      alunoCodigo: codigoEOL,
      dataInicio: dataInicioImpressaoRegistrosAnteriores,
      dataFim: dataFimImpressaoRegistrosAnteriores,
      turmaId: turmaSelecionada.id,
    })
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .catch(e => erros(e));
  };

  return (
    <DetalhesAluno
      exibirFrequencia={false}
      dados={dadosAlunoObjectCard}
      permiteAlterarImagem={!desabilitarCampos}
      onClickImprimir={gerar}
      desabilitarImprimir={
        !codigoEOL ||
        !dataInicioImpressaoRegistrosAnteriores ||
        !dataFimImpressaoRegistrosAnteriores
      }
    />
  );
};

export default ObjectCardRegistroIndividual;
