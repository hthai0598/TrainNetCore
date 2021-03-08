import React from 'react'
import styled from 'styled-components'

export const StyledInfoCard = styled.div`
  display: block;
  padding: 15px;
  background-color: #fff;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
  position: relative;
  &:before {
    display: block;
    content: '';
    position: absolute;
    top: -11px;
    left: 10px;
    width: calc(100% - 20px);
    border-bottom: 1px dashed #E3E5E5;
    padding-top: 10px;
    background: white;
    z-index: 10;
  }
`
export const CardTitle = styled.div`
  font-size: 14px;
  font-weight: 500;
  color: #000;
  margin-bottom: 20px;
`
export const StatisticWrapper = styled.div`
  display: flex;
  flex-wrap: wrap;
`
export const Info = styled.div`
  width: 50%;
  border-right: ${props => props.type === 'left' ? '1px solid rgba(0, 0, 0, .1)' : 'none'};
  padding-left: ${props => props.type === 'left' ? 0 : '15px'};
  p {
    margin-bottom: 0;
    &:first-of-type {
      color: ${props => props.color};
      font-size: 36px;
      font-weight: 500;
      line-height: 1.2;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    &:last-of-type {
      font-size: 12px;
      color: #666;    
    }
  }
`