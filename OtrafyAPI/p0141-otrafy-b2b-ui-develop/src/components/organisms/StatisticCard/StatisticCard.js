import React from 'react'
import PropTypes from 'prop-types'
import utils from '../../../utils'
import {
  CardWrapper,
  CardHeading,
  CardStatistic,
  CardIcon,
} from './CustomStyled'

const StatisticCard = ({ heading, statistic, baseColor, imgURL }) => {
  return (
    <CardWrapper className={'statistic-card'} style={{
      borderColor: baseColor,
    }}>
      <CardHeading>
        {heading}
      </CardHeading>
      <CardStatistic style={{
        color: baseColor,
      }}>
        {utils.thousandSeperator(statistic)}
      </CardStatistic>
      <CardIcon src={imgURL} alt={heading}/>
    </CardWrapper>
  )
}

StatisticCard.propTypes = {
  heading: PropTypes.string,
  statistic: PropTypes.number,
  baseColor: PropTypes.string,
}

export default StatisticCard