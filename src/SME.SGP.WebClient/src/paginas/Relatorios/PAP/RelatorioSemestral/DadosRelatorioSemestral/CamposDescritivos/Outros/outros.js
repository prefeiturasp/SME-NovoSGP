import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import Editor from '~/componentes/editor/editor';

const Outros = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const desabilitarCampos = useSelector(
    store => store.relatorioSemestral.desabilitarCampos
  );

  const [exibirCard, setExibirCard] = useState(false);

  const onClickExpandir = () => setExibirCard(!exibirCard);

  return (
    <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2 mt-4">
      <CardCollapse
        key="outros-collapse"
        onClick={onClickExpandir}
        titulo="Outros"
        indice="outros-collapse"
        show={exibirCard}
        alt="outros"
      >
        {exibirCard ? (
          <Editor
            label="Outras observações"
            id="outros-editor"
            inicial={dadosIniciais.outros}
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

Outros.propTypes = {
  onChange: PropTypes.func,
  dadosIniciais: PropTypes.oneOfType([PropTypes.object]),
  alunoDesabilitado: PropTypes.bool,
};

Outros.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default Outros;
