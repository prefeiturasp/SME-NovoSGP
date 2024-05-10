import { Form, Formik } from 'formik';
import PropTypes from 'prop-types';
import React, { useEffect, useRef, useState } from 'react';
import * as Yup from 'yup';
import { ModalConteudoHtml, SelectComponent } from '~/componentes';
import { erros } from '~/servicos/alertas';
import ServicoCompensacaoAusencia from '~/servicos/Paginas/DiarioClasse/ServicoCompensacaoAusencia';

const CopiarCompensacao = ({
  visivel,
  turmaId,
  listaBimestres,
  onCloseCopiarCompensacao,
  onCopiarCompensacoes,
  compensacoesParaCopiar,
  bimestreSugerido
}) => {
  const [listaTurmas, setListaTurmas] = useState([]);
  const [loader, setLoader] = useState(false);
  const [valoresIniciais, setValoresIniciais] = useState({
    turmas: [],
    bimestre: bimestreSugerido,
  });
  const refForm = useRef();

  useEffect(() => {
    if (turmaId) {
      setLoader(true);
      ServicoCompensacaoAusencia.obterTurmasCopia(turmaId)
        .then(c => {
          setListaTurmas(c.data);
          setLoader(false);
        })
        .catch(e => {
          erros(e);
          setLoader(false);
        });

      if (
        compensacoesParaCopiar &&
        compensacoesParaCopiar.compensacaoOrigemId
      ) {
        setValoresIniciais({
          turmas: compensacoesParaCopiar.turmasIds,
          bimestre: compensacoesParaCopiar.bimestre,
        });
      }
    }
  }, [turmaId, compensacoesParaCopiar]);

  const fecharCopiarCompensacao = () => {
    refForm.current.handleReset();
    onCloseCopiarCompensacao();
  };

  const copiar = valores => {
    const dadosTurmas = listaTurmas.filter(turma =>
      valores.turmas.find(codigo => codigo == turma.codigo)
    );
    onCopiarCompensacoes(valores, dadosTurmas);
    fecharCopiarCompensacao();
  };

  const validacoes = Yup.object({
    bimestre: Yup.string().required('Selecione um bimestre.'),
  });

  return (
    <Formik
      enableReinitialize
      initialValues={valoresIniciais}
      validationSchema={validacoes}
      onSubmit={values => copiar(values)}
      validateOnChange
      validateOnBlur
      ref={refForm}
    >
      {form => (
        <Form>
          <ModalConteudoHtml
            key="copiarCompensacao"
            visivel={visivel}
            onConfirmacaoPrincipal={e => {
              form.validateForm().then(() => {
                form.handleSubmit(e);
              });
            }}
            onConfirmacaoSecundaria={fecharCopiarCompensacao}
            onClose={fecharCopiarCompensacao}
            labelBotaoPrincipal="Selecionar"
            labelBotaoSecundario="Cancelar"
            titulo="Copiar Compensação"
            closable={true}
            loader={loader}
            desabilitarBotaoPrincipal={false}
          >
            <div>
              <SelectComponent
                label="Copiar para a(s) turma(s)"
                id="turmas"
                name="turmas"
                lista={listaTurmas}
                valueOption="codigo"
                valueText="nome"
                multiple
                placeholder="Selecione uma ou mais turmas"
                form={form}
              />
              <SelectComponent
                form={form}
                id="bimestre"
                label="Copiar para o bimestre"
                name="bimestre"
                lista={listaBimestres}
                valueOption="valor"
                valueText="descricao"
                placeholder="Selecione um bimestre"
              />
            </div>
          </ModalConteudoHtml>
        </Form>
      )}
    </Formik>
  );
};

CopiarCompensacao.propTypes = {
  visivel: PropTypes.bool,
  turmaId: PropTypes.string,
  listaBimestres: PropTypes.oneOfType([PropTypes.array, PropTypes.object]),
  onCloseCopiarCompensacao: PropTypes.func,
  onCopiarCompensacoes: PropTypes.func,
  compensacoesParaCopiar: PropTypes.oneOfType([
    PropTypes.array,
    PropTypes.object,
  ]),
};

CopiarCompensacao.defaultProps = {
  visivel: false,
  turmaId: '',
  listaBimestres: [],
  onCloseCopiarCompensacao: () => { },
  onCopiarCompensacoes: () => { },
  compensacoesParaCopiar: [],
};

export default CopiarCompensacao;
