import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

import { Button, Colors, Loader } from '~/componentes';

import { erros, sucesso, ServicoAcompanhamentoAprendizagem } from '~/servicos';

const BotaoGerarRelatorioAprendizagem = ({ semestre }) => {
  const [gerandoRelatorio, setGerandoRelatorio] = useState(false);

  const alunosAcompanhamentoAprendizagem = useSelector(
    store => store.acompanhamentoAprendizagem.alunosAcompanhamentoAprendizagem
  );

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const gerar = async () => {
    setGerandoRelatorio(true);
    await ServicoAcompanhamentoAprendizagem.gerar({
      turmaId: turmaSelecionada.id,
      semestre: parseInt(semestre, 10),
    })
      .then(() => {
        sucesso(
          'Solicitação de geração do relatório gerada com sucesso. Em breve você receberá uma notificação com o resultado'
        );
      })
      .catch(e => erros(e))
      .finally(setGerandoRelatorio(false));
  };

  return (
    <>
      {!!alunosAcompanhamentoAprendizagem?.length && (
        <Loader loading={gerandoRelatorio} ignorarTip>
          <Button
            className="btn-imprimir"
            icon="print"
            color={Colors.Azul}
            border
            onClick={gerar}
            id="btn-imprimir-relatorio-aprendizagem"
          />
        </Loader>
      )}
    </>
  );
};

BotaoGerarRelatorioAprendizagem.propTypes = {
  semestre: PropTypes.string,
};

BotaoGerarRelatorioAprendizagem.defaultProps = {
  semestre: '',
};

export default BotaoGerarRelatorioAprendizagem;
