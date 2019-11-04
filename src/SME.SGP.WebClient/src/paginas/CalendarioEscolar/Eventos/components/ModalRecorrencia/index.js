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

function ModalRecorrencia({ show, onCloseRepetir, dataInicioEvento }) {
  const [dataInicio, setDataInicio] = useState('');
  const [dataTermino, setDataTermino] = useState('');

  // Usado quando for selecionado a recorrencia semanal
  const [diasSemana, setDiasSemana] = useState([]);

  // Usado quando for selecionado a recorrencia mensal
  const [diaSemana, setDiaSemana] = useState('0');

  const [diaNumero, setDiaNumero] = useState(1);
  const [padraoRecorrencia, setPadraoRecorrencia] = useState('0');
  const [quantidadeRecorrencia, setQuantidadeRecorrencia] = useState(1);
  const [tipoRecorrencia, setTipoRecorrencia] = useState({
    label: 'Mês(ses)',
    value: 'M',
  });

  useEffect(() => {
    if (dataInicioEvento) {
      setDataInicio(dataInicioEvento);
    }
  });

  const onChangeWeekDay = day => {
    const exists = diasSemana.some(x => x.value === day.value);
    if (exists) {
      setDiasSemana([...diasSemana.filter(x => x.value !== day.value)]);
    } else {
      setDiasSemana([...diasSemana, day]);
    }
  };

  /**
   * @description Render helper text above WeekDays component
   */
  const renderHelperText = () => {
    let text = `Ocorre a cada `;
    if (diasSemana.length === 1) {
      text += diasSemana[0].value;
    } else if (diasSemana.length === 2) {
      text += `${diasSemana[0].value} e ${diasSemana[1].value}`;
    } else if (diasSemana.length > 2) {
      text += `${diasSemana
        .map((item, index) =>
          index !== diasSemana.length - 1 ? item.value : ''
        )
        .toString()} e ${diasSemana[diasSemana.length - 1].value}.`;
    }
    return text;
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
    setPadraoRecorrencia('0');
    setDiaSemana('0');
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

  return (
    <ContainerModal>
      <ModalConteudoHtml
        visivel={show}
        closable={!!true}
        onClose={() => onCloseModal()}
        onConfirmacaoSecundaria={() => onCloseModal()}
        titulo="Repetir"
        labelBotaoPrincipal="Salvar"
        labelBotaoSecundario="Descartar"
      >
        <Formik
          enableReinitialize
          initialValues={null}
          validationSchema={buildValidations}
          onSubmit={valores => null}
          validateOnChange
          validateOnBlur
          validate={val => console.log(val)}
        >
          {form => (
            <Form>
              <BootstrapRow paddingBottom={4}>
                <VerticalCentered className="col-lg-6">
                  <CampoData
                    form={form}
                    name="dataInicio"
                    label="Data início"
                    valor={dataInicio}
                    onChange={onChangeDataInicio}
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                  />
                </VerticalCentered>
                <VerticalCentered className="col-lg-6">
                  <CampoData
                    label="Data fim"
                    valor={dataTermino}
                    onChange={onChangeDataTermino}
                    placeholder="DD/MM/AAAA"
                    formatoData="DD/MM/YYYY"
                  />
                </VerticalCentered>
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
  dataInicioEvento: new Date(),
};

ModalRecorrencia.propTypes = {
  show: PropTypes.bool,
  onCloseRepetir: PropTypes.func,
  dataInicioEvento: PropTypes.objectOf(PropTypes.object),
};

export default ModalRecorrencia;
