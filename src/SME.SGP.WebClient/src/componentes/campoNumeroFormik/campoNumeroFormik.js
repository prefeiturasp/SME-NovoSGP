import { InputNumber } from 'antd';
import { Field } from 'formik';
import PropTypes from 'prop-types';
import React from 'react';
import Label from '../label';
import { Campo } from './campoNumeroFormik.css';

const CampoNumeroFormik = React.forwardRef((props, ref) => {
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
    maxlength,
    label,
    semMensagem,
    max,
    min,
    step,
    disabled,
    onBlur,
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

  return (
    <Campo>
      {label ? <Label text={label} control={name || ''} /> : ''}
      <Field name={name} id={name}>
        {({
          field: { value },
          form: { setFieldValue, setFieldTouched, errors },
        }) => (
          <div>
            <div>
              <InputNumber
                ref={ref}
                placeholder={placeholder}
                onChange={valor => {
                  form.setFieldValue(name, valor);
                  form.setFieldTouched(name, true);
                  onChange(valor);
                }}
                className={`form-control campo ${
                  possuiErro() ? 'is-invalid' : ''
                } ${className || ''} ${disabled ? 'desabilitado' : ''}`}
                readOnly={disabled}
                onKeyDown={onKeyDown}
                value={value}
                max={max}
                min={min}
                step={step}
                disabled={disabled}
                onBlur={onBlur}
              />
            </div>
          </div>
        )}
      </Field>
      {!semMensagem ? <span>{form.errors[name]}</span> : ''}
    </Campo>
  );
});

CampoNumeroFormik.propTypes = {
  onChange: PropTypes.func,
  onBlur: PropTypes.func,
  semMensagem: PropTypes.bool,
  type: PropTypes.string,
};

CampoNumeroFormik.defaultProps = {
  onChange: () => {},
  onBlur: () => {},
  semMensagem: false,
  type: 'number',
};

export default CampoNumeroFormik;
