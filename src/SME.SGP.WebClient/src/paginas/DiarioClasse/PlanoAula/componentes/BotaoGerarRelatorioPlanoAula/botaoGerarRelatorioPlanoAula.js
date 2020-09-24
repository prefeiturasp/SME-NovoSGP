import React, { useState } from 'react';
import { Colors, Loader } from '~/componentes';
import Button from '~/componentes/button';
import { erros, sucesso } from '~/servicos/alertas';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';
import shortid from 'shortid';

const BotaoGerarRelatorioPlanoAula = props => {
  const { planoAulaId } = props;

  const [imprimindo, setImprimindo] = useState(false);

  const gerarPlanoAula = () => {
    setImprimindo(true);
    ServicoPlanoAula.gerarRelatorioPlanoAula(planoAulaId)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado.'
        );
      })
      .finally(setImprimindo(false))
      .catch(e => erros(e));
  };

  return (
    <div className="mr-3">
      <Loader loading={imprimindo} ignorarTip>
        <Button
          id={shortid.generate()}
          className="btn-imprimir"
          icon="print"
          color={Colors.Azul}
          border
          onClick={() => gerarPlanoAula()}
          id="btn-imprimir-relatorio"
          disabled={!planoAulaId}
        />
      </Loader>
    </div>
  );
};

export default BotaoGerarRelatorioPlanoAula;
