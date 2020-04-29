import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const Encaminhamentos = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const [exibirCard, setExibirCard] = useState(false);

  const onClickExpandir = () => setExibirCard(!exibirCard);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key="encaminhamentos-collapse"
        onClick={onClickExpandir}
        titulo="Encaminhamentos"
        indice="encaminhamentos-collapse"
        show={exibirCard}
        alt="encaminhamentos"
      >
        {exibirCard ? (
          <Editor
            label="Encaminhamentos realizados"
            id="encaminhamentos-editor"
            inicial={dadosIniciais.encaminhamentos}
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

Encaminhamentos.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

Encaminhamentos.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default Encaminhamentos;
