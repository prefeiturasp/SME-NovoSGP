import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { Base } from '../colors';
import CardBody from '../cardBody';
import Icon from '../icon';

const CardLink = (props) => {

    const { icone, pack, label, url, disabled, alt, cols, className } = props;
    const color = disabled ? Base.CinzaDesabilitado : Base.Roxo;

    const CardLine = styled.div`
        border-bottom: 5px solid ${(disabled ? Base.CinzaDesabilitado : Base.Roxo)} !important;
    `;

    const Label = styled.h2`
        font-size: 16px;
        margin-top: 20px;
        color: ${color}
    `;

    const Div = styled.div`

    `;

    const getCols = () => {
        let retorno = "col-lg-3";

        if (!cols)
            return retorno;

        if (cols[0])
            retorno = `col-lg-${cols[0]}`;

        if (cols[1])
            retorno += ` col-md-${cols[1]}`;

        if (cols[2])
            retorno += ` col-sm-${cols[2]}`;

        if (cols[3])
            retorno += ` col-xs-${cols[3]}`;

        return retorno;
    }

    return (
        <div className={`row`}>
            <Div className={`${getCols()} px-2`}>
                <Link to={url} alt={alt}>
                    <CardLine className={`card ${className}`}>
                        <CardBody className="text-center">
                            <div className="col-md-12 p-0 text-center">
                                <Icon icon={icone} pack={pack} color={color} />
                                <Label>{label}</Label>
                            </div>
                        </CardBody>
                    </CardLine>
                </Link>
            </Div>
        </div>
    );
}

CardLink.defaultProps = {
    icone: "fa-check",
    pack: "fa fa-5x",
    label: "Insira a label",
    url: "/",
    disabled: false,
    alt: "",
    cols: [3],
    className: ""
};


export default CardLink;

