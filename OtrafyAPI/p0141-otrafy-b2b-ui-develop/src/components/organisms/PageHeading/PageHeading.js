import React from 'react'
import PropTypes from 'prop-types'
import {
  Wrapper,
  Heading,
  Tools,
} from './CustomStyled'

const PageHeading = ({ title, breadcrumbs, children }) => {
  return (
    <Wrapper>
      <Heading>
        <h1>{title}</h1>
        {breadcrumbs}
      </Heading>
      <Tools>
        {children}
      </Tools>
    </Wrapper>
  )
}

PageHeading.propTypes = {
  title: PropTypes.string,
  breadcrumbs: PropTypes.element,
  children: PropTypes.node,
}

export default PageHeading