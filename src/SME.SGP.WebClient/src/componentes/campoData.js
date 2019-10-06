import 'moment/locale/pt-br';

import { DatePicker } from 'antd';
import locale from 'antd/es/date-picker/locale/pt_BR';
import { Field } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import * as Yup from 'yup';

import { Base } from './colors';
import Label from './label';

class MomentSchema extends Yup.mixed {
  constructor() {
    super({ type: 'momentschema' });
    this.transforms.push(function(value) {
      if (this.isType(value)) return moment(value);
      return moment.invalid();
    });
  }
}

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }

  span[class*='is-invalid'] {
    .ant-calendar-picker-input {
      border-color: #dc3545 !important;
    }
  }

  .ant-calendar-picker-input {
    height: 38px;
  }

  .ant-calendar-picker {
    width: 100%;
  }
`;

const CampoData = props => {
  const {
    formatoData,
    placeholder,
    label,
    name,
    id,
    form,
    desabilitado,
    className,
    onChange,
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

  const campoDataAnt = () => {
    return (
      <Field
        disabled={desabilitado}
        locale={locale}
        format={formatoData}
        component={DatePicker}
        placeholder={placeholder}
        name={name}
        suffixIcon={<i className="fas fa-calendar-alt" />}
        name={name}
        id={id || name}
        onBlur={executaOnBlur}
        className={
          form ? `${possuiErro() ? 'is-invalid' : ''} ${className || ''}` : ''
        }
        onChange={valorData => {
          form.setFieldValue(name, valorData);
          onChange(valorData); // TODO
        }}
      />
    );
  };

  return (
    <>
      <Campo>
        <Label text={label} control={name} />
        {campoDataAnt()}
        {form ? <span>{form.errors[name]}</span> : ''}
      </Campo>
    </>
  );
};

CampoData.propTypes = {
  className: PropTypes.string,
  formatoData: PropTypes.string,
  placeholder: PropTypes.string,
  label: PropTypes.string,
  desabilitado: PropTypes.bool,
  onChange: PropTypes.func,
};

CampoData.defaultProps = {
  className: '',
  formatoData: 'DD/MM/YYYY HH:mm:ss',
  placeholder: 'placeholder',
  label: 'Label',
  desabilitado: false,
  onChange: () => {},
};

const momentSchema =new MomentSchema();

export { CampoData,  momentSchema } ;
