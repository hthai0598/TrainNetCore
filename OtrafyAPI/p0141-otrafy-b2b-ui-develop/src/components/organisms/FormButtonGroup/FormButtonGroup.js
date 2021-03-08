import React from 'react'
import styled from 'styled-components'

const Wrapper = styled.div`
  display: flex;
  justify-content: flex-end;
  .ant-btn {
    margin-left: 10px;
    height: 40px;
  }
`

const FormButtonGroup = ({ children }) => {
  return (
    <Wrapper>
      {children}
    </Wrapper>
  )
}

export default FormButtonGroup