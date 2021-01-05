import { Formik, Form } from 'formik';
import React, { useRef, useState } from 'react';
import shortid from 'shortid';
import * as Yup from 'yup';
import { Button, CampoData, Card, Colors, momentSchema } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';

const CadastroOcorrencias = () => {
  const [dataOcorrencia, setDataOcorrencia] = useState();
  const [horaOcorrencia, setHoraOcorrencia] = useState();
  const [tipoOcorrencia, setTipoOcorrencia] = useState();
  const [tituloOcorrencia, setTituloOcorrencia] = useState();
  const [descricao, setDescricao] = useState();
  const [refForm, setRefForm] = useState({});

  const [valoresIniciais, setValoresIniciais] = useState({
    dataOcorrencia: window.moment(),
  });

  const [validacoes, setValidacoes] = useState(undefined);

  const validaAntesDoSubmit = form => {
    const arrayCampos = Object.keys(valoresIniciais);
    arrayCampos.forEach(campo => {
      form.setFieldTouched(campo, true, true);
    });
    form.validateForm().then(() => {
      if (form.isValid || Object.keys(form.errors).length === 0) {
        form.submitForm(form);
      }
    });
  };

  const onSubmitFormulario = async valores => {};

  const onClickVoltar = () => {};
  const onClickCancelar = () => {};

  const onChangeDataOcorrencia = valor => {
    setDataOcorrencia(valor);
  };

  const onChangeHoraOcorrencia = valor => {
    setHoraOcorrencia(valor);
  };

  const criancas = [
    { nome: 'Ana', rf: '123456' },
    { nome: 'Julio', rf: '123456' },
    { nome: 'Pedro', rf: '123456' },
  ];

  return (
    <>
      <Cabecalho pagina="Cadastro de ocorrência" />
      <Card>
        <Formik
          enableReinitialize
          initialValues={valoresIniciais}
          validationSchema={validacoes}
          onSubmit={() => {}}
          validateOnBlur
          validateOnChange
          ref={refFormik => setRefForm(refFormik)}
        >
          {form => (
            <Form className="col-md-12 mb-4">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <Button
                  id={shortid.generate()}
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickVoltar}
                />
                <Button
                  id={shortid.generate()}
                  label="Cancelar"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickCancelar}
                />
                <Button
                  id={shortid.generate()}
                  label="Cadastrar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={validaAntesDoSubmit(form)}
                />
              </div>
              <div className="col-12 mb-3 font-weight-bold">
                <span>Crianças envolvidas na ocorrência</span>
              </div>
              <div className="col-12">
                {criancas.map((crianca, index) => {
                  return (
                    <div className="mb-3" key={`crianca-${index}`}>
                      <span>
                        {crianca.nome} ({crianca.rf})
                      </span>
                      <br />
                    </div>
                  );
                })}
              </div>
              <div className="col-12">
                <Button
                  id={shortid.generate()}
                  label="Editar crianças envolvidas"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={() => {}}
                  icon="user-edit"
                />
              </div>
              <div className="col-md-3 col-sm-12 col-lg-3 mt-3">
                <CampoData
                  label="Data da ocorrência"
                  name="dataOcorrencia"
                  form={form}
                  valor={dataOcorrencia}
                  onChange={onChangeDataOcorrencia}
                  placeholder="Selecione a data"
                  formatoData="DD/MM/YYYY"
                />
              </div>
              <div className="col-md-3 col-sm-12 col-lg-3 mt-3">
                <CampoData
                  label="Hora da ocorrência"
                  name="horaOcorrencia"
                  form={form}
                  valor={horaOcorrencia}
                  onChange={onChangeHoraOcorrencia}
                  placeholder="Selecione a data"
                  formatoData="HH:mm"
                  somenteHora
                />
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default CadastroOcorrencias;
