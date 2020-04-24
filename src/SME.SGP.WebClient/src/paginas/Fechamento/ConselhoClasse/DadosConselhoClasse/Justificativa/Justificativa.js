import React, { useState } from 'react';
import Editor from '~/componentes/editor/editor';
import { Formik } from 'formik';
import * as Yup from 'yup';
import { Historico } from './Justificativa.css';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';

const Justificativa = () => {
  const validacoes = {
    justificativa: Yup.string().required('Campo justificativa é obrigatório'),
  };
  const onChangeJustificativa = () => {};
  const salvarJustificativa = valor => {};

  const clicouBotaoSalvar = (form, e) => {
    e.persist();
    form.validateForm().then(() => form.handleSubmit(e));
  };
  return (
    <>
      <div className="row">
        <div className="col-md-12">
          <span>Justificativa de nota pós-conselho</span>
        </div>
        <div className="col-sm-12 col-md-8">
          <Formik
            enableReinitialize
            onSubmit={valor => salvarJustificativa(valor)}
            validationSchema={validacoes}
            validateOnBlur={false}
            validateOnChange={false}
          >
            {form => (
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
            )}
          </Formik>
        </div>
        <Historico className="col-sm-12 col-md-4">
          <span className="label">Histórico:</span>
          <br />
          <span>
            Notas (ou conceitos) da avaliação XYZ inseridos por por Nome
            Usuário(9999999) em 10/01/2019, às 15:00. Notas (ou conceitos) da
            avaliação ABC alterados por Nome Usuário(9999999) em 11/01/2019, às
            16:00.{' '}
          </span>
        </Historico>
      </div>
    </>
  );
};

export default Justificativa;
