import PropTypes from 'prop-types';
import React from 'react';
import { CampoData } from '~/componentes/campoData/campoData';

const CampoDinamicoPeriodo = props => {
  const { questaoAtual, form, label, desabilitado, onChange } = props;

  return (
    <div className="col-md-12 mb-3">
      {label}
      <div className="row">
        <div className="col-md-2">
          <CampoData
            form={form}
            id={`${questaoAtual?.id}.periodoInicio`}
            name={`${questaoAtual?.id}.periodoInicio`}
            placeholder="Início"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={onChange}
          />
        </div>
        <span style={{ marginTop: 5 }}>à</span>
        <div className="col-md-2">
          <CampoData
            form={form}
            id={`${questaoAtual?.id}.periodoFim`}
            name={`${questaoAtual?.id}.periodoFim`}
            placeholder="Fim"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={onChange}
          />
        </div>
      </div>
    </div>
  );
};

CampoDinamicoPeriodo.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
  onChange: PropTypes.func,
};

CampoDinamicoPeriodo.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
  onChange: () => {},
};

export default CampoDinamicoPeriodo;

{
  /* <div className="col-md-2">
          <CampoData
            form={form}
            id={`${questaoAtual?.id}-inicio`}
            name={`${questaoAtual?.id}-inicio`}
            placeholder="Início"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
            }}
          />
        </div>
        à
        <div className="col-md-2">
          <CampoData
            form={form}
            id={`fim${questaoAtual?.id}`}
            name={`fim${questaoAtual?.id}`}
            placeholder="Fim"
            formatoData="DD/MM/YYYY"
            desabilitado={desabilitado}
            onChange={() => {
              dispatch(setQuestionarioDinamicoEmEdicao(true));
            }}
          />
        </div>
      </div> */
}
