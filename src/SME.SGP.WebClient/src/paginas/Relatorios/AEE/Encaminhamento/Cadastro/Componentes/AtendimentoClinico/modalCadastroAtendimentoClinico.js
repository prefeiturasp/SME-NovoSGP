import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import shortid from 'shortid';
import * as Yup from 'yup';
import {
  CampoData,
  CampoTexto,
  Colors,
  ModalConteudoHtml,
  momentSchema,
  SelectComponent,
} from '~/componentes';
import Button from '~/componentes/button';

const ModalCadastroAtendimentoClinico = props => {
  const { onClose, exibirModal } = props;

  const [refForm, setRefForm] = useState({});

  const valoresIniciais = {
    diaSemana: '',
    atendimentoAtividade: '',
    localRealizacao: '',
    horarioInicio: '',
    horarioTermino: '',
  };

  const validacoes = Yup.object().shape({
    diaSemana: Yup.string()
      .nullable()
      .required('Campo obrigatório'),
    atendimentoAtividade: Yup.string()
      .nullable()
      .required('Campo obrigatório'),
    localRealizacao: Yup.string()
      .nullable()
      .required('Campo obrigatório'),
    horarioInicio: momentSchema.required('Campo obrigatório'),
    horarioTermino: momentSchema.required('Campo obrigatório'),
  });

  const listaDiasSemana = [
    {
      valor: 'Domingo',
      desc: 'Domingo',
    },
    {
      valor: 'Segunda',
      desc: 'Segunda',
    },
    {
      valor: 'Terça',
      desc: 'Terça',
    },
    {
      valor: 'Quarta',
      desc: 'Quarta',
    },
    {
      valor: 'Quinta',
      desc: 'Quinta',
    },
    {
      valor: 'Sexta',
      desc: 'Sexta',
    },
    {
      valor: 'Sábado',
      desc: 'Sábado',
    },
  ];

  const fecharModal = () => {
    refForm.resetForm();
    onClose();
  };

  const onSalvar = valores => {
    refForm.resetForm();
    onClose(valores);
  };

  const validaAntesDoSubmit = form => {
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
    <ModalConteudoHtml
      id={shortid.generate()}
      key="detalhamento-atendimento-clinico"
      visivel={exibirModal}
      titulo="Detalhamento de Atendimento Clínico"
      onClose={fecharModal}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <Formik
        ref={f => setRefForm(f)}
        enableReinitialize
        initialValues={valoresIniciais}
        validationSchema={validacoes}
        onSubmit={valores => {
          onSalvar(valores);
        }}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <div className="col-md-12 mb-2">
              <SelectComponent
                label="Dias da Semana"
                lista={listaDiasSemana}
                valueOption="valor"
                valueText="desc"
                form={form}
                name="diaSemana"
              />
            </div>
            <div className="col-md-12 mb-2">
              <CampoTexto
                form={form}
                name="atendimentoAtividade"
                label="Atendimento/Atividade"
                maxLength={100}
              />
            </div>
            <div className="col-md-12 mb-2">
              <CampoTexto
                form={form}
                name="localRealizacao"
                label="Local de realização"
                maxLength={100}
              />
            </div>
            <div className="col-md-12 mb-2">
              <CampoData
                form={form}
                name="horarioInicio"
                label="Horário de início"
                placeholder="09:00"
                formatoData="HH:mm"
                somenteHora
              />
            </div>
            <div className="col-md-12 mb-2">
              <CampoData
                form={form}
                name="horarioTermino"
                label="Horário término"
                placeholder="09:30"
                formatoData="HH:mm"
                somenteHora
              />
            </div>

            <div className="col-md-12 mt-2 d-flex justify-content-end">
              <Button
                key="btn-voltar"
                id="btn-voltar"
                label="Cancelar"
                color={Colors.Azul}
                border
                onClick={fecharModal}
                className="mt-2 mr-2"
              />
              <Button
                key="btn-salvar"
                id="btn-salvar"
                label="Adicionar Registro"
                color={Colors.Roxo}
                border
                onClick={() => validaAntesDoSubmit(form)}
                className="mt-2"
              />
            </div>
          </Form>
        )}
      </Formik>
    </ModalConteudoHtml>
  );
};

ModalCadastroAtendimentoClinico.propTypes = {
  onClose: PropTypes.func,
  exibirModal: PropTypes.bool,
};

ModalCadastroAtendimentoClinico.defaultProps = {
  onClose: () => {},
  exibirModal: false,
};

export default ModalCadastroAtendimentoClinico;
