import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';

// Form
import { Form, Formik } from 'formik';
import * as Yup from 'yup';

// Ant

// Styles
import {
  ContainerModal,
  VerticalCentered,
  IconMargin,
  SmallText,
} from './styles';

// Components
import { CampoData, momentSchema } from '~/componentes/campoData/campoData';
import ModalConteudoHtml from '~/componentes/modalConteudoHtml';
import BootstrapRow from './components/BootstrapRow';
import WeekDays from './components/WeekDays';
import MonthlyRecurrence from './components/MonthlyRecurrence';
import DropDownQuantidade from './components/DropDownQuantidade';
import DropDownTipoRecorrencia from './components/DropDownTipoRecorrencia';

function ModalRecorrencia({
  show,
  onCloseRepetir,
  onSaveRecorrencia,
  dataInicioEvento,
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
    value: 'M',
  });

  useEffect(() => {
    if (dataInicioEvento) {
      setDataInicio(dataInicioEvento);
    }
  });

  /**
   * @description Verifica se o botao de salvar deve ser habilitado
   */
  const formIsValid = () => {
    const isMonthSelected = tipoRecorrencia.value === 'M';
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

  /**
   * @description Render helper text above WeekDays component
   */
  const renderHelperText = () => {
    let text = `Ocorre a cada `;
    if (diasSemana.length === 1) {
      text += diasSemana[0].description;
    } else if (diasSemana.length === 2) {
      text += `${diasSemana[0].description} e ${diasSemana[1].description}`;
    } else if (diasSemana.length > 2) {
      text += `${diasSemana
        .map((item, index) =>
          index !== diasSemana.length - 1 ? item.description : ''
        )
        .toString()} e ${diasSemana[diasSemana.length - 1].description}.`;
    }
    return text;
  };

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
    onCloseRepetir();
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

    if (tipoRecorrencia.value === 'M') {
      val.padraoRecorrencia = Yup.string().required(
        'Padrão recorrência obrigatório'
      );
    }

    if (padraoRecorrencia.value === '0') {
      val.dayNumber = Yup.string().required('Dia obrigatório');
    }

    return Yup.object(val);
  };

  const onSubmitRecorrencia = () => {
    const recurrence = {
      dataInicio,
      dataTermino,
      diasSemana,
      diaSemana,
      diaNumero,
      padraoRecorrencia,
      quantidadeRecorrencia,
      tipoRecorrencia,
    };

    onSaveRecorrencia(recurrence);
  };

  return (
    <ContainerModal>
      <ModalConteudoHtml
        visivel={show}
        closable={!!true}
        onClose={() => onCloseModal()}
        onConfirmacaoSecundaria={() => onCloseModal()}
        onConfirmacaoPrincipal={() => onSubmitRecorrencia()}
        titulo="Repetir"
        labelBotaoPrincipal="Salvar"
        labelBotaoSecundario="Descartar"
        desabilitarBotaoPrincipal={!habilitaSalvar}
      >
        <Formik
          enableReinitialize
          initialValues={null}
          validationSchema={buildValidations}
          validateOnChange
          validateOnBlur
          validate={val => console.log(val)}
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
                  {tipoRecorrencia && tipoRecorrencia.value === 'S' ? (
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
              {diasSemana.length > 0 && (
                <BootstrapRow>
                  <VerticalCentered className="col-lg-12">
                    {renderHelperText()}
                  </VerticalCentered>
                </BootstrapRow>
              )}
              <BootstrapRow>
                <VerticalCentered className="col-lg-12">
                  <SmallText>
                    Se não selecionar uma data fim, será considerado o fim do
                    ano letivo.
                  </SmallText>
                </VerticalCentered>
              </BootstrapRow>
            </Form>
          )}
        </Formik>
      </ModalConteudoHtml>
    </ContainerModal>
  );
}

ModalRecorrencia.defaultProps = {
  show: false,
  onCloseRepetir: () => {},
  onSaveRecorrencia: () => {},
  dataInicioEvento: new Date(),
};

ModalRecorrencia.propTypes = {
  show: PropTypes.bool,
  onCloseRepetir: PropTypes.func,
  onSaveRecorrencia: PropTypes.func,
  dataInicioEvento: PropTypes.oneOfType([PropTypes.any]),
};

export default ModalRecorrencia;
