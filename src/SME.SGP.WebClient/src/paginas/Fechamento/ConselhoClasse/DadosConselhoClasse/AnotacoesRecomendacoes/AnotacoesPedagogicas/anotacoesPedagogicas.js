import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';

const AnotacoesPedagogicas = props => {
  const { onChange, dadosIniciais, alunoDesabilitado } = props;

  const dentroPeriodo = useSelector(
    store => store.conselhoClasse.dentroPeriodo
  );

  const desabilitarCampos = useSelector(
    store => store.conselhoClasse.desabilitarCampos
  );

  const [exibirCardAnotacao, setExibirCardAnotacao] = useState(true);

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
          <JoditEditor
            id="anotacoes-pedagogicas-editor"
            value={dadosIniciais.anotacoesPedagogicas}
            onChange={onChange}
            desabilitar={
              alunoDesabilitado || desabilitarCampos || !dentroPeriodo
            }
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
  alunoDesabilitado: PropTypes.bool,
};

AnotacoesPedagogicas.defaultProps = {
  onChange: () => {},
  dadosIniciais: {},
  alunoDesabilitado: false,
};

export default AnotacoesPedagogicas;
