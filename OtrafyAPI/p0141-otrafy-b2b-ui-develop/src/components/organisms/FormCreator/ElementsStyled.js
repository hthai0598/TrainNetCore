import React from 'react'
import styled from 'styled-components'

export const ElemWrapper = styled.div`
  background: #F9F9F9;
  border: 1px dashed #C4C4C4;
  border-radius: 4px;
  padding: 0 125px 15px 15px;
  margin-bottom: 15px;
  position: relative;
  > label {
    display: block;
    font-size: 12px;
    color: #020202;
    padding: 10px 0;
  }
  > p {
    font-size: 12px;
    color: #999;
  }
  &:hover {
    border: 1px solid ${props => props.theme.solidColor};
    transition: all ease .3s;
  }
`
export const ButtonGroup = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: absolute;
  right: 15px;
  bottom: 25px;
`
export const Button = styled.div`
  width: 22px;
  height: 22px;
  display: block;
  text-align: center;
  border-radius: 50%;
  position: relative;
  margin-left: 5px;
  &:hover {
    cursor: pointer;
  }
  img {
    height: 11px;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
  }
`
export const Element = styled.div`
  background: #FFFFFF;
  border: 1px solid #C6CACC;
  box-sizing: border-box;
  border-radius: 4px;
  padding: 8px 35px 8px 15px;
  position: relative;
  color: #919699;
  font-size: 14px;
  min-height: 40px;
  word-break: break-all;
  .img {
    display: block;
    content: '';
    position: absolute;
    opacity: .4;
    right: 15px;
    bottom: 10px;
  }
  &.multiLine {
    min-height: 79px;
  }
`