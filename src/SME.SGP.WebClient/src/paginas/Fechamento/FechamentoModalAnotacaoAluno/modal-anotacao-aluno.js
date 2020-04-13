import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { Loader, ModalConteudoHtml, Colors, Auditoria } from '~/componentes';
import api from '~/servicos/api';
import { erros, sucesso, erro, confirmar } from '~/servicos/alertas';
import Editor from '~/componentes/editor/editor';
import Button from '~/componentes/button';
import * as Yup from 'yup';
import { Formik, Form } from 'formik';
import { DadosAlunoModal, EditorAnotacao } from './modal-anotacao-aluno.css';

const ModalAnotacaoAluno = props => {
  const {
    exibirModal,
    onCloseModal,
    fechamentoId,
    codigoTurma,
    anoLetivo,
    dadosAlunoSelecionado,
  } = props;

  const [showModal, setShowModal] = useState(exibirModal);
  const [dadosAluno, setDadosAluno] = useState();
  const [modoEdicao, setModoEdicao] = useState(false);
  const [refForm, setRefForm] = useState({});

  const [valoresIniciais, setValoresIniciais] = useState({ anotacao: '' });
  const [validacoes] = useState(
    Yup.object({
      anotacao: Yup.string()
        .nullable()
        .required('Anotação obrigatória'),
    })
  );

  useEffect(() => {
    if (dadosAlunoSelecionado) {
      obterAnotacaoAluno(dadosAlunoSelecionado);
    }
  }, [dadosAlunoSelecionado]);

  const onClose = (salvou = false, excluiu = false) => {
    onCloseModal(salvou, excluiu);
  };

  const onClickSalvarExcluir = async (excluir = false, form = {}) => {
    const { codigoAluno } = dadosAlunoSelecionado;
    const { anotacao } = form;

    const params = {
      fechamentoId,
      codigoAluno,
      anotacao: excluir ? '' : anotacao,
    };
    const resultado = await api
      .post('/v1/fechamentos/turmas/anotacoes/alunos', params)
      .catch(e => erros(e));
    if (resultado && resultado.status == 200) {
      if (resultado.data.sucesso) {
        sucesso(`Anotação ${excluir ? 'excluída' : 'salva'} com sucesso`);
        onClose(!excluir, excluir);
      } else {
        erro(resultado.data.mensagemConsistencia);
      }
    }
  };

  const obterAnotacaoAluno = async dados => {
    const { codigoAluno } = dados;
    const resultado = await api
      .get(
        `v1/fechamentos/turmas/anotacoes/alunos/${codigoAluno}/fechamentos/${fechamentoId}/turmas/${codigoTurma}/anos/${anoLetivo}`
      )
      .catch(e => erros(e));
    if (resultado && resultado.data) {
      const { anotacao } = resultado.data;
      setValoresIniciais({ anotacao });
      setDadosAluno(resultado.data);
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
        onClose();
      }
    } else {
      onClose();
    }
  };

  const onChangeCampos = () => {
    if (!modoEdicao) {
      setModoEdicao(true);
    }
  };

  return dadosAluno ? (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="inserirAnotacao"
      visivel={showModal}
      titulo="Anotações do estudante"
      onClose={() => validaAntesDeFechar()}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width="650px"
      closable
    >
      <Formik
        ref={refForm => setRefForm(refForm)}
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
              <DadosAlunoModal>
                <i className="fas fa-user-circle icone-perfil" />
                <div>
                  <p>Nome do estudante: {dadosAluno.aluno.nome}</p>
                  <p>Nº de chamada: {dadosAluno.aluno.numeroChamada}</p>
                </div>
              </DadosAlunoModal>
            </div>
            <div className="col-md-12">
              <EditorAnotacao className="mt-3">
                <Editor form={form} name="anotacao" onChange={onChangeCampos} />
              </EditorAnotacao>
            </div>
            <div className="row">
              <div
                className="col-md-8 d-flex justify-content-start"
                style={{ marginTop: '-15px' }}
              >
                <Auditoria
                  criadoPor={dadosAluno.criadoPor}
                  criadoEm={dadosAluno.criadoEm}
                  alteradoPor={dadosAluno.alteradoPor}
                  alteradoEm={dadosAluno.alteradoEm}
                  alteradoRf={dadosAluno.alteradoRF}
                  criadoRf={dadosAluno.criadoRF}
                />
              </div>
              <div className="col-md-4 d-flex justify-content-end">
                <Button
                  key="btn-excluir-anotacao"
                  label="Excluir"
                  color={Colors.Roxo}
                  bold
                  border
                  className="mr-3 mt-2 padding-btn-confirmacao"
                  onClick={validaAntesDeExcluir}
                  disabled={
                    dadosAlunoSelecionado && !dadosAlunoSelecionado.temAnotacao
                  }
                />
                <Button
                  key="btn-sim-confirmacao-anotacao"
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

ModalAnotacaoAluno.propTypes = {
  exibirModal: PropTypes.bool,
  onCloseModal: PropTypes.func,
};

ModalAnotacaoAluno.defaultProps = {
  exibirModal: false,
  onCloseModal: () => {},
};

export default ModalAnotacaoAluno;
