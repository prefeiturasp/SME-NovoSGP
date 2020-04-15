import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const AnotacoesPedagogicas = props => {
  const { onChange, dadosIniciais } = props;

  const [exibirCardAnotacao, setExibirCardAnotacao] = useState(false);

  const onClickExpandirAnotacao = async () =>
    setExibirCardAnotacao(!exibirCardAnotacao);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
      <CardCollapse
        key="anotacoes-pedagogicas-collapse"
        onClick={onClickExpandirAnotacao}
        titulo="Anotações pedagógicas"
        indice="anotacoes-pedagogicas-collapse"
        show={exibirCardAnotacao}
        alt="anotacoes-pedagogicas"
      >
        {exibirCardAnotacao ? (
          <Editor
            id="anotacoes-pedagogicas-editor"
            inicial={dadosIniciais.anotacoesPedagogicas}
            onChange={onChange}
          />
        ) : (
          ''
        )}
      </CardCollapse>
    </div>
  );
};

AnotacoesPedagogicas.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
};

AnotacoesPedagogicas.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
};

export default AnotacoesPedagogicas;
