import React from 'react'
import styled from 'styled-components'

const Wrapper = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  padding: 30px 0;
`

const TableFooter = ({ children }) => {
  return (
    <Wrapper>
      {children}
    </Wrapper>
  )
}

export default TableFooter