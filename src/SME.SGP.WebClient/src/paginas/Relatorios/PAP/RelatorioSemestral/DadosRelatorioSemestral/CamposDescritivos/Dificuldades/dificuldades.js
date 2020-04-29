import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const Dificuldades = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const [exibirCard, setExibirCard] = useState(false);

  const onClickExpandir = () => setExibirCard(!exibirCard);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key="dificuldades-collapse"
        onClick={onClickExpandir}
        titulo="Dificuldades"
        indice="dificuldades-collapse"
        show={exibirCard}
        alt="dificuldades"
      >
        {exibirCard ? (
          <Editor
            label="Dificuldades apresentadas inicialmente"
            id="dificuldades-editor"
            inicial={dadosIniciais.dificuldades}
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

Dificuldades.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

Dificuldades.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default Dificuldades;
