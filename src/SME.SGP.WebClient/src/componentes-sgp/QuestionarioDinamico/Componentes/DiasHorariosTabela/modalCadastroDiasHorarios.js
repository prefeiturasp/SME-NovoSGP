import { Form, Formik } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import shortid from 'shortid';
import * as Yup from 'yup';
import {
  CampoData,
  Colors,
  Label,
  ModalConteudoHtml,
  momentSchema,
  SelectComponent,
} from '~/componentes';
import Button from '~/componentes/button';
import { confirmar } from '~/servicos';
import QuestionarioDinamicoFuncoes from '../../Funcoes/QuestionarioDinamicoFuncoes';

const ModalCadastroDiasHorario = props => {
  const { onClose, exibirModal, dadosIniciais } = props;

  const [refForm, setRefForm] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({});

  const [emEdicao, setEmEdicao] = useState(false);

  useEffect(() => {
    if (dadosIniciais?.id) {
      setValoresIniciais({
        id: dadosIniciais ? dadosIniciais.id : 0,
        diaSemana: dadosIniciais ? dadosIniciais.diaSemana : '',
        horarioInicio: dadosIniciais ? moment(dadosIniciais.horarioInicio) : '',
        horarioTermino: dadosIniciais
          ? moment(dadosIniciais.horarioTermino)
          : '',
      });
    }
  }, [dadosIniciais]);

  const validacoes = Yup.object().shape({
    diaSemana: Yup.string()
      .nullable()
      .required('Campo obrigatório'),
    horarioInicio: momentSchema.required('Campo obrigatório'),
    horarioTermino: momentSchema.required('Campo obrigatório'),
  });

  const listaDiasSemana = QuestionarioDinamicoFuncoes.obterListaDiasSemana();

  const limparDadosModal = () => {
    setValoresIniciais({});
    setEmEdicao(false);
    refForm.resetForm();
  };

  const fecharModal = async () => {
    if (emEdicao) {
      const confirmou = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmou) {
        limparDadosModal();
        onClose();
      }
    } else {
      limparDadosModal();
      onClose();
    }
  };

  const onSalvar = valores => {
    limparDadosModal();
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
      key="detalhamento-cadastro-dias-horarios"
      visivel={exibirModal}
      titulo="Dias e horários de frequência"
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
                onChange={() => setEmEdicao(true)}
              />
            </div>
            <div className="col-md-12 mb-2">
              <div className="row">
                <div className="col-md-12 mt-2">
                  <Label text="Horário" />
                </div>
                <div className="col-md-6 mb-2">
                  <CampoData
                    form={form}
                    name="horarioInicio"
                    placeholder="09:00"
                    formatoData="HH:mm"
                    somenteHora
                    onChange={() => setEmEdicao(true)}
                    label=""
                  />
                </div>
                <div className="col-md-6 mb-2">
                  <CampoData
                    form={form}
                    name="horarioTermino"
                    placeholder="09:30"
                    formatoData="HH:mm"
                    somenteHora
                    onChange={() => setEmEdicao(true)}
                    label=""
                  />
                </div>
              </div>
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
                label={dadosIniciais?.id ? 'Alterar' : 'Adicionar'}
                color={Colors.Roxo}
                border
                disabled={!emEdicao}
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

ModalCadastroDiasHorario.propTypes = {
  onClose: PropTypes.func,
  exibirModal: PropTypes.bool,
  dadosIniciais: PropTypes.oneOfType([PropTypes.any]),
};

ModalCadastroDiasHorario.defaultProps = {
  onClose: () => {},
  exibirModal: false,
  dadosIniciais: {},
};

export default ModalCadastroDiasHorario;
