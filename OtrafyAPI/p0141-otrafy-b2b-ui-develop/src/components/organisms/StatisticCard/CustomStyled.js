import React from 'react'
import styled from 'styled-components'

export const CardWrapper = styled.div`
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  border-radius: 6px;
  border: 1px solid;
  background-color: #fff;
  padding: 15px;
  position: relative;
`
export const CardHeading = styled.div`
  font-size: 14px;
  color: #666b74;
`
export const CardStatistic = styled.div`
  font-size: 36px;
  line-height: 1.2;
`
export const CardIcon = styled.img`
  position: absolute;
  right: 30px;
  top: 50%;
  transform: translateY(-50%);
  width: auto;
  height: 50px;
  opacity: .2;
`