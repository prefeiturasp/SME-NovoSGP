import { InputNumber } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React from 'react';
import styled, { css } from 'styled-components';
import { Base } from './colors';
import Label from './label';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
  }
  .ant-input-number {
    height: 38px;
    &-handler-wrap {
      ${({ esconderSetas }) =>
        esconderSetas &&
        css`
          display: none;
        `}
    }
  }

  height: 45px;
`;

const CampoNumero = React.forwardRef((props, ref) => {
  const {
    name,
    id,
    form,
    className,
    classNameCampo,
    type,
    maskType,
    placeholder,
    onChange,
    onKeyDown,
    value,
    desabilitado,
    maxlength,
    label,
    semMensagem,
    max,
    min,
    step,
    disabled,
    onBlur,
    ehDecimal,
    decimalSeparator,
    esconderSetas,
    onKeyUp,
  } = props;

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && relatedTarget.getAttribute('type') === 'button') {
      event.preventDefault();
    }
  };

  const validaFormatter = valor => {
    if (!ehDecimal) {
      valor = valor.toString().replace('.', '');
      valor = valor.toString().replace(',', '');
    }
    return valor;
  };

  const validaParser = valor => {
    if (!ehDecimal) {
      valor = valor.replace(/[^0-9.]/g, '');
    }
    return valor;
  };

  return (
    <>
      <Campo esconderSetas={esconderSetas} className={classNameCampo}>
        {label ? <Label text={label} control={name || ''} /> : ''}
        {form ? (
          <>
            {' '}
            <Field
              name={name}
              id={id || name}
              className={`form-control campo ${
                possuiErro() ? 'is-invalid' : ''
              } ${className || ''} ${desabilitado ? 'desabilitado' : ''}`}
              component={InputNumber}
              type={maskType || ''}
              readOnly={desabilitado}
              onBlur={executaOnBlur}
              maxLength={maxlength || ''}
              innerRef={ref}
              onKeyDown={onKeyDown}
              onKeyUp={onKeyUp}
              onChange={value => {
                form.setFieldValue(name, value);
                form.setFieldTouched(name, true);
                onChange(value);
              }}
              disabled={disabled}
            />
            {!semMensagem ? <span>{form.errors[name]}</span> : ''}
          </>
        ) : (
          <InputNumber
            name={name}
            ref={ref}
            placeholder={placeholder}
            onChange={onChange}
            readOnly={desabilitado}
            onKeyDown={onKeyDown}
            onKeyUp={onKeyUp}
            value={value}
            max={max}
            min={min}
            step={step}
            className={className}
            disabled={disabled}
            onBlur={onBlur}
            formatter={v => validaFormatter(v)}
            parser={v => validaParser(v)}
            decimalSeparator={decimalSeparator}
          />
        )}
      </Campo>
    </>
  );
});

CampoNumero.propTypes = {
  onChange: PropTypes.func,
  onBlur: PropTypes.func,
  semMensagem: PropTypes.bool,
  ehDecimal: PropTypes.bool,
  decimalSeparator: PropTypes.string,
  esconderSetas: PropTypes.bool,
  onKeyUp: PropTypes.func,
};

CampoNumero.defaultProps = {
  onChange: () => {},
  onBlur: () => {},
  semMensagem: false,
  ehDecimal: true,
  decimalSeparator: ',',
  esconderSetas: false,
  onKeyUp: () => {},
};

export default CampoNumero;
