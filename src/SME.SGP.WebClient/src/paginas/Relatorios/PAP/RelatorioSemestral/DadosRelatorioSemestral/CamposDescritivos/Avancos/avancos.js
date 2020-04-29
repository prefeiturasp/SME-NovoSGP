import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const Avancos = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const [exibirCard, setExibirCard] = useState(false);

  const onClickExpandir = () => setExibirCard(!exibirCard);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key="avancos-collapse"
        onClick={onClickExpandir}
        titulo="Avanços"
        indice="avancos-collapse"
        show={exibirCard}
        alt="avancos"
      >
        {exibirCard ? (
          <Editor
            label="Avanços observados "
            id="avancos-editor"
            inicial={dadosIniciais.avancos}
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

Avancos.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

Avancos.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default Avancos;
