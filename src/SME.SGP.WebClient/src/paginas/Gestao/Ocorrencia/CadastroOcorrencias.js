import { Form, Formik } from 'formik';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import * as Yup from 'yup';
import {
  Auditoria,
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
  confirmar,
  erro,
  verificaSomenteConsulta,
} from '~/servicos';
import { ordenarPor } from '~/utils/funcoes/gerais';

const CadastroOcorrencias = ({ match }) => {
  const [dataOcorrencia, setDataOcorrencia] = useState();
  const [horaOcorrencia, setHoraOcorrencia] = useState();
  const [refForm, setRefForm] = useState({});
  const [listaTiposOcorrencias, setListaTiposOcorrencias] = useState();
  const [modalCriancasVisivel, setModalCriancasVisivel] = useState(false);
  const [listaCriancas, setListaCriancas] = useState([]);
  const [criancasSelecionadas, setCriancasSelecionadas] = useState([]);
  const [criancasSelecionadasEdicao, setCriancasSelecionadasEdicao] = useState(
    []
  );
  const [
    codigosCriancasSelecionadas,
    setCodigosCriancasSelecionadas,
  ] = useState([]);
  const [valoresIniciais, setValoresIniciais] = useState({
    dataOcorrencia: window.moment(),
    descricao: '',
    ocorrenciaTipoId: '',
    titulo: '',
    alunos: [],
  });
  const [auditoria, setAuditoria] = useState();
  const [idOcorrencia, setIdOcorrencia] = useState();
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [modoEdicao, setModoEdicao] = useState(false);

  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada, permissoes } = usuario;
  const { podeExcluir, podeAlterar, podeIncluir } = permissoes[
    RotasDto.OCORRENCIAS
  ];

  useEffect(() => {
    setSomenteConsulta(
      verificaSomenteConsulta(permissoes[RotasDto.OCORRENCIAS])
    );
  }, [permissoes]);

  useEffect(() => {
    ServicoOcorrencias.buscarTiposOcorrencias().then(resp => {
      if (resp?.data) {
        setListaTiposOcorrencias(resp.data);
      }
    });
    ServicoOcorrencias.buscarCriancas(turmaSelecionada?.id).then(resp => {
      if (resp?.data) {
        setListaCriancas(resp.data);
      }
    });
  }, []);

  useEffect(() => {
    if (match?.params?.id) {
      setIdOcorrencia(match?.params?.id);
    }
  }, [match]);

  useEffect(() => {
    if (criancasSelecionadas) setModoEdicao(modificouCriancasSelecionadas());
  }, [criancasSelecionadas]);

  useEffect(() => {
    async function obterPorId(id) {
      setBreadcrumbManual(
        match?.url,
        'Alterar ocorrência',
        RotasDto.OCORRENCIAS
      );
      const ocorrencia = await ServicoOcorrencias.buscarOcorrencia(id);
      if (ocorrencia && Object.entries(ocorrencia).length) {
        ocorrencia.data.dataOcorrencia = window.moment(
          new Date(ocorrencia.data.dataOcorrencia)
        );
        const data = new Date(ocorrencia.data.dataOcorrencia);
        const horaMin = ocorrencia.data.horaOcorrencia.split(":");
        data.setHours(ocorrencia.data.horaOcorrencia ? horaMin[0] : '00');
        data.setMinutes(ocorrencia.data.horaOcorrencia ? horaMin[1] : '00');                
        ocorrencia.data.horaOcorrencia = window.moment(
          new Date(data)
        );
        setValoresIniciais(ocorrencia.data);
        refForm.setFieldValue(
          'ocorrenciaTipoId',
          ocorrencia.data.ocorrenciaTipoId
        );
        const criancas = ocorrencia.data.alunos.map(crianca => {
          return {
            nome: crianca.nome,
            codigoEOL: crianca.codigoAluno.toString(),
          };
        });
        setAuditoria(ocorrencia.data.auditoria);
        setCriancasSelecionadas(criancas);
        setCriancasSelecionadasEdicao(criancas);
      }
    }
    if (idOcorrencia) {
      obterPorId(idOcorrencia);
    }
  }, [idOcorrencia]);

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
    valores.turmaId = turmaSelecionada.id;
    valores.horaOcorrencia = valores.horaOcorrencia
      ? valores.horaOcorrencia.format('HH:mm').toString()
      : null;
    valores.codigosAlunos = criancasSelecionadas.map(a => {
      return a.codigoEOL;
    });
    if (match?.params?.id) {
      valores.id = match?.params?.id;
      ServicoOcorrencias.alterar(valores)
        .then(() => {
          sucesso('Ocorrência alterada com sucesso');
          history.push(RotasDto.OCORRENCIAS);
        })
        .catch(e => erros(e));
    } else {
      ServicoOcorrencias.incluir(valores)
        .then(() => {
          sucesso('Ocorrência salva com sucesso');
          history.push(RotasDto.OCORRENCIAS);
        })
        .catch(e => erros(e));
    }
  };

  const onClickExcluir = async () => {
    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir este registro?'
    );
    if (confirmado) {
      const parametros = { data: [match?.params?.id] };
      const exclusao = await ServicoOcorrencias.excluir(parametros);
      if (exclusao && exclusao.status === 200) {
        sucesso('Registro excluído com sucesso');
        history.push(RotasDto.OCORRENCIAS);
      } else {
        erro(exclusao);
      }
    }
  };

  const confirmarAntesDeVoltar = async form => {
    const confirmado = await confirmar(
      'Atenção',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
    if (confirmado) {
      validaAntesDoSubmit(form);
    } else {
      history.push(RotasDto.OCORRENCIAS);
    }
  };

  const modificouCriancasSelecionadas = () => {
    let modificou = false;
    if (idOcorrencia) {
      valoresIniciais.alunos.forEach(aluno => {
        const alunoExistente = criancasSelecionadas.find(
          c => c.codigoEOL.toString() === aluno.codigoAluno.toString()
        );
        if (!alunoExistente) {
          modificou = true;
        }
      });
    }
    if (criancasSelecionadas.length !== valoresIniciais.alunos.length) {
      modificou = true;
    } else {
      modificou = false;
    }
    return modificou;
  };

  const onClickVoltar = async form => {
    let existeDivergencia = modificouCriancasSelecionadas();
    if (existeDivergencia) {
      confirmarAntesDeVoltar(form);
    }
    if (form.values) {
      const campos = Object.keys(form.values);
      campos.forEach(key => {
        if (valoresIniciais[key] !== form.values[key]) {
          existeDivergencia = true;
          confirmarAntesDeVoltar(form);
        }
      });
    }
    if (!existeDivergencia) history.push(RotasDto.OCORRENCIAS);
  };

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
    setCriancasSelecionadas(criancasSelecionadasEdicao);
    if (!idOcorrencia) {
      setCriancasSelecionadas([]);
    }
    setModoEdicao(false);
  };

  const onChangeDataOcorrencia = valor => {
    setDataOcorrencia(valor);
    setModoEdicao(true);
  };

  const onChangeHoraOcorrencia = valor => {
    setHoraOcorrencia(valor);
    setModoEdicao(true);
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

  const onConfirmarModal = async () => {
    const criancas = [];
    codigosCriancasSelecionadas.forEach(codigo => {
      const crianca = listaCriancas.find(c => c.codigoEOL === codigo);
      criancas.push(crianca);
    });
    const criancasOrdenadas = await ordenarPor(criancas, 'nome');
    setCriancasSelecionadas([...criancasOrdenadas]);
    setModalCriancasVisivel(false);
  };

  const ehTurmaAnoAnterior = () => {
    return (
      turmaSelecionada?.anoLetivo.toString() !== window.moment().format('YYYY')
    );
  };

  const getNomeTurma = () => {
    return turmaSelecionada?.desc.split('-')[1].trim();
  };

  const naoPodeIncluirOuAlterar = () => {
    return idOcorrencia ? !podeAlterar : !podeIncluir;
  };

  const desabilitarCampos = () => {
    return (
      !criancasSelecionadas?.length > 0 ||
      ehTurmaAnoAnterior() ||
      somenteConsulta ||
      naoPodeIncluirOuAlterar()
    );
  };

  return (
    <>
      <Cabecalho pagina="Cadastro de ocorrência" />
      <Card>
        <ModalConteudoHtml
          titulo={`Selecione a(s) criança(s) envolvida(s) nesta ocorrência - ${getNomeTurma()}`}
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
          desabilitarBotaoPrincipal={
            ehTurmaAnoAnterior() || somenteConsulta || naoPodeIncluirOuAlterar()
          }
        >
          <div className="col-md-12 pt-2">
            <DataTable
              id="lista-criancas"
              idLinha="codigoEOL"
              selectedRowKeys={codigosCriancasSelecionadas}
              onSelectRow={codigo =>
                ehTurmaAnoAnterior() || somenteConsulta
                  ? {}
                  : onSelectLinhaAluno(codigo)
              }
              onClickRow={() => {}}
              columns={colunas}
              dataSource={listaCriancas}
              selectMultipleRows
              pagination={false}
              scroll={{ y: 500 }}
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
                  onClick={() => onClickVoltar(form)}
                />
                <Button
                  id={shortid.generate()}
                  label="Cancelar"
                  color={Colors.Azul}
                  border
                  className="mr-2"
                  onClick={onClickCancelar}
                  disabled={
                    !modoEdicao ||
                    ehTurmaAnoAnterior() ||
                    somenteConsulta ||
                    naoPodeIncluirOuAlterar()
                  }
                />
                {match?.params?.id ? (
                  <Button
                    id={shortid.generate()}
                    label="Excluir"
                    color={Colors.Vermelho}
                    border
                    className="mr-2"
                    onClick={onClickExcluir}
                    disabled={
                      ehTurmaAnoAnterior() || somenteConsulta || !podeExcluir
                    }
                  />
                ) : null}
                <Button
                  id={shortid.generate()}
                  label={idOcorrencia ? 'Alterar' : 'Cadastrar'}
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={() => validaAntesDoSubmit(form)}
                  disabled={
                    !modoEdicao ||
                    !criancasSelecionadas?.length > 0 ||
                    ehTurmaAnoAnterior() ||
                    somenteConsulta ||
                    naoPodeIncluirOuAlterar()
                  }
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
                  label={
                    ehTurmaAnoAnterior() ||
                    somenteConsulta ||
                    naoPodeIncluirOuAlterar()
                      ? 'Consultar crianças envolvidas'
                      : 'Editar crianças envolvidas'
                  }
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
                    desabilitado={desabilitarCampos()}
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
                    desabilitado={desabilitarCampos()}
                  />
                </div>
                <div className="col-md-6 col-sm-12 col-lg-6">
                  <SelectComponent
                    form={form}
                    id="tipoOcorrenciaId"
                    placeholder="Situação"
                    label="Tipo de ocorrência"
                    name="ocorrenciaTipoId"
                    valueOption="id"
                    valueText="descricao"
                    lista={listaTiposOcorrencias}
                    value={form.values.ocorrenciaTipoId}
                    onChange={() => setModoEdicao(true)}
                    disabled={desabilitarCampos()}
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
                    desabilitado={desabilitarCampos()}
                    onChange={() => setModoEdicao(true)}
                  />
                </div>
                <div className="col-12 mt-2">
                  <JoditEditor
                    label="Descrição"
                    form={form}
                    value={form.values.descricao}
                    name="descricao"
                    id="descricao"
                    permiteInserirArquivo
                    desabilitar={desabilitarCampos()}
                    onChange={() => setModoEdicao(true)}
                  />
                </div>
              </div>
            </Form>
          )}
        </Formik>
        {auditoria?.criadoEm ? (
          <Auditoria
            criadoEm={auditoria.criadoEm}
            criadoPor={auditoria.criadoPor}
            criadoRf={auditoria.criadoRf}
            alteradoPor={auditoria.alteradoPor}
            alteradoEm={auditoria.alteradoEm}
            alteradoRf={auditoria.alteradoRf}
          />
        ) : null}
      </Card>
    </>
  );
};

export default CadastroOcorrencias;
