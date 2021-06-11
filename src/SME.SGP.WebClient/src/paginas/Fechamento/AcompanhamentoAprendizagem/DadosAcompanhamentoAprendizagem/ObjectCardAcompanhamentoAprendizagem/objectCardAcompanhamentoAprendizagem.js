import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

import DetalhesAluno from '~/componentes/Alunos/Detalhes';

import { erros, sucesso, ServicoAcompanhamentoAprendizagem } from '~/servicos';

const ObjectCardAcompanhamentoAprendizagem = ({ semestre }) => {
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
      turmaId: turmaSelecionada.id,
      alunoCodigo,
      semestre: parseInt(semestre, 10),
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

ObjectCardAcompanhamentoAprendizagem.propTypes = {
  semestre: PropTypes.string,
};

ObjectCardAcompanhamentoAprendizagem.defaultProps = {
  semestre: '',
};

export default ObjectCardAcompanhamentoAprendizagem;
