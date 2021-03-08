import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 50px 15px;
  display: block;
  width: 100%;
  min-height: 650px;
`
export const FormWrapper = styled.div`
  display: block;
  margin-left: auto;
  margin-right: auto;
  max-width: 640px;
`
export const Heading = styled.div`
  font-size: 24px;
  font-weight: 500;
  margin-bottom: 20px;
  color: ${props => props.color};
`