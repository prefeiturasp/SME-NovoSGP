import React, { useState } from 'react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import Editor from '~/componentes/editor/editor';
import { Colors, Auditoria } from '~/componentes';
import Button from '~/componentes/button';

const Justificativa = () => {
  const validacoes = Yup.object({
    justificativa: Yup.string().required('Campo justificativa é obrigatório'),
  });

  const valoresIniciais = { justificativa: '' };
  const [auditoria, setAuditoria] = useState([]);
  const onChangeJustificativa = () => {};
  const salvarJustificativa = valor => {};

  const clicouBotaoSalvar = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.handleSubmit(e => e);
      }
    });
  };

  return (
    <>
      <div className="row">
        <div className="col-md-12">
          <span>Justificativa de nota pós-conselho</span>
        </div>
        <div className="col-md-12">
          <Formik
            enableReinitialize
            onSubmit={valor => salvarJustificativa(valor)}
            validationSchema={validacoes}
            initialValues={valoresIniciais}
            validateOnBlur={false}
            validateOnChange={false}
          >
            {form => (
              <Form>
                <fieldset className="mt-3">
                  <Editor
                    form={form}
                    name="justificativa"
                    id="justificativa"
                    onChange={onChangeJustificativa}
                  />
                  <div className="d-flex justify-content-end pt-2">
                    <Button
                      label="Salvar"
                      color={Colors.Roxo}
                      onClick={e => {
                        clicouBotaoSalvar(form, e);
                      }}
                      border
                    />
                  </div>
                </fieldset>
              </Form>
            )}
          </Formik>
        </div>
        <Auditoria
          criadoEm={auditoria.criadoEm}
          criadoPor={auditoria.criadoPor}
          criadoRf={auditoria.criadoRf}
          alteradoPor={auditoria.alteradoPor}
          alteradoEm={auditoria.alteradoEm}
          alteradoRf={auditoria.alteradoRf}
        />
      </div>
    </>
  );
};

export default Justificativa;
