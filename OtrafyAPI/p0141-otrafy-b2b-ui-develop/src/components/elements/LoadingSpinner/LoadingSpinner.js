import React from 'react'
import PropTypes from 'prop-types'
import {
  BlockWrapper,
  SectionWrapper,
  Spinner,
} from './CustomStyled'

const LoadingSpinner = ({ type, theme }) => {

  const blockStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%) rotate(0)',
  }
  const pageAnimation = () => {
    return (
      <style>
        {`
        @keyframes loading-spinner {
          0% { transform : translate(-50%, -50%) rotate(0); }
          100% { transform : translate(-50%, -50%) rotate(360deg); }
        }
        `}
      </style>
    )
  }
  const sectionAnimation = () => {
    return (
      <style>
        {`
        @keyframes loading-spinner {
          0% { transform : rotate(0); }
          100% { transform : rotate(360deg); }
        }
        `}
      </style>
    )
  }

  return (
    <React.Fragment>
      {
        type === 'page'
          ? <BlockWrapper>
            <Spinner theme={theme} style={blockStyle}/>
            {pageAnimation()}
          </BlockWrapper>
          : <SectionWrapper>
            <Spinner theme={theme}/>
            {sectionAnimation()}
          </SectionWrapper>
      }
    </React.Fragment>
  )
}

LoadingSpinner.propTypes = {
  type: PropTypes.oneOf(['page', 'section']).isRequired,
  theme: PropTypes.object.isRequired,
}

export default LoadingSpinner
