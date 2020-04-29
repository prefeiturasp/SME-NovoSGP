import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const HistoricoEstudante = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const [exibirCard, setExibirCard] = useState(false);

  const onClickExpandir = () => setExibirCard(!exibirCard);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key="historico-estudante-collapse"
        onClick={onClickExpandir}
        titulo="Histórico do estudante"
        indice="historico-estudante-collapse"
        show={exibirCard}
        alt="historico-estudante"
      >
        {exibirCard ? (
          <Editor
            label="Trajetória do estudante, reprovações, histórico de faltas, acompanhamento das aprendizagens"
            id="historico-estudante-editor"
            inicial={dadosIniciais.historicoEstudante}
            onChange={onChange}
            desabilitar={alunoDesabilitado || desabilitarCampos}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

HistoricoEstudante.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

HistoricoEstudante.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default HistoricoEstudante;
