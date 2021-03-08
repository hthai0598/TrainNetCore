import React from 'react'
import styled from 'styled-components'

export const RequestViewerWrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
  display: block;
  width: calc(100% - 340px);
`
export const RequestViewerHeading = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 0 10px;
  border-bottom: 1px solid #E3E5E5;
  margin-bottom: 15px;
  h2 {
    font-size: 24px;
    font-weight: 500;
    color: ${props => props.theme.solidColor};
    margin-bottom: 0;
    padding-right: 30px;
  }
`
export const FormNavigation = styled.div`
  display: flex;
  align-items: center;
  justify-content: flex-end;
  .total {
    display: inline-block;
    margin: 0 10px;
    color: #919699;
  }
`