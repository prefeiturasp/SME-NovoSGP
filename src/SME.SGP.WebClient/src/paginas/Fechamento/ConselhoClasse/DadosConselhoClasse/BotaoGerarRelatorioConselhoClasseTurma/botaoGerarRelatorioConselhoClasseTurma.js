import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Colors, Loader } from '~/componentes';
import Button from '~/componentes/button';
import { erros, sucesso } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';

const BotaoGerarRelatorioConselhoClasseTurma = () => {
  const conselhoClasseId = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse.conselhoClasseId
  );

  const fechamentoTurmaId = useSelector(
    store =>
      store.conselhoClasse.dadosPrincipaisConselhoClasse.fechamentoTurmaId
  );

  const [imprimindo, setImprimindo] = useState(false);

  const gerarConselhoClasseTurma = async () => {
    setImprimindo(true);
    await ServicoConselhoClasse.gerarConselhoClasseTurma(
      conselhoClasseId || 0,
      fechamentoTurmaId || 0
    )
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .finally(setImprimindo(false))
      .catch(e => erros(e));
  };

  return (
    <div>
      <Loader loading={imprimindo} ignorarTip>
        <Button
          className="btn-imprimir"
          icon="print"
          color={Colors.Azul}
          border
          onClick={() => gerarConselhoClasseTurma()}
          id="btn-imprimir-relatorio-pendencias"
        />
      </Loader>
    </div>
  );
};

export default BotaoGerarRelatorioConselhoClasseTurma;
