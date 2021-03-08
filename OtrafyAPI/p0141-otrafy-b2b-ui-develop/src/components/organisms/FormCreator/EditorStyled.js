import React from 'react'
import styled from 'styled-components'

export const Wrapper = styled.div`
  background: #FFFFFF;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  border-radius: 10px;
  padding: 15px;
  width: 48%;
  margin: 0 1.25%;
`
export const EditorHeading = styled.div`
  padding-bottom: 10px;
  border-bottom: 1px solid #E3E5E5;
  margin-bottom: 15px;
  h1 {
    color: ${props => props.theme.solidColor};
    font-size: 24px;
    font-weight: 500;
    margin-bottom: 0;
  }
  p {
    color: #919699;
    font-size: 14px;
    margin-bottom: 0;
  }
`
export const EditorMain = styled.div`
  height: 100%; 
`
export const EmptyWrapper = styled.div`
  background: #F9F9F9;
  border: 1px dashed #C4C4C4;
  box-sizing: border-box;
  border-radius: 4px;
  padding: 28px 72px;
  color: #919699;
`
export const PageFooter = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 15px;
`
export const PageIndicator = styled.div`
  display: inline-flex;
  align-items: center;
  justify-content: flex-end;
  color: #919699;
`
export const FormPageNavigation = styled.a`
  text-decoration: none;
  color: ${props => props.color.solidColor};
  text-transform: uppercase;
  font-size: 14px;
  &:hover {
    color: ${props => props.color.solidColor};
  }
  &:before {
    display: ${props => props.type === 'next' ? 'inline-block' : 'none'}
  }
  &:after {
    display: ${props => props.type !== 'next' ? 'inline-block' : 'none'}
  }
  &:before, &:after {
    content: '-';
    color: #919699;
    margin: 0 10px;
  }
`