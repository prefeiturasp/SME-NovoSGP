import React, { useState } from 'react';
import { Form, Formik } from 'formik';
import * as Yup from 'yup';
import Editor from '~/componentes/editor/editor';
import { Colors, Auditoria } from '~/componentes';
import Button from '~/componentes/button';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erro, sucesso } from '~/servicos/alertas';
import { useSelector, useDispatch } from 'react-redux';
import { setExpandirLinha } from '~/redux/modulos/notasConceitos/actions';

const Justificativa = () => {
  const dispatch = useDispatch();

  const validacoes = Yup.object({
    justificativa: Yup.string().required('Campo justificativa é obrigatório'),
  });

  const dadosPrincipaisConselhoClasse = useSelector(
    store => store.conselhoClasse.dadosPrincipaisConselhoClasse
  );

  const notaConceito = useSelector(
    store => store.conselhoClasse.notaConceitoPosConselhoAtual
  );

  const {
    conselhoClasseId,
    fechamentoTurmaId,
    alunoCodigo,
  } = dadosPrincipaisConselhoClasse;

  const valoresIniciais = {
    justificativa: notaConceito ? notaConceito.justificativa : '',
  };
  const [auditoria, setAuditoria] = useState(
    notaConceito && notaConceito.auditoria ? notaConceito.auditoria : {}
  );

  const salvarJustificativa = async valor => {
    const notaDto = {
      justificativa: valor.justificativa,
      nota: notaConceito.nota,
      conceito: notaConceito.conceito,
      codigoComponenteCurricular: notaConceito.codigoComponenteCurricular,
    };
    await ServicoConselhoClasse.salvarNotaPosConselho(
      conselhoClasseId,
      fechamentoTurmaId,
      alunoCodigo,
      notaDto
    )
      .then(() => {
        if (notaConceito && notaConceito.idCampo) {
          const linha = {};
          linha[notaConceito.idCampo] = false;
          dispatch(setExpandirLinha(linha));
        }
        sucesso('Registro salvo com sucesso');
      })
      .catch(e => erro(e));
  };

  const clicouBotaoSalvar = form => {
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

  return (
    <>
      <div className="row">
        <div className="col-md-12 d-flex justify-content-start">
          <span>Justificativa de nota pós-conselho</span>
        </div>
        <div className="col-md-12">
          <Formik
            enableReinitialize
            onSubmit={valor => salvarJustificativa(valor)}
            validationSchema={validacoes}
            initialValues={valoresIniciais}
            validateOnBlur={false}
            validateOnChange={false}
          >
            {form => (
              <Form>
                <fieldset className="mt-3">
                  <Editor
                    form={form}
                    name="justificativa"
                    id="justificativa"
                    desabilitar={notaConceito && notaConceito.registroSalvo}
                  />
                  <div className="d-flex justify-content-end pt-2">
                    <Button
                      label="Salvar"
                      color={Colors.Roxo}
                      onClick={e => {
                        clicouBotaoSalvar(form, e);
                      }}
                      disabled={notaConceito && notaConceito.registroSalvo}
                      border
                    />
                  </div>
                </fieldset>
              </Form>
            )}
          </Formik>
        </div>
        <Auditoria
          criadoEm={auditoria.criadoEm}
          criadoPor={auditoria.criadoPor}
          criadoRf={auditoria.criadoRf}
          alteradoPor={auditoria.alteradoPor}
          alteradoEm={auditoria.alteradoEm}
          alteradoRf={auditoria.alteradoRf}
        />
      </div>
    </>
  );
};

export default Justificativa;
