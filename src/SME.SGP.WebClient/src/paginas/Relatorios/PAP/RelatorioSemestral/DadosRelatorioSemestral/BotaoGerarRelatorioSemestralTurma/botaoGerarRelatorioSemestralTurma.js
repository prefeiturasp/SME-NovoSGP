import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Colors, Loader } from '~/componentes';
import Button from '~/componentes/button';
import { erros, sucesso } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/RelatorioSemestral/ServicoRelatorioSemestral';

const BotaoGerarRelatorioSemestralTurma = props => {
  const { semestreSelecionado } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [gerandoRelatorio, setGerandoRelatorio] = useState(false);

  const gerar = async () => {
    setGerandoRelatorio(true);
    const params = {
      turmaCodigo: turmaSelecionada.turma,
      semestre: semestreSelecionado,
    };
    await ServicoRelatorioSemestral.gerar(params)
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado'
        );
      })
      .catch(e => erros(e))
      .finally(setGerandoRelatorio(false));
  };

  return (
    <div>
      <Loader loading={gerandoRelatorio} ignorarTip>
        <Button
          className="btn-imprimir"
          icon="print"
          color={Colors.Azul}
          border
          onClick={gerar}
          id="btn-imprimir-relatorio-semestral"
        />
      </Loader>
    </div>
  );
};

BotaoGerarRelatorioSemestralTurma.propTypes = {
  semestreSelecionado: PropTypes.string,
};

BotaoGerarRelatorioSemestralTurma.defaultProps = {
  semestreSelecionado: '',
};

export default BotaoGerarRelatorioSemestralTurma;
