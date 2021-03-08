import React from 'react'
import PropTypes from 'prop-types'

const ActiveStatusIndicator = ({ status, type }) => {
  return (
    <div style={{
      display: 'inline-flex',
      textAlign: 'center',
      alignItems: 'center',
      borderRadius: 40,
      padding: type === 'button' ? '5px 15px' : '0',
      backgroundColor: type === 'button'
        ? status ? '#ecf9f6' : 'rgb(254, 237, 235)'
        : 'none',
      color: status ? 'rgb(61, 190, 163)' : 'rgb(244, 67, 54)',
    }}>
      <span style={{
        display: 'inline-block',
        content: '',
        width: 8,
        height: 8,
        borderRadius: '50%',
        marginRight: 4,
        backgroundColor: status ? 'rgb(61, 190, 163)' : 'rgb(244, 67, 54)',
      }}/>
      {status ? 'Active' : 'Inactive'}
    </div>
  )
}

ActiveStatusIndicator.propTypes = {
  status: PropTypes.bool,
  type: PropTypes.oneOf([
    'inline',
    'button',
  ]),
}

export default ActiveStatusIndicator