import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import * as Yup from 'yup';
import {
  Base,
  Button,
  CampoData,
  CampoTexto,
  Card,
  Colors,
  DataTable,
  ModalConteudoHtml,
  momentSchema,
  SelectComponent,
} from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import { RotasDto } from '~/dtos';
import { ServicoOcorrencias, history } from '~/servicos';

const CadastroOcorrencias = () => {
  const [dataOcorrencia, setDataOcorrencia] = useState();
  const [horaOcorrencia, setHoraOcorrencia] = useState();
  const [refForm, setRefForm] = useState({});
  const [listaTiposOcorrencias, setListaTiposOcorrencias] = useState();
  const [modalCriancasVisivel, setModalCriancasVisivel] = useState(false);
  const [listaCriancas, setListaCriancas] = useState([]);

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada: turmaSelecionadaStore } = usuario;

  useEffect(() => {
    ServicoOcorrencias.buscarTiposOcorrencias().then(resp => {
      if (resp?.data) {
        setListaTiposOcorrencias(resp.data);
      }
    });
    ServicoOcorrencias.buscarCriancas(turmaSelecionadaStore?.turma).then(
      resp => {
        if (resp?.data) {
          setListaCriancas(resp.data);
        }
      }
    );
  }, []);

  const [valoresIniciais, setValoresIniciais] = useState({
    dataOcorrencia: window.moment(),
  });

  const [validacoes, setValidacoes] = useState(
    Yup.object({
      dataOcorrencia: momentSchema.required('Campo obrigatório'),
      tipo: Yup.string().required('Campo obrigatório'),
      titulo: Yup.string()
        .required('Campo obrigatório')
        .min(10, 'O título deve ter pelo menos 10 caracteres'),
      descricao: Yup.string().required('Campo obrigatório'),
    })
  );

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

  const onClickExcluir = () => {};

  const onClickVoltar = () => history.push(RotasDto.OCORRENCIAS);
  const onClickCancelar = () => {
    refForm.resetForm();
    setDataOcorrencia(window.moment());
  };

  const onChangeDataOcorrencia = valor => {
    setDataOcorrencia(valor);
  };

  const onChangeHoraOcorrencia = valor => {
    setHoraOcorrencia(valor);
  };

  const desabilitarData = current => {
    if (current) {
      return (
        current < window.moment().startOf('year') || current >= window.moment()
      );
    }
    return false;
  };

  const [criancasSelecionadas, setCriancasSelecionadas] = useState([]);
  const colunas = [
    {
      title: 'Criança',
      dataIndex: 'nome',
      width: '100%',
    },
  ];

  return (
    <>
      <Cabecalho pagina="Cadastro de ocorrência" />
      <Card>
        <ModalConteudoHtml
          titulo="Selecione a(s) criança(s) envolvida(s) nesta ocorrência - "
          visivel={modalCriancasVisivel}
          onClose={() => {
            setModalCriancasVisivel(false);
          }}
          onConfirmacaoSecundaria={() => {
            setModalCriancasVisivel(false);
          }}
          onConfirmacaoPrincipal={() => {
            setModalCriancasVisivel(false);
          }}
          labelBotaoPrincipal="Confirmar"
          labelBotaoSecundario="Cancelar"
          closable
          width="50%"
          fecharAoClicarFora
          fecharAoClicarEsc
        >
          <div className="col-md-12 pt-2">
            <DataTable
              id="lista-criancas"
              selectedRowKeys={null}
              onSelectRow={() => {}}
              onClickRow={() => {}}
              columns={colunas}
              dataSource={listaCriancas}
              selectMultipleRows
            />
          </div>
        </ModalConteudoHtml>
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
                  label="Excluir"
                  color={Colors.Vermelho}
                  border
                  className="mr-2"
                  onClick={onClickExcluir}
                />
                <Button
                  id={shortid.generate()}
                  label="Cadastrar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={() => validaAntesDoSubmit(form)}
                  disabled={!form.isValid}
                />
              </div>
              <div className="p-0 col-12 mb-3 font-weight-bold">
                <span>Crianças envolvidas na ocorrência</span>
              </div>
              <div className="p-0 col-12">
                {criancasSelecionadas.slice(0, 3).map((crianca, index) => {
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
              {criancasSelecionadas?.length > 3 ? (
                <div>
                  <span style={{ color: Base.CinzaBotao, fontSize: '12px' }}>
                    Mais {criancasSelecionadas.length - 3}{' '}
                    {criancasSelecionadas.length > 4 ? 'crianças' : 'criança'}
                  </span>
                </div>
              ) : (
                ''
              )}
              <div className="p-0 col-12 mt-3">
                <Button
                  id={shortid.generate()}
                  label="Editar crianças envolvidas"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={() => setModalCriancasVisivel(true)}
                  icon="user-edit"
                />
              </div>
              <div className="row mt-3">
                <div className="col-md-3 col-sm-12 col-lg-3">
                  <CampoData
                    label="Data da ocorrência"
                    name="dataOcorrencia"
                    form={form}
                    valor={dataOcorrencia}
                    onChange={onChangeDataOcorrencia}
                    placeholder="Selecione a data"
                    formatoData="DD/MM/YYYY"
                    desabilitarData={desabilitarData}
                  />
                </div>
                <div className="col-md-3 col-sm-12 col-lg-3">
                  <CampoData
                    label="Hora da ocorrência"
                    name="horaOcorrencia"
                    form={form}
                    valor={horaOcorrencia}
                    onChange={onChangeHoraOcorrencia}
                    placeholder="Selecione a hora"
                    formatoData="HH:mm"
                    somenteHora
                    campoOpcional
                  />
                </div>
                <div className="col-md-6 col-sm-12 col-lg-6">
                  <SelectComponent
                    name="tipo"
                    id="tipoOcorrenciaId"
                    lista={listaTiposOcorrencias}
                    valueOption="id"
                    valueText="descricao"
                    onChange={() => {}}
                    placeholder="Situação"
                    label="Tipo de ocorrência"
                    form={form}
                  />
                </div>
                <div className="col-md-6 col-sm-12 col-lg-6 mt-2">
                  <CampoTexto
                    form={form}
                    name="titulo"
                    id="tituloOcorrencia"
                    label="Título da ocorrência"
                    placeholder="Situação"
                    maxLength={50}
                  />
                </div>
                <div className="col-12 mt-2">
                  <JoditEditor
                    label="Descrição"
                    form={form}
                    value={form.values.descricao}
                    name="descricao"
                    id="descricao"
                    onChange={() => {}}
                    permiteInserirArquivo
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
      </Card>
    </>
  );
};

export default CadastroOcorrencias;
