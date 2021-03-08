import React from 'react'
import uuid from 'uuid'
import PropTypes from 'prop-types'
import {
  TimelineWrapper, TimelineItem,
} from './CustomStyled'

const CustomTimeline = ({ data }) => {
  return (
    <TimelineWrapper>
      {
        data.map(item =>
          <TimelineItem key={uuid()}>
            <div className="info">
              <h3>{item.formName} {item.status}</h3>
              <p>by {item.buyerName}</p>
            </div>
            <div className="date">
              {item.date}
            </div>
          </TimelineItem>,
        )
      }
    </TimelineWrapper>
  )
}

CustomTimeline.propTypes = {
  data: PropTypes.array,
}

export default CustomTimeline