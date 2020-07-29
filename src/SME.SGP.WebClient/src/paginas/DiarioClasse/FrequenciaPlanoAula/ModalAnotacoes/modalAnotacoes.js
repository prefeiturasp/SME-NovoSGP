import { Form, Formik } from 'formik';
import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import shortid from 'shortid';
import * as Yup from 'yup';
import { Auditoria, Colors, Loader, ModalConteudoHtml } from '~/componentes';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import Button from '~/componentes/button';
import Editor from '~/componentes/editor/editor';
import SelectComponent from '~/componentes/select';
import { confirmar, sucesso } from '~/servicos/alertas';
import { EditorAnotacao } from './modalAnotacoes.css';

const ModalAnotacoesFrequencia = props => {
  const {
    exibirModal,
    onCloseModal,
    dadosModalAnotacao,
    dadosListaFrequencia,
    ehInfantil,
  } = props;

  const [showModal, setShowModal] = useState(exibirModal);
  const [carregandoMotivosAusencia, setCarregandoMotivosAusencia] = useState(
    exibirModal
  );
  const [listaMotivoAusencia, setListaMotivoAusencia] = useState([]);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [refForm, setRefForm] = useState({});
  const [valoresIniciais, setValoresIniciais] = useState({
    anotacao: '',
    motivoAusecia: undefined,
  });
  const [validacoes] = useState(
    Yup.object({
      anotacao: Yup.string()
        .nullable()
        .required('Anotação obrigatória'),
      motivoAusecia: Yup.string()
        .nullable()
        .required('Motivo ausência obrigatório'),
    })
  );
  // TODO MOCK REMOVER!
  const [dadosEstudanteOuCrianca, setDadosEstudanteOuCrianca] = useState({
    aluno: {
      nome: 'Marcos',
      numeroChamada: 20,
      dataNascimento: moment(),
      codigoEOL: 789987,
      frequencia: 89,
      situacao: 'Matriculado',
      dataSituacao: moment(),
    },
  });

  const obterAnotacao = async dados => {
    // TODO - Chamar endpoint para consultar anotação!
    // const resultado = await api.get(`v1/consultadados`).catch(e => erros(e));
    console.log(dados);
    const resultado = {};
    if (resultado && resultado.data) {
      const { anotacao } = resultado.data;
      setValoresIniciais({ anotacao });
      setDadosEstudanteOuCrianca(resultado.data);
    }
  };

  const obterListaMotivosAusencia = () => {
    // TODO - Chamar endpoint com a lista de ausências!
    setTimeout(() => {
      const listaMock = [
        { descricao: 'Atestado Médico do Aluno', valor: 1 },
        { descricao: 'Atestado Médico de pessoa da Família', valor: 2 },
        { descricao: 'Doença na Família, sem atestado', valor: 3 },
      ];
      setListaMotivoAusencia(listaMock);
      setCarregandoMotivosAusencia(false);
    }, 2000);
  };

  useEffect(() => {
    if (dadosModalAnotacao) {
      obterAnotacao(dadosModalAnotacao);
      obterListaMotivosAusencia();
    }
  }, [dadosModalAnotacao]);

  const fecharAposSalvarExcluir = (salvou, excluiu) => {
    const linhaEditada = dadosListaFrequencia.find(
      item => item.codigoAluno === dadosModalAnotacao.codigoAluno
    );
    const index = dadosListaFrequencia.indexOf(linhaEditada);
    if (salvou) {
      dadosListaFrequencia[index].temAnotacao = true;
    } else if (excluiu) {
      dadosListaFrequencia[index].temAnotacao = false;
    }
    onCloseModal();
  };

  const onClickSalvarExcluir = async (excluir = false, form = {}) => {
    const { codigoAluno } = dadosModalAnotacao;
    const { anotacao } = form;

    console.log('Código: ' + codigoAluno);
    console.log('Anotação:');
    console.log(anotacao);

    // TODO Chamar endpoint para salvar anotação!
    // const resultado = await api.post('/v1/salvaranotacao').catch(e => erros(e));
    const resultado = { status: 200 };

    if (resultado && resultado.status === 200) {
      sucesso(`Anotação ${excluir ? 'excluída' : 'salva'} com sucesso`);
      fecharAposSalvarExcluir(!excluir, excluir);
    }
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

  const validaAntesDeExcluir = async () => {
    setShowModal(false);
    const confirmado = await confirmar(
      'Atenção',
      '',
      'Você tem certeza que deseja excluir este registro?'
    );
    if (confirmado) {
      onClickSalvarExcluir(true);
    } else {
      setShowModal(true);
    }
  };

  const validaAntesDeFechar = async () => {
    if (modoEdicao) {
      setShowModal(false);
      const confirmado = await confirmar(
        'Atenção',
        '',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        if (refForm) {
          validaAntesDoSubmit(refForm.getFormikContext());
        }
      } else {
        onCloseModal();
      }
    } else {
      onCloseModal();
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  return dadosEstudanteOuCrianca ? (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="inserir-anotacao"
      visivel={showModal}
      titulo={`Anotações ${ehInfantil ? 'da criança' : 'do estudante'}`}
      onClose={() => validaAntesDeFechar()}
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
        onSubmit={texto => onClickSalvarExcluir(false, texto)}
        validateOnChange
        validateOnBlur
      >
        {form => (
          <Form>
            <div className="col-md-12">
              <DetalhesAluno dados={dadosEstudanteOuCrianca.aluno} />
            </div>
            <div className="col-md-12 mt-2">
              <Loader loading={carregandoMotivosAusencia} tip="">
                <SelectComponent
                  form={form}
                  id="motivo-ausencia"
                  name="motivoAusecia"
                  lista={listaMotivoAusencia}
                  valueOption="valor"
                  valueText="descricao"
                  onChange={onChangeCampos}
                  placeholder="Selecione um motivo"
                />
              </Loader>
            </div>
            <div className="col-md-12 mt-2">
              <EditorAnotacao>
                <Editor form={form} name="anotacao" onChange={onChangeCampos} />
              </EditorAnotacao>
            </div>
            <div className="row">
              <div
                className="col-md-8 d-flex justify-content-start"
                style={{ marginTop: '-15px' }}
              >
                <Auditoria
                  criadoPor={dadosEstudanteOuCrianca.criadoPor}
                  criadoEm={dadosEstudanteOuCrianca.criadoEm}
                  alteradoPor={dadosEstudanteOuCrianca.alteradoPor}
                  alteradoEm={dadosEstudanteOuCrianca.alteradoEm}
                  alteradoRf={dadosEstudanteOuCrianca.alteradoRF}
                  criadoRf={dadosEstudanteOuCrianca.criadoRF}
                />
              </div>
              <div className="col-md-4 d-flex justify-content-end">
                <Button
                  key="btn-voltar-anotacao"
                  id="btn-voltar-anotacao"
                  label="Voltar"
                  icon="arrow-left"
                  color={Colors.Azul}
                  border
                  onClick={validaAntesDeFechar}
                  className="mr-3 mt-2 padding-btn-confirmacao"
                />
                <Button
                  key="btn-excluir-anotacao"
                  id="btn-excluir-anotacao"
                  label="Excluir"
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={validaAntesDeExcluir}
                  disabled={
                    dadosModalAnotacao && !dadosModalAnotacao.temAnotacao
                  }
                />
                <Button
                  id="btn-salvar-anotacao"
                  key="btn-salvar-anotacao"
                  label="Salvar"
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={() => validaAntesDoSubmit(form)}
                />
              </div>
            </div>
          </Form>
        )}
      </Formik>
    </ModalConteudoHtml>
  ) : (
    ''
  );
};

ModalAnotacoesFrequencia.propTypes = {
  exibirModal: PropTypes.bool,
  onCloseModal: PropTypes.func,
  dadosModalAnotacao: PropTypes.oneOfType([PropTypes.object]),
  dadosListaFrequencia: PropTypes.oneOfType([PropTypes.array]),
  ehInfantil: PropTypes.bool,
};

ModalAnotacoesFrequencia.defaultProps = {
  exibirModal: false,
  onCloseModal: () => {},
  dadosModalAnotacao: {},
  dadosListaFrequencia: [],
  ehInfantil: false,
};

export default ModalAnotacoesFrequencia;
