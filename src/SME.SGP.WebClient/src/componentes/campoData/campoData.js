import 'moment/locale/pt-br';

import { DatePicker, TimePicker } from 'antd';
import locale from 'antd/es/date-picker/locale/pt_BR';
import { Field } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';
import * as Yup from 'yup';

import { Base } from '../colors';
import Label from '../label';

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

    .ant-time-picker-input {
      border-color: #dc3545 !important;
    }
  }

  .ant-calendar-picker-input {
    height: 38px;
  }

  .ant-time-picker-input {
    height: 38px;
  }

  .ant-time-picker {
    width: 100%;
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
    valor,
    desabilitarData,
    somenteHora,
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

  const campoDataAntComValidacoes = () => {
    return (
      <Field
        disabled={desabilitado}
        locale={locale}
        format={formatoData}
        component={DatePicker}
        placeholder={placeholder}
        suffixIcon={<i className="fas fa-calendar-alt" />}
        name={name}
        id={id || name}
        onBlur={executaOnBlur}
        className={
          form ? `${possuiErro() ? 'is-invalid' : ''} ${className || ''}` : ''
        }
        onChange={valorData => {
          valorData = valorData || '';
          form.setFieldValue(name, valorData);
          onChange(valorData);
        }}
        value={form.values[name] || null}
        disabledDate={desabilitarData}
      />
    );
  };

  const campoDataAntSemValidacoes = () => {
    return (
      <DatePicker
        disabled={desabilitado}
        locale={locale}
        format={formatoData}
        placeholder={placeholder}
        suffixIcon={<i className="fas fa-calendar-alt" />}
        name={name}
        id={id || name}
        onBlur={executaOnBlur}
        className={className || ''}
        onChange={valorData => {
          valorData = valorData || '';
          onChange(valorData);
        }}
        value={valor || null}
      />
    );
  };

  const campoHoraAntComValidacoes = () => {
    return (
      <Field
        disabled={desabilitado}
        locale={locale}
        format={formatoData}
        component={TimePicker}
        placeholder={placeholder}
        name={name}
        id={id || name}
        onBlur={executaOnBlur}
        className={
          form ? `${possuiErro() ? 'is-invalid' : ''} ${className || ''}` : ''
        }
        onChange={valorHora => {
          valorHora = valorHora || '';
          form.setFieldValue(name, valorHora);
          onChange(valorHora);
        }}
        value={form.values[name] || null}
      />
    );
  };

  const validaTipoCampo = () => {
    if (somenteHora) {
      return form ? campoHoraAntComValidacoes() : 'CRIAR COMPONENTE!!';
    }
    return form ? campoDataAntComValidacoes() : campoDataAntSemValidacoes();
  };

  return (
    <>
      <Campo>
        {label ? <Label text={label} control={name} /> : ''}
        {validaTipoCampo()}
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
  somenteHora: PropTypes.bool,
  onChange: PropTypes.func,
  valor: PropTypes.any,
};

CampoData.defaultProps = {
  className: '',
  formatoData: 'DD/MM/YYYY HH:mm:ss',
  placeholder: 'placeholder',
  label: '',
  desabilitado: false,
  somenteHora: false,
  onChange: () => {},
};

const momentSchema = new MomentSchema();

Yup.addMethod(
  Yup.mixed,
  'dataMenorIgualQue',
  // eslint-disable-next-line func-names
  function(nomeDataInicial, nomeDataFinal, mensagem) {
    // eslint-disable-next-line func-names
    return this.test('dataMenorIgualQue', mensagem, function() {
      let dataValida = true;
      const dataInicial = this.parent[nomeDataInicial];
      const dataFinal = this.parent[nomeDataFinal];

      if (
        dataInicial &&
        dataFinal &&
        dataInicial.isSameOrAfter(dataFinal, 'date')
      ) {
        dataValida = false;
      }
      return dataValida;
    });
  }
);
export { CampoData, momentSchema };
