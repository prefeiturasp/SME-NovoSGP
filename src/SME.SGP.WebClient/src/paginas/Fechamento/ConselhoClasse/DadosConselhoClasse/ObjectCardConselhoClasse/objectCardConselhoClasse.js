import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erro, sucesso } from '~/servicos/alertas';
import { Loader } from '~/componentes';

const ObjectCardConselhoClasse = props => {
  const { conselhoClasseId, fechamentoTurmaId } = props;
  const [gerandoConselhoClasse, setGerandoConselhoClasse] = useState(false);
  const dadosAlunoObjectCard = useSelector(
    store => store.conselhoClasse.dadosAlunoObjectCard
  );

  const conselhoClasseAlunoId = useSelector(
    store =>
      store.conselhoClasse.dadosPrincipaisConselhoClasse.conselhoClasseAlunoId
  );

  const gerarConselhoClasseAluno = async () => {
    setGerandoConselhoClasse(true);
    await ServicoConselhoClasse.gerarConselhoClasseAluno(
      conselhoClasseId,
      fechamentoTurmaId,
      dadosAlunoObjectCard.codigoEOL
    )
      .then(() => {
        sucesso('Gerando conselho de classe do aluno');
      })
      .finally(setGerandoConselhoClasse(false))
      .catch(e => erro(e));
  };

  return (
    <Loader loading={gerandoConselhoClasse}>
      <DetalhesAluno
        dados={dadosAlunoObjectCard}
        desabilitarImprimir={!conselhoClasseAlunoId}
        onClickImprimir={gerarConselhoClasseAluno}
      />
    </Loader>
  );
};

export default ObjectCardConselhoClasse;
