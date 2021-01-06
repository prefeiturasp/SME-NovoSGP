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
import {
  ServicoOcorrencias,
  history,
  erros,
  setBreadcrumbManual,
  sucesso,
} from '~/servicos';

const CadastroOcorrencias = ({ match }) => {
  const [dataOcorrencia, setDataOcorrencia] = useState();
  const [horaOcorrencia, setHoraOcorrencia] = useState();
  const [tipoOcorrenciaId, setTipoOcorrenciaId] = useState();
  const [refForm, setRefForm] = useState({});
  const [listaTiposOcorrencias, setListaTiposOcorrencias] = useState();
  const [modalCriancasVisivel, setModalCriancasVisivel] = useState(false);
  const [listaCriancas, setListaCriancas] = useState([]);
  const [criancasSelecionadas, setCriancasSelecionadas] = useState([]);
  const [
    codigosCriancasSelecionadas,
    setCodigosCriancasSelecionadas,
  ] = useState([]);
  const [valoresIniciais, setValoresIniciais] = useState({
    dataOcorrencia: window.moment(),
  });

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada: turmaSelecionadaStore } = usuario;

  useEffect(() => {
    ServicoOcorrencias.buscarTiposOcorrencias().then(resp => {
      if (resp?.data) {
        setListaTiposOcorrencias(resp.data);
      }
    });
    ServicoOcorrencias.buscarCriancas(turmaSelecionadaStore?.id).then(resp => {
      if (resp?.data) {
        setListaCriancas(resp.data);
      }
    });
  }, []);

  useEffect(() => {
    if (match?.params?.id) {
      setBreadcrumbManual(
        match?.url,
        'Alterar ocorrência',
        RotasDto.OCORRENCIAS
      );
      ServicoOcorrencias.buscarOcorrencia(match?.params?.id).then(resp => {
        if (resp?.data) {
          resp.data.dataOcorrencia = window.moment(
            new Date(resp.data.dataOcorrencia)
          );
          setValoresIniciais(resp.data);
          const criancas = resp.data.alunos.map(crianca => {
            return {
              nome: crianca.nome,
              codigoEOL: crianca.codigoAluno.toString(),
            };
          });
          setCriancasSelecionadas(criancas);
        }
      });
    }
  }, [match]);

  const validacoes = Yup.object({
    dataOcorrencia: momentSchema.required('Campo obrigatório'),
    ocorrenciaTipoId: Yup.string().required('Campo obrigatório'),
    titulo: Yup.string()
      .required('Campo obrigatório')
      .min(10, 'O título deve ter pelo menos 10 caracteres'),
    descricao: Yup.string().required('Campo obrigatório'),
  });

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

  const onSubmitFormulario = valores => {
    valores.turmaId = turmaSelecionadaStore.id;
    valores.codigosAlunos = criancasSelecionadas.map(a => {
      return a.codigoEOL;
    });
    if (match?.params?.id) {
      valores.id = match?.parms?.id;
      ServicoOcorrencias.alterar(valores)
        .then(() => {
          sucesso('Ocorrência alterada com sucesso');
        })
        .catch(e => erros(e));
    } else {
      ServicoOcorrencias.incluir(valores)
        .then(() => {
          sucesso('Ocorrência salva com sucesso');
        })
        .catch(e => erros(e));
    }
  };

  const onClickExcluir = () => {};

  const onClickVoltar = () => history.push(RotasDto.OCORRENCIAS);

  const onClickEditarCriancas = () => {
    setModalCriancasVisivel(true);
    setCodigosCriancasSelecionadas(
      criancasSelecionadas?.length
        ? criancasSelecionadas.map(c => {
            return c.codigoEOL;
          })
        : []
    );
  };

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

  const colunas = [
    {
      title: 'Criança',
      dataIndex: 'nome',
      width: '100%',
      render: (text, row) => (
        <span>
          {row.nome} ({row.codigoEOL})
        </span>
      ),
    },
  ];

  const onSelectLinhaAluno = codigos => {
    setCodigosCriancasSelecionadas(codigos);
  };

  const onConfirmarModal = () => {
    const criancas = [];
    codigosCriancasSelecionadas.forEach(codigo => {
      const crianca = listaCriancas.find(c => c.codigoEOL === codigo);
      criancas.push(crianca);
    });
    setCriancasSelecionadas([...criancas]);
    setModalCriancasVisivel(false);
  };

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
            onConfirmarModal();
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
              idLinha="codigoEOL"
              selectedRowKeys={codigosCriancasSelecionadas}
              onSelectRow={codigo => onSelectLinhaAluno(codigo)}
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
          onSubmit={valores => onSubmitFormulario(valores)}
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
                {match?.params?.id ? (
                  <Button
                    id={shortid.generate()}
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    onClick={onClickExcluir}
                  />
                ) : null}
                <Button
                  id={shortid.generate()}
                  label={match?.params?.id ? 'Alterar' : 'Cadastrar'}
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={() => validaAntesDoSubmit(form)}
                  disabled={!form.isValid || !criancasSelecionadas?.length > 0}
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
                        {crianca.nome} ({crianca.codigoEOL})
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
                  onClick={() => onClickEditarCriancas()}
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
                    name="ocorrenciaTipoId"
                    id="tipoOcorrenciaId"
                    lista={listaTiposOcorrencias}
                    valor={tipoOcorrenciaId}
                    valueOption="id"
                    onChange={valor => setTipoOcorrenciaId(valor)}
                    valueText="descricao"
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
