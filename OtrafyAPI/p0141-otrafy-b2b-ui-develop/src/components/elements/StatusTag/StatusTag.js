import React from 'react'
import PropTypes from 'prop-types'
import { StyledTag } from './CustomStyled'

const StatusTag = ({ mainColor, children }) => {

  const style = () => {
    switch (mainColor) {
      case 'green':
        return {
          color: '#3DBEA3', backgroundColor: '#ecf9f6',
        }
      case 'yellow':
        return {
          color: '#FF9800', backgroundColor: '#fff5e6',
        }
      case 'red':
        return {
          color: '#F44336', backgroundColor: '#feedeb',
        }
      case 'blue':
        return {
          color: '#2196F3', backgroundColor: '#e9f5fe',
        }
      case 'gray':
        return {
          color: '#666666', backgroundColor: '#f0f0f0',
        }
    }
  }

  return (
    <StyledTag style={style()}>
      {children}
    </StyledTag>
  )
}

StatusTag.propTypes = {
  children: PropTypes.node.isRequired,
  mainColor: PropTypes.oneOf(['green', 'yellow', 'red', 'blue', 'gray']).isRequired,
}

export default StatusTag