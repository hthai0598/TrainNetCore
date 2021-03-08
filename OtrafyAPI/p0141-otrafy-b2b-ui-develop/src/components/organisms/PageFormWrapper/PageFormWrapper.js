import React from 'react'
import PropTypes from 'prop-types'
import { inject, observer } from 'mobx-react'
import {
  Wrapper,
  FormWrapper,
  Heading,
} from './CustomStyled'

const PageFormWrapper = ({ form, title, commonStore }) => {
  return (
    <Wrapper>
      <FormWrapper>
        <Heading color={commonStore.appTheme.solidColor}>
          {title}
        </Heading>
        {form}
      </FormWrapper>
    </Wrapper>
  )
}

PageFormWrapper.propTypes = {
  form: PropTypes.node,
  title: PropTypes.string,
}

export default inject('commonStore')(observer(PageFormWrapper))