import React, { useState, useEffect } from 'react';
import t from 'prop-types';

// Form
import { Field } from 'formik';

// Componentes
import { Input, Button } from 'antd';

// Styles
import { InputRFEstilo } from './styles';

// Funções
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';
import Loader from '~/componentes/loader';

function InputRF({
  pessoaSelecionada,
  onSelect,
  onChange,
  form,
  name,
  id,
  className,
  desabilitado,
  maxlength,
  onKeyDown,
  style,
  placeholderRF,
  exibirLoader,
}) {
  const [valor, setValor] = useState('');

  const onSubmitRF = rf => {
    onSelect({ rf });
  };

  const botao = (
    <Button
      onClick={() => onSubmitRF(valor)}
      disabled={!valor || desabilitado}
      type="link"
    >
      <i className="fa fa-search fa-lg" />
    </Button>
  );

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && relatedTarget.getAttribute('type') === 'button') {
      event.preventDefault();
    }
  };

  const onChangeRf = e => {
    if (form && valorNuloOuVazio(e.target.value)) {
      form.setFieldValue(name, e.target.value, false);
      form.setFieldTouched(name);
    }
    setValor(e.target.value);
    onChange(e.target.value);
  };

  useEffect(() => {
    setValor(pessoaSelecionada && pessoaSelecionada.professorRf);
    if (form) {
      form.setFieldValue(
        name,
        pessoaSelecionada && pessoaSelecionada.professorRf
      );
    }
  }, [pessoaSelecionada]);

  useEffect(() => {
    if (form && form.initialValues) {
      setValor(form.initialValues.professorRf);
    }
  }, [form?.initialValues]);

  useEffect(() => {
    if (form) {
      const { values: valores } = form;
      if (valores && valorNuloOuVazio(valores.professorRf)) {
        setValor('');
      }
    }
  }, [form?.values]);

  return (
    <Loader loading={exibirLoader}>
      {form ? (
        <InputRFEstilo>
          <Field
            name={name}
            id={id || name}
            className={`campo ${possuiErro() ? 'is-invalid' : ''} ${className ||
              ''} ${desabilitado ? 'desabilitado' : ''}`}
            component={Input}
            readOnly={desabilitado}
            onBlur={executaOnBlur}
            maxLength={maxlength || 7}
            value={valor}
            placeholder={placeholderRF}
            onKeyDown={onKeyDown}
            onChange={onChangeRf}
            style={style}
            suffix={botao}
            onPressEnter={e => onSubmitRF(e.target.value)}
            disabled={desabilitado}
            allowClear
          />
          {form && form.touched[name] && (
            <span className="mensagemErro">{form.errors[name]}</span>
          )}
        </InputRFEstilo>
      ) : (
        <InputRFEstilo>
          <Input
            value={valor}
            placeholder={placeholderRF}
            onChange={onChangeRf}
            onPressEnter={e => onSubmitRF(e.target.value)}
            suffix={botao}
            disabled={desabilitado}
            allowClear
            maxLength={maxlength || 7}
          />
        </InputRFEstilo>
      )}
    </Loader>
  );
}

InputRF.propTypes = {
  pessoaSelecionada: t.oneOfType([t.objectOf(t.object), t.any]),
  onSelect: t.func,
  form: t.oneOfType([t.objectOf(t.object), t.any]),
  name: t.string,
  id: t.string,
  className: t.string,
  desabilitado: t.bool,
  maxlength: t.number,
  onKeyDown: t.func,
  style: t.objectOf(t.object),
  placeholderRF: t.string.isRequired,
  exibirLoader: t.bool,
};

InputRF.defaultProps = {
  pessoaSelecionada: {},
  onSelect: () => null,
  form: null,
  name: '',
  id: '',
  className: '',
  desabilitado: false,
  maxlength: null,
  onKeyDown: null,
  style: {},
  exibirLoader: false,
};

export default InputRF;
