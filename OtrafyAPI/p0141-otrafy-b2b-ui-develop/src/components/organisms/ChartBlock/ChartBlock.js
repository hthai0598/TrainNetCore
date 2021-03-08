import React from 'react'
import PropTypes from 'prop-types'
import {
  ChartCard,
  CardHeading,
} from './CustomStyled'

const ChartBlock = ({ chart, title, addon }) => {
  return (
    <ChartCard>
      <CardHeading>
        <div className={'title'}>
          {title}
        </div>
        <div className={'addon'}>
          {addon}
        </div>
      </CardHeading>
      {chart}
    </ChartCard>
  )
}

ChartBlock.propTypes = {
  title: PropTypes.string,
  chart: PropTypes.node,
  addon: PropTypes.node,
}

export default ChartBlock