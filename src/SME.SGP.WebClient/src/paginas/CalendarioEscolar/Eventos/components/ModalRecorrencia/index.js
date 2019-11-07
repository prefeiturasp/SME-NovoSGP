import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Styles
import { ContainerModal, VerticalCentered, IconMargin } from './styles';

// Components
import {
  CampoData,
  momentSchema,
  ModalConteudoHtml,
  Loader,
} from '~/componentes';
import BootstrapRow from './components/BootstrapRow';
import WeekDays from './components/WeekDays';
import MonthlyRecurrence from './components/MonthlyRecurrence';
import DropDownQuantidade from './components/DropDownQuantidade';
import DropDownTipoRecorrencia from './components/DropDownTipoRecorrencia';
import HelperText from './components/HelperText';
import WarningText from './components/WarningText';

function ModalRecorrencia({
  show,
  loading,
  initialValues,
  onCloseRecorrencia,
  onSaveRecorrencia,
}) {
  const [habilitaSalvar, setHabilitaSalvar] = useState(false);

  const [dataInicio, setDataInicio] = useState('');
  const [dataTermino, setDataTermino] = useState('');

  // Usado quando for selecionado a recorrencia semanal
  const [diasSemana, setDiasSemana] = useState([]);
  // Usado quando for selecionado a recorrencia mensal
  const [diaSemana, setDiaSemana] = useState();

  const [diaNumero, setDiaNumero] = useState(1);
  const [padraoRecorrencia, setPadraoRecorrencia] = useState();
  const [quantidadeRecorrencia, setQuantidadeRecorrencia] = useState(1);
  const [tipoRecorrencia, setTipoRecorrencia] = useState({
    label: 'Mês(es)',
    value: '2',
  });

  const [defaultValues, setDefaultValues] = useState({
    dataInicio: null,
    dataTermino: null,
    diaNumero: null,
    diasSemana: [],
    tipoRecorrencia: '',
    padraoRecorrencia: '',
    quantidadeRecorrencia: null,
  });

  /**
   * @description Verifica se o botao de salvar deve ser habilitado
   */
  const formIsValid = () => {
    const isMonthSelected = tipoRecorrencia.value === '2';
    const noDiaIsValid = padraoRecorrencia === '0' && diaNumero > 0;
    const notNoDiaIsValid =
      padraoRecorrencia && padraoRecorrencia !== '0' && diaSemana;

    if (dataInicio && isMonthSelected && (noDiaIsValid || notNoDiaIsValid)) {
      return true;
    }

    if (dataInicio && !isMonthSelected && diasSemana.length > 0) {
      return true;
    }

    return false;
  };

  useEffect(() => {
    if (initialValues) {
      setDataInicio(initialValues.dataInicio);
      setDefaultValues(initialValues);
    }
  });

  useEffect(() => {
    setHabilitaSalvar(formIsValid());
  }, [
    dataInicio,
    dataTermino,
    diasSemana,
    diaSemana,
    diaNumero,
    padraoRecorrencia,
    quantidadeRecorrencia,
    tipoRecorrencia,
  ]);

  const onChangeWeekDay = day => {
    const exists = diasSemana.some(x => x.value === day.value);
    if (exists) {
      setDiasSemana([...diasSemana.filter(x => x.value !== day.value)]);
    } else {
      setDiasSemana([...diasSemana, day]);
    }
  };

  const onCloseModal = () => {
    setDataInicio('');
    setDataTermino('');
    setDiasSemana([]);
    onCloseRecorrencia();
  };

  const onChangeDataInicio = e => setDataInicio(e);
  const onChangeDataTermino = e => setDataTermino(e);
  const onChangeRecurrence = value => setPadraoRecorrencia(value);
  const onChangeWeekDayMonth = value => setDiaSemana(value);
  const onChangeDayNumber = value => setDiaNumero(value);

  const onChangeTipoRecorrencia = value => {
    setTipoRecorrencia(value);
    setDiasSemana([]);
    setPadraoRecorrencia(null);
    setDiaSemana(null);
  };

  const buildValidations = () => {
    const val = {
      dataInicio: momentSchema.required('Data obrigatória'),
    };

    if (tipoRecorrencia.value === '2') {
      val.padraoRecorrencia = Yup.string().required(
        'Padrão recorrência obrigatório'
      );
    }

    if (padraoRecorrencia.value === '0') {
      val.diaSemana = Yup.string().required('Dia obrigatório');
    }

    return Yup.object(val);
  };

  const onSubmitRecorrencia = () => {
    onSaveRecorrencia({
      dataInicio,
      dataTermino,
      diasSemana,
      diaSemana,
      diaNumero,
      padraoRecorrencia,
      quantidadeRecorrencia,
      tipoRecorrencia,
    });
  };

  return (
    <ContainerModal>
      <ModalConteudoHtml
        titulo="Repetir"
        visivel={show}
        closable={!!true}
        onClose={() => onCloseModal()}
        onConfirmacaoSecundaria={() => onCloseModal()}
        onConfirmacaoPrincipal={() => onSubmitRecorrencia()}
        labelBotaoPrincipal="Salvar"
        labelBotaoSecundario="Descartar"
        desabilitarBotaoPrincipal={!habilitaSalvar || loading}
        esconderBotaoSecundario={loading}
      >
        <Loader loading={loading}>
          <Formik
            enableReinitialize
            initialValues={defaultValues}
            validationSchema={buildValidations}
            validateOnChange
            validateOnBlur
          >
            {form => (
              <Form>
                <BootstrapRow paddingBottom={4}>
                  <div className="col-lg-6">
                    <CampoData
                      form={form}
                      name="dataInicio"
                      label="Data início"
                      valor={dataInicio}
                      onChange={onChangeDataInicio}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                    />
                  </div>
                  <div className="col-lg-6">
                    <CampoData
                      label="Data fim"
                      valor={dataTermino}
                      onChange={onChangeDataTermino}
                      placeholder="DD/MM/AAAA"
                      formatoData="DD/MM/YYYY"
                    />
                  </div>
                </BootstrapRow>
                <BootstrapRow paddingBottom={3}>
                  <VerticalCentered className="col-lg-12">
                    <div className="item">
                      <IconMargin className="fas fa-retweet" />
                      <span>Repetir a cada</span>
                    </div>
                    <div className="item">
                      <DropDownQuantidade
                        onChange={value => setQuantidadeRecorrencia(value)}
                        value={quantidadeRecorrencia}
                      />
                    </div>
                    <div className="item">
                      <DropDownTipoRecorrencia
                        onChange={value => onChangeTipoRecorrencia(value)}
                        value={tipoRecorrencia}
                      />
                    </div>
                  </VerticalCentered>
                </BootstrapRow>
                <BootstrapRow paddingBottom={3}>
                  <VerticalCentered className="col-lg-12">
                    {tipoRecorrencia && tipoRecorrencia.value === '1' ? (
                      <WeekDays
                        currentState={diasSemana || null}
                        onChange={e => onChangeWeekDay(e)}
                      />
                    ) : (
                      <MonthlyRecurrence
                        currentRecurrence={padraoRecorrencia}
                        currentDayNumber={diaNumero}
                        currentWeekDay={diaSemana}
                        onChangeRecurrence={onChangeRecurrence}
                        onChangeWeekDay={onChangeWeekDayMonth}
                        onChangeDayNumber={onChangeDayNumber}
                        form={form}
                      />
                    )}
                  </VerticalCentered>
                </BootstrapRow>
                <HelperText diasSemana={diasSemana} />
                <WarningText dataTermino={dataTermino} />
              </Form>
            )}
          </Formik>
        </Loader>
      </ModalConteudoHtml>
    </ContainerModal>
  );
}

ModalRecorrencia.defaultProps = {
  show: false,
  loading: false,
  initialValues: {},
  onCloseRecorrencia: () => {},
  onSaveRecorrencia: () => {},
};

ModalRecorrencia.propTypes = {
  show: PropTypes.bool,
  loading: PropTypes.bool,
  initialValues: PropTypes.oneOfType([PropTypes.any, PropTypes.object]),
  onCloseRecorrencia: PropTypes.func,
  onSaveRecorrencia: PropTypes.func,
};

export default ModalRecorrencia;
