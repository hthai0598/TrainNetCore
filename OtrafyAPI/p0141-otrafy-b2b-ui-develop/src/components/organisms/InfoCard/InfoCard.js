import React from 'react'
import PropTypes from 'prop-types'
import { Tooltip } from 'antd'
import {
  StyledInfoCard,
  CardTitle,
  StatisticWrapper,
  Info,
} from './CustomStyled'

const InfoCard = ({ title, left, right, leftTxt, rightTxt, color }) => {
  return (
    <StyledInfoCard>
      <CardTitle>{title}</CardTitle>
      <StatisticWrapper>
        <Info type={'left'} color={color}>
          <Tooltip title={left} placement="topLeft">
            <p>{left}</p>
          </Tooltip>
          <p>{leftTxt}</p>
        </Info>
        <Info type={'right'} color={color}>
          <Tooltip title={right} placement="topLeft">
            <p>{right}</p>
          </Tooltip>
          <p>{rightTxt}</p>
        </Info>

      </StatisticWrapper>
    </StyledInfoCard>
  )
}

InfoCard.propTypes = {
  title: PropTypes.string,
  left: PropTypes.number,
  right: PropTypes.number,
  leftTxt: PropTypes.string,
  rightTxt: PropTypes.string,
  color: PropTypes.string,
}

export default InfoCard