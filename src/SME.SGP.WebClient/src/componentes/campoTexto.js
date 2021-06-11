import { Input } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import { Base } from './colors';
import Label from './label';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
  }
  .ant-input {
    height: ${({ height }) => height}px;
  }
  label {
    font-weight: bold;
  }
  .ant-input-affix-wrapper .ant-input:not(:first-child) {
    padding-left: 40px;
  }
`;

const CampoTexto = React.forwardRef((props, ref) => {
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
    maxLength,
    label,
    semMensagem,
    style,
    iconeBusca,
    allowClear,
    minRowsTextArea,
    height,
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

  const onChangeCampo = e => {
    form.setFieldValue(name, e.target.value);
    form.setFieldTouched(name, true, true);
    onChange(e);
  };

  return (
    <Campo className={classNameCampo} height={height}>
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
            component={type || Input}
            type={maskType}
            readOnly={desabilitado}
            disabled={desabilitado}
            onBlur={executaOnBlur}
            maxLength={maxLength || ''}
            innerRef={ref}
            onKeyDown={onKeyDown}
            placeholder={placeholder}
            onChange={onChangeCampo}
            style={style}
            prefix={iconeBusca ? <i className="fa fa-search fa-lg" /> : ''}
            value={value || form.values[name]}
            rows={minRowsTextArea}
          />
          {!semMensagem && form && form.touched[name] ? (
            <span>{form.errors[name]}</span>
          ) : (
            ''
          )}
        </>
      ) : (
        <Input
          ref={ref}
          placeholder={placeholder}
          onChange={onChange}
          disabled={desabilitado}
          onKeyDown={onKeyDown}
          value={value}
          prefix={iconeBusca ? <i className="fa fa-search fa-lg" /> : ''}
          allowClear={allowClear}
        />
      )}
    </Campo>
  );
});

CampoTexto.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  form: PropTypes.oneOfType([PropTypes.any]),
  className: PropTypes.string,
  classNameCampo: PropTypes.string,
  type: PropTypes.string,
  maskType: PropTypes.string,
  placeholder: PropTypes.string,
  onChange: PropTypes.oneOfType([PropTypes.func]),
  onKeyDown: PropTypes.oneOfType([PropTypes.func]),
  value: PropTypes.oneOfType([PropTypes.any]),
  desabilitado: PropTypes.bool,
  maxLength: PropTypes.oneOfType([PropTypes.number]),
  label: PropTypes.string,
  semMensagem: PropTypes.bool,
  style: PropTypes.oneOfType([PropTypes.any]),
  iconeBusca: PropTypes.bool,
  allowClear: PropTypes.bool,
  minRowsTextArea: PropTypes.string,
  height: PropTypes.string,
};

CampoTexto.defaultProps = {
  name: '',
  id: '',
  form: null,
  className: '',
  classNameCampo: '',
  type: '',
  maskType: '',
  placeholder: '',
  onChange: () => {},
  onKeyDown: () => {},
  value: '',
  desabilitado: false,
  maxLength: 100,
  label: '',
  semMensagem: false,
  style: {},
  iconeBusca: false,
  allowClear: true,
  minRowsTextArea: '2',
  height: '38',
};

export default CampoTexto;
