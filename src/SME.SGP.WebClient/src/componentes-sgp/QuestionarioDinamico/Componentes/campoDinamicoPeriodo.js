import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch } from 'react-redux';
import { setQuestionarioDinamicoEmEdicao } from '~/redux/modulos/questionarioDinamico/actions';
import { CampoData } from '~/componentes/campoData/campoData';

const CampoDinamicoPeriodo = props => {
  const dispatch = useDispatch();

  const { questaoAtual, form, label, desabilitado } = props;

  return (
    <div className="col-md-12 mb-3">
      {label}
      <div className="col-md-2">
        <CampoData
          id={`${questaoAtual?.id}`}
          name={`${questaoAtual?.id}`}
          form={form}
          formatoData="DD/MM/YYYY"
          desabilitado={desabilitado}
          placeholder={['Início', 'Término']}
          onChange={() => {
            dispatch(setQuestionarioDinamicoEmEdicao(true));
          }}
          periodo
        />
      </div>
      {/* <RangePicker
        form={form}
        id={`${questaoAtual?.id}-inicio`}
        name={`${questaoAtual?.id}-inicio`}
        defaultValue={[
          moment(dataAtual, dateFormat),
          moment(dataAtual, dateFormat),
        ]}
        disabled={desabilitado}
        onChange={() => {
          dispatch(setQuestionarioDinamicoEmEdicao(true));
        }}
        locale={locale}
        placeholder={['Início', 'Término']}
        format={dateFormat}
        dateRender={current => {
          const style = {};
          if (current.date() === 1) {
            style.border = '1px solid #1890ff';
            style.borderRadius = '50%';
          }
          return (
            <div className="ant-picker-cell-inner" style={style}>
              {current.date()}
            </div>
          );
        }}
      /> */}
    </div>
  );
};

CampoDinamicoPeriodo.propTypes = {
  questaoAtual: PropTypes.oneOfType([PropTypes.any]),
  form: PropTypes.oneOfType([PropTypes.any]),
  label: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
};

CampoDinamicoPeriodo.defaultProps = {
  questaoAtual: null,
  form: null,
  label: '',
  desabilitado: false,
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
