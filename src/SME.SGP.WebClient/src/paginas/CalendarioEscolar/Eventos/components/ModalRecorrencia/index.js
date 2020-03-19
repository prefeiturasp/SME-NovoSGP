import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';

// Form
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Styles
import { ContainerModal, VerticalCentralizado, IconMargin } from './styles';

// Components
import { CampoData, momentSchema, ModalConteudoHtml } from '~/componentes';
import LinhaBootstrap from './components/LinhaBootstrap';
import DiasDaSemana from './components/DiasDaSemana';
import RecorrenciaMensal from './components/RecorrenciaMensal';
import DropDownQuantidade from './components/DropDownQuantidade';
import DropDownTipoRecorrencia from './components/DropDownTipoRecorrencia';
import TextoDiasDaSemana from './components/TextoDiasDaSemana';
import TextoInformativo from './components/TextoInformativo';

function ModalRecorrencia({
  show,
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

  const valoresDefault = {
    dataInicio: null,
    dataTermino: null,
    diaNumero: null,
    diasSemana: [],
    tipoRecorrencia: '',
    padraoRecorrencia: '',
    quantidadeRecorrencia: null,
  };

  const [valoresIniciais, setValoresIniciais] = useState(valoresDefault);

  /**
   * @description Verifica se o botao de salvar deve ser habilitado
   */
  const formIsValid = useCallback(() => {
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
  }, [
    dataInicio,
    diaNumero,
    diaSemana,
    diasSemana.length,
    padraoRecorrencia,
    tipoRecorrencia.value,
  ]);

  useEffect(() => {
    if (initialValues) {
      setDataInicio(initialValues.dataInicio);
      setValoresIniciais(initialValues);
    }
  }, [initialValues]);

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
    formIsValid,
  ]);

  const onChangeWeekDay = day => {
    const exists = diasSemana.some(x => x.valor === day.valor);
    if (exists) {
      setDiasSemana([...diasSemana.filter(x => x.valor !== day.valor)]);
    } else {
      setDiasSemana([...diasSemana, day]);
    }
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

  const constroiValidacoes = () => {
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

  const onCloseModal = () => {
    setPadraoRecorrencia(undefined);
    setDataInicio('');
    setDataTermino('');
    setDiasSemana([]);
    setValoresIniciais(valoresDefault);
    onSaveRecorrencia(undefined);
    onCloseRecorrencia();
  };

  const desabilitarData = current => {
    if (current) {
      return (
        current < window.moment().startOf('year') ||
        current > window.moment().endOf('year')
      );
    }
    return false;
  };

  return (
    <ContainerModal>
      <ModalConteudoHtml
        titulo="Repetir"
        visivel={show}
        closable
        onClose={() => onCloseModal()}
        onConfirmacaoSecundaria={() => onCloseModal()}
        onConfirmacaoPrincipal={() => onSubmitRecorrencia()}
        labelBotaoPrincipal="Salvar"
        labelBotaoSecundario="Descartar"
        desabilitarBotaoPrincipal={!habilitaSalvar}
      >
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={constroiValidacoes}
          validateOnChange
          validateOnBlur
        >
          {form => (
            <Form>
              <LinhaBootstrap paddingBottom={4}>
                <div className="col-lg-6">
                  <CampoData
                    form={form}
                    name="dataInicio"
                    label="Data início"
                    valor={dataInicio}
                    onChange={onChangeDataInicio}
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                    desabilitarData={desabilitarData}
                  />
                </div>
                <div className="col-lg-6">
                  <CampoData
                    label="Data fim"
                    valor={dataTermino}
                    onChange={onChangeDataTermino}
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                    desabilitarData={desabilitarData}
                  />
                </div>
              </LinhaBootstrap>
              <LinhaBootstrap paddingBottom={3}>
                <VerticalCentralizado className="col-lg-12">
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
                </VerticalCentralizado>
              </LinhaBootstrap>
              <LinhaBootstrap paddingBottom={3}>
                <div className="col-lg-12">
                  {tipoRecorrencia && tipoRecorrencia.value === '1' ? (
                    <DiasDaSemana
                      currentState={diasSemana || null}
                      onChange={e => onChangeWeekDay(e)}
                    />
                  ) : (
                    <RecorrenciaMensal
                      currentRecurrence={padraoRecorrencia}
                      currentDayNumber={diaNumero}
                      currentWeekDay={diaSemana}
                      onChangeRecurrence={onChangeRecurrence}
                      onChangeWeekDay={onChangeWeekDayMonth}
                      onChangeDayNumber={onChangeDayNumber}
                      form={form}
                    />
                  )}
                </div>
              </LinhaBootstrap>
              <TextoDiasDaSemana diasSemana={diasSemana} />
              <TextoInformativo dataTermino={dataTermino} />
            </Form>
          )}
        </Formik>
      </ModalConteudoHtml>
    </ContainerModal>
  );
}

ModalRecorrencia.defaultProps = {
  show: false,
  initialValues: {},
  onCloseRecorrencia: () => {},
  onSaveRecorrencia: () => {},
};

ModalRecorrencia.propTypes = {
  show: PropTypes.bool,
  initialValues: PropTypes.oneOfType([PropTypes.any, PropTypes.object]),
  onCloseRecorrencia: PropTypes.func,
  onSaveRecorrencia: PropTypes.func,
};

export default ModalRecorrencia;
