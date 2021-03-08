import React from 'react'
import styled from 'styled-components'

export const SignatureEditButtonRow = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-wrap: wrap;
`
export const ButtonGroup = styled.div`
  display: flex;
  align-items: center;
  margin: 0 -5px;
  .ant-btn {
    margin: 0 5px;
  }
`
export const SignatureWrapper = styled.div`
  border: 1px solid #d9d9d9;
  margin-bottom: 15px;
  img {
    width: 100%;
  }
`